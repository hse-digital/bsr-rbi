using System.Globalization;
using System.Runtime.InteropServices;
using AutoMapper;
using Flurl;
using Flurl.Http;
using HSE.RP.API.Enums;
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

    [Function(nameof(SyncBuildingInspectorClass))]
    public async Task<HttpResponseData> SyncBuildingInspectorClass([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request, [DurableClient] DurableTaskClient durableTaskClient)
    {
        var buildingProfessionApplication = await request.ReadAsJsonAsync<BuildingProfessionApplicationModel>();
        await durableTaskClient.ScheduleNewOrchestrationInstanceAsync(nameof(SynchroniseBuildingInspectorClass), buildingProfessionApplication);
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
                    AlternativeEmail = buildingProfessionApplicationModel.PersonalDetails.ApplicantAlternativeEmail is null ? null : buildingProfessionApplicationModel.PersonalDetails.ApplicantAlternativeEmail.Email,
                    PhoneNumber = buildingProfessionApplicationModel.PersonalDetails.ApplicantPhone.PhoneNumber ?? null,
                    AlternativePhoneNumber = buildingProfessionApplicationModel.PersonalDetails.ApplicantAlternativePhone is null ? null : buildingProfessionApplicationModel.PersonalDetails.ApplicantAlternativePhone.PhoneNumber ?? "",
                    Address = buildingProfessionApplicationModel.PersonalDetails.ApplicantAddress is null ? new BuildingAddress { } : buildingProfessionApplicationModel.PersonalDetails.ApplicantAddress,
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

    [Function(nameof(SynchroniseBuildingInspectorClass))]
    public async Task SynchroniseBuildingInspectorClass([OrchestrationTrigger] TaskOrchestrationContext orchestrationContext)
    {
        var buildingProfessionApplicationModel = orchestrationContext.GetInput<BuildingProfessionApplicationModel>();

        var dynamicsBuildingProfessionApplication = await orchestrationContext.CallActivityAsync<DynamicsBuildingProfessionApplication>(nameof(GetBuildingProfessionApplicationUsingId), buildingProfessionApplicationModel.Id);

        if (dynamicsBuildingProfessionApplication != null)
        {
            //Update registration classes-------------
            //----------------------------------------

            var dynamicsRegistrationClasses = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorRegistrationClass>>(nameof(GetRegistrationClassesUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);

            //Check which registration class is selected in model
            var selectedRegistrationClassId = (int)buildingProfessionApplicationModel.InspectorClass.ClassType.Class;

            //If the selected registration class is not in the list of registration classes or is deactivated, then add/reactivate it
            if (!dynamicsRegistrationClasses.Any(x => x._bsr_biclassid_value == BuildingInspectorClassNames.Ids[selectedRegistrationClassId] && x.statecode!=1))
            {
                var registrationClass = new BuildingInspectorRegistrationClass
                {
                    BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                    ApplicantId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                    ClassId = BuildingInspectorClassNames.Ids[selectedRegistrationClassId],
                    StatusCode = (int)BuildingInspectorRegistrationClassStatus.Applied,
                    StateCode = 0
                };
                await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationClass), registrationClass);
            }

            //If the selected class has changed set its status to inactive 
            var classesToUpdate = dynamicsRegistrationClasses.Where(x => x._bsr_biclassid_value != BuildingInspectorClassNames.Ids[selectedRegistrationClassId] && x._bsr_biclassid_value != BuildingInspectorClassNames.Ids[4]).ToList();
            if (classesToUpdate.Any())
            {
                foreach (DynamicsBuildingInspectorRegistrationClass classToUpdate in classesToUpdate)
                {
                    var registrationClass = new BuildingInspectorRegistrationClass
                    {
                        Id = classToUpdate.bsr_biregclassid,
                        BuildingProfessionApplicationId = classToUpdate._bsr_biapplicationid_value,
                        ApplicantId = classToUpdate._bsr_buildinginspectorid_value,
                        ClassId = classToUpdate._bsr_biclassid_value,
                        StatusCode = 2,
                        StateCode = 1
                    };

                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationClass), registrationClass);
                }
            }

            //If user has also selected class 4 then create record for it
            if (buildingProfessionApplicationModel.InspectorClass.ClassTechnicalManager == "yes")
            {
                if (!dynamicsRegistrationClasses.Any(x => x._bsr_biclassid_value == BuildingInspectorClassNames.Ids[4] && x.statecode != 1))
                {
                    var registrationClass = new BuildingInspectorRegistrationClass
                    {
                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                        ApplicantId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                        ClassId = BuildingInspectorClassNames.Ids[4],
                        StatusCode = (int)BuildingInspectorRegistrationClassStatus.Applied,
                        StateCode = 0
                    };
                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationClass), registrationClass);
                }
            }
            //Check if previously existed and deactivate
            else
            {
                classesToUpdate = dynamicsRegistrationClasses.Where(x => x._bsr_biclassid_value == BuildingInspectorClassNames.Ids[4]).ToList();
                if (classesToUpdate.Any())
                {
                    foreach (DynamicsBuildingInspectorRegistrationClass classToUpdate in classesToUpdate)
                    {
                        var registrationClass = new BuildingInspectorRegistrationClass
                        {
                            Id = classToUpdate.bsr_biregclassid,
                            BuildingProfessionApplicationId = classToUpdate._bsr_biapplicationid_value,
                            ApplicantId = classToUpdate._bsr_buildinginspectorid_value,
                            ClassId = classToUpdate._bsr_biclassid_value,
                            StatusCode = 2,
                            StateCode = 1
                        };

                        await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationClass), registrationClass);
                    }
                }
            }

            //Update Registration Countries-------------
            //------------------------------------------

            var dynamicsRegistrationCountries = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorRegistrationCountry>>(nameof(GetRegistrationCountriesUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);

            //Get selected countries
            var selectedCountries = buildingProfessionApplicationModel.InspectorClass.InspectorCountryOfWork;

            //If the selected countries are not in the list of registration countries or is deactivated, then add/reactivate it
            if(buildingProfessionApplicationModel.InspectorClass.InspectorCountryOfWork.England == true)
            {
                //Check the list of registration countries to see if England is there
                if (!dynamicsRegistrationCountries.Any(x => x._bsr_countryid_value == BuildingInspectorCountryNames.Ids["England"] && x.statecode != 1))
                {
                    var registrationCountry = new BuildingInspectorRegistrationCountry
                    {
                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                        CountryID= BuildingInspectorCountryNames.Ids["England"],
                        StatusCode = 1,
                        StateCode = 0
                    };
                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationCountry), registrationCountry);
                }
            }
            else if(buildingProfessionApplicationModel.InspectorClass.InspectorCountryOfWork.England == false)
            {
                //Check the list of registration countries to see if England was previously selected
                if (dynamicsRegistrationCountries.Any(x => x._bsr_countryid_value == BuildingInspectorCountryNames.Ids["England"]))
                {
                    var buildingInspectorRegistrationCountriesToUpdate = dynamicsRegistrationCountries.Where(x => x._bsr_countryid_value == BuildingInspectorCountryNames.Ids["England"]).ToList();
                    foreach(DynamicsBuildingInspectorRegistrationCountry countryToUpdate in buildingInspectorRegistrationCountriesToUpdate)
                    {
                        var registrationCountry = new BuildingInspectorRegistrationCountry
                        {
                            Id = countryToUpdate.bsr_biregcountryid,
                            BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                            BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                            CountryID = BuildingInspectorCountryNames.Ids["England"],
                            StatusCode = 2,
                            StateCode = 1
                        };
                        await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationCountry), registrationCountry);
                    }
                }
            }
            if (buildingProfessionApplicationModel.InspectorClass.InspectorCountryOfWork.Wales == true)
            {
                //Check the list of registration countries to see if Wales is there
                if (!dynamicsRegistrationCountries.Any(x => x._bsr_countryid_value == BuildingInspectorCountryNames.Ids["Wales"] && x.statecode != 1))
                {
                    var registrationCountry = new BuildingInspectorRegistrationCountry
                    {
                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                        CountryID = BuildingInspectorCountryNames.Ids["Wales"],
                        StatusCode = 1,
                        StateCode = 0
                    };
                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationCountry), registrationCountry);
                }
            }
            else if (buildingProfessionApplicationModel.InspectorClass.InspectorCountryOfWork.Wales == false)
            {
                //Check the list of registration countries to see if Wales was previously selected
                if (dynamicsRegistrationCountries.Any(x => x._bsr_countryid_value == BuildingInspectorCountryNames.Ids["Wales"]))
                {
                    var buildingInspectorRegistrationCountriesToUpdate = dynamicsRegistrationCountries.Where(x => x._bsr_countryid_value == BuildingInspectorCountryNames.Ids["Wales"]).ToList();
                    foreach (DynamicsBuildingInspectorRegistrationCountry countryToUpdate in buildingInspectorRegistrationCountriesToUpdate)
                    {
                        var registrationCountry = new BuildingInspectorRegistrationCountry
                        {
                            Id = countryToUpdate.bsr_biregcountryid,
                            BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                            BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                            CountryID = BuildingInspectorCountryNames.Ids["Wales"],
                            StatusCode = 2,
                            StateCode = 1
                        };
                        await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationCountry), registrationCountry);
                    }
                }
            }

        }

    }


    [Function(nameof(CreateOrUpdateRegistrationClass))]
    public async Task CreateOrUpdateRegistrationClass([ActivityTrigger] BuildingInspectorRegistrationClass buildingInspectorRegistrationClass)
    {
        await dynamicsService.CreateOrUpdateRegistrationClass(buildingInspectorRegistrationClass);
    }

    [Function(nameof(CreateOrUpdateRegistrationCountry))]
    public async Task CreateOrUpdateRegistrationCountry([ActivityTrigger] BuildingInspectorRegistrationCountry buildingInspectorRegistrationcountry)
    {
        await dynamicsService.CreateOrUpdateRegistrationCountry(buildingInspectorRegistrationcountry);
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

    [Function(nameof(GetRegistrationClassesUsingApplicationId))]
    public Task<List<DynamicsBuildingInspectorRegistrationClass>> GetRegistrationClassesUsingApplicationId([ActivityTrigger] string applicationId)
    {
        return dynamicsService.GetRegistrationClassesUsingApplicationId(applicationId);
    }

    [Function(nameof(GetRegistrationCountriesUsingApplicationId))]
    public Task<List<DynamicsBuildingInspectorRegistrationCountry>> GetRegistrationCountriesUsingApplicationId([ActivityTrigger] string applicationId)
    {
        return dynamicsService.GetRegistrationCountriesUsingApplicationId(applicationId);
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
            bsr_address1lacode = contactWrapper.Model.Address.CustodianCode,
            bsr_address1ladescription = contactWrapper.Model.Address.CustodianDescription,
            bsr_manualaddress = contactWrapper.Model.Address.IsManual is null ? null :
                                contactWrapper.Model.Address.IsManual is true ? YesNoOption.Yes :
                                YesNoOption.No,
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