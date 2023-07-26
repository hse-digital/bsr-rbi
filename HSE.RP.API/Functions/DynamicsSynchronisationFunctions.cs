using System.Globalization;
using AutoMapper;
using Flurl;
using Flurl.Http;
using HSE.RP.API.Extensions;
using HSE.RP.API.Models;
using HSE.RP.API.Models.DynamicsSynchronisation;
using HSE.RP.API.Models.Payment.Response;
using HSE.RP.API.Services;
using HSE.RP.Domain.Entities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Options;
using BuildingApplicationStatus = HSE.RP.Domain.Entities.BuildingApplicationStatus;

namespace HSE.RP.API.Functions;

public class DynamicsSynchronisationFunctions
{
    private readonly DynamicsService dynamicsService;
    private readonly IMapper mapper;
    private readonly IntegrationsOptions integrationOptions;

    public DynamicsSynchronisationFunctions(DynamicsService dynamicsService, IOptions<IntegrationsOptions> integrationOptions, IMapper mapper)
    {
        this.dynamicsService = dynamicsService;
        this.mapper = mapper;
        this.integrationOptions = integrationOptions.Value;
    }


    [Function(nameof(SyncDeclaration))]
    public async Task<HttpResponseData> SyncDeclaration([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request, [DurableClient] DurableTaskClient durableTaskClient)
    {
        var buildingProfessionApplicationModel = await request.ReadAsJsonAsync<BuildingProfessionApplicationModel>();
        await durableTaskClient.ScheduleNewOrchestrationInstanceAsync(nameof(SynchroniseDeclaration), buildingProfessionApplicationModel);

        return request.CreateResponse();
    }

    [Function(nameof(SyncPayment))]
    public async Task<HttpResponseData> SyncPayment([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request, [DurableClient] DurableTaskClient durableTaskClient)
    {
        var buildingProfessionApplicationModel = await request.ReadAsJsonAsync<BuildingProfessionApplicationModel>();
        await durableTaskClient.ScheduleNewOrchestrationInstanceAsync(nameof(SynchronisePayment), buildingProfessionApplicationModel);

        return request.CreateResponse();
    }

    [Function(nameof(SyncPersonalDetails))]
    public async Task<HttpResponseData> SyncPersonalDetails([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request, [DurableClient] DurableTaskClient durableTaskClient)
    {
        var buildingProfessionApplication = await request.ReadAsJsonAsync<BuildingProfessionApplicationModel>();
        await durableTaskClient.ScheduleNewOrchestrationInstanceAsync(nameof(SynchronisePersonalDetails), buildingProfessionApplication);
        return request.CreateResponse();
    }

    [Function(nameof(SynchroniseDeclaration))]
    public async Task SynchroniseDeclaration([OrchestrationTrigger] TaskOrchestrationContext orchestrationContext)
    {
        var buildingProfessionApplicationModel = orchestrationContext.GetInput<BuildingProfessionApplicationModel>();

        var dynamicsBuildingApplication = await orchestrationContext.CallActivityAsync<DynamicsBuildingProfessionApplication>(nameof(GetBuildingProfessionApplicationUsingId), buildingProfessionApplicationModel.Id);
        if (dynamicsBuildingApplication != null)
        {
            await orchestrationContext.CallActivityAsync(nameof(UpdateBuildingProfessionApplication), new BuildingProfessionApplicationWrapper(buildingProfessionApplicationModel, dynamicsBuildingApplication));
        }
    }

    [Function(nameof(SynchronisePayment))]
    public async Task SynchronisePayment([OrchestrationTrigger] TaskOrchestrationContext orchestrationContext)
    {
        var buildingProfessionApplicationModel = orchestrationContext.GetInput<BuildingProfessionApplicationModel>();

        var dynamicsBuildingProfessionApplication = await orchestrationContext.CallActivityAsync<DynamicsBuildingProfessionApplication>(nameof(GetBuildingProfessionApplicationUsingId), buildingProfessionApplicationModel.Id);
        if (dynamicsBuildingProfessionApplication != null)
        {
            var buildingProfessionApplicationWrapper = new BuildingProfessionApplicationWrapper(buildingProfessionApplicationModel, dynamicsBuildingProfessionApplication);
            await orchestrationContext.CallActivityAsync(nameof(UpdateBuildingProfessionApplication), buildingProfessionApplicationWrapper);

            var payments = await orchestrationContext.CallActivityAsync<List<DynamicsPayment>>(nameof(GetDynamicsPayments), buildingProfessionApplicationModel.Id);
            var paymentSyncTasks = payments.Select(async payment =>
            {
                var paymentResponse = await orchestrationContext.CallActivityAsync<PaymentResponseModel>(nameof(GetPaymentStatus), payment.bsr_govukpaymentid);
                if (paymentResponse != null)
                {
                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdatePayment), new BuildingProfessionApplicationPayment(dynamicsBuildingProfessionApplication.bsr_buildingproappid, paymentResponse));
                    if (paymentResponse.Status == "success"/* && dynamicsBuildingProfessionApplication.bsr_applicationstage != BuildingApplicationStage.ApplicationSubmitted*/)
                    {
                        await orchestrationContext.CallActivityAsync(nameof(UpdateBuildingProfessionApplicationToSubmitted), dynamicsBuildingProfessionApplication);
                    }
                }

                return paymentResponse;
            }).ToArray();

            await Task.WhenAll(paymentSyncTasks);
        }
    }

    [Function(nameof(SynchronisePersonalDetails))]
    public async Task SynchronisePersonalDetails([OrchestrationTrigger] TaskOrchestrationContext orchestrationContext)
    {
        var buildingProfessionApplicationModel = orchestrationContext.GetInput<BuildingProfessionApplicationModel>();

        var dynamicsBuildingProfessionApplication = await orchestrationContext.CallActivityAsync<DynamicsBuildingProfessionApplication>(nameof(GetBuildingProfessionApplicationUsingId), buildingProfessionApplicationModel.Id);
        if (dynamicsBuildingProfessionApplication != null)
        {
            var dynamicsContact = await orchestrationContext.CallActivityAsync<DynamicsContact>(nameof(GetContactUsingId), dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid);

            if (dynamicsContact != null)
            {
                var contact = new Contact
                {
                    Id = dynamicsContact.contactid ?? "",
                    FirstName = buildingProfessionApplicationModel.PersonalDetails.ApplicantName.FirstName ?? "",
                    LastName = buildingProfessionApplicationModel.PersonalDetails.ApplicantName.LastName ?? "",
                    Email = buildingProfessionApplicationModel.PersonalDetails.ApplicantEmail.Email ?? "",
                    AlternativeEmail =  buildingProfessionApplicationModel.PersonalDetails.ApplicantAlternativeEmail is null ? null : buildingProfessionApplicationModel.PersonalDetails.ApplicantAlternativeEmail.Email,
                    PhoneNumber = buildingProfessionApplicationModel.PersonalDetails.ApplicantPhone.PhoneNumber ?? null,
                    AlternativePhoneNumber = buildingProfessionApplicationModel.PersonalDetails.ApplicantAlternativePhone is null ? null : buildingProfessionApplicationModel.PersonalDetails.ApplicantAlternativePhone.PhoneNumber ?? "",
                    Address = buildingProfessionApplicationModel.PersonalDetails.ApplicantAddress is null  ? new BuildingAddress { } : buildingProfessionApplicationModel.PersonalDetails.ApplicantAddress,
                    birthdate = buildingProfessionApplicationModel.PersonalDetails.ApplicantDateOfBirth is null ? null :
                    new DateOnly(int.Parse(buildingProfessionApplicationModel.PersonalDetails.ApplicantDateOfBirth.Year),
                                             int.Parse(buildingProfessionApplicationModel.PersonalDetails.ApplicantDateOfBirth.Month),
                                             int.Parse(buildingProfessionApplicationModel.PersonalDetails.ApplicantDateOfBirth.Day)),
                    NationalInsuranceNumber = buildingProfessionApplicationModel.PersonalDetails.ApplicantNationalInsuranceNumber is null ? null : buildingProfessionApplicationModel.PersonalDetails.ApplicantNationalInsuranceNumber.NationalInsuranceNumber
                };

                var contactWrapper = new ContactWrapper(contact, dynamicsContact);


                await orchestrationContext.CallActivityAsync(nameof(UpdateContact), contactWrapper);
            }
        }
    }

    [Function(nameof(GetBuildingProfessionApplicationUsingId))]
    public Task<DynamicsBuildingProfessionApplication> GetBuildingProfessionApplicationUsingId([ActivityTrigger] string applicationId)
    {
        return dynamicsService.GetBuildingProfessionApplicationUsingId(applicationId);
    }

    [Function(nameof(GetContactUsingId))]
    public Task<DynamicsContact> GetContactUsingId([ActivityTrigger] string contactId)
    {
        return dynamicsService.GetContactUsingId(contactId);
    }

    [Function(nameof(UpdateBuildingProfessionApplication))]
    public Task UpdateBuildingProfessionApplication([ActivityTrigger] BuildingProfessionApplicationWrapper buildingProfessionApplicationWrapper)
    {
        //var stage = buildingProfessionApplicationWrapper.DynamicsBuildingProfessionApplication.bsr_applicationstage == BuildingApplicationStage.ApplicationSubmitted ? BuildingApplicationStage.ApplicationSubmitted : buildingProfessionApplicationWrapper.Stage;
        return dynamicsService.UpdateBuildingProfessionApplication(buildingProfessionApplicationWrapper.DynamicsBuildingProfessionApplication, new DynamicsBuildingProfessionApplication
        {
            //bsr_applicationstage = stage,
            //bsr_declarationconfirmed = buildingProfessionApplicationWrapper.Stage is BuildingApplicationStage.ApplicationSubmitted or BuildingApplicationStage.PayAndApply,
        });
    }

    [Function(nameof(UpdateContact))]
    public Task UpdateContact([ActivityTrigger] ContactWrapper contactWrapper)
    {
        //var stage = buildingProfessionApplicationWrapper.DynamicsBuildingProfessionApplication.bsr_applicationstage == BuildingApplicationStage.ApplicationSubmitted ? BuildingApplicationStage.ApplicationSubmitted : buildingProfessionApplicationWrapper.Stage;
        return dynamicsService.UpdateContact(contactWrapper.DynamicsContact, new DynamicsContact
        {
            firstname = contactWrapper.Model.FirstName,
            lastname = contactWrapper.Model.LastName,
            emailaddress1 = contactWrapper.Model.Email,
            emailaddress2 = contactWrapper.Model.AlternativeEmail,
            telephone1 = contactWrapper.Model.PhoneNumber,
            business2 = contactWrapper.Model.AlternativePhoneNumber,
            address1_addresstypecode = 3,
            address1_line1 = contactWrapper.Model.Address.Address,
            address1_line2 = contactWrapper.Model.Address.AddressLineTwo,
            address1_city = contactWrapper.Model.Address.Town,
            address1_postalcode = contactWrapper.Model.Address.Postcode,
            bsr_address1uprn = contactWrapper.Model.Address.UPRN,
            bsr_address1usrn = contactWrapper.Model.Address.USRN,
            bsr_address1lacode = contactWrapper.Model.Address.CustodianCode , 
            bsr_address1ladescription = contactWrapper.Model.Address.CustodianDescription,
            birthdate = contactWrapper.Model.birthdate,
            bsr_nationalinsuranceno = contactWrapper.Model.NationalInsuranceNumber
        });
    }

    [Function(nameof(UpdateBuildingProfessionApplicationToSubmitted))]
    public Task UpdateBuildingProfessionApplicationToSubmitted([ActivityTrigger] DynamicsBuildingProfessionApplication buildingProfessionApplication)
    {
        return dynamicsService.UpdateBuildingProfessionApplication(buildingProfessionApplication, new DynamicsBuildingProfessionApplication
        {
/*            bsr_submittedon = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            bsr_applicationstage = BuildingApplicationStage.ApplicationSubmitted*/
        });
    }



    [Function(nameof(GetDynamicsPayments))]
    public Task<List<DynamicsPayment>> GetDynamicsPayments([ActivityTrigger] string applicationId)
    {
        return dynamicsService.GetPayments(applicationId);
    }

    [Function(nameof(CreateOrUpdatePayment))]
    public async Task CreateOrUpdatePayment([ActivityTrigger] BuildingProfessionApplicationPayment buildingProfessionApplicationPayment)
    {
        await dynamicsService.CreatePayment(buildingProfessionApplicationPayment);
    }

    [Function(nameof(GetPaymentStatus))]
    public async Task<PaymentResponseModel> GetPaymentStatus([ActivityTrigger] string paymentId)
    {
        PaymentApiResponseModel response;
        var retryCount = 0;

        do
        {
            response = await integrationOptions.PaymentEndpoint
                .AppendPathSegments("v1", "payments", paymentId)
                .WithOAuthBearerToken(integrationOptions.PaymentApiKey)
                .GetJsonAsync<PaymentApiResponseModel>();

            if (ShouldRetry(response.state.status, retryCount))
            {
                await Task.Delay(3000);
                retryCount++;
            }
        } while (ShouldRetry(response.state.status, retryCount));

        return mapper.Map<PaymentResponseModel>(response);
    }

    [Function(nameof(UpdateBuildingProfessionApplicationInCosmos))]
    [CosmosDBOutput("hseportal", "regulated_building_professions", Connection = "CosmosConnection")]
    public BuildingProfessionApplicationModel UpdateBuildingProfessionApplicationInCosmos([ActivityTrigger] BuildingProfessionApplicationModel buildingProfessionApplicationModel)
    {
        return buildingProfessionApplicationModel;
    }

    private bool ShouldRetry(string status, int retryCount)
    {
        if (status != "success" && status != "failed" && status != "cancelled" && status != "error")
        {
            return retryCount < 3;
        }

        return false;
    }
}