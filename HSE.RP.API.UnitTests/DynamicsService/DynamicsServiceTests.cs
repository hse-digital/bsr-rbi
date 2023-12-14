using HSE.RP.API.Enums;
using HSE.RP.API.Models;
using HSE.RP.API.Models.DynamicsSynchronisation;
using HSE.RP.API.Services;
using HSE.RP.Domain.Entities;
using Microsoft.DurableTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using HSE.RP.API.Extensions;
using HSE.RP.API.Model;
using Grpc.Core;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System.Reflection.Metadata;
using System.Net;
using FluentAssertions.Execution;
using Flurl.Http.Testing;
using Flurl.Http;

namespace HSE.RP.API.UnitTests.DynamicsServiceTest
{
    public class DynamicsServiceTests : UnitTestBase
    {
        private readonly IDynamicsService _dynamicsService;
        private BuildingProfessionApplicationModel buildingProfessionApplicationModel;
        private BuildingProfessionApplicationModel buildingProfessionApplicationModelNewApplication;

        private DynamicsBuildingProfessionApplication dynamicsBuildingProfessionApplicationNewApplication;

        private Contact contactNewApplication;


        private DynamicsContact dynamicsContact;
        private DynamicsContact dynamicsContactNewApplication;



        private DynamicsAccount dynamicsAccount;

        private const string DynamicsAuthToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Ii1LSTNROW5OUjdiUm9meG1lWm9YcWJIWkd";



