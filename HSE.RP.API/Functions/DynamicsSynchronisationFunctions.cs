using AutoMapper;
using Flurl;
using Flurl.Http;
using HSE.RP.API.Enums;
using HSE.RP.API.Extensions;
using HSE.RP.API.Mappers;
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

namespace HSE.RP.API.Functions;

public class DynamicsSynchronisationFunctions
{
    private readonly IDynamicsService dynamicsService;
    private readonly IMapper mapper;
    private readonly IntegrationsOptions integrationOptions;

    public DynamicsSynchronisationFunctions(IDynamicsService dynamicsService, IOptions<IntegrationsOptions> integrationOptions, IMapper mapper)
    {
        this.dynamicsService = dynamicsService;
        this.mapper = mapper;
        this.integrationOptions = integrationOptions.Value;
    }

    [Function(nameof(SyncApplicationStage))]
    public async Task<HttpResponseData> SyncApplicationStage([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request, EncodedRequest encodedRequest, [DurableClient] DurableTaskClient durableTaskClient)
    {
        //var buildingProfessionApplicationModel = await request.ReadAsJsonAsync<BuildingProfessionApplicationModel>();
        var buildingProfessionApplicationModel = encodedRequest.GetDecodedData<BuildingProfessionApplicationModel>();
        await durableTaskClient.ScheduleNewOrchestrationInstanceAsync(nameof(SynchroniseApplicationStage), buildingProfessionApplicationModel);
        return request.CreateResponse();
    }

    [Function(nameof(SyncEmploymentDetails))]
    public async Task<HttpResponseData> SyncEmploymentDetails([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request, EncodedRequest encodedRequest, [DurableClient] DurableTaskClient durableTaskClient)
    {
        //var buildingProfessionApplicationModel = await request.ReadAsJsonAsync<BuildingProfessionApplicationModel>();
        var buildingProfessionApplicationModel = encodedRequest.GetDecodedData<BuildingProfessionApplicationModel>();
        await durableTaskClient.ScheduleNewOrchestrationInstanceAsync(nameof(SynchroniseEmploymentDetails), buildingProfessionApplicationModel);
        return request.CreateResponse();
    }

    [Function(nameof(SyncFullApplication))]
    public async Task<HttpResponseData> SyncFullApplication([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request, EncodedRequest encodedRequest, [DurableClient] DurableTaskClient durableTaskClient)
    {
        //var buildingProfessionApplicationModel = await request.ReadAsJsonAsync<BuildingProfessionApplicationModel>();
        var buildingProfessionApplicationModel = encodedRequest.GetDecodedData<BuildingProfessionApplicationModel>();
        await durableTaskClient.ScheduleNewOrchestrationInstanceAsync(nameof(SynchroniseFullApplication), buildingProfessionApplicationModel);
        return request.CreateResponse();
    }

    [Function(nameof(SyncDeclaration))]
    public async Task<HttpResponseData> SyncDeclaration([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request, EncodedRequest encodedRequest, [DurableClient] DurableTaskClient durableTaskClient)
    {
        //var buildingProfessionApplicationModel = await request.ReadAsJsonAsync<BuildingProfessionApplicationModel>();
        var buildingProfessionApplicationModel = encodedRequest.GetDecodedData<BuildingProfessionApplicationModel>();
        await durableTaskClient.ScheduleNewOrchestrationInstanceAsync(nameof(SynchroniseDeclaration), buildingProfessionApplicationModel);

        return request.CreateResponse();
    }

    [Function(nameof(SyncPayment))]
    public async Task<HttpResponseData> SyncPayment([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request, EncodedRequest encodedRequest, [DurableClient] DurableTaskClient durableTaskClient)
    {
        //var buildingProfessionApplicationModel = await request.ReadAsJsonAsync<BuildingProfessionApplicationModel>();
        var buildingProfessionApplicationModel = encodedRequest.GetDecodedData<BuildingProfessionApplicationModel>();
        await durableTaskClient.ScheduleNewOrchestrationInstanceAsync(nameof(SynchronisePayment), buildingProfessionApplicationModel);

        return request.CreateResponse();
    }

    [Function(nameof(SyncPersonalDetails))]
    public async Task<HttpResponseData> SyncPersonalDetails([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request, EncodedRequest encodedRequest, [DurableClient] DurableTaskClient durableTaskClient)
    {
        //var buildingProfessionApplication = await request.ReadAsJsonAsync<BuildingProfessionApplicationModel>();
        var buildingProfessionApplication = encodedRequest.GetDecodedData<BuildingProfessionApplicationModel>();
        await durableTaskClient.ScheduleNewOrchestrationInstanceAsync(nameof(SynchronisePersonalDetails), buildingProfessionApplication);
        return request.CreateResponse();
    }

    [Function(nameof(SyncBuildingInspectorClass))]
    public async Task<HttpResponseData> SyncBuildingInspectorClass([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request, EncodedRequest encodedRequest, [DurableClient] DurableTaskClient durableTaskClient)
    {
        //var buildingProfessionApplication = await request.ReadAsJsonAsync<BuildingProfessionApplicationModel>();
        var buildingProfessionApplication = encodedRequest.GetDecodedData<BuildingProfessionApplicationModel>();
        await durableTaskClient.ScheduleNewOrchestrationInstanceAsync(nameof(SynchroniseBuildingInspectorClass), buildingProfessionApplication);
        return request.CreateResponse();
    }

    [Function(nameof(SyncCompetency))]
    public async Task<HttpResponseData> SyncCompetency([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request, EncodedRequest encodedRequest, [DurableClient] DurableTaskClient durableTaskClient)
    {
        //var buildingProfessionApplication = await request.ReadAsJsonAsync<BuildingProfessionApplicationModel>();
        var buildingProfessionApplication = encodedRequest.GetDecodedData<BuildingProfessionApplicationModel>();
        await durableTaskClient.ScheduleNewOrchestrationInstanceAsync(nameof(SynchroniseCompetency), buildingProfessionApplication);
        return request.CreateResponse();
    }

    [Function(nameof(SyncProfessionalBodyMemberships))]
    public async Task<HttpResponseData> SyncProfessionalBodyMemberships([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request, EncodedRequest encodedRequest, [DurableClient] DurableTaskClient durableTaskClient)
    {
        //var buildingProfessionApplication = await request.ReadAsJsonAsync<BuildingProfessionApplicationModel>();
        var buildingProfessionApplication = encodedRequest.GetDecodedData<BuildingProfessionApplicationModel>();
        await durableTaskClient.ScheduleNewOrchestrationInstanceAsync(nameof(SynchroniseProfessionalBodyMemberships), buildingProfessionApplication);
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
                    if (paymentResponse.Status == "success" && dynamicsBuildingProfessionApplication.statuscode != (int)BuildingProfessionApplicationStatus.Completed)
                    {
                        await orchestrationContext.CallActivityAsync(nameof(UpdateBuildingProfessionApplicationToSubmitted), dynamicsBuildingProfessionApplication);
                    }
                }

                return paymentResponse;
            }).ToArray();

            await Task.WhenAll(paymentSyncTasks);
        }
    }

    [Function(nameof(SynchroniseApplicationStage))]
    public async Task SynchroniseApplicationStage([OrchestrationTrigger] TaskOrchestrationContext orchestrationContext)
    {

        ApplicationStageMapper applicationStageMapper = new ApplicationStageMapper();

        var buildingProfessionApplicationModel = orchestrationContext.GetInput<BuildingProfessionApplicationModel>();

        var dynamicsBuildingProfessionApplication = await orchestrationContext.CallActivityAsync<DynamicsBuildingProfessionApplication>(nameof(GetBuildingProfessionApplicationUsingId), buildingProfessionApplicationModel.Id);

        BuildingProfessionApplicationStage? applicationStage = applicationStageMapper.ToBuildingApplicationStage(buildingProfessionApplicationModel.ApplicationStage);

        if(applicationStage != null)
        {
            await orchestrationContext.CallActivityAsync(nameof(UpdateBuildingInspectorApplicationStage), dynamicsBuildingProfessionApplication with { bsr_buildingprofessionalapplicationstage = applicationStage});
        }

    }




    [Function(nameof(SynchroniseEmploymentDetails))]
    public async Task SynchroniseEmploymentDetails([OrchestrationTrigger] TaskOrchestrationContext orchestrationContext)
    {
        var buildingProfessionApplicationModel = orchestrationContext.GetInput<BuildingProfessionApplicationModel>();

        var dynamicsBuildingProfessionApplication = await orchestrationContext.CallActivityAsync<DynamicsBuildingProfessionApplication>(nameof(GetBuildingProfessionApplicationUsingId), buildingProfessionApplicationModel.Id);

        var dynamicsEmploymentDetails = await orchestrationContext.CallActivityAsync<DynamicsBuildingInspectorEmploymentDetail>(nameof(GetEmploymentDetailsUsingId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);

        if (buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmploymentTypeSelection.EmploymentType == EmploymentType.Unemployed)
        {
            //IF unemployed delete employment details

            var employmentDetails = new BuildingInspectorEmploymentDetail
            {
                Id = dynamicsEmploymentDetails?.bsr_biemploymentdetailid ?? null,
                BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                EmployerIdAccount = null,
                EmployerIdContact = $"/contacts({dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid})",
                EmploymentTypeId = BuildingInspectorEmploymentTypeSelection.Ids[(int)buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmploymentTypeSelection.EmploymentType],
                IsCurrent = true,
                StatusCode = 1,
                StateCode = 0
            };

            //Create or update employment
            await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateBuildingInspectorEmploymentDetails), employmentDetails);

        }
        else
        {
            string employerIdContact = null;
            string employerIdAccount = null;
            //Lookup employer
            if ((buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmploymentTypeSelection.EmploymentType == EmploymentType.PublicSector
                || buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmploymentTypeSelection.EmploymentType == EmploymentType.PrivateSector
                || buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmploymentTypeSelection.EmploymentType == EmploymentType.Other
                && buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmployerName.OtherBusinessSelection == "yes"))
            {
                //Lookup employer relationship in accounts table
                var employerDetails = await orchestrationContext.CallActivityAsync<DynamicsAccount>(nameof(CreateOrUpdateEmployer), buildingProfessionApplicationModel);

                employerIdAccount = $"/accounts({employerDetails.accountid})";

            }
            else if (buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmploymentTypeSelection.EmploymentType == EmploymentType.Other
                && buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmployerName.OtherBusinessSelection == "no")
            {
                employerIdContact = $"/contacts({dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid})";

            }


            var employmentDetails = new BuildingInspectorEmploymentDetail
            {
                Id = dynamicsEmploymentDetails?.bsr_biemploymentdetailid ?? null,
                BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                EmployerIdAccount = employerIdAccount,
                EmployerIdContact = employerIdContact,
                EmploymentTypeId = BuildingInspectorEmploymentTypeSelection.Ids[(int)buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmploymentTypeSelection.EmploymentType],
                IsCurrent = true,
                StatusCode = 1,
                StateCode = 0
            };

            //Create or update employment
            await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateBuildingInspectorEmploymentDetails), employmentDetails);
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

    [Function(nameof(SynchroniseCompetency))]
    public async Task SynchroniseCompetency([OrchestrationTrigger] TaskOrchestrationContext orchestrationContext)
    {
        var buildingProfessionApplicationModel = orchestrationContext.GetInput<BuildingProfessionApplicationModel>();

        var dynamicsBuildingProfessionApplication = await orchestrationContext.CallActivityAsync<DynamicsBuildingProfessionApplication>(nameof(GetBuildingProfessionApplicationUsingId), buildingProfessionApplicationModel.Id);

        if (dynamicsBuildingProfessionApplication != null)
        {

            if (buildingProfessionApplicationModel.Competency.NoCompetencyAssessment.Declaration == true || buildingProfessionApplicationModel.Competency.CompetencyIndependentAssessmentStatus.IAStatus == "no")
            {

                await orchestrationContext.CallActivityAsync(nameof(UpdateBuildingProfessionApplicationCompetency), new BuildingProfessionApplicationWrapper(buildingProfessionApplicationModel, dynamicsBuildingProfessionApplication with
                {
                    bsr_hasindependentassessment = false,
                    bsr_assessmentorganisationid = null,
                    bsr_assessmentdate = null,
                    bsr_assessmentcertnumber = null
                }));


                //If no competency assessment is selected, then create new registration for class 1
                var dynamicsRegistrationClasses = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorRegistrationClass>>(nameof(GetRegistrationClassesUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);

                //Create new if doesnt exist
                if (!dynamicsRegistrationClasses.Any(x => x._bsr_biclassid_value == BuildingInspectorClassNames.Ids[1] && x.statecode != 1))
                {
                    var registrationClass = new BuildingInspectorRegistrationClass
                    {
                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                        ApplicantId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                        ClassId = BuildingInspectorClassNames.Ids[1],
                        StatusCode = (int)BuildingInspectorRegistrationClassStatus.Registered,
                        StateCode = 0
                    };
                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationClass), registrationClass);
                }

            }

            if (buildingProfessionApplicationModel.Competency.CompetencyIndependentAssessmentStatus.IAStatus == "yes")
            {
                await orchestrationContext.CallActivityAsync(nameof(UpdateBuildingProfessionApplicationCompetency), new BuildingProfessionApplicationWrapper(buildingProfessionApplicationModel, dynamicsBuildingProfessionApplication with
                {
                    bsr_hasindependentassessment = true,
                    bsr_assessmentorganisationid = buildingProfessionApplicationModel.Competency.CompetencyAssessmentOrganisation.ComAssessmentOrganisation is null ? null : $"accounts({AssessmentOrganisationNames.Ids[buildingProfessionApplicationModel.Competency.CompetencyAssessmentOrganisation.ComAssessmentOrganisation]})",
                    bsr_assessmentdate = buildingProfessionApplicationModel.Competency.CompetencyDateOfAssessment is null ? null :
                                        new DateOnly(int.Parse(buildingProfessionApplicationModel.Competency.CompetencyDateOfAssessment.Year),
                                                     int.Parse(buildingProfessionApplicationModel.Competency.CompetencyDateOfAssessment.Month),
                                                     int.Parse(buildingProfessionApplicationModel.Competency.CompetencyDateOfAssessment.Day)),
                    bsr_assessmentcertnumber = buildingProfessionApplicationModel.Competency.CompetencyAssessmentCertificateNumber is null ? null : buildingProfessionApplicationModel.Competency.CompetencyAssessmentCertificateNumber.CertificateNumber,
                }));
            }
        }



    }

    [Function(nameof(SynchroniseProfessionalBodyMemberships))]
    public async Task SynchroniseProfessionalBodyMemberships([OrchestrationTrigger] TaskOrchestrationContext orchestrationContext)
    {
        var buildingProfessionApplicationModel = orchestrationContext.GetInput<BuildingProfessionApplicationModel>();
        var dynamicsBuildingProfessionApplication = await orchestrationContext.CallActivityAsync<DynamicsBuildingProfessionApplication>(nameof(GetBuildingProfessionApplicationUsingId), buildingProfessionApplicationModel.Id);
        var dynamicsProfessionalMemberships = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorProfessionalBodyMembership>>(nameof(GetBuildingInspectorProfessionalBodyMembershipsUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);


        if (dynamicsBuildingProfessionApplication != null)
        {

            if (buildingProfessionApplicationModel.ProfessionalMemberships.IsProfessionBodyRelevantYesNo == "no")
            {
                if (dynamicsProfessionalMemberships != null)
                {
                    //Check for existing memberships and set to inactive
                    foreach (var membership in dynamicsProfessionalMemberships)
                    {

                        var professionalBody = new BuildingInspectorProfessionalBodyMembership
                        {
                            Id = membership.bsr_biprofessionalmembershipid,
                            StatusCode = 2,
                            StateCode = 1,
                            BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                            BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                            ProfessionalBodyId = membership._bsr_professionalbodyid_value,
                            MembershipNumber = null,
                            CurrentMembershipLevelOrType = null,
                            YearId = null
                        };
                        await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateBuildingInspectorProfessionalBodyMembership), professionalBody);
                    }
                }
            }
            else
            {

                //Check each body and update/create as required

                if (buildingProfessionApplicationModel.ProfessionalMemberships.CABE.CompletionState == ComponentCompletionState.Complete)
                {

                    var dynamicsBSRYear = await orchestrationContext.CallActivityAsync<DynamicsYear>(nameof(GetDynamicsYear), buildingProfessionApplicationModel.ProfessionalMemberships.CABE.MembershipYear.ToString());

                    var professionalBody = new BuildingInspectorProfessionalBodyMembership
                    {
                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                        ProfessionalBodyId = BuildingInspectorProfessionalBodyIds.Ids["CABE"],
                        MembershipNumber = buildingProfessionApplicationModel.ProfessionalMemberships.CABE.MembershipNumber,
                        CurrentMembershipLevelOrType = buildingProfessionApplicationModel.ProfessionalMemberships.CABE.MembershipLevel,
                        YearId = dynamicsBSRYear.bsr_yearid,
                        StatusCode = 1,
                        StateCode = 0
                    };
                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateBuildingInspectorProfessionalBodyMembership), professionalBody);
                }
                else if (buildingProfessionApplicationModel.ProfessionalMemberships.CABE.CompletionState != ComponentCompletionState.Complete)
                {
                    if (dynamicsProfessionalMemberships.Any(x => x._bsr_professionalbodyid_value == BuildingInspectorProfessionalBodyIds.Ids["CABE"]
                                                                && x._bsr_biapplicationid_value == dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid
                                                                && x.statuscode == 1))
                    {
                        var professionalBody = new BuildingInspectorProfessionalBodyMembership
                        {
                            BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                            BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                            ProfessionalBodyId = BuildingInspectorProfessionalBodyIds.Ids["CABE"],
                            MembershipNumber = null,
                            CurrentMembershipLevelOrType = null,
                            YearId = null,
                            StatusCode = 2,
                            StateCode = 1
                        };
                        await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateBuildingInspectorProfessionalBodyMembership), professionalBody);
                    }
                }

                if (buildingProfessionApplicationModel.ProfessionalMemberships.CIOB.CompletionState == ComponentCompletionState.Complete)
                {
                    var dynamicsBSRYear = await orchestrationContext.CallActivityAsync<DynamicsYear>(nameof(GetDynamicsYear), buildingProfessionApplicationModel.ProfessionalMemberships.CIOB.MembershipYear.ToString());

                    var professionalBody = new BuildingInspectorProfessionalBodyMembership
                    {
                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                        ProfessionalBodyId = BuildingInspectorProfessionalBodyIds.Ids["CIOB"],
                        MembershipNumber = buildingProfessionApplicationModel.ProfessionalMemberships.CIOB.MembershipNumber,
                        CurrentMembershipLevelOrType = buildingProfessionApplicationModel.ProfessionalMemberships.CIOB.MembershipLevel,
                        YearId = dynamicsBSRYear.bsr_yearid,
                        StatusCode = 1,
                        StateCode = 0
                    };
                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateBuildingInspectorProfessionalBodyMembership), professionalBody);
                }
                else if (buildingProfessionApplicationModel.ProfessionalMemberships.CIOB.CompletionState != ComponentCompletionState.Complete)
                {
                    if (dynamicsProfessionalMemberships.Any(x => x._bsr_professionalbodyid_value == BuildingInspectorProfessionalBodyIds.Ids["CIOB"]
                                            && x._bsr_biapplicationid_value == dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid
                                            && x.statuscode == 1))
                    {
                        var professionalBody = new BuildingInspectorProfessionalBodyMembership
                        {
                            BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                            BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                            ProfessionalBodyId = BuildingInspectorProfessionalBodyIds.Ids["CIOB"],
                            MembershipNumber = null,
                            CurrentMembershipLevelOrType = null,
                            YearId = null,
                            StatusCode = 2,
                            StateCode = 1
                        };
                        await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateBuildingInspectorProfessionalBodyMembership), professionalBody);
                    }
                }

                if (buildingProfessionApplicationModel.ProfessionalMemberships.RICS.CompletionState == ComponentCompletionState.Complete)
                {
                    var dynamicsBSRYear = await orchestrationContext.CallActivityAsync<DynamicsYear>(nameof(GetDynamicsYear), buildingProfessionApplicationModel.ProfessionalMemberships.RICS.MembershipYear.ToString());

                    var professionalBody = new BuildingInspectorProfessionalBodyMembership
                    {
                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                        ProfessionalBodyId = BuildingInspectorProfessionalBodyIds.Ids["RICS"],
                        MembershipNumber = buildingProfessionApplicationModel.ProfessionalMemberships.RICS.MembershipNumber,
                        CurrentMembershipLevelOrType = buildingProfessionApplicationModel.ProfessionalMemberships.RICS.MembershipLevel,
                        YearId = dynamicsBSRYear.bsr_yearid,
                        StatusCode = 1,
                        StateCode = 0
                    };
                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateBuildingInspectorProfessionalBodyMembership), professionalBody);
                }
                else if (buildingProfessionApplicationModel.ProfessionalMemberships.RICS.CompletionState != ComponentCompletionState.Complete)
                {
                    if (dynamicsProfessionalMemberships.Any(x => x._bsr_professionalbodyid_value == BuildingInspectorProfessionalBodyIds.Ids["RICS"]
                                            && x._bsr_biapplicationid_value == dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid
                                            && x.statuscode == 1))
                    {
                        var professionalBody = new BuildingInspectorProfessionalBodyMembership
                        {
                            BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                            BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                            ProfessionalBodyId = BuildingInspectorProfessionalBodyIds.Ids["RICS"],
                            MembershipNumber = null,
                            CurrentMembershipLevelOrType = null,
                            YearId = null,
                            StatusCode = 2,
                            StateCode = 1
                        };
                        await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateBuildingInspectorProfessionalBodyMembership), professionalBody);
                    }
                }

                if (buildingProfessionApplicationModel.ProfessionalMemberships.OTHER.CompletionState == ComponentCompletionState.Complete)
                {
                    var dynamicsBSRYear = await orchestrationContext.CallActivityAsync<DynamicsYear>(nameof(GetDynamicsYear), buildingProfessionApplicationModel.ProfessionalMemberships.OTHER.MembershipYear.ToString());

                    var professionalBody = new BuildingInspectorProfessionalBodyMembership
                    {
                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                        ProfessionalBodyId = BuildingInspectorProfessionalBodyIds.Ids["OTHER"],
                        MembershipNumber = buildingProfessionApplicationModel.ProfessionalMemberships.OTHER.MembershipNumber,
                        CurrentMembershipLevelOrType = buildingProfessionApplicationModel.ProfessionalMemberships.OTHER.MembershipLevel,
                        YearId = null,
                        StatusCode = 1,
                        StateCode = 0
                    };
                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateBuildingInspectorProfessionalBodyMembership), professionalBody);
                }
                else if (buildingProfessionApplicationModel.ProfessionalMemberships.OTHER.CompletionState != ComponentCompletionState.Complete)
                {
                    if (dynamicsProfessionalMemberships.Any(x => x._bsr_professionalbodyid_value == BuildingInspectorProfessionalBodyIds.Ids["OTHER"]
                                            && x._bsr_biapplicationid_value == dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid
                                            && x.statuscode == 1))
                    {
                        var professionalBody = new BuildingInspectorProfessionalBodyMembership
                        {
                            BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                            BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                            ProfessionalBodyId = BuildingInspectorProfessionalBodyIds.Ids["OTHER"],
                            MembershipNumber = null,
                            CurrentMembershipLevelOrType = null,
                            YearId = null,
                            StatusCode = 2,
                            StateCode = 1
                        };
                        await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateBuildingInspectorProfessionalBodyMembership), professionalBody);
                    }
                }


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

            //Set Building Profession Application (bsr_buildingprofessionapplication)	bsr_hasindependentassessment = no
            if (selectedRegistrationClassId == (int)BuildingInspectorClassType.Class1)
            {
                await orchestrationContext.CallActivityAsync(nameof(UpdateBuildingProfessionApplication), new BuildingProfessionApplicationWrapper(buildingProfessionApplicationModel, dynamicsBuildingProfessionApplication with
                {
                    bsr_hasindependentassessment = false
                })); ;
            }

            //If the selected registration class is not in the list of registration classes or is deactivated, then add/reactivate it
            if (!dynamicsRegistrationClasses.Any(x => x._bsr_biclassid_value == BuildingInspectorClassNames.Ids[selectedRegistrationClassId] && x.statecode != 1))
            {
                var registrationClass = new BuildingInspectorRegistrationClass
                {
                    BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                    ApplicantId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                    ClassId = BuildingInspectorClassNames.Ids[selectedRegistrationClassId],
                    StatusCode = selectedRegistrationClassId == (int)BuildingInspectorClassType.Class1 ? (int)BuildingInspectorRegistrationClassStatus.Registered : (int)BuildingInspectorRegistrationClassStatus.Applied,
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
            if (buildingProfessionApplicationModel.InspectorClass.InspectorCountryOfWork.England == true)
            {
                //Check the list of registration countries to see if England is there
                if (!dynamicsRegistrationCountries.Any(x => x._bsr_countryid_value == BuildingInspectorCountryNames.Ids["England"] && x.statecode != 1))
                {
                    var registrationCountry = new BuildingInspectorRegistrationCountry
                    {
                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                        CountryID = BuildingInspectorCountryNames.Ids["England"],
                        StatusCode = 1,
                        StateCode = 0
                    };
                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationCountry), registrationCountry);
                }
            }
            else if (buildingProfessionApplicationModel.InspectorClass.InspectorCountryOfWork.England == false)
            {
                //Check the list of registration countries to see if England was previously selected
                if (dynamicsRegistrationCountries.Any(x => x._bsr_countryid_value == BuildingInspectorCountryNames.Ids["England"]))
                {
                    var buildingInspectorRegistrationCountriesToUpdate = dynamicsRegistrationCountries.Where(x => x._bsr_countryid_value == BuildingInspectorCountryNames.Ids["England"]).ToList();
                    foreach (DynamicsBuildingInspectorRegistrationCountry countryToUpdate in buildingInspectorRegistrationCountriesToUpdate)
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


            //Update Registration Activities-------------
            //-------------------------------------------



            //Get selected activities
            var selectedActivities = buildingProfessionApplicationModel.InspectorClass.Activities;

            //If selected class is class 1 then no need to update activities, but we need to set any previously selected activities to inactive
            if (selectedRegistrationClassId == 1)
            {
                var dynamicsRegistrationActivities = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorRegistrationActivity>>(nameof(GetRegistrationActivitiesUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);
                var activityToUpdate = dynamicsRegistrationActivities.Where(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["AssessingPlans"] || x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["Inspect"]).ToList();

                foreach (var activity in activityToUpdate)
                {
                    var registrationActivity = new BuildingInspectorRegistrationActivity
                    {
                        Id = activity.bsr_biregactivityId,
                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                        BuildingCategoryId = activity._bsr_bibuildingcategoryid_value,
                        ActivityId = activity._bsr_biactivityid_value,
                        StatusCode = 2,
                        StateCode = 1
                    };
                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                }
            }
            else if (selectedRegistrationClassId == 2)
            {
                if (buildingProfessionApplicationModel.InspectorClass.Activities.AssessingPlans == true)
                {

                    foreach (var category in buildingProfessionApplicationModel.InspectorClass.AssessingPlansClass2.GetType().GetProperties().Where(x => x.Name != "CompletionState").ToList())

                    {
                        //Get all existing activities for application
                        var dynamicsRegistrationActivities = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorRegistrationActivity>>(nameof(GetRegistrationActivitiesUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);

                        var categoryName = category.Name + "Class2";

                        //get value of current category

                        //If false check if previously selected
                        if (category.GetValue(buildingProfessionApplicationModel.InspectorClass.AssessingPlansClass2) == null || (bool)category.GetValue(buildingProfessionApplicationModel.InspectorClass.AssessingPlansClass2) == false)
                        {
                            if (dynamicsRegistrationActivities.Any(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["AssessingPlans"]
                                && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]))
                            {

                                var buildingInspectorRegistrationActivitiesToUpdate = dynamicsRegistrationActivities.Where(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["AssessingPlans"]
                                                        && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName] && x.statecode != 1).ToList();

                                foreach (var activityToUpdate in buildingInspectorRegistrationActivitiesToUpdate)
                                {

                                    var registrationActivity = new BuildingInspectorRegistrationActivity
                                    {
                                        Id = activityToUpdate.bsr_biregactivityId,
                                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                                        BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName],
                                        ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                                        StatusCode = 2,
                                        StateCode = 1
                                    };
                                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                                }
                            }
                        }
                        else
                        {
                            if (!dynamicsRegistrationActivities.Any(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["AssessingPlans"]
                                && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]
                                && x.statecode != 1)
                             )
                            {
                                var registrationActivity = new BuildingInspectorRegistrationActivity
                                {
                                    BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                                    BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                                    BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName],
                                    ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                                    StatusCode = (int)BuildingInspectorRegistrationActivityStatus.Applied,
                                    StateCode = 0
                                };

                                await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                            }
                            else if (dynamicsRegistrationActivities.Any(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["AssessingPlans"]
                                && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]))
                            {

                                var activityToUpdate = dynamicsRegistrationActivities.FirstOrDefault(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["AssessingPlans"]
                                                        && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]);


                                var registrationActivity = new BuildingInspectorRegistrationActivity
                                {
                                    Id = activityToUpdate.bsr_biregactivityId,
                                    BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                                    BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                                    BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName],
                                    ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                                    StatusCode = (int)BuildingInspectorRegistrationActivityStatus.Applied,
                                    StateCode = 0
                                };
                                await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                            }

                        }

                    }

                }
                //Check for existing assessing plans in either category and set to inactive

                if (buildingProfessionApplicationModel.InspectorClass.Activities.Inspection == true)
                {

                    foreach (var category in buildingProfessionApplicationModel.InspectorClass.Class2InspectBuildingCategories.GetType().GetProperties().Where(x => x.Name != "CompletionState").ToList())
                    {
                        //Get all existing activities for application
                        var dynamicsRegistrationActivities = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorRegistrationActivity>>(nameof(GetRegistrationActivitiesUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);

                        var categoryName = category.Name + "Class2";

                        //get value of current category

                        //If false check if previously selected
                        if (category.GetValue(buildingProfessionApplicationModel.InspectorClass.Class2InspectBuildingCategories) == null || (bool)category.GetValue(buildingProfessionApplicationModel.InspectorClass.Class2InspectBuildingCategories) == false)
                        {
                            if (dynamicsRegistrationActivities.Any(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["Inspect"]
                                && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]))
                            {

                                var buildingInspectorRegistrationActivitiesToUpdate = dynamicsRegistrationActivities.Where(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["Inspect"]
                                                        && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName] && x.statecode != 1).ToList();

                                foreach (var activityToUpdate in buildingInspectorRegistrationActivitiesToUpdate)
                                {

                                    var registrationActivity = new BuildingInspectorRegistrationActivity
                                    {
                                        Id = activityToUpdate.bsr_biregactivityId,
                                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                                        BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName],
                                        ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                                        StatusCode = 2,
                                        StateCode = 1
                                    };
                                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                                }
                            }
                        }
                        else
                        {
                            if (!dynamicsRegistrationActivities.Any(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["Inspect"]
                                && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]
                                && x.statecode != 1)
                             )
                            {
                                var registrationActivity = new BuildingInspectorRegistrationActivity
                                {
                                    BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                                    BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                                    BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName],
                                    ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                                    StatusCode = (int)BuildingInspectorRegistrationActivityStatus.Applied,
                                    StateCode = 0
                                };

                                await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                            }
                            else if (dynamicsRegistrationActivities.Any(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["Inspect"]
                                && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]))
                            {

                                var activityToUpdate = dynamicsRegistrationActivities.FirstOrDefault(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["Inspect"]
                                                        && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]);


                                var registrationActivity = new BuildingInspectorRegistrationActivity
                                {
                                    Id = activityToUpdate.bsr_biregactivityId,
                                    BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                                    BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                                    BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName],
                                    ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                                    StatusCode = (int)BuildingInspectorRegistrationActivityStatus.Applied,
                                    StateCode = 0
                                };
                                await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                            }

                        }

                    }

                }
                //Check for existing assessing plans in either category and set to inactive


                //Set class 3 activities to inactive
                var class3DynamicsRegistrationActivities = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorRegistrationActivity>>(nameof(GetRegistrationActivitiesUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);

                var class3AssessCategories = buildingProfessionApplicationModel.InspectorClass.AssessingPlansClass3.GetType().GetProperties().Where(x => x.Name != "CompletionState").ToList();
                var class3InspectCategories = buildingProfessionApplicationModel.InspectorClass.Class3InspectBuildingCategories.GetType().GetProperties().Where(x => x.Name != "CompletionState").ToList();
                var class3Categories = class3AssessCategories.Concat(class3InspectCategories).ToList();
                foreach (var category in class3Categories)
                {
                    var activitiesToUpdate = class3DynamicsRegistrationActivities.Where(x => x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[category.Name + "Class3"]).ToList();
                    foreach (var activity in activitiesToUpdate)
                    {
                        var registrationActivity = new BuildingInspectorRegistrationActivity
                        {
                            Id = activity.bsr_biregactivityId,
                            BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                            BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                            BuildingCategoryId = activity._bsr_bibuildingcategoryid_value,
                            ActivityId = activity._bsr_biactivityid_value,
                            StatusCode = 2,
                            StateCode = 1
                        };
                        await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                    }
                }
            }
            else if (selectedRegistrationClassId == 3)
            {
                if (buildingProfessionApplicationModel.InspectorClass.Activities.AssessingPlans == true)
                {

                    foreach (var category in buildingProfessionApplicationModel.InspectorClass.AssessingPlansClass3.GetType().GetProperties().Where(x => x.Name != "CompletionState").ToList())

                    {
                        //Get all existing activities for application
                        var dynamicsRegistrationActivities = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorRegistrationActivity>>(nameof(GetRegistrationActivitiesUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);

                        var categoryName = category.Name + "Class3";

                        //get value of current category

                        //If false check if previously selected
                        if (category.GetValue(buildingProfessionApplicationModel.InspectorClass.AssessingPlansClass3) == null || (bool)category.GetValue(buildingProfessionApplicationModel.InspectorClass.AssessingPlansClass3) == false)
                        {
                            if (dynamicsRegistrationActivities.Any(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["AssessingPlans"]
                                && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]))
                            {

                                var buildingInspectorRegistrationActivitiesToUpdate = dynamicsRegistrationActivities.Where(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["AssessingPlans"]
                                                        && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName] && x.statecode != 1).ToList();

                                foreach (var activityToUpdate in buildingInspectorRegistrationActivitiesToUpdate)
                                {

                                    var registrationActivity = new BuildingInspectorRegistrationActivity
                                    {
                                        Id = activityToUpdate.bsr_biregactivityId,
                                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                                        BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName],
                                        ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                                        StatusCode = 2,
                                        StateCode = 1
                                    };
                                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                                }
                            }
                        }
                        else
                        {
                            if (!dynamicsRegistrationActivities.Any(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["AssessingPlans"]
                                && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]
                                && x.statecode != 1)
                             )
                            {
                                var registrationActivity = new BuildingInspectorRegistrationActivity
                                {
                                    BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                                    BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                                    BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName],
                                    ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                                    StatusCode = (int)BuildingInspectorRegistrationActivityStatus.Applied,
                                    StateCode = 0
                                };

                                await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                            }
                            else if (dynamicsRegistrationActivities.Any(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["AssessingPlans"]
                                && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]))
                            {

                                var activityToUpdate = dynamicsRegistrationActivities.FirstOrDefault(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["AssessingPlans"]
                                                        && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]);


                                var registrationActivity = new BuildingInspectorRegistrationActivity
                                {
                                    Id = activityToUpdate.bsr_biregactivityId,
                                    BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                                    BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                                    BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName],
                                    ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                                    StatusCode = (int)BuildingInspectorRegistrationActivityStatus.Applied,
                                    StateCode = 0
                                };
                                await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                            }

                        }

                    }

                }
                //Check for existing assessing plans in either category and set to inactive

                if (buildingProfessionApplicationModel.InspectorClass.Activities.Inspection == true)
                {


                    foreach (var category in buildingProfessionApplicationModel.InspectorClass.Class3InspectBuildingCategories.GetType().GetProperties().Where(x => x.Name != "CompletionState").ToList())
                    {
                        //Get all existing activities for application
                        var dynamicsRegistrationActivities = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorRegistrationActivity>>(nameof(GetRegistrationActivitiesUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);

                        var categoryName = category.Name + "Class3";

                        //get value of current category

                        //If false check if previously selected
                        if (category.GetValue(buildingProfessionApplicationModel.InspectorClass.Class3InspectBuildingCategories) == null || (bool)category.GetValue(buildingProfessionApplicationModel.InspectorClass.Class3InspectBuildingCategories) == false)
                        {
                            if (dynamicsRegistrationActivities.Any(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["Inspect"]
                                && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]))
                            {

                                var buildingInspectorRegistrationActivitiesToUpdate = dynamicsRegistrationActivities.Where(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["Inspect"]
                                                        && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName] && x.statecode != 1).ToList();

                                foreach (var activityToUpdate in buildingInspectorRegistrationActivitiesToUpdate)
                                {

                                    var registrationActivity = new BuildingInspectorRegistrationActivity
                                    {
                                        Id = activityToUpdate.bsr_biregactivityId,
                                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                                        BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName],
                                        ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                                        StatusCode = 2,
                                        StateCode = 1
                                    };
                                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                                }
                            }
                        }
                        else
                        {
                            if (!dynamicsRegistrationActivities.Any(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["Inspect"]
                                && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]
                                && x.statecode != 1)
                             )
                            {
                                var registrationActivity = new BuildingInspectorRegistrationActivity
                                {
                                    BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                                    BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                                    BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName],
                                    ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                                    StatusCode = (int)BuildingInspectorRegistrationActivityStatus.Applied,
                                    StateCode = 0
                                };

                                await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                            }
                            else if (dynamicsRegistrationActivities.Any(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["Inspect"]
                                && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]))
                            {

                                var activityToUpdate = dynamicsRegistrationActivities.FirstOrDefault(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["Inspect"]
                                                        && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]);


                                var registrationActivity = new BuildingInspectorRegistrationActivity
                                {
                                    Id = activityToUpdate.bsr_biregactivityId,
                                    BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                                    BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                                    BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName],
                                    ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                                    StatusCode = (int)BuildingInspectorRegistrationActivityStatus.Applied,
                                    StateCode = 0
                                };
                                await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                            }

                        }

                    }

                }
                //Check for existing assessing plans in either category and set to inactive


                //Set class 2 activities to inactive
                var class2ynamicsRegistrationActivities = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorRegistrationActivity>>(nameof(GetRegistrationActivitiesUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);

                var class2AssessCategories = buildingProfessionApplicationModel.InspectorClass.AssessingPlansClass2.GetType().GetProperties().Where(x => x.Name != "CompletionState").ToList();
                var class2InspectCategories = buildingProfessionApplicationModel.InspectorClass.Class2InspectBuildingCategories.GetType().GetProperties().Where(x => x.Name != "CompletionState").ToList();
                var class2Categories = class2AssessCategories.Concat(class2InspectCategories).ToList();
                foreach (var category in class2Categories)
                {
                    var activitiesToUpdate = class2ynamicsRegistrationActivities.Where(x => x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[category.Name + "Class2"]).ToList();
                    foreach (var activity in activitiesToUpdate)
                    {
                        var registrationActivity = new BuildingInspectorRegistrationActivity
                        {
                            Id = activity.bsr_biregactivityId,
                            BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                            BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                            BuildingCategoryId = activity._bsr_bibuildingcategoryid_value,
                            ActivityId = activity._bsr_biactivityid_value,
                            StatusCode = 2,
                            StateCode = 1
                        };
                        await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                    }
                }

            }

            if (buildingProfessionApplicationModel.InspectorClass.Activities.AssessingPlans == false)
            {
                var dynamicsRegistrationActivities = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorRegistrationActivity>>(nameof(GetRegistrationActivitiesUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);

                var activityToUpdate = dynamicsRegistrationActivities.Where(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["AssessingPlans"]).ToList();

                foreach (var activity in activityToUpdate)
                {
                    var registrationActivity = new BuildingInspectorRegistrationActivity
                    {
                        Id = activity.bsr_biregactivityId,
                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                        BuildingCategoryId = activity._bsr_bibuildingcategoryid_value,
                        ActivityId = activity._bsr_biactivityid_value,
                        StatusCode = 2,
                        StateCode = 1
                    };
                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                }

            }
            if (buildingProfessionApplicationModel.InspectorClass.Activities.Inspection == false)
            {
                var dynamicsRegistrationActivities = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorRegistrationActivity>>(nameof(GetRegistrationActivitiesUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);

                var activityToUpdate = dynamicsRegistrationActivities.Where(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["Inspect"]).ToList();

                foreach (var activity in activityToUpdate)
                {
                    var registrationActivity = new BuildingInspectorRegistrationActivity
                    {
                        Id = activity.bsr_biregactivityId,
                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                        BuildingCategoryId = activity._bsr_bibuildingcategoryid_value,
                        ActivityId = activity._bsr_biactivityid_value,
                        StatusCode = 2,
                        StateCode = 1
                    };
                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
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
    public async Task CreateOrUpdateRegistrationCountry([ActivityTrigger] BuildingInspectorRegistrationCountry buildingInspectorRegistrationCountry)
    {
        await dynamicsService.CreateOrUpdateRegistrationCountry(buildingInspectorRegistrationCountry);
    }

    [Function(nameof(CreateOrUpdateRegistrationActivity))]
    public async Task CreateOrUpdateRegistrationActivity([ActivityTrigger] BuildingInspectorRegistrationActivity buildingInspectorRegistrationActivity)
    {
        await dynamicsService.CreateOrUpdateRegistrationActivity(buildingInspectorRegistrationActivity);
    }

    [Function(nameof(CreateOrUpdateBuildingInspectorProfessionalBodyMembership))]
    public async Task CreateOrUpdateBuildingInspectorProfessionalBodyMembership([ActivityTrigger] BuildingInspectorProfessionalBodyMembership buildingInspectorProfessionalBodyMembership)
    {
        await dynamicsService.CreateOrUpdateBuildingInspectorProfessionalBodyMembership(buildingInspectorProfessionalBodyMembership);
    }

    [Function(nameof(CreateOrUpdateBuildingInspectorEmploymentDetails))]
    public async Task CreateOrUpdateBuildingInspectorEmploymentDetails([ActivityTrigger] BuildingInspectorEmploymentDetail buildingInspectorEmploymentDetail)
    {
        await dynamicsService.CreateOrUpdateBuildingInspectorEmploymentDetails(buildingInspectorEmploymentDetail);
    }

    [Function(nameof(GetBuildingProfessionApplicationUsingId))]
    public Task<DynamicsBuildingProfessionApplication> GetBuildingProfessionApplicationUsingId([ActivityTrigger] string applicationId)
    {
        return dynamicsService.GetBuildingProfessionApplicationUsingId(applicationId);
    }

    [Function(nameof(GetEmploymentDetailsUsingId))]
    public Task<DynamicsBuildingInspectorEmploymentDetail> GetEmploymentDetailsUsingId([ActivityTrigger] string applicationId)
    {
        return dynamicsService.GetEmploymentDetailsUsingId(applicationId);
    }



    [Function(nameof(GetContactUsingId))]
    public Task<DynamicsContact> GetContactUsingId([ActivityTrigger] string contactId)
    {
        return dynamicsService.GetContactUsingId(contactId);
    }

    [Function(nameof(CreateOrUpdateEmployer))]
    public Task<DynamicsAccount> CreateOrUpdateEmployer([ActivityTrigger] BuildingProfessionApplicationModel buildingProfessionApplicationModel)
    {
        return dynamicsService.CreateOrUpdateEmployer(buildingProfessionApplicationModel);
    }

    [Function(nameof(GetRegistrationClassesUsingApplicationId))]
    public Task<List<DynamicsBuildingInspectorRegistrationClass>> GetRegistrationClassesUsingApplicationId([ActivityTrigger] string applicationId)
    {
        return dynamicsService.GetRegistrationClassesUsingApplicationId(applicationId);
    }

    [Function(nameof(GetBuildingInspectorProfessionalBodyMembershipsUsingApplicationId))]
    public Task<List<DynamicsBuildingInspectorProfessionalBodyMembership>> GetBuildingInspectorProfessionalBodyMembershipsUsingApplicationId([ActivityTrigger] string applicationId)
    {
        return dynamicsService.GetBuildingInspectorProfessionalBodyMembershipsUsingApplicationId(applicationId);
    }

    [Function(nameof(GetDynamicsYear))]
    public Task<DynamicsYear> GetDynamicsYear([ActivityTrigger] string Year)
    {
        return dynamicsService.GetDynamicsYear(Year);
    }

    [Function(nameof(GetRegistrationCountriesUsingApplicationId))]
    public Task<List<DynamicsBuildingInspectorRegistrationCountry>> GetRegistrationCountriesUsingApplicationId([ActivityTrigger] string applicationId)
    {
        return dynamicsService.GetRegistrationCountriesUsingApplicationId(applicationId);
    }

    [Function(nameof(GetRegistrationActivitiesUsingApplicationId))]
    public Task<List<DynamicsBuildingInspectorRegistrationActivity>> GetRegistrationActivitiesUsingApplicationId([ActivityTrigger] string applicationId)
    {
        return dynamicsService.GetRegistrationActivitiesUsingApplicationId(applicationId);
    }

    [Function(nameof(UpdateBuildingProfessionApplication))]
    public Task UpdateBuildingProfessionApplication([ActivityTrigger] BuildingProfessionApplicationWrapper buildingProfessionApplicationWrapper)
    {
        //var stage = buildingProfessionApplicationWrapper.DynamicsBuildingProfessionApplication.bsr_applicationstage == BuildingApplicationStage.ApplicationSubmitted ? BuildingApplicationStage.ApplicationSubmitted : buildingProfessionApplicationWrapper.Stage;
        return dynamicsService.UpdateBuildingProfessionApplication(buildingProfessionApplicationWrapper.DynamicsBuildingProfessionApplication, new DynamicsBuildingProfessionApplication
        {
        });
    }

    [Function(nameof(UpdateBuildingProfessionApplicationCompetency))]
    public Task UpdateBuildingProfessionApplicationCompetency([ActivityTrigger] BuildingProfessionApplicationWrapper buildingProfessionApplicationWrapper)
    {
        //var stage = buildingProfessionApplicationWrapper.DynamicsBuildingProfessionApplication.bsr_applicationstage == BuildingApplicationStage.ApplicationSubmitted ? BuildingApplicationStage.ApplicationSubmitted : buildingProfessionApplicationWrapper.Stage;
        return dynamicsService.UpdateBuildingProfessionApplicationCompetency(buildingProfessionApplicationWrapper.DynamicsBuildingProfessionApplication, new DynamicsBuildingProfessionApplication
        {
            bsr_hasindependentassessment = buildingProfessionApplicationWrapper.DynamicsBuildingProfessionApplication.bsr_hasindependentassessment,
            bsr_assessmentorganisationid = buildingProfessionApplicationWrapper.DynamicsBuildingProfessionApplication.bsr_assessmentorganisationid,
            bsr_assessmentdate = buildingProfessionApplicationWrapper.DynamicsBuildingProfessionApplication.bsr_assessmentdate,
            bsr_assessmentcertnumber = buildingProfessionApplicationWrapper.DynamicsBuildingProfessionApplication.bsr_assessmentcertnumber
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
            statuscode = (int)BuildingProfessionApplicationStatus.Submitted
        });
    }

    [Function(nameof(UpdateBuildingInspectorApplicationStage))]
    public Task UpdateBuildingInspectorApplicationStage([ActivityTrigger] DynamicsBuildingProfessionApplication buildingProfessionApplication)
    {
        return dynamicsService.UpdateBuildingProfessionApplication(buildingProfessionApplication, new DynamicsBuildingProfessionApplication
        {
            bsr_buildingprofessionalapplicationstage = buildingProfessionApplication.bsr_buildingprofessionalapplicationstage
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

    [Function(nameof(SynchroniseFullApplication))]
    public async Task SynchroniseFullApplication([OrchestrationTrigger] TaskOrchestrationContext orchestrationContext)
    {
        //This can probably be handled better but it's a quick fix for now

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


            //Update registration classes-------------
            //----------------------------------------

            var dynamicsRegistrationClasses = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorRegistrationClass>>(nameof(GetRegistrationClassesUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);

            //Check which registration class is selected in model
            var selectedRegistrationClassId = (int)buildingProfessionApplicationModel.InspectorClass.ClassType.Class;

            //Set Building Profession Application (bsr_buildingprofessionapplication)	bsr_hasindependentassessment = no
            if (selectedRegistrationClassId == (int)BuildingInspectorClassType.Class1)
            {
                await orchestrationContext.CallActivityAsync(nameof(UpdateBuildingProfessionApplication), new BuildingProfessionApplicationWrapper(buildingProfessionApplicationModel, dynamicsBuildingProfessionApplication with
                {
                    bsr_hasindependentassessment = false
                })); ;
            }

            //If the selected registration class is not in the list of registration classes or is deactivated, then add/reactivate it
            if (!dynamicsRegistrationClasses.Any(x => x._bsr_biclassid_value == BuildingInspectorClassNames.Ids[selectedRegistrationClassId] && x.statecode != 1))
            {
                var registrationClass = new BuildingInspectorRegistrationClass
                {
                    BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                    ApplicantId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                    ClassId = BuildingInspectorClassNames.Ids[selectedRegistrationClassId],
                    StatusCode = selectedRegistrationClassId == (int)BuildingInspectorClassType.Class1 ? (int)BuildingInspectorRegistrationClassStatus.Registered : (int)BuildingInspectorRegistrationClassStatus.Applied,
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
            if (buildingProfessionApplicationModel.InspectorClass.InspectorCountryOfWork.England == true)
            {
                //Check the list of registration countries to see if England is there
                if (!dynamicsRegistrationCountries.Any(x => x._bsr_countryid_value == BuildingInspectorCountryNames.Ids["England"] && x.statecode != 1))
                {
                    var registrationCountry = new BuildingInspectorRegistrationCountry
                    {
                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                        CountryID = BuildingInspectorCountryNames.Ids["England"],
                        StatusCode = 1,
                        StateCode = 0
                    };
                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationCountry), registrationCountry);
                }
            }
            else if (buildingProfessionApplicationModel.InspectorClass.InspectorCountryOfWork.England == false)
            {
                //Check the list of registration countries to see if England was previously selected
                if (dynamicsRegistrationCountries.Any(x => x._bsr_countryid_value == BuildingInspectorCountryNames.Ids["England"]))
                {
                    var buildingInspectorRegistrationCountriesToUpdate = dynamicsRegistrationCountries.Where(x => x._bsr_countryid_value == BuildingInspectorCountryNames.Ids["England"]).ToList();
                    foreach (DynamicsBuildingInspectorRegistrationCountry countryToUpdate in buildingInspectorRegistrationCountriesToUpdate)
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


            //Update Registration Activities-------------
            //-------------------------------------------



            //Get selected activities
            var selectedActivities = buildingProfessionApplicationModel.InspectorClass.Activities;

            //If selected class is class 1 then no need to update activities, but we need to set any previously selected activities to inactive
            if (selectedRegistrationClassId == 1)
            {
                var dynamicsRegistrationActivities = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorRegistrationActivity>>(nameof(GetRegistrationActivitiesUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);
                var activityToUpdate = dynamicsRegistrationActivities.Where(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["AssessingPlans"] || x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["Inspect"]).ToList();

                foreach (var activity in activityToUpdate)
                {
                    var registrationActivity = new BuildingInspectorRegistrationActivity
                    {
                        Id = activity.bsr_biregactivityId,
                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                        BuildingCategoryId = activity._bsr_bibuildingcategoryid_value,
                        ActivityId = activity._bsr_biactivityid_value,
                        StatusCode = 2,
                        StateCode = 1
                    };
                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                }
            }
            else if (selectedRegistrationClassId == 2)
            {
                if (buildingProfessionApplicationModel.InspectorClass.Activities.AssessingPlans == true)
                {

                    foreach (var category in buildingProfessionApplicationModel.InspectorClass.AssessingPlansClass2.GetType().GetProperties().Where(x => x.Name != "CompletionState").ToList())

                    {
                        //Get all existing activities for application
                        var dynamicsRegistrationActivities = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorRegistrationActivity>>(nameof(GetRegistrationActivitiesUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);

                        var categoryName = category.Name + "Class2";

                        //get value of current category

                        //If false check if previously selected
                        if (category.GetValue(buildingProfessionApplicationModel.InspectorClass.AssessingPlansClass2) == null || (bool)category.GetValue(buildingProfessionApplicationModel.InspectorClass.AssessingPlansClass2) == false)
                        {
                            if (dynamicsRegistrationActivities.Any(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["AssessingPlans"]
                                && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]))
                            {

                                var buildingInspectorRegistrationActivitiesToUpdate = dynamicsRegistrationActivities.Where(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["AssessingPlans"]
                                                        && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName] && x.statecode != 1).ToList();

                                foreach (var activityToUpdate in buildingInspectorRegistrationActivitiesToUpdate)
                                {

                                    var registrationActivity = new BuildingInspectorRegistrationActivity
                                    {
                                        Id = activityToUpdate.bsr_biregactivityId,
                                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                                        BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName],
                                        ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                                        StatusCode = 2,
                                        StateCode = 1
                                    };
                                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                                }
                            }
                        }
                        else
                        {
                            if (!dynamicsRegistrationActivities.Any(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["AssessingPlans"]
                                && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]
                                && x.statecode != 1)
                             )
                            {
                                var registrationActivity = new BuildingInspectorRegistrationActivity
                                {
                                    BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                                    BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                                    BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName],
                                    ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                                    StatusCode = (int)BuildingInspectorRegistrationActivityStatus.Applied,
                                    StateCode = 0
                                };

                                await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                            }
                            else if (dynamicsRegistrationActivities.Any(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["AssessingPlans"]
                                && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]))
                            {

                                var activityToUpdate = dynamicsRegistrationActivities.FirstOrDefault(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["AssessingPlans"]
                                                        && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]);


                                var registrationActivity = new BuildingInspectorRegistrationActivity
                                {
                                    Id = activityToUpdate.bsr_biregactivityId,
                                    BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                                    BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                                    BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName],
                                    ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                                    StatusCode = (int)BuildingInspectorRegistrationActivityStatus.Applied,
                                    StateCode = 0
                                };
                                await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                            }

                        }

                    }

                }
                //Check for existing assessing plans in either category and set to inactive

                if (buildingProfessionApplicationModel.InspectorClass.Activities.Inspection == true)
                {

                    foreach (var category in buildingProfessionApplicationModel.InspectorClass.Class2InspectBuildingCategories.GetType().GetProperties().Where(x => x.Name != "CompletionState").ToList())
                    {
                        //Get all existing activities for application
                        var dynamicsRegistrationActivities = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorRegistrationActivity>>(nameof(GetRegistrationActivitiesUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);

                        var categoryName = category.Name + "Class2";

                        //get value of current category

                        //If false check if previously selected
                        if (category.GetValue(buildingProfessionApplicationModel.InspectorClass.Class2InspectBuildingCategories) == null || (bool)category.GetValue(buildingProfessionApplicationModel.InspectorClass.Class2InspectBuildingCategories) == false)
                        {
                            if (dynamicsRegistrationActivities.Any(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["Inspect"]
                                && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]))
                            {

                                var buildingInspectorRegistrationActivitiesToUpdate = dynamicsRegistrationActivities.Where(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["Inspect"]
                                                        && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName] && x.statecode != 1).ToList();

                                foreach (var activityToUpdate in buildingInspectorRegistrationActivitiesToUpdate)
                                {

                                    var registrationActivity = new BuildingInspectorRegistrationActivity
                                    {
                                        Id = activityToUpdate.bsr_biregactivityId,
                                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                                        BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName],
                                        ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                                        StatusCode = 2,
                                        StateCode = 1
                                    };
                                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                                }
                            }
                        }
                        else
                        {
                            if (!dynamicsRegistrationActivities.Any(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["Inspect"]
                                && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]
                                && x.statecode != 1)
                             )
                            {
                                var registrationActivity = new BuildingInspectorRegistrationActivity
                                {
                                    BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                                    BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                                    BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName],
                                    ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                                    StatusCode = (int)BuildingInspectorRegistrationActivityStatus.Applied,
                                    StateCode = 0
                                };

                                await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                            }
                            else if (dynamicsRegistrationActivities.Any(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["Inspect"]
                                && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]))
                            {

                                var activityToUpdate = dynamicsRegistrationActivities.FirstOrDefault(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["Inspect"]
                                                        && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]);


                                var registrationActivity = new BuildingInspectorRegistrationActivity
                                {
                                    Id = activityToUpdate.bsr_biregactivityId,
                                    BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                                    BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                                    BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName],
                                    ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                                    StatusCode = (int)BuildingInspectorRegistrationActivityStatus.Applied,
                                    StateCode = 0
                                };
                                await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                            }

                        }

                    }

                }
                //Check for existing assessing plans in either category and set to inactive


                //Set class 3 activities to inactive
                var class3DynamicsRegistrationActivities = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorRegistrationActivity>>(nameof(GetRegistrationActivitiesUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);

                var class3AssessCategories = buildingProfessionApplicationModel.InspectorClass.AssessingPlansClass3.GetType().GetProperties().Where(x => x.Name != "CompletionState").ToList();
                var class3InspectCategories = buildingProfessionApplicationModel.InspectorClass.Class3InspectBuildingCategories.GetType().GetProperties().Where(x => x.Name != "CompletionState").ToList();
                var class3Categories = class3AssessCategories.Concat(class3InspectCategories).ToList();
                foreach (var category in class3Categories)
                {
                    var activitiesToUpdate = class3DynamicsRegistrationActivities.Where(x => x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[category.Name + "Class3"]).ToList();
                    foreach (var activity in activitiesToUpdate)
                    {
                        var registrationActivity = new BuildingInspectorRegistrationActivity
                        {
                            Id = activity.bsr_biregactivityId,
                            BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                            BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                            BuildingCategoryId = activity._bsr_bibuildingcategoryid_value,
                            ActivityId = activity._bsr_biactivityid_value,
                            StatusCode = 2,
                            StateCode = 1
                        };
                        await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                    }
                }
            }
            else if (selectedRegistrationClassId == 3)
            {
                if (buildingProfessionApplicationModel.InspectorClass.Activities.AssessingPlans == true)
                {

                    foreach (var category in buildingProfessionApplicationModel.InspectorClass.AssessingPlansClass3.GetType().GetProperties().Where(x => x.Name != "CompletionState").ToList())

                    {
                        //Get all existing activities for application
                        var dynamicsRegistrationActivities = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorRegistrationActivity>>(nameof(GetRegistrationActivitiesUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);

                        var categoryName = category.Name + "Class3";

                        //get value of current category

                        //If false check if previously selected
                        if (category.GetValue(buildingProfessionApplicationModel.InspectorClass.AssessingPlansClass3) == null || (bool)category.GetValue(buildingProfessionApplicationModel.InspectorClass.AssessingPlansClass3) == false)
                        {
                            if (dynamicsRegistrationActivities.Any(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["AssessingPlans"]
                                && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]))
                            {

                                var buildingInspectorRegistrationActivitiesToUpdate = dynamicsRegistrationActivities.Where(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["AssessingPlans"]
                                                        && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName] && x.statecode != 1).ToList();

                                foreach (var activityToUpdate in buildingInspectorRegistrationActivitiesToUpdate)
                                {

                                    var registrationActivity = new BuildingInspectorRegistrationActivity
                                    {
                                        Id = activityToUpdate.bsr_biregactivityId,
                                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                                        BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName],
                                        ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                                        StatusCode = 2,
                                        StateCode = 1
                                    };
                                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                                }
                            }
                        }
                        else
                        {
                            if (!dynamicsRegistrationActivities.Any(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["AssessingPlans"]
                                && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]
                                && x.statecode != 1)
                             )
                            {
                                var registrationActivity = new BuildingInspectorRegistrationActivity
                                {
                                    BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                                    BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                                    BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName],
                                    ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                                    StatusCode = (int)BuildingInspectorRegistrationActivityStatus.Applied,
                                    StateCode = 0
                                };

                                await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                            }
                            else if (dynamicsRegistrationActivities.Any(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["AssessingPlans"]
                                && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]))
                            {

                                var activityToUpdate = dynamicsRegistrationActivities.FirstOrDefault(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["AssessingPlans"]
                                                        && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]);


                                var registrationActivity = new BuildingInspectorRegistrationActivity
                                {
                                    Id = activityToUpdate.bsr_biregactivityId,
                                    BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                                    BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                                    BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName],
                                    ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                                    StatusCode = (int)BuildingInspectorRegistrationActivityStatus.Applied,
                                    StateCode = 0
                                };
                                await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                            }

                        }

                    }

                }
                //Check for existing assessing plans in either category and set to inactive

                if (buildingProfessionApplicationModel.InspectorClass.Activities.Inspection == true)
                {


                    foreach (var category in buildingProfessionApplicationModel.InspectorClass.Class3InspectBuildingCategories.GetType().GetProperties().Where(x => x.Name != "CompletionState").ToList())
                    {
                        //Get all existing activities for application
                        var dynamicsRegistrationActivities = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorRegistrationActivity>>(nameof(GetRegistrationActivitiesUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);

                        var categoryName = category.Name + "Class3";

                        //get value of current category

                        //If false check if previously selected
                        if (category.GetValue(buildingProfessionApplicationModel.InspectorClass.Class3InspectBuildingCategories) == null || (bool)category.GetValue(buildingProfessionApplicationModel.InspectorClass.Class3InspectBuildingCategories) == false)
                        {
                            if (dynamicsRegistrationActivities.Any(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["Inspect"]
                                && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]))
                            {

                                var buildingInspectorRegistrationActivitiesToUpdate = dynamicsRegistrationActivities.Where(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["Inspect"]
                                                        && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName] && x.statecode != 1).ToList();

                                foreach (var activityToUpdate in buildingInspectorRegistrationActivitiesToUpdate)
                                {

                                    var registrationActivity = new BuildingInspectorRegistrationActivity
                                    {
                                        Id = activityToUpdate.bsr_biregactivityId,
                                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                                        BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName],
                                        ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                                        StatusCode = 2,
                                        StateCode = 1
                                    };
                                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                                }
                            }
                        }
                        else
                        {
                            if (!dynamicsRegistrationActivities.Any(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["Inspect"]
                                && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]
                                && x.statecode != 1)
                             )
                            {
                                var registrationActivity = new BuildingInspectorRegistrationActivity
                                {
                                    BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                                    BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                                    BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName],
                                    ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                                    StatusCode = (int)BuildingInspectorRegistrationActivityStatus.Applied,
                                    StateCode = 0
                                };

                                await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                            }
                            else if (dynamicsRegistrationActivities.Any(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["Inspect"]
                                && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]))
                            {

                                var activityToUpdate = dynamicsRegistrationActivities.FirstOrDefault(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["Inspect"]
                                                        && x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[categoryName]);


                                var registrationActivity = new BuildingInspectorRegistrationActivity
                                {
                                    Id = activityToUpdate.bsr_biregactivityId,
                                    BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                                    BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                                    BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids[categoryName],
                                    ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                                    StatusCode = (int)BuildingInspectorRegistrationActivityStatus.Applied,
                                    StateCode = 0
                                };
                                await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                            }

                        }

                    }

                }
                //Check for existing assessing plans in either category and set to inactive


                //Set class 2 activities to inactive
                var class2ynamicsRegistrationActivities = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorRegistrationActivity>>(nameof(GetRegistrationActivitiesUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);

                var class2AssessCategories = buildingProfessionApplicationModel.InspectorClass.AssessingPlansClass2.GetType().GetProperties().Where(x => x.Name != "CompletionState").ToList();
                var class2InspectCategories = buildingProfessionApplicationModel.InspectorClass.Class2InspectBuildingCategories.GetType().GetProperties().Where(x => x.Name != "CompletionState").ToList();
                var class2Categories = class2AssessCategories.Concat(class2InspectCategories).ToList();
                foreach (var category in class2Categories)
                {
                    var activitiesToUpdate = class2ynamicsRegistrationActivities.Where(x => x._bsr_bibuildingcategoryid_value == BuildingInspectorBuildingCategoryNames.Ids[category.Name + "Class2"]).ToList();
                    foreach (var activity in activitiesToUpdate)
                    {
                        var registrationActivity = new BuildingInspectorRegistrationActivity
                        {
                            Id = activity.bsr_biregactivityId,
                            BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                            BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                            BuildingCategoryId = activity._bsr_bibuildingcategoryid_value,
                            ActivityId = activity._bsr_biactivityid_value,
                            StatusCode = 2,
                            StateCode = 1
                        };
                        await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                    }
                }

            }

            if (buildingProfessionApplicationModel.InspectorClass.Activities.AssessingPlans == false)
            {
                var dynamicsRegistrationActivities = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorRegistrationActivity>>(nameof(GetRegistrationActivitiesUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);

                var activityToUpdate = dynamicsRegistrationActivities.Where(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["AssessingPlans"]).ToList();

                foreach (var activity in activityToUpdate)
                {
                    var registrationActivity = new BuildingInspectorRegistrationActivity
                    {
                        Id = activity.bsr_biregactivityId,
                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                        BuildingCategoryId = activity._bsr_bibuildingcategoryid_value,
                        ActivityId = activity._bsr_biactivityid_value,
                        StatusCode = 2,
                        StateCode = 1
                    };
                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                }

            }
            if (buildingProfessionApplicationModel.InspectorClass.Activities.Inspection == false)
            {
                var dynamicsRegistrationActivities = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorRegistrationActivity>>(nameof(GetRegistrationActivitiesUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);

                var activityToUpdate = dynamicsRegistrationActivities.Where(x => x._bsr_biactivityid_value == BuildingInspectorActivityNames.Ids["Inspect"]).ToList();

                foreach (var activity in activityToUpdate)
                {
                    var registrationActivity = new BuildingInspectorRegistrationActivity
                    {
                        Id = activity.bsr_biregactivityId,
                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                        BuildingCategoryId = activity._bsr_bibuildingcategoryid_value,
                        ActivityId = activity._bsr_biactivityid_value,
                        StatusCode = 2,
                        StateCode = 1
                    };
                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationActivity), registrationActivity);
                }

            }


            if (buildingProfessionApplicationModel.Competency.NoCompetencyAssessment.Declaration == true || buildingProfessionApplicationModel.Competency.CompetencyIndependentAssessmentStatus.IAStatus == "no")
            {

                await orchestrationContext.CallActivityAsync(nameof(UpdateBuildingProfessionApplicationCompetency), new BuildingProfessionApplicationWrapper(buildingProfessionApplicationModel, dynamicsBuildingProfessionApplication with
                {
                    bsr_hasindependentassessment = false,
                    bsr_assessmentorganisationid = null,
                    bsr_assessmentdate = null,
                    bsr_assessmentcertnumber = null
                }));


                //If no competency assessment is selected, then create new registration for class 1
                dynamicsRegistrationClasses = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorRegistrationClass>>(nameof(GetRegistrationClassesUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);

                //Create new if doesnt exist
                if (!dynamicsRegistrationClasses.Any(x => x._bsr_biclassid_value == BuildingInspectorClassNames.Ids[1] && x.statecode != 1))
                {
                    var registrationClass = new BuildingInspectorRegistrationClass
                    {
                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                        ApplicantId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                        ClassId = BuildingInspectorClassNames.Ids[1],
                        StatusCode = (int)BuildingInspectorRegistrationClassStatus.Registered,
                        StateCode = 0
                    };
                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateRegistrationClass), registrationClass);
                }

            }

            if (buildingProfessionApplicationModel.Competency.CompetencyIndependentAssessmentStatus.IAStatus == "yes")
            {
                await orchestrationContext.CallActivityAsync(nameof(UpdateBuildingProfessionApplicationCompetency), new BuildingProfessionApplicationWrapper(buildingProfessionApplicationModel, dynamicsBuildingProfessionApplication with
                {
                    bsr_hasindependentassessment = true,
                    bsr_assessmentorganisationid = buildingProfessionApplicationModel.Competency.CompetencyAssessmentOrganisation.ComAssessmentOrganisation is null ? null : $"accounts({AssessmentOrganisationNames.Ids[buildingProfessionApplicationModel.Competency.CompetencyAssessmentOrganisation.ComAssessmentOrganisation]})",
                    bsr_assessmentdate = buildingProfessionApplicationModel.Competency.CompetencyDateOfAssessment is null ? null :
                                        new DateOnly(int.Parse(buildingProfessionApplicationModel.Competency.CompetencyDateOfAssessment.Year),
                                                     int.Parse(buildingProfessionApplicationModel.Competency.CompetencyDateOfAssessment.Month),
                                                     int.Parse(buildingProfessionApplicationModel.Competency.CompetencyDateOfAssessment.Day)),
                    bsr_assessmentcertnumber = buildingProfessionApplicationModel.Competency.CompetencyAssessmentCertificateNumber is null ? null : buildingProfessionApplicationModel.Competency.CompetencyAssessmentCertificateNumber.CertificateNumber,
                }));
            }

            var dynamicsProfessionalMemberships = await orchestrationContext.CallActivityAsync<List<DynamicsBuildingInspectorProfessionalBodyMembership>>(nameof(GetBuildingInspectorProfessionalBodyMembershipsUsingApplicationId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);


            if (buildingProfessionApplicationModel.ProfessionalMemberships.IsProfessionBodyRelevantYesNo == "no")
            {
                if (dynamicsProfessionalMemberships != null)
                {
                    //Check for existing memberships and set to inactive
                    foreach (var membership in dynamicsProfessionalMemberships)
                    {

                        var professionalBody = new BuildingInspectorProfessionalBodyMembership
                        {
                            Id = membership.bsr_biprofessionalmembershipid,
                            StatusCode = 2,
                            StateCode = 1,
                            BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                            BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                            ProfessionalBodyId = membership._bsr_professionalbodyid_value,
                            MembershipNumber = null,
                            CurrentMembershipLevelOrType = null,
                            YearId = null
                        };
                        await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateBuildingInspectorProfessionalBodyMembership), professionalBody);
                    }
                }
            }
            else
            {

                //Check each body and update/create as required

                if (buildingProfessionApplicationModel.ProfessionalMemberships.CABE.CompletionState == ComponentCompletionState.Complete)
                {

                    var dynamicsBSRYear = await orchestrationContext.CallActivityAsync<DynamicsYear>(nameof(GetDynamicsYear), buildingProfessionApplicationModel.ProfessionalMemberships.CABE.MembershipYear.ToString());

                    var professionalBody = new BuildingInspectorProfessionalBodyMembership
                    {
                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                        ProfessionalBodyId = BuildingInspectorProfessionalBodyIds.Ids["CABE"],
                        MembershipNumber = buildingProfessionApplicationModel.ProfessionalMemberships.CABE.MembershipNumber,
                        CurrentMembershipLevelOrType = buildingProfessionApplicationModel.ProfessionalMemberships.CABE.MembershipLevel,
                        YearId = dynamicsBSRYear.bsr_yearid,
                        StatusCode = 1,
                        StateCode = 0
                    };
                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateBuildingInspectorProfessionalBodyMembership), professionalBody);
                }
                else if (buildingProfessionApplicationModel.ProfessionalMemberships.CABE.CompletionState != ComponentCompletionState.Complete)
                {
                    if (dynamicsProfessionalMemberships.Any(x => x._bsr_professionalbodyid_value == BuildingInspectorProfessionalBodyIds.Ids["CABE"]
                                                                && x._bsr_biapplicationid_value == dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid
                                                                && x.statuscode == 1))
                    {
                        var professionalBody = new BuildingInspectorProfessionalBodyMembership
                        {
                            BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                            BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                            ProfessionalBodyId = BuildingInspectorProfessionalBodyIds.Ids["CABE"],
                            MembershipNumber = null,
                            CurrentMembershipLevelOrType = null,
                            YearId = null,
                            StatusCode = 2,
                            StateCode = 1
                        };
                        await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateBuildingInspectorProfessionalBodyMembership), professionalBody);
                    }
                }

                if (buildingProfessionApplicationModel.ProfessionalMemberships.CIOB.CompletionState == ComponentCompletionState.Complete)
                {
                    var dynamicsBSRYear = await orchestrationContext.CallActivityAsync<DynamicsYear>(nameof(GetDynamicsYear), buildingProfessionApplicationModel.ProfessionalMemberships.CIOB.MembershipYear.ToString());

                    var professionalBody = new BuildingInspectorProfessionalBodyMembership
                    {
                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                        ProfessionalBodyId = BuildingInspectorProfessionalBodyIds.Ids["CIOB"],
                        MembershipNumber = buildingProfessionApplicationModel.ProfessionalMemberships.CIOB.MembershipNumber,
                        CurrentMembershipLevelOrType = buildingProfessionApplicationModel.ProfessionalMemberships.CIOB.MembershipLevel,
                        YearId = dynamicsBSRYear.bsr_yearid,
                        StatusCode = 1,
                        StateCode = 0
                    };
                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateBuildingInspectorProfessionalBodyMembership), professionalBody);
                }
                else if (buildingProfessionApplicationModel.ProfessionalMemberships.CIOB.CompletionState != ComponentCompletionState.Complete)
                {
                    if (dynamicsProfessionalMemberships.Any(x => x._bsr_professionalbodyid_value == BuildingInspectorProfessionalBodyIds.Ids["CIOB"]
                                            && x._bsr_biapplicationid_value == dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid
                                            && x.statuscode == 1))
                    {
                        var professionalBody = new BuildingInspectorProfessionalBodyMembership
                        {
                            BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                            BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                            ProfessionalBodyId = BuildingInspectorProfessionalBodyIds.Ids["CIOB"],
                            MembershipNumber = null,
                            CurrentMembershipLevelOrType = null,
                            YearId = null,
                            StatusCode = 2,
                            StateCode = 1
                        };
                        await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateBuildingInspectorProfessionalBodyMembership), professionalBody);
                    }
                }

                if (buildingProfessionApplicationModel.ProfessionalMemberships.RICS.CompletionState == ComponentCompletionState.Complete)
                {
                    var dynamicsBSRYear = await orchestrationContext.CallActivityAsync<DynamicsYear>(nameof(GetDynamicsYear), buildingProfessionApplicationModel.ProfessionalMemberships.RICS.MembershipYear.ToString());

                    var professionalBody = new BuildingInspectorProfessionalBodyMembership
                    {
                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                        ProfessionalBodyId = BuildingInspectorProfessionalBodyIds.Ids["RICS"],
                        MembershipNumber = buildingProfessionApplicationModel.ProfessionalMemberships.RICS.MembershipNumber,
                        CurrentMembershipLevelOrType = buildingProfessionApplicationModel.ProfessionalMemberships.RICS.MembershipLevel,
                        YearId = dynamicsBSRYear.bsr_yearid,
                        StatusCode = 1,
                        StateCode = 0
                    };
                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateBuildingInspectorProfessionalBodyMembership), professionalBody);
                }
                else if (buildingProfessionApplicationModel.ProfessionalMemberships.RICS.CompletionState != ComponentCompletionState.Complete)
                {
                    if (dynamicsProfessionalMemberships.Any(x => x._bsr_professionalbodyid_value == BuildingInspectorProfessionalBodyIds.Ids["RICS"]
                                            && x._bsr_biapplicationid_value == dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid
                                            && x.statuscode == 1))
                    {
                        var professionalBody = new BuildingInspectorProfessionalBodyMembership
                        {
                            BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                            BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                            ProfessionalBodyId = BuildingInspectorProfessionalBodyIds.Ids["RICS"],
                            MembershipNumber = null,
                            CurrentMembershipLevelOrType = null,
                            YearId = null,
                            StatusCode = 2,
                            StateCode = 1
                        };
                        await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateBuildingInspectorProfessionalBodyMembership), professionalBody);
                    }
                }

                if (buildingProfessionApplicationModel.ProfessionalMemberships.OTHER.CompletionState == ComponentCompletionState.Complete)
                {
                    var dynamicsBSRYear = await orchestrationContext.CallActivityAsync<DynamicsYear>(nameof(GetDynamicsYear), buildingProfessionApplicationModel.ProfessionalMemberships.OTHER.MembershipYear.ToString());

                    var professionalBody = new BuildingInspectorProfessionalBodyMembership
                    {
                        BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                        BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                        ProfessionalBodyId = BuildingInspectorProfessionalBodyIds.Ids["OTHER"],
                        MembershipNumber = buildingProfessionApplicationModel.ProfessionalMemberships.OTHER.MembershipNumber,
                        CurrentMembershipLevelOrType = buildingProfessionApplicationModel.ProfessionalMemberships.OTHER.MembershipLevel,
                        YearId = null,
                        StatusCode = 1,
                        StateCode = 0
                    };
                    await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateBuildingInspectorProfessionalBodyMembership), professionalBody);
                }
                else if (buildingProfessionApplicationModel.ProfessionalMemberships.OTHER.CompletionState != ComponentCompletionState.Complete)
                {
                    if (dynamicsProfessionalMemberships.Any(x => x._bsr_professionalbodyid_value == BuildingInspectorProfessionalBodyIds.Ids["OTHER"]
                                            && x._bsr_biapplicationid_value == dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid
                                            && x.statuscode == 1))
                    {
                        var professionalBody = new BuildingInspectorProfessionalBodyMembership
                        {
                            BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                            BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                            ProfessionalBodyId = BuildingInspectorProfessionalBodyIds.Ids["OTHER"],
                            MembershipNumber = null,
                            CurrentMembershipLevelOrType = null,
                            YearId = null,
                            StatusCode = 2,
                            StateCode = 1
                        };
                        await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateBuildingInspectorProfessionalBodyMembership), professionalBody);
                    }
                }


            }

            var dynamicsEmploymentDetails = await orchestrationContext.CallActivityAsync<DynamicsBuildingInspectorEmploymentDetail>(nameof(GetEmploymentDetailsUsingId), dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);

            if (buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmploymentTypeSelection.EmploymentType == EmploymentType.Unemployed)
            {
                //IF unemployed delete employment details

                var employmentDetails = new BuildingInspectorEmploymentDetail
                {
                    Id = dynamicsEmploymentDetails?.bsr_biemploymentdetailid ?? null,
                    BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                    BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                    EmployerIdAccount = null,
                    EmployerIdContact = $"/contacts({dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid})",
                    EmploymentTypeId = BuildingInspectorEmploymentTypeSelection.Ids[(int)buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmploymentTypeSelection.EmploymentType],
                    IsCurrent = true,
                    StatusCode = 1,
                    StateCode = 0
                };

                //Create or update employment
                await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateBuildingInspectorEmploymentDetails), employmentDetails);

            }
            else
            {
                string employerIdContact = null;
                string employerIdAccount = null;
                //Lookup employer
                if ((buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmploymentTypeSelection.EmploymentType == EmploymentType.PublicSector
                    || buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmploymentTypeSelection.EmploymentType == EmploymentType.PrivateSector
                    || buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmploymentTypeSelection.EmploymentType == EmploymentType.Other
                    && buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmployerName.OtherBusinessSelection == "yes"))
                {
                    //Lookup employer relationship in accounts table
                    var employerDetails = await orchestrationContext.CallActivityAsync<DynamicsAccount>(nameof(CreateOrUpdateEmployer), buildingProfessionApplicationModel);

                    employerIdAccount = $"/accounts({employerDetails.accountid})";

                }
                else if (buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmploymentTypeSelection.EmploymentType == EmploymentType.Other
                    && buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmployerName.OtherBusinessSelection == "no")
                {
                    employerIdContact = $"/contacts({dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid})";

                }


                var employmentDetails = new BuildingInspectorEmploymentDetail
                {
                    Id = dynamicsEmploymentDetails?.bsr_biemploymentdetailid ?? null,
                    BuildingProfessionApplicationId = dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                    BuildingInspectorId = dynamicsBuildingProfessionApplication.bsr_applicantid_contact.contactid,
                    EmployerIdAccount = employerIdAccount,
                    EmployerIdContact = employerIdContact,
                    EmploymentTypeId = BuildingInspectorEmploymentTypeSelection.Ids[(int)buildingProfessionApplicationModel.ProfessionalActivity.EmploymentDetails.EmploymentTypeSelection.EmploymentType],
                    IsCurrent = true,
                    StatusCode = 1,
                    StateCode = 0
                };

                //Create or update employment
                await orchestrationContext.CallActivityAsync(nameof(CreateOrUpdateBuildingInspectorEmploymentDetails), employmentDetails);

            }
        }

    }
}