        public DynamicsServiceTests()
        {
            _dynamicsService = DynamicsService;

            buildingProfessionApplicationModelNewApplication = new BuildingProfessionApplicationModel
            {
                Id = "RBCP00509D2P",
                CosmosId = "6EC5ACFE-7055-4978-BF24-CE0F2E00400E",
                PersonalDetails = new PersonalDetails
                {
                    ApplicantName = new ApplicantName
                    {
                        FirstName = "John",
                        LastName = "Smith",
                        CompletionState = ComponentCompletionState.Complete,
                    },
                    ApplicantEmail = new ApplicantEmail
                    {
                        Email = "jsmyth@website.com",
                        CompletionState = ComponentCompletionState.Complete,
                    },
                    ApplicantPhone = new ApplicantPhone
                    {
                        PhoneNumber = "0123456789",
                        CompletionState = ComponentCompletionState.Complete,
                    }

                }
            };

            dynamicsBuildingProfessionApplicationNewApplication = new DynamicsBuildingProfessionApplication
            {
                
                bsr_applicantid = "contacts(123456789)",
                bsr_applicantid_contact = null,
                bsr_buildingprofessiontypecode = BuildingProfessionType.BuildingInspector,
                bsr_buildingproappid = null,
                bsr_buildingprofessionapplicationid = "21ca463a-8988-ee11-be36-0022481b5210",
                bsr_assessmentorganisationid = null,
                _bsr_applicantid_value = null,
                CosmosId = "6EC5ACFE-7055-4978-BF24-CE0F2E00400E",
                bsr_buildingprofessionalapplicationstage = null,
                statuscode = 760810009,
                bsr_assessmentcertnumber = null,
                bsr_assessmentdate = null,
                bsr_hasindependentassessment = false
            };

            buildingProfessionApplicationModel = new BuildingProfessionApplicationModel
            {
                Id = "RBCP00509D2P",
                CosmosId = "6EC5ACFE-7055-4978-BF24-CE0F2E00400E",
                PersonalDetails = new PersonalDetails
                {
                    ApplicantName = new ApplicantName
                    {
                        FirstName = "John",
                        LastName = "Smith",
                        CompletionState = ComponentCompletionState.Complete,
                    },
                    ApplicantEmail = new ApplicantEmail
                    {
                        Email = "jsmyth@website.com",
                        CompletionState = ComponentCompletionState.Complete,

                    },
                    ApplicantAlternativeEmail = new ApplicantEmail
                    {
                        Email = "jsmythalt@website.com",
                        CompletionState = ComponentCompletionState.Complete,

                    },
                    ApplicantPhone = new ApplicantPhone
                    {
                        PhoneNumber = "0123456789",
                        CompletionState = ComponentCompletionState.Complete,

                    },
                    ApplicantAlternativePhone = new ApplicantPhone
                    {
                        PhoneNumber = "0145678992",
                        CompletionState = ComponentCompletionState.Complete,

                    },
                    ApplicantAddress = new AddressModel
                    {
                        CompletionState = ComponentCompletionState.Complete,
                        UPRN = "10033544614",
                        USRN = null,
                        Address = "BUCKINGHAM PALACE, LONDON, SW1A 1AA",
                        AddressLineTwo = null,
                        BuildingName = null,
                        Number = "",
                        Street = null,
                        Town = "LONDON",
                        Country = "E",
                        AdministrativeArea = "",
                        Postcode = "SW1A 1AA",
                        IsManual = false,
                        ClassificationCode = "PP",
                        CustodianCode = null,
                        CustodianDescription = null
                    },
                    ApplicantDateOfBirth = new ApplicantDateOfBirth
                    {
                        Day = "06",
                        Month = "06",
                        Year = "1990",
                        CompletionState = ComponentCompletionState.Complete,
                    },
                    ApplicantNationalInsuranceNumber = new ApplicantNationalInsuranceNumber
                    {
                        NationalInsuranceNumber = "QQ 12 34 56 C",
                        CompletionState = ComponentCompletionState.Complete,
                    },
                },
                InspectorClass = new Models.BuildingInspectorClass
                {
                    ClassType = new ClassSelection
                    {
                        Class = BuildingInspectorClassType.Class2,
                        CompletionState = ComponentCompletionState.Complete
                    },
                    Activities = new BuildingInspectorRegulatedActivies
                    {
                        AssessingPlans = true,
                        Inspection = true,
                        CompletionState = ComponentCompletionState.Complete
                    },
                    AssessingPlansClass2 = new BuildingAssessingPlansCategoriesClass2
                    {
                        CategoryA = true,
                        CategoryB = false,
                        CategoryC = false,
                        CategoryD = false,
                        CategoryE = false,
                        CategoryF = false,
                        CompletionState = ComponentCompletionState.Complete
                    },
                    AssessingPlansClass3 = new BuildingAssessingPlansCategoriesClass3
                    {
                        CategoryG = false,
                        CategoryH = false,
                        CategoryA = false,
                        CategoryB = false,
                        CategoryC = false,
                        CategoryD = false,
                        CategoryE = false,
                        CategoryF = false,
                        CompletionState = ComponentCompletionState.NotStarted
                    },
                    ClassTechnicalManager = "yes",
                    InspectorCountryOfWork = new BuildingInspectorCountryOfWork
                    {
                        England = true,
                        Wales = true,
                        CompletionState = ComponentCompletionState.Complete
                    },
                    Class2InspectBuildingCategories = new Class2InspectBuildingCategories
                    {
                        CategoryA = true,
                        CategoryB = false,
                        CategoryC = false,
                        CategoryD = false,
                        CategoryE = false,
                        CategoryF = false,
                        CompletionState = ComponentCompletionState.Complete
                    },
                    Class3InspectBuildingCategories = new Class3InspectBuildingCategories
                    {
                        CategoryG = false,
                        CategoryH = false,
                        CategoryA = false,
                        CategoryB = false,
                        CategoryC = false,
                        CategoryD = false,
                        CategoryE = false,
                        CategoryF = false,
                        CompletionState = ComponentCompletionState.NotStarted
                    }
                },
                Competency = new Competency
                {
                    CompetencyIndependentAssessmentStatus = new CompetencyIndependentAssessmentStatus
                    {
                        IAStatus = "yes",
                        CompletionState = ComponentCompletionState.Complete
                    },
                    CompetencyAssessmentOrganisation = new CompetencyAssessmentOrganisation
                    {
                        ComAssessmentOrganisation = "CABE",
                        CompletionState = ComponentCompletionState.Complete
                    },
                    NoCompetencyAssessment = new NoCompetencyAssessment
                    {
                        Declaration = false,
                        CompletionState = ComponentCompletionState.NotStarted
                    },
                    CompetencyDateOfAssessment = new CompetencyDateOfAssessment
                    {
                        CompletionState = ComponentCompletionState.Complete,
                        Day = "31",
                        Month = "3",
                        Year = "2022"
                    },
                    CompetencyAssessmentCertificateNumber = new CompetencyAssessmentCertificateNumber
                    {
                        CertificateNumber = "CABE1262IJSBFAHS840",
                        CompletionState = ComponentCompletionState.Complete
                    }
                },
                ProfessionalActivity = new ProfessionalActivity
                {
                    ApplicantProfessionalBodyMembership = new ApplicantProfessionalBodyMembership
                    {
                        MembershipBodyCode = "",
                        MembershipNumber = "",
                        MembershipLevel = "",
                        MembershipYear = 0,
                        CompletionState = ComponentCompletionState.NotStarted
                    },
                    EmploymentDetails = new ApplicantEmploymentDetails
                    {
                        EmploymentTypeSelection = new EmploymentTypeSelection
                        {
                            EmploymentType = EmploymentType.Other,
                            CompletionState = ComponentCompletionState.Complete
                        },
                        EmployerName = new EmployerName
                        {
                            FullName = "TRS LEEDS LIMITED",
                            OtherBusinessSelection = "no",
                            CompletionState = ComponentCompletionState.Complete
                        },
                        EmployerAddress = new AddressModel
                        {
                            CompletionState = ComponentCompletionState.Complete,
                            UPRN = "100070396159",
                            USRN = null,
                            Address = "25, HAVELOCK ROAD, GREET, BIRMINGHAM, B11 3RQ",
                            AddressLineTwo = null,
                            BuildingName = null,
                            Number = "",
                            Street = "HAVELOCK ROAD",
                            Town = "BIRMINGHAM",
                            Country = "E",
                            AdministrativeArea = "",
                            Postcode = "B11 3RQ",
                            IsManual = false,
                            ClassificationCode = "RD03",
                            CustodianCode = null,
                            CustodianDescription = null
                        },
                        CompletionState = ComponentCompletionState.Complete
                    }
                },
                ProfessionalMemberships = new ApplicantProfessionBodyMemberships
                {
                    IsProfessionBodyRelevantYesNo = "no",
                    RICS = new ApplicantProfessionalBodyMembership
                    {
                        MembershipBodyCode = "RICS",
                        MembershipNumber = "",
                        MembershipLevel = "",
                        MembershipYear = 0,
                        CompletionState = ComponentCompletionState.NotStarted
                    },
                    CABE = new ApplicantProfessionalBodyMembership
                    {
                        MembershipBodyCode = "",
                        MembershipNumber = "",
                        MembershipLevel = "",
                        MembershipYear = 0,
                        CompletionState = ComponentCompletionState.NotStarted
                    },
                    CIOB = new ApplicantProfessionalBodyMembership
                    {
                        MembershipBodyCode = "",
                        MembershipNumber = "",
                        MembershipLevel = "",
                        MembershipYear = 0,
                        CompletionState = ComponentCompletionState.NotStarted
                    },
                    OTHER = new ApplicantProfessionalBodyMembership
                    {
                        MembershipBodyCode = "OTHER",
                        MembershipNumber = "",
                        MembershipLevel = "",
                        MembershipYear = 0,
                        CompletionState = ComponentCompletionState.Complete
                    },
                    CompletionState = ComponentCompletionState.Complete
                },
                ApplicationStage = ApplicationStage.ApplicationSubmitted,
                StageStatus = new Dictionary<string, StageCompletionState>
                {
                    { "EmailVerification", StageCompletionState.Complete },
                    { "PhoneVerification", StageCompletionState.Complete },
                    { "PersonalDetails", StageCompletionState.Complete },
                    { "BuildingInspectorClass", StageCompletionState.Complete },
                    { "Competency", StageCompletionState.Complete },
                    { "ProfessionalActivity", StageCompletionState.Complete },
                    { "Declaration", StageCompletionState.Complete },
                    { "Payment", StageCompletionState.Complete }
                }
            };


            dynamicsContactNewApplication = new DynamicsContact
            {
                firstname = "John",
                lastname = "Smith",
                emailaddress1 = "jsmyth@website.com",
                telephone1 = "0123456789",
                jobRoleReferenceId = $"/bsr_jobroles({DynamicsJobRole.Ids["building_inspector"]})"
            };

            contactNewApplication = new Contact
            {
                Id = "123456789",
                FirstName = "John",
                LastName = "Smith",
                Email = "jsmyth@website.com",
                PhoneNumber = "0123456789",
                jobRoleReferenceId = "/bsr_jobroles(c77d5afd-e006-ee11-8f6e-0022481b5210)"
            };

            dynamicsContact = new DynamicsContact
            {
                contactid = "123456789",
                firstname = "John",
                lastname = "Smith",
                emailaddress1 = "jsmyth@website.com",
                emailaddress2 = "jsmythalt@website.com",
                telephone1 = "0123456789",
                business2 = "0145678992",
                address1_addresstypecode = 3,
                address1_line1 = "BUCKINGHAM PALACE, LONDON, SW1A 1AA",
                address1_line2 = null,
                address1_city = "LONDON",
                address1_postalcode = "SW1A 1AA",
                bsr_address1uprn = "10033544614",
                bsr_address1usrn = null,
                bsr_address1lacode = null,
                bsr_address1ladescription = null,
                bsr_manualaddress = YesNoOption.No,
                birthdate = new DateOnly(1990, 06, 06),
                bsr_nationalinsuranceno = "QQ 12 34 56 C"
            };

            HttpTest.ForCallsTo("https://login.microsoftonline.com/6b5953be-6b1d-4980-b26b-56ed8b0bf3dc/oauth2/token")
            .RespondWithJson(new DynamicsAuthenticationModel { AccessToken = DynamicsAuthToken });
        }

        [Fact]
        public async Task WhenCalledCreateNewContact_NoExistingContact()
        {
            //Arrange
            HttpTest.RespondWithJson(body: new DynamicsResponse<DynamicsContact> { value = new List<DynamicsContact>() });
            HttpTest.RespondWith(status: 204, headers: BuildODataEntityHeader(dynamicsContact.contactid));


            //Act
            var contact = await _dynamicsService.CreateContactAsync(buildingProfessionApplicationModel);

            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts")
            .WithQueryParam("$filter", $"firstname eq '{buildingProfessionApplicationModel.PersonalDetails.ApplicantName.FirstName}' and lastname eq '{buildingProfessionApplicationModel.PersonalDetails.ApplicantName.LastName}' and statuscode eq 1 and emailaddress1 eq '{buildingProfessionApplicationModel.PersonalDetails.ApplicantEmail.Email}' and contains(telephone1, '{buildingProfessionApplicationModel.PersonalDetails.ApplicantPhone.PhoneNumber.Replace("+", string.Empty).EscapeSingleQuote()}')")
            .WithQueryParam("$expand", "bsr_contacttype_contact")
            .WithOAuthBearerToken(DynamicsAuthToken)
            .WithVerb(HttpMethod.Get);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts({contact.Id})/bsr_contacttype_contact/$ref")
            .WithRequestJson(new DynamicsContactType { contactTypeReferenceId = $"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_contacttypes({DynamicsContactTypes.BIApplicant})" })
            .WithOAuthBearerToken(DynamicsAuthToken)
            .WithVerb(HttpMethod.Post);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts")
            .WithRequestJson(dynamicsContactNewApplication)
            .WithOAuthBearerToken(DynamicsAuthToken)
            .WithVerb(HttpMethod.Post);

            Assert.Equal(contact, contactNewApplication);

        }

        [Fact]
        public async Task WhenCalledCreateNewContact_ExistingContact()
        {
            //Arrange
            HttpTest.RespondWithJson(
                body: new DynamicsResponse<DynamicsContact>
                {
                    value = new List<DynamicsContact> { dynamicsContactNewApplication with {
                        contactid = dynamicsContact.contactid }
                    }
                });

            //Act
            var contact = await _dynamicsService.CreateContactAsync(buildingProfessionApplicationModel);

            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts")
            .WithQueryParam("$filter", $"firstname eq '{buildingProfessionApplicationModel.PersonalDetails.ApplicantName.FirstName}' and lastname eq '{buildingProfessionApplicationModel.PersonalDetails.ApplicantName.LastName}' and statuscode eq 1 and emailaddress1 eq '{buildingProfessionApplicationModel.PersonalDetails.ApplicantEmail.Email}' and contains(telephone1, '{buildingProfessionApplicationModel.PersonalDetails.ApplicantPhone.PhoneNumber.Replace("+", string.Empty).EscapeSingleQuote()}')")
            .WithQueryParam("$expand", "bsr_contacttype_contact")
            .WithOAuthBearerToken(DynamicsAuthToken)
            .WithVerb(HttpMethod.Get);

            Assert.Equal(contact, contactNewApplication);

        }

        [Fact]
        public async Task WhenCalledCreateBuildingProfessionApplication_NoExistingApplication()
        {

            //Arrange
            HttpTest.RespondWithJson(body: new DynamicsResponse<DynamicsContact> { value = new List<DynamicsContact>() });
            HttpTest.RespondWith(status: 204, headers: BuildODataEntityHeader(dynamicsContact.contactid));
            HttpTest.RespondWith(status: 200);
            HttpTest.RespondWith(status: 204, headers: BuildODataEntityHeader(dynamicsBuildingProfessionApplicationNewApplication.bsr_buildingprofessionapplicationid));

            //Act
            var contact = await _dynamicsService.CreateContactAsync(buildingProfessionApplicationModel);

            var application = await _dynamicsService.CreateBuildingProfessionApplicationAsync(buildingProfessionApplicationModelNewApplication, contact);


            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts")
            .WithQueryParam("$filter", $"firstname eq '{buildingProfessionApplicationModel.PersonalDetails.ApplicantName.FirstName}' and lastname eq '{buildingProfessionApplicationModel.PersonalDetails.ApplicantName.LastName}' and statuscode eq 1 and emailaddress1 eq '{buildingProfessionApplicationModel.PersonalDetails.ApplicantEmail.Email}' and contains(telephone1, '{buildingProfessionApplicationModel.PersonalDetails.ApplicantPhone.PhoneNumber.Replace("+", string.Empty).EscapeSingleQuote()}')")
            .WithQueryParam("$expand", "bsr_contacttype_contact")
            .WithOAuthBearerToken(DynamicsAuthToken)
            .WithVerb(HttpMethod.Get);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts({contact.Id})/bsr_contacttype_contact/$ref")
            .WithRequestJson(new DynamicsContactType { contactTypeReferenceId = $"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_contacttypes({DynamicsContactTypes.BIApplicant})" })
            .WithOAuthBearerToken(DynamicsAuthToken)
            .WithVerb(HttpMethod.Post);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts")
            .WithRequestJson(dynamicsContactNewApplication)
            .WithOAuthBearerToken(DynamicsAuthToken)
            .WithVerb(HttpMethod.Post);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_buildingprofessionapplications")
            .WithRequestJson(new DynamicsBuildingProfessionApplication(bsr_applicantid: $"/contacts({contact.Id})", bsr_buildingprofessiontypecode: BuildingProfessionType.BuildingInspector, statuscode: (int)BuildingProfessionApplicationStatus.New, CosmosId: buildingProfessionApplicationModelNewApplication.CosmosId, bsr_hasindependentassessment: false))
            .WithOAuthBearerToken(DynamicsAuthToken)
            .WithVerb(HttpMethod.Post);

            Assert.Equal(contact, contactNewApplication);
            Assert.Equal(application, buildingProfessionApplicationModelNewApplication with { Id = dynamicsBuildingProfessionApplicationNewApplication.bsr_buildingprofessionapplicationid });

        }

        [Fact]
        public async Task WhenCalledCreateBuildingProfessionApplication_ExistingApplication()
        {

            //Arrange
            HttpTest.RespondWithJson(body: new DynamicsResponse<DynamicsContact> { value = new List<DynamicsContact>() });
            HttpTest.RespondWith(status: 204, headers: BuildODataEntityHeader(dynamicsContact.contactid));
            HttpTest.RespondWith(status: 200);
            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_buildingprofessionapplications").WithVerb(HttpMethod.Post).RespondWith(status: 412);


            HttpTest.RespondWithJson(body: new DynamicsResponse<DynamicsBuildingProfessionApplication> { value = new List<DynamicsBuildingProfessionApplication>{ dynamicsBuildingProfessionApplicationNewApplication}});



            //Act
            var contact = await _dynamicsService.CreateContactAsync(buildingProfessionApplicationModel);

            var application = await _dynamicsService.CreateBuildingProfessionApplicationAsync(buildingProfessionApplicationModelNewApplication, contact);


            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts")
            .WithQueryParam("$filter", $"firstname eq '{buildingProfessionApplicationModel.PersonalDetails.ApplicantName.FirstName}' and lastname eq '{buildingProfessionApplicationModel.PersonalDetails.ApplicantName.LastName}' and statuscode eq 1 and emailaddress1 eq '{buildingProfessionApplicationModel.PersonalDetails.ApplicantEmail.Email}' and contains(telephone1, '{buildingProfessionApplicationModel.PersonalDetails.ApplicantPhone.PhoneNumber.Replace("+", string.Empty).EscapeSingleQuote()}')")
            .WithQueryParam("$expand", "bsr_contacttype_contact")
            .WithOAuthBearerToken(DynamicsAuthToken)
            .WithVerb(HttpMethod.Get);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts({contact.Id})/bsr_contacttype_contact/$ref")
            .WithRequestJson(new DynamicsContactType { contactTypeReferenceId = $"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_contacttypes({DynamicsContactTypes.BIApplicant})" })
            .WithOAuthBearerToken(DynamicsAuthToken)
            .WithVerb(HttpMethod.Post);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts")
            .WithRequestJson(dynamicsContactNewApplication)
            .WithOAuthBearerToken(DynamicsAuthToken)
            .WithVerb(HttpMethod.Post);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_buildingprofessionapplications")
            .WithRequestJson(new DynamicsBuildingProfessionApplication(bsr_applicantid: $"/contacts({contact.Id})", bsr_buildingprofessiontypecode: BuildingProfessionType.BuildingInspector, statuscode: (int)BuildingProfessionApplicationStatus.New, CosmosId: buildingProfessionApplicationModelNewApplication.CosmosId, bsr_hasindependentassessment: false))
            .WithOAuthBearerToken(DynamicsAuthToken)
            .WithVerb(HttpMethod.Post);


            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_buildingprofessionapplications")
            .WithRequestJson(new DynamicsBuildingProfessionApplication(bsr_applicantid: $"/contacts({contact.Id})", bsr_buildingprofessiontypecode: BuildingProfessionType.BuildingInspector, statuscode: (int)BuildingProfessionApplicationStatus.New, CosmosId: buildingProfessionApplicationModelNewApplication.CosmosId, bsr_hasindependentassessment: false))
            .WithOAuthBearerToken(DynamicsAuthToken)
            .WithVerb(HttpMethod.Post);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_buildingprofessionapplications")
            .WithQueryParam("$filter", $"bsr_cosmosid eq '{buildingProfessionApplicationModelNewApplication.CosmosId}'")
            .WithOAuthBearerToken(DynamicsAuthToken)
            .WithVerb(HttpMethod.Get);

            Assert.Equal(contact, contactNewApplication);
            Assert.Equal(application, buildingProfessionApplicationModelNewApplication with { Id = dynamicsBuildingProfessionApplicationNewApplication.bsr_buildingprofessionapplicationid });

        }
    }

}
