﻿using HSE.RP.API.Enums;
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
using HSE.RP.API.Services;
using Grpc.Core;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System.Reflection.Metadata;
using System.Net;
using FluentAssertions.Execution;
using Flurl.Http.Testing;
using Flurl.Http;
using Moq;
using FluentAssertions;
using BuildingInspectorClass = HSE.RP.API.Models.BuildingInspectorClass;
using HSE.RP.API.UnitTests.TestData;
using DurableTask.Core;
using HSE.RP.API.Functions;
using System.Net.Mail;
using HSE.RP.API.Mappers;
using HSE.RP.API.Models.DynamicsDataExport;
using Google.Protobuf.WellKnownTypes;

namespace HSE.RP.API.UnitTests.DynamicsServiceTest
{
    public class DynamicsServiceTests : UnitTestBase
    {

        private readonly IDynamicsService _dynamicsService;
        private readonly IDynamicsSynchronisationFunctions _dynamicsSynchronisationFunctions;

        //Create dynamicsservice using mockdynamicsservice
        private BuildingProfessionApplicationModel buildingProfessionApplicationModel;

        private BuildingProfessionApplicationModel buildingProfessionApplicationModelNewApplication;

        private DynamicsBuildingProfessionApplication dynamicsBuildingProfessionApplicationNewApplication;

        private Contact contactNewApplication;


        private DynamicsContact dynamicsContact;
        private DynamicsContact dynamicsContactNewApplication;


        private List<object[]> registrationClassTestData;


        private DynamicsAccount dynamicsAccount;

        private const string DynamicsAuthToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Ii1LSTNROW5OUjdiUm9meG1lWm9YcWJIWkd";

        public static InspectorClassTestConfigrations dynamicsServiceInspectorClassConfigrations = new InspectorClassTestConfigrations();
        public static CompetencyTestConfigrations dynamicsServiceCompetencyTestConfigrations = new CompetencyTestConfigrations();
        public static ProfessionalMembershipTestConfigurations dynamicsServiceProfessionalMembershipTestConfigurations = new ProfessionalMembershipTestConfigurations();

        public DynamicsServiceTests()
        {
            _dynamicsService = DynamicsService;
            _dynamicsSynchronisationFunctions = DynamicsSynchronisationFunctions;




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
                _bsr_applicantid_value = "123456789",
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
                InspectorClass = new BuildingInspectorClass
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
                    RICS = new ApplicantProfessionalBodyMembership
                    {
                        MembershipBodyCode = "RICS",
                        MembershipNumber = "",
                        MembershipLevel = "",
                        MembershipYear = 0,
                        CompletionState = ComponentCompletionState.NotStarted,
                        RemoveOptionSelected = ""
                    },
                    CABE = new ApplicantProfessionalBodyMembership
                    {
                        MembershipBodyCode = "",
                        MembershipNumber = "",
                        MembershipLevel = "",
                        MembershipYear = 0,
                        CompletionState = ComponentCompletionState.NotStarted,
                        RemoveOptionSelected = ""
                    },
                    CIOB = new ApplicantProfessionalBodyMembership
                    {
                        MembershipBodyCode = "",
                        MembershipNumber = "",
                        MembershipLevel = "",
                        MembershipYear = 0,
                        CompletionState = ComponentCompletionState.NotStarted,
                        RemoveOptionSelected = ""
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
        public async Task GetContactsAsync()
        {

            //Arrange

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts")
            .WithAnyQueryParam("$filter", $"firstname eq '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantName.FirstName}' and lastname eq '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantName.LastName}' and emailaddress1 eq '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantEmail.Email}' ")            
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(new DynamicsResponse<DynamicsContact> { value = new List<DynamicsContact> { dynamicsContact } });


            //Act

            var testGetContacts = await _dynamicsService.GetContactsAsync(buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantName.FirstName, buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantName.LastName, buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantEmail.Email);



            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts")
            .WithAnyQueryParam("$filter", $"firstname eq '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantName.FirstName}' and lastname eq '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantName.LastName}' and emailaddress1 eq '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantEmail.Email}' ")
            .WithVerb(HttpMethod.Get);

            Assert.NotEmpty(testGetContacts.value);
            Assert.Equal(dynamicsContact.contactid, testGetContacts.value.First().contactid);

        }

        [Fact]
        public async Task CreateNewContactAsync()
        {

            //Arrange

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts")
            .WithVerb(HttpMethod.Post)
            .WithRequestJson(dynamicsContactNewApplication)
            .RespondWith(status: 204, headers: BuildODataEntityHeader("123456789"));
            //Act

            var testCreateContact = await _dynamicsService.CreateNewContactAsync(dynamicsContactNewApplication);



            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts")
            .WithRequestJson(dynamicsContactNewApplication)
            .WithVerb(HttpMethod.Post);


            Assert.Equal("123456789", testCreateContact.contactid);

        }

        [Fact]
        public async Task FindExistingContactAsync_ContactFound()
        {

            //Arrange

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts")
            .WithAnyQueryParam("$filter", $"firstname eq '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantName.FirstName.EscapeSingleQuote()}' and lastname eq '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantName.LastName.EscapeSingleQuote()}' and statuscode eq 1 and emailaddress1 eq '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantEmail.Email.EscapeSingleQuote()}' and contains(telephone1, '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantPhone.PhoneNumber.Replace("+", string.Empty).EscapeSingleQuote()}')")
            .WithQueryParam("$expand", "bsr_contacttype_contact")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(new DynamicsResponse<DynamicsContact> { value = new List<DynamicsContact> { dynamicsContact } });


            //Act

            var testGetContacts = await _dynamicsService.FindExistingContactAsync(buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantName.FirstName, buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantName.LastName, buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantEmail.Email,buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantPhone.PhoneNumber);



            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts")
            .WithAnyQueryParam("$filter", $"firstname eq '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantName.FirstName.EscapeSingleQuote()}' and lastname eq '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantName.LastName.EscapeSingleQuote()}' and statuscode eq 1 and emailaddress1 eq '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantEmail.Email.EscapeSingleQuote()}' and contains(telephone1, '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantPhone.PhoneNumber.Replace("+", string.Empty).EscapeSingleQuote()}')")
            .WithQueryParam("$expand", "bsr_contacttype_contact")
            .WithVerb(HttpMethod.Get);

            Assert.NotNull(testGetContacts);
            Assert.Equal(dynamicsContact.contactid, testGetContacts.contactid);

        }

        [Fact]
        public async Task FindExistingContactAsync_ContactNotFound()
        {

            //Arrange

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts")
            .WithAnyQueryParam("$filter", $"firstname eq '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantName.FirstName.EscapeSingleQuote()}' and lastname eq '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantName.LastName.EscapeSingleQuote()}' and statuscode eq 1 and emailaddress1 eq '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantEmail.Email.EscapeSingleQuote()}' and contains(telephone1, '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantPhone.PhoneNumber.Replace("+", string.Empty).EscapeSingleQuote()}')")
            .WithQueryParam("$expand", "bsr_contacttype_contact")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(new DynamicsResponse<DynamicsContact> { value = new List<DynamicsContact> { } });


            //Act

            var testGetContacts = await _dynamicsService.FindExistingContactAsync(buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantName.FirstName, buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantName.LastName, buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantEmail.Email, buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantPhone.PhoneNumber);



            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts")
            .WithAnyQueryParam("$filter", $"firstname eq '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantName.FirstName.EscapeSingleQuote()}' and lastname eq '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantName.LastName.EscapeSingleQuote()}' and statuscode eq 1 and emailaddress1 eq '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantEmail.Email.EscapeSingleQuote()}' and contains(telephone1, '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantPhone.PhoneNumber.Replace("+", string.Empty).EscapeSingleQuote()}')")
            .WithQueryParam("$expand", "bsr_contacttype_contact")
            .WithVerb(HttpMethod.Get);

            Assert.Null(testGetContacts);

        }

        [Fact]
        public async Task FindExistingBuildingProfessionalBodyMembership_MembershipFound()
        {

            //Arrange

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biprofessionalmemberships")
            .WithAnyQueryParam("$filter", $"_bsr_biapplicationid_value eq '{dynamicsServiceProfessionalMembershipTestConfigurations.RICSBuildingProfessionApplicationBodyMembership.BuildingProfessionApplicationId}' and _bsr_professionalbodyid_value eq '{dynamicsServiceProfessionalMembershipTestConfigurations.RICSBuildingProfessionApplicationBodyMembership.Id}'")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(new DynamicsResponse<DynamicsBuildingInspectorProfessionalBodyMembership> { value = new List<DynamicsBuildingInspectorProfessionalBodyMembership> { dynamicsServiceProfessionalMembershipTestConfigurations.RICSDynamicsBuildingInspectorProfessionalBodyMembership } });


            //Act

            var testGetMembership = await _dynamicsService.FindExistingBuildingProfessionalBodyMembership(dynamicsServiceProfessionalMembershipTestConfigurations.RICSBuildingProfessionApplicationBodyMembership.Id, dynamicsServiceProfessionalMembershipTestConfigurations.RICSBuildingProfessionApplicationBodyMembership.BuildingProfessionApplicationId);



            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biprofessionalmemberships")
            .WithAnyQueryParam("$filter", $"_bsr_biapplicationid_value eq '{dynamicsServiceProfessionalMembershipTestConfigurations.RICSBuildingProfessionApplicationBodyMembership.BuildingProfessionApplicationId}' and _bsr_professionalbodyid_value eq '{dynamicsServiceProfessionalMembershipTestConfigurations.RICSBuildingProfessionApplicationBodyMembership.Id}'")
            .WithVerb(HttpMethod.Get);

            Assert.NotNull(testGetMembership);
            Assert.Equal(dynamicsServiceProfessionalMembershipTestConfigurations.RICSDynamicsBuildingInspectorProfessionalBodyMembership.bsr_biprofessionalmembershipid, testGetMembership.bsr_biprofessionalmembershipid);

        }

        [Fact]
        public async Task FindExistingBuildingProfessionalBodyMembership_MembershipNotFound()
        {

            //Arrange

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biprofessionalmemberships")
            .WithAnyQueryParam("$filter", $"_bsr_biapplicationid_value eq '{dynamicsServiceProfessionalMembershipTestConfigurations.RICSBuildingProfessionApplicationBodyMembership.BuildingProfessionApplicationId}' and _bsr_professionalbodyid_value eq '{dynamicsServiceProfessionalMembershipTestConfigurations.RICSBuildingProfessionApplicationBodyMembership.Id}'")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(new DynamicsResponse<DynamicsBuildingInspectorProfessionalBodyMembership> { value = new List<DynamicsBuildingInspectorProfessionalBodyMembership> { } });


            //Act

            var testGetMembership = await _dynamicsService.FindExistingBuildingProfessionalBodyMembership(dynamicsServiceProfessionalMembershipTestConfigurations.RICSBuildingProfessionApplicationBodyMembership.Id, dynamicsServiceProfessionalMembershipTestConfigurations.RICSBuildingProfessionApplicationBodyMembership.BuildingProfessionApplicationId);



            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biprofessionalmemberships")
            .WithAnyQueryParam("$filter", $"_bsr_biapplicationid_value eq '{dynamicsServiceProfessionalMembershipTestConfigurations.RICSBuildingProfessionApplicationBodyMembership.BuildingProfessionApplicationId}' and _bsr_professionalbodyid_value eq '{dynamicsServiceProfessionalMembershipTestConfigurations.RICSBuildingProfessionApplicationBodyMembership.Id}'")
            .WithVerb(HttpMethod.Get);

            Assert.Null(testGetMembership);

        }

        [Fact]
        public async Task CreateDynamicsPaymentAsync()
        {

            //Arrange


            var testDynamicsPayment = new DynamicsPayment
            {
                buildingProfessionApplicationReferenceId = $"/bsr_buildingprofessionapplications({dynamicsBuildingProfessionApplicationNewApplication.bsr_buildingprofessionapplicationid})",
                bsr_amountpaid = 100,
                bsr_cardtypecreditdebit = DynamicsPaymentCardType.Credit,
                bsr_lastfourdigitsofcardnumber = "1234",
                bsr_cardexpirydate = "12/23",
                bsr_cardbrandegvisa = "visa",
                bsr_billingaddress = "25, HAVELOCK ROAD, GREET, BIRMINGHAM, B11 3RQ",
                bsr_emailaddress = "",
                bsr_service = "rbiportal"
            };

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_payments")
            .WithVerb(HttpMethod.Post)
            .WithRequestJson(testDynamicsPayment)
            .RespondWith(status: 204, headers: BuildODataEntityHeader("123456789"));


            //Act

            var testPaymentResult = await _dynamicsService.CreateDynamicsPaymentAsync(testDynamicsPayment);



            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_payments")
            .WithVerb(HttpMethod.Post)
            .WithRequestJson(testDynamicsPayment);

            Assert.Equal("123456789", testPaymentResult.bsr_paymentid);

        }

        [Fact]
        public async Task UpdateDynamicsPaymentAsync()
        {

            //Arrange


            var testDynamicsPayment = new DynamicsPayment
            {
                bsr_paymentid = "123456789",
                buildingProfessionApplicationReferenceId = $"/bsr_buildingprofessionapplications({dynamicsBuildingProfessionApplicationNewApplication.bsr_buildingprofessionapplicationid})",
                bsr_amountpaid = 100,
                bsr_cardtypecreditdebit = DynamicsPaymentCardType.Credit,
                bsr_lastfourdigitsofcardnumber = "1234",
                bsr_cardexpirydate = "12/23",
                bsr_cardbrandegvisa = "visa",
                bsr_billingaddress = "25, HAVELOCK ROAD, GREET, BIRMINGHAM, B11 3RQ",
                bsr_emailaddress = "",
                bsr_service = "rbiportal",
                bsr_govukpaystatus = "success"
            };

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_payments({testDynamicsPayment.bsr_paymentid})")
            .WithVerb(HttpMethod.Patch)
            .WithRequestJson(testDynamicsPayment)
            .RespondWith(status: 204);


            //Act

            await _dynamicsService.UpdateInvoicePaymentAsync(testDynamicsPayment);



            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_payments({testDynamicsPayment.bsr_paymentid})")
            .WithVerb(HttpMethod.Patch)
            .WithRequestJson(testDynamicsPayment);



        }

        [Fact]
        public async Task UpdatePersonalDetails()
        {



            CountryCodeMapper CountryCodeMapper = new CountryCodeMapper();



            //Arrange

            var address = new BuildingAddress
            {
                Address = buildingProfessionApplicationModel.PersonalDetails.ApplicantAddress.Address ?? "",
                AddressLineTwo = buildingProfessionApplicationModel.PersonalDetails.ApplicantAddress.AddressLineTwo ?? "",
                BuildingName = buildingProfessionApplicationModel.PersonalDetails.ApplicantAddress.BuildingName ?? "",
                Number = buildingProfessionApplicationModel.PersonalDetails.ApplicantAddress.Number ?? "",
                Street = buildingProfessionApplicationModel.PersonalDetails.ApplicantAddress.Street ?? "",
                Town = buildingProfessionApplicationModel.PersonalDetails.ApplicantAddress.Town ?? "",
                Country = CountryCodeMapper.MapCountry(buildingProfessionApplicationModel.PersonalDetails.ApplicantAddress.Country ?? null),
                AdministrativeArea = buildingProfessionApplicationModel.PersonalDetails.ApplicantAddress.AdministrativeArea ?? "",
                Postcode = buildingProfessionApplicationModel.PersonalDetails.ApplicantAddress.Postcode ?? "",
                IsManual = buildingProfessionApplicationModel.PersonalDetails.ApplicantAddress.IsManual ?? false,
                ClassificationCode = buildingProfessionApplicationModel.PersonalDetails.ApplicantAddress.ClassificationCode ?? "",
                CustodianCode = buildingProfessionApplicationModel.PersonalDetails.ApplicantAddress.CustodianCode ?? "",
                CustodianDescription = buildingProfessionApplicationModel.PersonalDetails.ApplicantAddress.CustodianDescription ?? ""
            };


            var testContact = new Contact
            {
                Id = dynamicsContact.contactid ?? "",
                FirstName = buildingProfessionApplicationModel.PersonalDetails.ApplicantName.FirstName ?? "",
                LastName = buildingProfessionApplicationModel.PersonalDetails.ApplicantName.LastName ?? "",
                Email = buildingProfessionApplicationModel.PersonalDetails.ApplicantEmail.Email ?? "",
                AlternativeEmail = buildingProfessionApplicationModel.PersonalDetails.ApplicantAlternativeEmail is null ? null : buildingProfessionApplicationModel.PersonalDetails.ApplicantAlternativeEmail.Email,
                PhoneNumber = buildingProfessionApplicationModel.PersonalDetails.ApplicantPhone.PhoneNumber ?? null,
                AlternativePhoneNumber = buildingProfessionApplicationModel.PersonalDetails.ApplicantAlternativePhone is null ? null : buildingProfessionApplicationModel.PersonalDetails.ApplicantAlternativePhone.PhoneNumber ?? "",
                Address = address,
                birthdate = buildingProfessionApplicationModel.PersonalDetails.ApplicantDateOfBirth is null ? null :
                new DateOnly(int.Parse(buildingProfessionApplicationModel.PersonalDetails.ApplicantDateOfBirth.Year),
                                         int.Parse(buildingProfessionApplicationModel.PersonalDetails.ApplicantDateOfBirth.Month),
                                         int.Parse(buildingProfessionApplicationModel.PersonalDetails.ApplicantDateOfBirth.Day)),
                NationalInsuranceNumber = buildingProfessionApplicationModel.PersonalDetails.ApplicantNationalInsuranceNumber is null ? null : buildingProfessionApplicationModel.PersonalDetails.ApplicantNationalInsuranceNumber.NationalInsuranceNumber,
            };

            var testContactWrapper = new ContactWrapper(testContact, dynamicsContact);

            var testNewDynamicsContact = new DynamicsContact
            {
                firstname = testContactWrapper.Model.FirstName,
                lastname = testContactWrapper.Model.LastName,
                emailaddress1 = testContactWrapper.Model.Email,
                emailaddress2 = testContactWrapper.Model.AlternativeEmail,
                telephone1 = testContactWrapper.Model.PhoneNumber,
                business2 = testContactWrapper.Model.AlternativePhoneNumber,
                address1_addresstypecode = 3,
                address1_line1 = testContactWrapper.Model.Address.Address,
                address1_line2 = testContactWrapper.Model.Address.AddressLineTwo,
                address1_city = testContactWrapper.Model.Address.Town,
                address1_country = testContactWrapper.Model.Address.Country,
                address1_postalcode = testContactWrapper.Model.Address.Postcode,
                bsr_address1uprn = testContactWrapper.Model.Address.UPRN,
                bsr_address1usrn = testContactWrapper.Model.Address.USRN,
                bsr_address1lacode = testContactWrapper.Model.Address.CustodianCode,
                bsr_address1ladescription = testContactWrapper.Model.Address.CustodianDescription,
                bsr_manualaddress = testContactWrapper.Model.Address.IsManual is null ? null :
                                testContactWrapper.Model.Address.IsManual is true ? YesNoOption.Yes :
                                YesNoOption.No,
                birthdate = testContactWrapper.Model.birthdate,
                bsr_nationalinsuranceno = testContactWrapper.Model.NationalInsuranceNumber,
                            countryReferenceId = testContactWrapper.Model.Address.Country is null ? null :
                                         testContactWrapper.Model.Address.Country == "England" ? $"/bsr_countries({BuildingInspectorCountryNames.Ids["England"]})" :
                                         testContactWrapper.Model.Address.Country == "Wales" ? $"/bsr_countries({BuildingInspectorCountryNames.Ids["Wales"]})" :
                                         null
            };


            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts({testContact.Id})")
            .WithRequestJson(testNewDynamicsContact)
            .WithVerb(HttpMethod.Patch)
            .RespondWith(status: 204);

            //Act

            await _dynamicsSynchronisationFunctions.UpdateContact(testContactWrapper);



            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts({testContact.Id})")
            .WithRequestJson(testNewDynamicsContact)
            .WithVerb(HttpMethod.Patch);

        }

        [Fact]
        public async Task CreateNewContact_NoExistingContact()
        {
            //Arrange
            HttpTest.RespondWithJson(body: new DynamicsResponse<DynamicsContact> { value = new List<DynamicsContact>() });
            HttpTest.RespondWith(status: 204, headers: BuildODataEntityHeader(dynamicsContact.contactid));
            HttpTest.RespondWithJson(body: new DynamicsResponse<DynamicsContactType> { value = new List<DynamicsContactType>() });


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
        public async Task CreateNewContact_ExistingContact()
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
        public async Task CreateBuildingProfessionApplication_NoExistingApplication()
        {

            //Arrange
            HttpTest.RespondWithJson(body: new DynamicsResponse<DynamicsContact> { value = new List<DynamicsContact>() });
            HttpTest.RespondWith(status: 204, headers: BuildODataEntityHeader(dynamicsContact.contactid));
            HttpTest.RespondWithJson(body: new DynamicsResponse<DynamicsContactType> { value = new List<DynamicsContactType>() });
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
        public async Task CreateBuildingProfessionApplication_ExistingApplication()
        {

            //Arrange
            HttpTest.RespondWithJson(body: new DynamicsResponse<DynamicsContact> { value = new List<DynamicsContact>() });
            HttpTest.RespondWith(status: 204, headers: BuildODataEntityHeader(dynamicsContact.contactid));
            HttpTest.RespondWithJson(body: new DynamicsResponse<DynamicsContactType> { value = new List<DynamicsContactType>() });
            HttpTest.RespondWith(status: 200);

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_buildingprofessionapplications").WithVerb(HttpMethod.Post).RespondWith(status: 412);


            HttpTest.RespondWithJson(body: new DynamicsResponse<DynamicsBuildingProfessionApplication> { value = new List<DynamicsBuildingProfessionApplication> { dynamicsBuildingProfessionApplicationNewApplication } });



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

        [Fact]
        public async Task RegisterNewBuildingProfessionApplication_NoExistingApplication()
        {

            //Arrange

            //Create contact check if existing, return null
            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts")
            .WithQueryParam("$filter", $"firstname eq '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantName.FirstName}' and lastname eq '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantName.LastName}' and statuscode eq 1 and emailaddress1 eq '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantEmail.Email}' and contains(telephone1, '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantPhone.PhoneNumber.Replace("+", string.Empty).EscapeSingleQuote()}')")
            .WithQueryParam("$expand", "bsr_contacttype_contact")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(body: new DynamicsResponse<DynamicsContact> { value = new List<DynamicsContact>() });

            //For calls to check contact type return empty list
            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts({dynamicsContact.contactid})/bsr_contacttype_contact")
            .WithQueryParam("$filter", $"bsr_contacttypeid eq '{DynamicsContactTypes.BIApplicant}'")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(body: new DynamicsResponse<DynamicsContactType>
            {
                value = new List<DynamicsContactType>()

            });

            //Create contact
            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts")
            .WithRequestJson(dynamicsContactNewApplication)
            .WithVerb(HttpMethod.Post)
            .RespondWith(status: 204, headers: BuildODataEntityHeader(dynamicsContact.contactid));

            //Update Contact Type
            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts({dynamicsContact.contactid})/bsr_contacttype_contact/$ref")
            .WithRequestJson(new DynamicsContactType { contactTypeReferenceId = $"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_contacttypes({DynamicsContactTypes.BIApplicant})" })
            .WithVerb(HttpMethod.Post)
            .RespondWith(status: 204);

            //Create Building Profession Application
            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_buildingprofessionapplications")
            .WithRequestJson(new DynamicsBuildingProfessionApplication(
                    bsr_applicantid: $"/contacts({dynamicsContact.contactid})",
                    bsr_buildingprofessiontypecode: BuildingProfessionType.BuildingInspector,
                    statuscode: (int)BuildingProfessionApplicationStatus.New,
                    CosmosId: buildingProfessionApplicationModelNewApplication.CosmosId,
                    bsr_hasindependentassessment: false))
             .WithVerb(HttpMethod.Post)
             .RespondWith(status: 204, headers: BuildODataEntityHeader(dynamicsBuildingProfessionApplicationNewApplication.bsr_buildingprofessionapplicationid));

            //Get Contact ID for new contact
            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts({dynamicsContact.contactid})")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(dynamicsContactNewApplication with { contactid = dynamicsContact.contactid });

            //Update Application with contact id
            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts({dynamicsContact.contactid})")
            .WithRequestJson(dynamicsContactNewApplication with
            {
                contactid = dynamicsContact.contactid,
                bsr_buildingprofessionapplicationid = $"/bsr_buildingprofessionapplications({dynamicsBuildingProfessionApplicationNewApplication.bsr_buildingprofessionapplicationid})"
            })
            .WithVerb(HttpMethod.Patch)
            .RespondWith(status: 204);

            //Get Application
            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_buildingprofessionapplications({dynamicsBuildingProfessionApplicationNewApplication.bsr_buildingprofessionapplicationid})")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(status: 204, body: dynamicsBuildingProfessionApplicationNewApplication with { bsr_buildingproappid = "RBCP00509D2P" });


            //Act
            var testBuildingProfessionApplication = await _dynamicsService.RegisterNewBuildingProfessionApplicationAsync(buildingProfessionApplicationModelNewApplication);

            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts")
            .WithQueryParam("$filter", $"firstname eq '{testBuildingProfessionApplication.PersonalDetails.ApplicantName.FirstName}' and lastname eq '{testBuildingProfessionApplication.PersonalDetails.ApplicantName.LastName}' and statuscode eq 1 and emailaddress1 eq '{testBuildingProfessionApplication.PersonalDetails.ApplicantEmail.Email}' and contains(telephone1, '{testBuildingProfessionApplication.PersonalDetails.ApplicantPhone.PhoneNumber.Replace("+", string.Empty).EscapeSingleQuote()}')")
            .WithQueryParam("$expand", "bsr_contacttype_contact")
            .WithOAuthBearerToken(DynamicsAuthToken)
            .WithVerb(HttpMethod.Get);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts")
            .WithRequestJson(dynamicsContactNewApplication)
            .WithVerb(HttpMethod.Post);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts({dynamicsContact.contactid})/bsr_contacttype_contact/$ref")
            .WithRequestJson(new DynamicsContactType { contactTypeReferenceId = $"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_contacttypes({DynamicsContactTypes.BIApplicant})" })
            .WithVerb(HttpMethod.Post);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_buildingprofessionapplications")
            .WithRequestJson(new DynamicsBuildingProfessionApplication(
                    bsr_applicantid: $"/contacts({dynamicsContact.contactid})",
                    bsr_buildingprofessiontypecode: BuildingProfessionType.BuildingInspector,
                    statuscode: (int)BuildingProfessionApplicationStatus.New,
                    CosmosId: testBuildingProfessionApplication.CosmosId,
                    bsr_hasindependentassessment: false))
             .WithVerb(HttpMethod.Post);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts({dynamicsContact.contactid})")
            .WithVerb(HttpMethod.Get);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts({dynamicsContact.contactid})")
            .WithRequestJson(dynamicsContactNewApplication with
            {
                contactid = dynamicsContact.contactid,
                bsr_buildingprofessionapplicationid = $"/bsr_buildingprofessionapplications({dynamicsBuildingProfessionApplicationNewApplication.bsr_buildingprofessionapplicationid})"
            }
            )
            .WithVerb(HttpMethod.Patch);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_buildingprofessionapplications({dynamicsBuildingProfessionApplicationNewApplication.bsr_buildingprofessionapplicationid})")
            .WithVerb(HttpMethod.Get);

            Assert.Equal(testBuildingProfessionApplication, buildingProfessionApplicationModelNewApplication);


        }


        [Fact]
        public async Task RegisterNewBuildingProfessionApplication_ExistingApplication()
        {

            //Arrange


            //Create contact check if existing, return existing
            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts")
            .WithQueryParam("$filter", $"firstname eq '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantName.FirstName}' and lastname eq '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantName.LastName}' and statuscode eq 1 and emailaddress1 eq '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantEmail.Email}' and contains(telephone1, '{buildingProfessionApplicationModelNewApplication.PersonalDetails.ApplicantPhone.PhoneNumber.Replace("+", string.Empty).EscapeSingleQuote()}')")
            .WithQueryParam("$expand", "bsr_contacttype_contact")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(body: new DynamicsResponse<DynamicsContact>
            {
                value = new List<DynamicsContact> { dynamicsContactNewApplication with {
                        contactid = dynamicsContact.contactid }
                    }
            });

            //For calls to check contact type return empty list
            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts({dynamicsContact.contactid})/bsr_contacttype_contact")
            .WithQueryParam("$filter", $"bsr_contacttypeid eq '{DynamicsContactTypes.BIApplicant}'")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(body: new DynamicsResponse<DynamicsContactType>
            {
                value = new List<DynamicsContactType>()
            
            });


            //Create Building Profession Application return 412 as application exists
            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_buildingprofessionapplications")
            .WithRequestJson(new DynamicsBuildingProfessionApplication(
                    bsr_applicantid: $"/contacts({dynamicsContact.contactid})",
                    bsr_buildingprofessiontypecode: BuildingProfessionType.BuildingInspector,
                    statuscode: (int)BuildingProfessionApplicationStatus.New,
                    CosmosId: buildingProfessionApplicationModelNewApplication.CosmosId,
                    bsr_hasindependentassessment: false))
             .WithVerb(HttpMethod.Post)
             .RespondWith(status: 412);

            //Return existing application
            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_buildingprofessionapplications")
            .WithQueryParam("$filter", $"bsr_cosmosid eq '{dynamicsBuildingProfessionApplicationNewApplication.CosmosId}'")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(body: new DynamicsResponse<DynamicsBuildingProfessionApplication>
            {
                value = new List<DynamicsBuildingProfessionApplication> { dynamicsBuildingProfessionApplicationNewApplication }
            });

            //Get Contact ID for new contact
            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts({dynamicsContact.contactid})")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(dynamicsContactNewApplication with { contactid = dynamicsContact.contactid });

            //Update Application with contact id
            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts({dynamicsContact.contactid})")
            .WithRequestJson(dynamicsContactNewApplication with
            {
                contactid = dynamicsContact.contactid,
                bsr_buildingprofessionapplicationid = $"/bsr_buildingprofessionapplications({dynamicsBuildingProfessionApplicationNewApplication.bsr_buildingprofessionapplicationid})"
            })
            .WithVerb(HttpMethod.Patch)
            .RespondWith(status: 204);

            //Get Application
            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_buildingprofessionapplications({dynamicsBuildingProfessionApplicationNewApplication.bsr_buildingprofessionapplicationid})")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(status: 204, body: dynamicsBuildingProfessionApplicationNewApplication with { bsr_buildingproappid = "RBCP00509D2P" });


            //Act
            var testBuildingProfessionApplication = await _dynamicsService.RegisterNewBuildingProfessionApplicationAsync(buildingProfessionApplicationModelNewApplication);

            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts")
            .WithQueryParam("$filter", $"firstname eq '{testBuildingProfessionApplication.PersonalDetails.ApplicantName.FirstName}' and lastname eq '{testBuildingProfessionApplication.PersonalDetails.ApplicantName.LastName}' and statuscode eq 1 and emailaddress1 eq '{testBuildingProfessionApplication.PersonalDetails.ApplicantEmail.Email}' and contains(telephone1, '{testBuildingProfessionApplication.PersonalDetails.ApplicantPhone.PhoneNumber.Replace("+", string.Empty).EscapeSingleQuote()}')")
            .WithQueryParam("$expand", "bsr_contacttype_contact")
            .WithOAuthBearerToken(DynamicsAuthToken)
            .WithVerb(HttpMethod.Get);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_buildingprofessionapplications")
            .WithRequestJson(new DynamicsBuildingProfessionApplication(
                    bsr_applicantid: $"/contacts({dynamicsContact.contactid})",
                    bsr_buildingprofessiontypecode: BuildingProfessionType.BuildingInspector,
                    statuscode: (int)BuildingProfessionApplicationStatus.New,
                    CosmosId: testBuildingProfessionApplication.CosmosId,
                    bsr_hasindependentassessment: false))
             .WithVerb(HttpMethod.Post);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_buildingprofessionapplications")
            .WithQueryParam("$filter", $"bsr_cosmosid eq '{dynamicsBuildingProfessionApplicationNewApplication.CosmosId}'")
            .WithVerb(HttpMethod.Get);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts({dynamicsContact.contactid})")
            .WithVerb(HttpMethod.Get);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/contacts({dynamicsContact.contactid})")
            .WithRequestJson(dynamicsContactNewApplication with
            {
                contactid = dynamicsContact.contactid,
                bsr_buildingprofessionapplicationid = $"/bsr_buildingprofessionapplications({dynamicsBuildingProfessionApplicationNewApplication.bsr_buildingprofessionapplicationid})"
            }
            )
            .WithVerb(HttpMethod.Patch);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_buildingprofessionapplications({dynamicsBuildingProfessionApplicationNewApplication.bsr_buildingprofessionapplicationid})")
            .WithVerb(HttpMethod.Get);

            Assert.Equal(testBuildingProfessionApplication, buildingProfessionApplicationModelNewApplication);


        }

        [Fact]
        public async Task GetRegistrationClassesUsingApplicationId_NoClassesExist()
        {

            //Arrange
            var testBuildingProfessionApplication = buildingProfessionApplicationModelNewApplication with { InspectorClass = dynamicsServiceInspectorClassConfigrations.Class1 };
            var testDynamicsBuildingProfessionApplication = dynamicsBuildingProfessionApplicationNewApplication;

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregclasses")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{testDynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid}'")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(body: new DynamicsResponse<DynamicsBuildingInspectorRegistrationClass>
            {
                value = new List<DynamicsBuildingInspectorRegistrationClass>()

            });

            //Act

            var classes = await _dynamicsService.GetRegistrationClassesUsingApplicationId(testDynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);



            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregclasses")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{testDynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid}'")
            .WithVerb(HttpMethod.Get);

            Assert.Empty(classes);

        }

        [Fact]
        public async Task GetRegistrationClassesUsingApplicationId_ClassesExist()
        {

            //Arrange
            var testBuildingProfessionApplication = buildingProfessionApplicationModelNewApplication with { InspectorClass = dynamicsServiceInspectorClassConfigrations.Class1 };
            var testDynamicsBuildingProfessionApplication = dynamicsBuildingProfessionApplicationNewApplication;

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregclasses")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{testDynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid}'")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(body: new DynamicsResponse<DynamicsBuildingInspectorRegistrationClass>
            {
                value = new List<DynamicsBuildingInspectorRegistrationClass>()
            {

                new DynamicsBuildingInspectorRegistrationClass
                {
                    bsr_biregclassid = "dd31ab1b-b671-ee11-8179-0022481b5210",
                    _bsr_biapplicationid_value = testDynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid,
                    _bsr_biclassid_value = BuildingInspectorClassNames.Ids[1],
                    _bsr_buildinginspectorid_value = testDynamicsBuildingProfessionApplication._bsr_applicantid_value,
                    bsr_name = "Class 1 trainee building inspector",
                    statuscode = (int)BuildingInspectorRegistrationClassStatus.Registered,
                    statecode = 0
                }
            }
            });

            //Act

            var classes = await _dynamicsService.GetRegistrationClassesUsingApplicationId(testDynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);



            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregclasses")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{testDynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid}'")
            .WithVerb(HttpMethod.Get);

            Assert.Single(classes);

        }

        [Fact]
        public async Task UpdateBuildingProfessionApplication()
        {

            //Arrange
            var existingDynamicsApplication = dynamicsBuildingProfessionApplicationNewApplication;
            var updatedExistingDynamicsApplication = dynamicsBuildingProfessionApplicationNewApplication with { bsr_hasindependentassessment = false };

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_buildingprofessionapplications({dynamicsBuildingProfessionApplicationNewApplication.bsr_buildingprofessionapplicationid})")
            .WithRequestJson(updatedExistingDynamicsApplication)
            .WithVerb(HttpMethod.Patch)
            .RespondWith(status: 204);


            //Act

            await _dynamicsService.UpdateBuildingProfessionApplication(existingDynamicsApplication, updatedExistingDynamicsApplication);



            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_buildingprofessionapplications({dynamicsBuildingProfessionApplicationNewApplication.bsr_buildingprofessionapplicationid})")
            .WithRequestJson(updatedExistingDynamicsApplication);



        }

        public static IEnumerable<object[]> ClassRegistrationNoExisting =>
        new List<object[]>
        {
                new object[] { dynamicsServiceInspectorClassConfigrations.Class1Registration with { Id = null}, dynamicsServiceInspectorClassConfigrations.Class1DynamicsRegistration },
                new object[] { dynamicsServiceInspectorClassConfigrations.Class2Registration with { Id = null}, dynamicsServiceInspectorClassConfigrations.Class2DynamicsRegistration },
                new object[] { dynamicsServiceInspectorClassConfigrations.Class3Registration with { Id = null}, dynamicsServiceInspectorClassConfigrations.Class3DynamicsRegistration },
                new object[] { dynamicsServiceInspectorClassConfigrations.Class4Registration with { Id = null}, dynamicsServiceInspectorClassConfigrations.Class4DynamicsRegistration }
        };

        public static IEnumerable<object[]> ClassRegistrationExisting =>
        new List<object[]>
        {
               new object[] { dynamicsServiceInspectorClassConfigrations.Class1Registration with { Id = null, StatusCode = (int)BuildingInspectorRegistrationClassStatus.Registered }, dynamicsServiceInspectorClassConfigrations.Class1DynamicsRegistration },
               new object[] { dynamicsServiceInspectorClassConfigrations.Class2Registration with { Id = null, StatusCode = (int)BuildingInspectorRegistrationClassStatus.Registered }, dynamicsServiceInspectorClassConfigrations.Class2DynamicsRegistration },
               new object[] { dynamicsServiceInspectorClassConfigrations.Class3Registration with { Id = null, StatusCode = (int)BuildingInspectorRegistrationClassStatus.Registered }, dynamicsServiceInspectorClassConfigrations.Class3DynamicsRegistration },
               new object[] { dynamicsServiceInspectorClassConfigrations.Class4Registration with { Id = null, StatusCode = (int)BuildingInspectorRegistrationClassStatus.Registered }, dynamicsServiceInspectorClassConfigrations.Class4DynamicsRegistration }
        };

        public static IEnumerable<object[]> ClassRegistrationExistingIdProvided =>
        new List<object[]>
        {
               new object[] { dynamicsServiceInspectorClassConfigrations.Class1Registration with { StatusCode = (int)BuildingInspectorRegistrationClassStatus.Registered }, dynamicsServiceInspectorClassConfigrations.Class1DynamicsRegistration },
               new object[] { dynamicsServiceInspectorClassConfigrations.Class2Registration with { StatusCode = (int)BuildingInspectorRegistrationClassStatus.Registered }, dynamicsServiceInspectorClassConfigrations.Class2DynamicsRegistration },
               new object[] { dynamicsServiceInspectorClassConfigrations.Class3Registration with { StatusCode = (int)BuildingInspectorRegistrationClassStatus.Registered }, dynamicsServiceInspectorClassConfigrations.Class3DynamicsRegistration },
               new object[] { dynamicsServiceInspectorClassConfigrations.Class4Registration with { StatusCode = (int)BuildingInspectorRegistrationClassStatus.Registered }, dynamicsServiceInspectorClassConfigrations.Class4DynamicsRegistration }
        };

        [Theory]
        [MemberData(nameof(ClassRegistrationNoExisting))]
        public async Task CreateOrUpdateRegistrationClass_NoExistingRegistration(BuildingInspectorRegistrationClass buildingInspectorRegistrationClass, DynamicsBuildingInspectorRegistrationClass dynamicsBuildingInspectorRegistrationClass)
        {
            //Arrange

            var testDynamicsBuildingInspectorRegistraionClass = new DynamicsBuildingInspectorRegistrationClass(
            buidingProfessionApplicationReferenceId: $"/bsr_buildingprofessionapplications({buildingInspectorRegistrationClass.BuildingProfessionApplicationId})",
            contactRefId: $"/contacts({buildingInspectorRegistrationClass.ApplicantId})",
            classRef: $"/bsr_biclasses({buildingInspectorRegistrationClass.ClassId})",
            statuscode: buildingInspectorRegistrationClass.StatusCode,
            statecode: buildingInspectorRegistrationClass.StateCode
            );

            //Create contact check if existing, return existing
            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregclasses")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{buildingInspectorRegistrationClass.BuildingProfessionApplicationId}' and _bsr_biclassid_value eq '{buildingInspectorRegistrationClass.ClassId}'")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(body: new DynamicsResponse<DynamicsBuildingInspectorRegistrationClass>
            {
                value = new List<DynamicsBuildingInspectorRegistrationClass>()
            });


            //Create Building Profession Application return 412 as application exists
            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregclasses")
            .WithRequestJson(testDynamicsBuildingInspectorRegistraionClass)
            .WithVerb(HttpMethod.Post)
            .RespondWith(status: 204, headers: BuildODataEntityHeader(dynamicsBuildingInspectorRegistrationClass.bsr_biregclassid));

            //Act
            var testRegisterClass = await _dynamicsService.CreateOrUpdateRegistrationClass(buildingInspectorRegistrationClass);

            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregclasses")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{testRegisterClass.BuildingProfessionApplicationId}' and _bsr_biclassid_value eq '{testRegisterClass.ClassId}'")
            .WithVerb(HttpMethod.Get);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregclasses")
            .WithRequestJson(testDynamicsBuildingInspectorRegistraionClass)
            .WithVerb(HttpMethod.Post);

            Assert.Equal(testRegisterClass, buildingInspectorRegistrationClass with { Id = dynamicsBuildingInspectorRegistrationClass.bsr_biregclassid });
        }

        [Theory]
        [MemberData(nameof(ClassRegistrationExisting))]
        public async Task CreateOrUpdateRegistrationClass_ExistingRegistration(BuildingInspectorRegistrationClass buildingInspectorRegistrationClass, DynamicsBuildingInspectorRegistrationClass dynamicsBuildingInspectorRegistrationClass)
        {
            //Arrange

            var testDynamicsBuildingInspectorRegistraionClass = new DynamicsBuildingInspectorRegistrationClass(
            buidingProfessionApplicationReferenceId: $"/bsr_buildingprofessionapplications({buildingInspectorRegistrationClass.BuildingProfessionApplicationId})",
            contactRefId: $"/contacts({buildingInspectorRegistrationClass.ApplicantId})",
            classRef: $"/bsr_biclasses({buildingInspectorRegistrationClass.ClassId})",
            statuscode: buildingInspectorRegistrationClass.StatusCode,
            statecode: buildingInspectorRegistrationClass.StateCode
            );

            //Create contact check if existing, return existing
            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregclasses")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{buildingInspectorRegistrationClass.BuildingProfessionApplicationId}' and _bsr_biclassid_value eq '{buildingInspectorRegistrationClass.ClassId}'")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(body: new DynamicsResponse<DynamicsBuildingInspectorRegistrationClass>
            {
                value = new List<DynamicsBuildingInspectorRegistrationClass>()
                {
                    dynamicsBuildingInspectorRegistrationClass
                }
            });



            //Create Building Profession Application return 412 as application exists
            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregclasses({dynamicsBuildingInspectorRegistrationClass.bsr_biregclassid})")
            .WithRequestJson(testDynamicsBuildingInspectorRegistraionClass)
            .WithVerb(HttpMethod.Patch)
            .RespondWith(status: 204, headers: BuildODataEntityHeader(dynamicsBuildingInspectorRegistrationClass.bsr_biregclassid));

            //Act
            var testRegisterClass = await _dynamicsService.CreateOrUpdateRegistrationClass(buildingInspectorRegistrationClass);

            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregclasses")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{testRegisterClass.BuildingProfessionApplicationId}' and _bsr_biclassid_value eq '{testRegisterClass.ClassId}'")
            .WithVerb(HttpMethod.Get);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregclasses({dynamicsBuildingInspectorRegistrationClass.bsr_biregclassid})")
            .WithRequestJson(testDynamicsBuildingInspectorRegistraionClass)
            .WithVerb(HttpMethod.Patch);

            Assert.Equal(testRegisterClass, buildingInspectorRegistrationClass with { Id = dynamicsBuildingInspectorRegistrationClass.bsr_biregclassid });
            Assert.Equal(testRegisterClass.StatusCode, buildingInspectorRegistrationClass.StatusCode);
        }

        [Theory]
        [MemberData(nameof(ClassRegistrationExistingIdProvided))]
        public async Task CreateOrUpdateRegistrationClass_ExistingRegistrationIdProvided(BuildingInspectorRegistrationClass buildingInspectorRegistrationClass, DynamicsBuildingInspectorRegistrationClass dynamicsBuildingInspectorRegistrationClass)
        {
            //Arrange

            var testDynamicsBuildingInspectorRegistraionClass = new DynamicsBuildingInspectorRegistrationClass(
            buidingProfessionApplicationReferenceId: $"/bsr_buildingprofessionapplications({buildingInspectorRegistrationClass.BuildingProfessionApplicationId})",
            contactRefId: $"/contacts({buildingInspectorRegistrationClass.ApplicantId})",
            classRef: $"/bsr_biclasses({buildingInspectorRegistrationClass.ClassId})",
            statuscode: buildingInspectorRegistrationClass.StatusCode,
            statecode: buildingInspectorRegistrationClass.StateCode
            );

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregclasses({dynamicsBuildingInspectorRegistrationClass.bsr_biregclassid})")
            .WithRequestJson(testDynamicsBuildingInspectorRegistraionClass)
            .WithVerb(HttpMethod.Patch)
            .RespondWith(status: 204, headers: BuildODataEntityHeader(dynamicsBuildingInspectorRegistrationClass.bsr_biregclassid));

            //Act
            var testRegisterClass = await _dynamicsService.CreateOrUpdateRegistrationClass(buildingInspectorRegistrationClass);

            //Assert

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregclasses({dynamicsBuildingInspectorRegistrationClass.bsr_biregclassid})")
            .WithRequestJson(testDynamicsBuildingInspectorRegistraionClass)
            .WithVerb(HttpMethod.Patch);

            Assert.Equal(testRegisterClass, buildingInspectorRegistrationClass with { Id = dynamicsBuildingInspectorRegistrationClass.bsr_biregclassid });
            Assert.Equal(testRegisterClass.StatusCode, buildingInspectorRegistrationClass.StatusCode);
        }

        [Fact]
        public async Task GetRegistrationCountriesUsingApplicationId_CountriesExisting()
        {

            //Arrange
            var testBuildingProfessionApplication = buildingProfessionApplicationModelNewApplication with { InspectorClass = dynamicsServiceInspectorClassConfigrations.Class1 };
            var testDynamicsBuildingProfessionApplication = dynamicsBuildingProfessionApplicationNewApplication;

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregcountries")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{testDynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid}'")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(body: new DynamicsResponse<DynamicsBuildingInspectorRegistrationCountry>
            {
                value = new List<DynamicsBuildingInspectorRegistrationCountry>()
            {
                dynamicsServiceInspectorClassConfigrations.BIRegistrationCountryEngland,
                dynamicsServiceInspectorClassConfigrations.BIRegistrationCountryWales
            }
            });

            //Act

            var countries = await _dynamicsService.GetRegistrationCountriesUsingApplicationId(testDynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);



            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregcountries")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{testDynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid}'")
            .WithVerb(HttpMethod.Get);

            Assert.Equal(2, countries.Count);

        }

        [Fact]
        public async Task GetRegistrationCountriesUsingApplicationId_CountriesNoExisting()
        {

            //Arrange
            var testBuildingProfessionApplication = buildingProfessionApplicationModelNewApplication with { InspectorClass = dynamicsServiceInspectorClassConfigrations.Class1 };
            var testDynamicsBuildingProfessionApplication = dynamicsBuildingProfessionApplicationNewApplication;

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregcountries")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{testDynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid}'")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(body: new DynamicsResponse<DynamicsBuildingInspectorRegistrationCountry>
            {
                value = new List<DynamicsBuildingInspectorRegistrationCountry>()
            });

            //Act

            var countries = await _dynamicsService.GetRegistrationCountriesUsingApplicationId(testDynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);



            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregcountries")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{testDynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid}'")
            .WithVerb(HttpMethod.Get);

            Assert.Empty(countries);

        }

        public static IEnumerable<object[]> CountryRegistrationNoExisting =>
        new List<object[]>
        {
            new object[] { dynamicsServiceInspectorClassConfigrations.BICountryEngland with { Id = null}, dynamicsServiceInspectorClassConfigrations.BIRegistrationCountryEngland },
            new object[] { dynamicsServiceInspectorClassConfigrations.BICountryWales with { Id = null}, dynamicsServiceInspectorClassConfigrations.BIRegistrationCountryWales }

        };

        public static IEnumerable<object[]> CountryRegistrationExisting =>
        new List<object[]>
        {
            new object[] { dynamicsServiceInspectorClassConfigrations.BICountryEngland with {Id = null, StatusCode=2,StateCode=1 }, dynamicsServiceInspectorClassConfigrations.BIRegistrationCountryEngland },
            new object[] { dynamicsServiceInspectorClassConfigrations.BICountryWales with { Id = null, StatusCode = 2, StateCode=1 }, dynamicsServiceInspectorClassConfigrations.BIRegistrationCountryWales },
        };

        public static IEnumerable<object[]> CountryRegistrationExistingIdProvided =>
        new List<object[]>
        {
            new object[] { dynamicsServiceInspectorClassConfigrations.BICountryEngland with { StatusCode=2,StateCode=1 }, dynamicsServiceInspectorClassConfigrations.BIRegistrationCountryEngland },
            new object[] { dynamicsServiceInspectorClassConfigrations.BICountryWales with { StatusCode = 2, StateCode=1 }, dynamicsServiceInspectorClassConfigrations.BIRegistrationCountryWales },
        };

        [Theory]
        [MemberData(nameof(CountryRegistrationNoExisting))]
        public async Task CreateOrUpdateRegistrationCountry_NoExistingRegistration(BuildingInspectorRegistrationCountry buildingInspectorRegistrationCountry, DynamicsBuildingInspectorRegistrationCountry dynamicsBuildingInspectorRegistrationCountry)
        {
            //Arrange

            var testDynamicsBuildingInspectorRegistraionCountry = new DynamicsBuildingInspectorRegistrationCountry(
                buidingProfessionApplicationReferenceId: $"/bsr_buildingprofessionapplications({buildingInspectorRegistrationCountry.BuildingProfessionApplicationId})",
                contactRefId: $"/contacts({buildingInspectorRegistrationCountry.BuildingInspectorId})",
                countryRefId: $"/bsr_countries({buildingInspectorRegistrationCountry.CountryID})",
                statuscode: buildingInspectorRegistrationCountry.StatusCode,
                statecode: buildingInspectorRegistrationCountry.StateCode
                );


            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregcountries")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{buildingInspectorRegistrationCountry.BuildingProfessionApplicationId}' and _bsr_countryid_value eq '{buildingInspectorRegistrationCountry.CountryID}'")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(body: new DynamicsResponse<DynamicsBuildingInspectorRegistrationCountry>
            {
                value = new List<DynamicsBuildingInspectorRegistrationCountry>()
            });


            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregcountries")
            .WithRequestJson(testDynamicsBuildingInspectorRegistraionCountry)
            .WithVerb(HttpMethod.Post)
            .RespondWith(status: 204, headers: BuildODataEntityHeader(dynamicsBuildingInspectorRegistrationCountry.bsr_biregcountryid));

            //Act
            var testRegisterCountry = await _dynamicsService.CreateOrUpdateRegistrationCountry(buildingInspectorRegistrationCountry);

            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregcountries")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{testRegisterCountry.BuildingProfessionApplicationId}' and _bsr_countryid_value eq '{testRegisterCountry.CountryID}'")
            .WithVerb(HttpMethod.Get);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregcountries")
            .WithRequestJson(testDynamicsBuildingInspectorRegistraionCountry)
            .WithVerb(HttpMethod.Post);

            Assert.Equal(testRegisterCountry, buildingInspectorRegistrationCountry with { Id = dynamicsBuildingInspectorRegistrationCountry.bsr_biregcountryid });
        }

        [Theory]
        [MemberData(nameof(CountryRegistrationExisting))]
        public async Task CreateOrUpdateRegistrationCountry_ExistingRegistration(BuildingInspectorRegistrationCountry buildingInspectorRegistrationCountry, DynamicsBuildingInspectorRegistrationCountry dynamicsBuildingInspectorRegistrationCountry)
        {
            //Arrange

            var testDynamicsBuildingInspectorRegistraionCountry = new DynamicsBuildingInspectorRegistrationCountry(
                buidingProfessionApplicationReferenceId: $"/bsr_buildingprofessionapplications({buildingInspectorRegistrationCountry.BuildingProfessionApplicationId})",
                contactRefId: $"/contacts({buildingInspectorRegistrationCountry.BuildingInspectorId})",
                countryRefId: $"/bsr_countries({buildingInspectorRegistrationCountry.CountryID})",
                statuscode: buildingInspectorRegistrationCountry.StatusCode,
                statecode: buildingInspectorRegistrationCountry.StateCode
                );

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregcountries")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{buildingInspectorRegistrationCountry.BuildingProfessionApplicationId}' and _bsr_countryid_value eq '{buildingInspectorRegistrationCountry.CountryID}'")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(body: new DynamicsResponse<DynamicsBuildingInspectorRegistrationCountry>
            {
                value = new List<DynamicsBuildingInspectorRegistrationCountry>()
                {
                    dynamicsBuildingInspectorRegistrationCountry
                }
            });


            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregcountries({dynamicsBuildingInspectorRegistrationCountry.bsr_biregcountryid})")
            .WithRequestJson(testDynamicsBuildingInspectorRegistraionCountry)
            .WithVerb(HttpMethod.Patch)
            .RespondWith(status: 204, headers: BuildODataEntityHeader(dynamicsBuildingInspectorRegistrationCountry.bsr_biregcountryid));

            //Act
            var testRegisterCountry = await _dynamicsService.CreateOrUpdateRegistrationCountry(buildingInspectorRegistrationCountry);

            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregcountries")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{buildingInspectorRegistrationCountry.BuildingProfessionApplicationId}' and _bsr_countryid_value eq '{buildingInspectorRegistrationCountry.CountryID}'")
            .WithVerb(HttpMethod.Get);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregcountries({dynamicsBuildingInspectorRegistrationCountry.bsr_biregcountryid})")
            .WithRequestJson(testDynamicsBuildingInspectorRegistraionCountry)
            .WithVerb(HttpMethod.Patch);

            Assert.Equal(testRegisterCountry, buildingInspectorRegistrationCountry with { Id = dynamicsBuildingInspectorRegistrationCountry.bsr_biregcountryid });
            Assert.Equal(testRegisterCountry.StatusCode, buildingInspectorRegistrationCountry.StatusCode);
            Assert.Equal(testRegisterCountry.StateCode, buildingInspectorRegistrationCountry.StateCode);
        }

        [Theory]
        [MemberData(nameof(CountryRegistrationExistingIdProvided))]
        public async Task CreateOrUpdateRegistrationCountry_ExistingRegistrationIdProvided(BuildingInspectorRegistrationCountry buildingInspectorRegistrationCountry, DynamicsBuildingInspectorRegistrationCountry dynamicsBuildingInspectorRegistrationCountry)
        {
            //Arrange

            var testDynamicsBuildingInspectorRegistraionCountry = new DynamicsBuildingInspectorRegistrationCountry(
                buidingProfessionApplicationReferenceId: $"/bsr_buildingprofessionapplications({buildingInspectorRegistrationCountry.BuildingProfessionApplicationId})",
                contactRefId: $"/contacts({buildingInspectorRegistrationCountry.BuildingInspectorId})",
                countryRefId: $"/bsr_countries({buildingInspectorRegistrationCountry.CountryID})",
                statuscode: buildingInspectorRegistrationCountry.StatusCode,
                statecode: buildingInspectorRegistrationCountry.StateCode
                );



            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregcountries({dynamicsBuildingInspectorRegistrationCountry.bsr_biregcountryid})")
            .WithRequestJson(testDynamicsBuildingInspectorRegistraionCountry)
            .WithVerb(HttpMethod.Patch)
            .RespondWith(status: 204, headers: BuildODataEntityHeader(dynamicsBuildingInspectorRegistrationCountry.bsr_biregcountryid));

            //Act
            var testRegisterCountry = await _dynamicsService.CreateOrUpdateRegistrationCountry(buildingInspectorRegistrationCountry);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregcountries({dynamicsBuildingInspectorRegistrationCountry.bsr_biregcountryid})")
            .WithRequestJson(testDynamicsBuildingInspectorRegistraionCountry)
            .WithVerb(HttpMethod.Patch);

            Assert.Equal(testRegisterCountry, buildingInspectorRegistrationCountry with { Id = dynamicsBuildingInspectorRegistrationCountry.bsr_biregcountryid });
            Assert.Equal(testRegisterCountry.StatusCode, buildingInspectorRegistrationCountry.StatusCode);
            Assert.Equal(testRegisterCountry.StateCode, buildingInspectorRegistrationCountry.StateCode);
        }



        public static IEnumerable<object[]> ActivitiesNoExisting =>
        new List<object[]>
        {
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans2A with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans2A },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans2B with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans2B },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans2C with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans2C },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans2D with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans2D },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans2E with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans2E },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans2F with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans2F },

            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings2A with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings2A },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings2B with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings2B },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings2C with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings2C },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings2D with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings2D },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings2E with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings2E },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings2F with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings2F },
            
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans3A with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans3A },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans3B with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans3B },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans3C with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans3C },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans3D with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans3D },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans3E with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans3E },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans3F with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans3F },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans3G with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans3G },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans3H with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans3H },

            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings3A with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings3A },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings3B with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings3B },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings3C with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings3C },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings3D with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings3D },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings3E with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings3E },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings3F with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings3F },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings3G with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings3G },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings3H with { Id = null}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings3H },

        };
        public static IEnumerable<object[]> ActivitiesExisting =>
        new List<object[]>
        {
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans2A with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans2A },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans2B with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans2B },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans2C with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans2C },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans2D with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans2D },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans2E with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans2E },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans2F with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans2F },

            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings2A with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings2A },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings2B with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings2B },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings2C with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings2C },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings2D with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings2D },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings2E with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings2E },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings2F with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings2F },

            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans3A with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans3A },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans3B with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans3B },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans3C with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans3C },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans3D with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans3D },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans3E with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans3E },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans3F with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans3F },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans3G with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans3G },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans3H with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans3H },

            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings3A with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings3A },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings3B with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings3B },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings3C with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings3C },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings3D with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings3D },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings3E with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings3E },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings3F with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings3F },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings3G with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings3G },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings3H with { Id = null, StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings3H },

        };
        public static IEnumerable<object[]> ActivitiesExistingIdProvided =>
        new List<object[]>
        {
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans2A with { StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans2A },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans2B with { StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans2B },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans2C with { StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans2C },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans2D with { StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans2D },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans2E with { StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans2E },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans2F with { StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans2F },

            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings2A with { StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings2A },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings2B with { StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings2B },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings2C with { StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings2C },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings2D with { StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings2D },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings2E with { StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings2E },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings2F with { StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings2F },

            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans3A with {StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans3A },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans3B with {StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans3B },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans3C with {StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans3C },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans3D with {StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans3D },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans3E with {StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans3E },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans3F with {StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans3F },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans3G with {StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans3G },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityAssessingPlans3H with {StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityAssessingPlans3H },

            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings3A with { StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings3A },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings3B with { StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings3B },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings3C with { StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings3C },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings3D with { StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings3D },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings3E with { StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings3E },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings3F with { StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings3F },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings3G with { StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings3G },
            new object[] { dynamicsServiceInspectorClassConfigrations.BIActivityInspectBuildings3H with { StatusCode = 2, StateCode =1}, dynamicsServiceInspectorClassConfigrations.DynamicsBIActivityInspectBuildings3H },

};

        [Theory]
        [MemberData(nameof(ActivitiesNoExisting))]
        public async Task FindExistingBuildingInspectorRegistrationActivity_NoActivitiesExist(BuildingInspectorRegistrationActivity buildingInspectorRegistrationActivity, DynamicsBuildingInspectorRegistrationActivity dynamicsBuildingInspectorRegistrationActivity)
        {

            //Arrange
            var testBuildingProfessionApplication = buildingProfessionApplicationModelNewApplication with { InspectorClass = dynamicsServiceInspectorClassConfigrations.Class2 };
            var testDynamicsBuildingProfessionApplication = dynamicsBuildingProfessionApplicationNewApplication;

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregactivities")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{testDynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid}' and _bsr_biactivityid_value eq '{buildingInspectorRegistrationActivity.ActivityId}' and _bsr_bibuildingcategoryid_value eq '{buildingInspectorRegistrationActivity.BuildingCategoryId}'")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(body: new DynamicsResponse<DynamicsBuildingInspectorRegistrationActivity>
            {
                value = new List<DynamicsBuildingInspectorRegistrationActivity>()
            });

            //Act

            var activities = await _dynamicsService.FindExistingBuildingInspectorRegistrationActivity(buildingInspectorRegistrationActivity.ActivityId, buildingInspectorRegistrationActivity.BuildingCategoryId, testDynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);



            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregactivities")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{testDynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid}' and _bsr_biactivityid_value eq '{buildingInspectorRegistrationActivity.ActivityId}' and _bsr_bibuildingcategoryid_value eq '{buildingInspectorRegistrationActivity.BuildingCategoryId}'")
            .WithVerb(HttpMethod.Get);

            Assert.Null(activities);

        }

        [Theory]
        [MemberData(nameof(ActivitiesExisting))]
        public async Task FindExistingBuildingInspectorRegistrationActivity_ActivitiesExist(BuildingInspectorRegistrationActivity buildingInspectorRegistrationActivity, DynamicsBuildingInspectorRegistrationActivity dynamicsBuildingInspectorRegistrationActivity)
        {

            //Arrange
            var testBuildingProfessionApplication = buildingProfessionApplicationModelNewApplication with { InspectorClass = dynamicsServiceInspectorClassConfigrations.Class2 };
            var testDynamicsBuildingProfessionApplication = dynamicsBuildingProfessionApplicationNewApplication;

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregactivities")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{testDynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid}' and _bsr_biactivityid_value eq '{buildingInspectorRegistrationActivity.ActivityId}' and _bsr_bibuildingcategoryid_value eq '{buildingInspectorRegistrationActivity.BuildingCategoryId}'")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(body: new DynamicsResponse<DynamicsBuildingInspectorRegistrationActivity>
            {
                value = new List<DynamicsBuildingInspectorRegistrationActivity>()
                {
                    dynamicsBuildingInspectorRegistrationActivity
                }
            });

            //Act

            var activities = await _dynamicsService.FindExistingBuildingInspectorRegistrationActivity(buildingInspectorRegistrationActivity.ActivityId, buildingInspectorRegistrationActivity.BuildingCategoryId, testDynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid);



            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregactivities")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{testDynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid}' and _bsr_biactivityid_value eq '{buildingInspectorRegistrationActivity.ActivityId}' and _bsr_bibuildingcategoryid_value eq '{buildingInspectorRegistrationActivity.BuildingCategoryId}'")
            .WithVerb(HttpMethod.Get);

            Assert.NotNull(activities);

        }

        [Theory]
        [MemberData(nameof(ActivitiesNoExisting))]
        public async Task CreateOrUpdateRegistrationActivity_NoExistingRegistration(BuildingInspectorRegistrationActivity buildingInspectorRegistrationActivity, DynamicsBuildingInspectorRegistrationActivity dynamicsBuildingInspectorRegistrationActivity)
        {
            //Arrange


            var testDynamicsBuildingInspectorRegistraionActivity = new DynamicsBuildingInspectorRegistrationActivity(
                buidingProfessionApplicationReferenceId: $"/bsr_buildingprofessionapplications({buildingInspectorRegistrationActivity.BuildingProfessionApplicationId})",
                contactRefId: $"/contacts({buildingInspectorRegistrationActivity.BuildingInspectorId})",
                buildingActivityReferenceId: $"/bsr_biactivities({buildingInspectorRegistrationActivity.ActivityId})",
                buidingCategoryReferenceId: $"/bsr_bibuildingcategories({buildingInspectorRegistrationActivity.BuildingCategoryId})",
                statuscode: buildingInspectorRegistrationActivity.StatusCode,
                statecode: buildingInspectorRegistrationActivity.StateCode
                );


            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregactivities")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{buildingInspectorRegistrationActivity.BuildingProfessionApplicationId}' and _bsr_biactivityid_value eq '{buildingInspectorRegistrationActivity.ActivityId}' and _bsr_bibuildingcategoryid_value eq '{buildingInspectorRegistrationActivity.BuildingCategoryId}'")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(body: new DynamicsResponse<DynamicsBuildingInspectorRegistrationActivity>
            {
                value = new List<DynamicsBuildingInspectorRegistrationActivity>()
            });


            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregactivities")
            .WithRequestJson(testDynamicsBuildingInspectorRegistraionActivity)
            .WithVerb(HttpMethod.Post)
            .RespondWith(status: 204, headers: BuildODataEntityHeader(dynamicsBuildingInspectorRegistrationActivity.bsr_biregactivityId));

            //Act
            var testRegisterActivity = await _dynamicsService.CreateOrUpdateRegistrationActivity(buildingInspectorRegistrationActivity);

            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregactivities")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{buildingInspectorRegistrationActivity.BuildingProfessionApplicationId}' and _bsr_biactivityid_value eq '{buildingInspectorRegistrationActivity.ActivityId}' and _bsr_bibuildingcategoryid_value eq '{buildingInspectorRegistrationActivity.BuildingCategoryId}'")
            .WithVerb(HttpMethod.Get);


            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregactivities")
            .WithRequestJson(testDynamicsBuildingInspectorRegistraionActivity)
            .WithVerb(HttpMethod.Post);

            Assert.Equal(testRegisterActivity, buildingInspectorRegistrationActivity with { Id = dynamicsBuildingInspectorRegistrationActivity.bsr_biregactivityId });
        }

        [Theory]
        [MemberData(nameof(ActivitiesExisting))]
        public async Task CreateOrUpdateRegistrationActivity_ExistingRegistration(BuildingInspectorRegistrationActivity buildingInspectorRegistrationActivity, DynamicsBuildingInspectorRegistrationActivity dynamicsBuildingInspectorRegistrationActivity)
        {
            //Arrange

            var testDynamicsBuildingInspectorRegistraionActivity = new DynamicsBuildingInspectorRegistrationActivity(
                buidingProfessionApplicationReferenceId: $"/bsr_buildingprofessionapplications({buildingInspectorRegistrationActivity.BuildingProfessionApplicationId})",
                contactRefId: $"/contacts({buildingInspectorRegistrationActivity.BuildingInspectorId})",
                buildingActivityReferenceId: $"/bsr_biactivities({buildingInspectorRegistrationActivity.ActivityId})",
                buidingCategoryReferenceId: $"/bsr_bibuildingcategories({buildingInspectorRegistrationActivity.BuildingCategoryId})",
                statuscode: buildingInspectorRegistrationActivity.StatusCode,
                statecode: buildingInspectorRegistrationActivity.StateCode
                );

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregactivities")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{buildingInspectorRegistrationActivity.BuildingProfessionApplicationId}' and _bsr_biactivityid_value eq '{buildingInspectorRegistrationActivity.ActivityId}' and _bsr_bibuildingcategoryid_value eq '{buildingInspectorRegistrationActivity.BuildingCategoryId}'")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(body: new DynamicsResponse<DynamicsBuildingInspectorRegistrationActivity>
            {
                value = new List<DynamicsBuildingInspectorRegistrationActivity>()
                { dynamicsBuildingInspectorRegistrationActivity }
            });


            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregactivities({dynamicsBuildingInspectorRegistrationActivity.bsr_biregactivityId})")
            .WithRequestJson(testDynamicsBuildingInspectorRegistraionActivity)
            .WithVerb(HttpMethod.Patch)
            .RespondWith(status: 204, headers: BuildODataEntityHeader(dynamicsBuildingInspectorRegistrationActivity.bsr_biregactivityId));

            //Act
            var testRegisterActivity = await _dynamicsService.CreateOrUpdateRegistrationActivity(buildingInspectorRegistrationActivity);

            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregactivities")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{buildingInspectorRegistrationActivity.BuildingProfessionApplicationId}' and _bsr_biactivityid_value eq '{buildingInspectorRegistrationActivity.ActivityId}' and _bsr_bibuildingcategoryid_value eq '{buildingInspectorRegistrationActivity.BuildingCategoryId}'")
            .WithVerb(HttpMethod.Get);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregactivities({dynamicsBuildingInspectorRegistrationActivity.bsr_biregactivityId})")
            .WithRequestJson(testDynamicsBuildingInspectorRegistraionActivity)
            .WithVerb(HttpMethod.Patch);

            Assert.Equal(testRegisterActivity, buildingInspectorRegistrationActivity with { Id = dynamicsBuildingInspectorRegistrationActivity.bsr_biregactivityId });
            Assert.Equal(testRegisterActivity.StatusCode, buildingInspectorRegistrationActivity.StatusCode);
            Assert.Equal(testRegisterActivity.StateCode, buildingInspectorRegistrationActivity.StateCode);
        }

        [Theory]
        [MemberData(nameof(ActivitiesExistingIdProvided))]
        public async Task CreateOrUpdateRegistrationActivity_ExistingRegistrationIdProvided(BuildingInspectorRegistrationActivity buildingInspectorRegistrationActivity, DynamicsBuildingInspectorRegistrationActivity dynamicsBuildingInspectorRegistrationActivity)
        {
            //Arrange

            var testDynamicsBuildingInspectorRegistraionActivity = new DynamicsBuildingInspectorRegistrationActivity(
                buidingProfessionApplicationReferenceId: $"/bsr_buildingprofessionapplications({buildingInspectorRegistrationActivity.BuildingProfessionApplicationId})",
                contactRefId: $"/contacts({buildingInspectorRegistrationActivity.BuildingInspectorId})",
                buildingActivityReferenceId: $"/bsr_biactivities({buildingInspectorRegistrationActivity.ActivityId})",
                buidingCategoryReferenceId: $"/bsr_bibuildingcategories({buildingInspectorRegistrationActivity.BuildingCategoryId})",
                statuscode: buildingInspectorRegistrationActivity.StatusCode,
                statecode: buildingInspectorRegistrationActivity.StateCode
                );

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregactivities({dynamicsBuildingInspectorRegistrationActivity.bsr_biregactivityId})")
            .WithRequestJson(testDynamicsBuildingInspectorRegistraionActivity)
            .WithVerb(HttpMethod.Patch)
            .RespondWith(status: 204, headers: BuildODataEntityHeader(dynamicsBuildingInspectorRegistrationActivity.bsr_biregactivityId));

            //Act
            var testRegisterActivity = await _dynamicsService.CreateOrUpdateRegistrationActivity(buildingInspectorRegistrationActivity);

            //Assert

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biregactivities({dynamicsBuildingInspectorRegistrationActivity.bsr_biregactivityId})")
            .WithRequestJson(testDynamicsBuildingInspectorRegistraionActivity)
            .WithVerb(HttpMethod.Patch);

            Assert.Equal(testRegisterActivity, buildingInspectorRegistrationActivity with { Id = dynamicsBuildingInspectorRegistrationActivity.bsr_biregactivityId });
            Assert.Equal(testRegisterActivity.StatusCode, buildingInspectorRegistrationActivity.StatusCode);
            Assert.Equal(testRegisterActivity.StateCode, buildingInspectorRegistrationActivity.StateCode);
        }

        public static IEnumerable<object[]> CompetencyTestData =>
        new List<object[]>
        {
            new object[] { dynamicsServiceCompetencyTestConfigrations.BSCF, dynamicsServiceCompetencyTestConfigrations.BSCFDynamicsBuildingProfessionApplicationNewApplication },
            new object[] { dynamicsServiceCompetencyTestConfigrations.CABE, dynamicsServiceCompetencyTestConfigrations.CABEDynamicsBuildingProfessionApplicationNewApplication },
            new object[] { dynamicsServiceCompetencyTestConfigrations.TTD, dynamicsServiceCompetencyTestConfigrations.TTDDynamicsBuildingProfessionApplicationNewApplication },

        };

        [Theory]
        [MemberData(nameof(CompetencyTestData))]
        public async Task UpdateBuildingProfessionApplicationCompetency(Competency competencyModel, DynamicsBuildingProfessionApplication dynamicsBuildingProfessionApplication)
        {
            //Arrange

            var testDynamicsCompetency = dynamicsBuildingProfessionApplication with
            {
                bsr_hasindependentassessment = true,
                bsr_assessmentorganisationid = competencyModel.CompetencyAssessmentOrganisation.ComAssessmentOrganisation is null ? null : $"accounts({AssessmentOrganisationNames.Ids[competencyModel.CompetencyAssessmentOrganisation.ComAssessmentOrganisation]})",
                bsr_assessmentdate = competencyModel.CompetencyDateOfAssessment is null ? null :
                                        new DateOnly(int.Parse(competencyModel.CompetencyDateOfAssessment.Year),
                                                     int.Parse(competencyModel.CompetencyDateOfAssessment.Month),
                                                     int.Parse(competencyModel.CompetencyDateOfAssessment.Day)),
                bsr_assessmentcertnumber = competencyModel.CompetencyAssessmentCertificateNumber is null ? null : competencyModel.CompetencyAssessmentCertificateNumber.CertificateNumber,
            };

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_buildingprofessionapplications({dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid})")
            .WithRequestJson(dynamicsBuildingProfessionApplication)
            .WithVerb(HttpMethod.Patch)
            .RespondWith(status: 204);


            //Act
            await _dynamicsService.UpdateBuildingProfessionApplicationCompetency(testDynamicsCompetency, testDynamicsCompetency);

            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_buildingprofessionapplications({dynamicsBuildingProfessionApplication.bsr_buildingprofessionapplicationid})")
            .WithRequestJson(testDynamicsCompetency)
            .WithVerb(HttpMethod.Patch);
        }

        public static IEnumerable<object[]> YearTestData =>
        new List<object[]>
        {
            new object[] { dynamicsServiceProfessionalMembershipTestConfigurations.RICSApplicantProfessionalBodyMembership, dynamicsServiceProfessionalMembershipTestConfigurations.RICSDynamicsBuildingInspectorProfessionalBodyMembership },
            new object[] { dynamicsServiceProfessionalMembershipTestConfigurations.CABEApplicantProfessionalBodyMembership, dynamicsServiceProfessionalMembershipTestConfigurations.CABEDynamicsBuildingInspectorProfessionalBodyMembership },
            new object[] { dynamicsServiceProfessionalMembershipTestConfigurations.CIOBApplicantProfessionalBodyMembership, dynamicsServiceProfessionalMembershipTestConfigurations.CIOBDynamicsBuildingInspectorProfessionalBodyMembership },

        };

        [Theory]
        [MemberData(nameof(YearTestData))]
        public async Task GetDynamicsYear(ApplicantProfessionalBodyMembership applicantProfessionalBodyMembership, DynamicsBuildingInspectorProfessionalBodyMembership dynamicsBuildingInspectorProfessionalBodyMembership)
        {
            //Arrange
            DynamicsYear year = new DynamicsYear(
    bsr_name: applicantProfessionalBodyMembership.MembershipYear.ToString(),
    bsr_yearid: ProfessionalMembershipTestConfigurations.GetYear(applicantProfessionalBodyMembership.MembershipYear));


            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_years")
            .WithQueryParam("$filter", $"bsr_name eq '{applicantProfessionalBodyMembership.MembershipYear}'")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(body: new DynamicsResponse<DynamicsYear>
            {
                value = new List<DynamicsYear>()
                {
                    year         
                }
            });


            //Act

            var dynamicsYear = await _dynamicsService.GetDynamicsYear(applicantProfessionalBodyMembership.MembershipYear.ToString());



            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_years")
            .WithQueryParam("$filter", $"bsr_name eq '{applicantProfessionalBodyMembership.MembershipYear}'")
            .WithVerb(HttpMethod.Get);

            Assert.Equal(year, dynamicsYear);
            Assert.Equal(dynamicsYear.bsr_yearid, dynamicsBuildingInspectorProfessionalBodyMembership._bsr_yearid_value);

        }

        [Fact]
        public async Task GetBuildingInspectorProfessionalBodyMembershipsUsingApplicationId_MembershipsExist()
        {
            //Arrange

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biprofessionalmemberships")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{dynamicsBuildingProfessionApplicationNewApplication.bsr_buildingprofessionapplicationid}'")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(body: new DynamicsResponse<DynamicsBuildingInspectorProfessionalBodyMembership>
            {
                value = new List<DynamicsBuildingInspectorProfessionalBodyMembership>()
                {
                    dynamicsServiceProfessionalMembershipTestConfigurations.RICSDynamicsBuildingInspectorProfessionalBodyMembership,
                    dynamicsServiceProfessionalMembershipTestConfigurations.CABEDynamicsBuildingInspectorProfessionalBodyMembership,
                    dynamicsServiceProfessionalMembershipTestConfigurations.CIOBDynamicsBuildingInspectorProfessionalBodyMembership,
                    dynamicsServiceProfessionalMembershipTestConfigurations.OtherDynamicsBuildingInspectorProfessionalBodyMembership,
                }
            });


            //Act

            var professionalMemberships = await _dynamicsService.GetBuildingInspectorProfessionalBodyMembershipsUsingApplicationId(dynamicsBuildingProfessionApplicationNewApplication.bsr_buildingprofessionapplicationid);



            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biprofessionalmemberships")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{dynamicsBuildingProfessionApplicationNewApplication.bsr_buildingprofessionapplicationid}'")
            .WithVerb(HttpMethod.Get);

            Assert.NotEmpty(professionalMemberships);
            Assert.Equal(4, professionalMemberships.Count);

        }

        [Fact]
        public async Task GetBuildingInspectorProfessionalBodyMembershipsUsingApplicationId_NoMembershipsExist()
        {
            //Arrange

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biprofessionalmemberships")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{dynamicsBuildingProfessionApplicationNewApplication.bsr_buildingprofessionapplicationid}'")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(body: new DynamicsResponse<DynamicsBuildingInspectorProfessionalBodyMembership>
            {
                value = new List<DynamicsBuildingInspectorProfessionalBodyMembership>()
                {
                }
            });


            //Act

            var professionalMemberships = await _dynamicsService.GetBuildingInspectorProfessionalBodyMembershipsUsingApplicationId(dynamicsBuildingProfessionApplicationNewApplication.bsr_buildingprofessionapplicationid);



            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biprofessionalmemberships")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{dynamicsBuildingProfessionApplicationNewApplication.bsr_buildingprofessionapplicationid}'")
            .WithVerb(HttpMethod.Get);

            Assert.Empty(professionalMemberships);

        }


        public static IEnumerable<object[]> MembershipsNoExisting =>
        new List<object[]>
        {
            new object[] { dynamicsServiceProfessionalMembershipTestConfigurations.RICSBuildingProfessionApplicationBodyMembership with { Id = null}, dynamicsServiceProfessionalMembershipTestConfigurations.RICSDynamicsBuildingInspectorProfessionalBodyMembership },
            new object[] { dynamicsServiceProfessionalMembershipTestConfigurations.CABEBuildingProfessionApplicationBodyMembership with { Id = null }, dynamicsServiceProfessionalMembershipTestConfigurations.CABEDynamicsBuildingInspectorProfessionalBodyMembership },
            new object[] { dynamicsServiceProfessionalMembershipTestConfigurations.CIOBBuildingProfessionApplicationBodyMembership with { Id = null }, dynamicsServiceProfessionalMembershipTestConfigurations.CIOBDynamicsBuildingInspectorProfessionalBodyMembership },
            new object[] { dynamicsServiceProfessionalMembershipTestConfigurations.OtherBuildingProfessionApplicationBodyMembership with { Id = null }, dynamicsServiceProfessionalMembershipTestConfigurations.OtherDynamicsBuildingInspectorProfessionalBodyMembership },
        };

        public static IEnumerable<object[]> MembershipsExisting =>
        new List<object[]>
        {
            new object[] { dynamicsServiceProfessionalMembershipTestConfigurations.RICSBuildingProfessionApplicationBodyMembership with {Id = null, MembershipNumber = "9999" }, dynamicsServiceProfessionalMembershipTestConfigurations.RICSDynamicsBuildingInspectorProfessionalBodyMembership },
            new object[] { dynamicsServiceProfessionalMembershipTestConfigurations.CABEBuildingProfessionApplicationBodyMembership with { Id = null, MembershipNumber = "9999" }, dynamicsServiceProfessionalMembershipTestConfigurations.CABEDynamicsBuildingInspectorProfessionalBodyMembership },
            new object[] { dynamicsServiceProfessionalMembershipTestConfigurations.CIOBBuildingProfessionApplicationBodyMembership with { Id = null, MembershipNumber = "9999" }, dynamicsServiceProfessionalMembershipTestConfigurations.CIOBDynamicsBuildingInspectorProfessionalBodyMembership },
            new object[] { dynamicsServiceProfessionalMembershipTestConfigurations.OtherBuildingProfessionApplicationBodyMembership with { Id = null, MembershipNumber = "9999" }, dynamicsServiceProfessionalMembershipTestConfigurations.OtherDynamicsBuildingInspectorProfessionalBodyMembership },
        };


        public static IEnumerable<object[]> MembershipsExistingIdProvided =>
        new List<object[]>
        {
            new object[] { dynamicsServiceProfessionalMembershipTestConfigurations.RICSBuildingProfessionApplicationBodyMembership with {MembershipNumber = "9999" }, dynamicsServiceProfessionalMembershipTestConfigurations.RICSDynamicsBuildingInspectorProfessionalBodyMembership },
            new object[] { dynamicsServiceProfessionalMembershipTestConfigurations.CABEBuildingProfessionApplicationBodyMembership with {MembershipNumber = "9999" }, dynamicsServiceProfessionalMembershipTestConfigurations.CABEDynamicsBuildingInspectorProfessionalBodyMembership },
            new object[] { dynamicsServiceProfessionalMembershipTestConfigurations.CIOBBuildingProfessionApplicationBodyMembership with {MembershipNumber = "9999" }, dynamicsServiceProfessionalMembershipTestConfigurations.CIOBDynamicsBuildingInspectorProfessionalBodyMembership },
            new object[] { dynamicsServiceProfessionalMembershipTestConfigurations.OtherBuildingProfessionApplicationBodyMembership with {MembershipNumber = "9999" }, dynamicsServiceProfessionalMembershipTestConfigurations.OtherDynamicsBuildingInspectorProfessionalBodyMembership },
        };


        [Theory]
        [MemberData(nameof(MembershipsNoExisting))]
        public async Task CreateOrUpdateBuildingInspectorProfessionalBodyMembership_NoExistingMembership(BuildingInspectorProfessionalBodyMembership buildingInspectorProfessionalBodyMembership, DynamicsBuildingInspectorProfessionalBodyMembership dynamicsBuildingInspectorProfessionalBodyMembership)
        {
            //Arrange
            var testDynamicsBuildingInspectorProfessionalBodyMembership = new DynamicsBuildingInspectorProfessionalBodyMembership(
                buidingProfessionApplicationReferenceId: $"/bsr_buildingprofessionapplications({buildingInspectorProfessionalBodyMembership.BuildingProfessionApplicationId})",
                contactRefId: $"/contacts({buildingInspectorProfessionalBodyMembership.BuildingInspectorId})",
                professionalBodyRefId: $"/accounts({buildingInspectorProfessionalBodyMembership.ProfessionalBodyId})",
                bsr_membershipnumber: buildingInspectorProfessionalBodyMembership.MembershipNumber,
                bsr_currentmembershiplevelortype: buildingInspectorProfessionalBodyMembership.CurrentMembershipLevelOrType,
                statuscode: buildingInspectorProfessionalBodyMembership.StatusCode ?? 1,
                statecode: buildingInspectorProfessionalBodyMembership.StateCode ?? 0,
                yearRefId: buildingInspectorProfessionalBodyMembership.YearId is null ? null : $"/bsr_years({buildingInspectorProfessionalBodyMembership.YearId})"
                );

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biprofessionalmemberships")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{buildingInspectorProfessionalBodyMembership.BuildingProfessionApplicationId}' and _bsr_professionalbodyid_value eq '{buildingInspectorProfessionalBodyMembership.ProfessionalBodyId}'")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(body: new DynamicsResponse<DynamicsBuildingInspectorProfessionalBodyMembership>
            {
                value = new List<DynamicsBuildingInspectorProfessionalBodyMembership>()
            });


            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biprofessionalmemberships")
            .WithRequestJson(testDynamicsBuildingInspectorProfessionalBodyMembership)
            .WithVerb(HttpMethod.Post)
            .RespondWith(status: 204, headers: BuildODataEntityHeader(dynamicsBuildingInspectorProfessionalBodyMembership.bsr_biprofessionalmembershipid));

            //Act
            var testRegisterMembership = await _dynamicsService.CreateOrUpdateBuildingInspectorProfessionalBodyMembership(buildingInspectorProfessionalBodyMembership);

            //Assert
            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biprofessionalmemberships")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{buildingInspectorProfessionalBodyMembership.BuildingProfessionApplicationId}' and _bsr_professionalbodyid_value eq '{buildingInspectorProfessionalBodyMembership.ProfessionalBodyId}'")
            .WithVerb(HttpMethod.Get);


            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biprofessionalmemberships")
            .WithRequestJson(testDynamicsBuildingInspectorProfessionalBodyMembership)
            .WithVerb(HttpMethod.Post);

            Assert.Equal(testRegisterMembership, buildingInspectorProfessionalBodyMembership with { Id = dynamicsBuildingInspectorProfessionalBodyMembership.bsr_biprofessionalmembershipid });
        }

        [Theory]
        [MemberData(nameof(MembershipsExisting))]
        public async Task CreateOrUpdateBuildingInspectorProfessionalBodyMembership_ExistingMembership(BuildingInspectorProfessionalBodyMembership buildingInspectorProfessionalBodyMembership, DynamicsBuildingInspectorProfessionalBodyMembership dynamicsBuildingInspectorProfessionalBodyMembership)
        {

            //Arrange
            var testDynamicsBuildingInspectorProfessionalBodyMembership = new DynamicsBuildingInspectorProfessionalBodyMembership(
                buidingProfessionApplicationReferenceId: $"/bsr_buildingprofessionapplications({buildingInspectorProfessionalBodyMembership.BuildingProfessionApplicationId})",
                contactRefId: $"/contacts({buildingInspectorProfessionalBodyMembership.BuildingInspectorId})",
                professionalBodyRefId: $"/accounts({buildingInspectorProfessionalBodyMembership.ProfessionalBodyId})",
                bsr_membershipnumber: buildingInspectorProfessionalBodyMembership.MembershipNumber,
                bsr_currentmembershiplevelortype: buildingInspectorProfessionalBodyMembership.CurrentMembershipLevelOrType,
                statuscode: buildingInspectorProfessionalBodyMembership.StatusCode ?? 1,
                statecode: buildingInspectorProfessionalBodyMembership.StateCode ?? 0,
                yearRefId: buildingInspectorProfessionalBodyMembership.YearId is null ? null : $"/bsr_years({buildingInspectorProfessionalBodyMembership.YearId})"
                );

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biprofessionalmemberships")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{buildingInspectorProfessionalBodyMembership.BuildingProfessionApplicationId}' and _bsr_professionalbodyid_value eq '{buildingInspectorProfessionalBodyMembership.ProfessionalBodyId}'")
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(body: new DynamicsResponse<DynamicsBuildingInspectorProfessionalBodyMembership>
            {
                value = new List<DynamicsBuildingInspectorProfessionalBodyMembership>()
                {
                    dynamicsBuildingInspectorProfessionalBodyMembership
                }
            });



            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biprofessionalmemberships({dynamicsBuildingInspectorProfessionalBodyMembership.bsr_biprofessionalmembershipid})")
            .WithRequestJson(testDynamicsBuildingInspectorProfessionalBodyMembership)
            .WithVerb(HttpMethod.Patch)
            .RespondWith(status: 204, headers: BuildODataEntityHeader(dynamicsBuildingInspectorProfessionalBodyMembership.bsr_biprofessionalmembershipid));

            //Act
            var testRegisterMembership = await _dynamicsService.CreateOrUpdateBuildingInspectorProfessionalBodyMembership(buildingInspectorProfessionalBodyMembership);

            //Assert
            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biprofessionalmemberships")
            .WithQueryParam("$filter", $"_bsr_biapplicationid_value eq '{buildingInspectorProfessionalBodyMembership.BuildingProfessionApplicationId}' and _bsr_professionalbodyid_value eq '{buildingInspectorProfessionalBodyMembership.ProfessionalBodyId}'")
            .WithVerb(HttpMethod.Get);

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biprofessionalmemberships({dynamicsBuildingInspectorProfessionalBodyMembership.bsr_biprofessionalmembershipid})")
            .WithRequestJson(testDynamicsBuildingInspectorProfessionalBodyMembership)
            .WithVerb(HttpMethod.Patch);

            Assert.Equal(testRegisterMembership, buildingInspectorProfessionalBodyMembership with { Id = dynamicsBuildingInspectorProfessionalBodyMembership.bsr_biprofessionalmembershipid });
            Assert.Equal(testRegisterMembership.MembershipNumber, buildingInspectorProfessionalBodyMembership.MembershipNumber);
            Assert.Equal(testRegisterMembership.MembershipNumber, buildingInspectorProfessionalBodyMembership.MembershipNumber);
        }

        [Theory]
        [MemberData(nameof(MembershipsExistingIdProvided))]
        public async Task CreateOrUpdateBuildingInspectorProfessionalBodyMembership_ExistingMembershipIdProvided(BuildingInspectorProfessionalBodyMembership buildingInspectorProfessionalBodyMembership, DynamicsBuildingInspectorProfessionalBodyMembership dynamicsBuildingInspectorProfessionalBodyMembership)
        {

            //Arrange
            var testDynamicsBuildingInspectorProfessionalBodyMembership = new DynamicsBuildingInspectorProfessionalBodyMembership(
                buidingProfessionApplicationReferenceId: $"/bsr_buildingprofessionapplications({buildingInspectorProfessionalBodyMembership.BuildingProfessionApplicationId})",
                contactRefId: $"/contacts({buildingInspectorProfessionalBodyMembership.BuildingInspectorId})",
                professionalBodyRefId: $"/accounts({buildingInspectorProfessionalBodyMembership.ProfessionalBodyId})",
                bsr_membershipnumber: buildingInspectorProfessionalBodyMembership.MembershipNumber,
                bsr_currentmembershiplevelortype: buildingInspectorProfessionalBodyMembership.CurrentMembershipLevelOrType,
                statuscode: buildingInspectorProfessionalBodyMembership.StatusCode ?? 1,
                statecode: buildingInspectorProfessionalBodyMembership.StateCode ?? 0,
                yearRefId: buildingInspectorProfessionalBodyMembership.YearId is null ? null : $"/bsr_years({buildingInspectorProfessionalBodyMembership.YearId})"
                );

            HttpTest.ForCallsTo($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biprofessionalmemberships({buildingInspectorProfessionalBodyMembership.Id})")
            .WithRequestJson(testDynamicsBuildingInspectorProfessionalBodyMembership)
            .WithVerb(HttpMethod.Patch)
            .RespondWith(status: 204, headers: BuildODataEntityHeader(dynamicsBuildingInspectorProfessionalBodyMembership.bsr_biprofessionalmembershipid));

            //Act
            var testRegisterMembership = await _dynamicsService.CreateOrUpdateBuildingInspectorProfessionalBodyMembership(buildingInspectorProfessionalBodyMembership);

            //Assert

            HttpTest.ShouldHaveCalled($"{DynamicsOptions.EnvironmentUrl}/api/data/v9.2/bsr_biprofessionalmemberships({buildingInspectorProfessionalBodyMembership.Id})")
            .WithRequestJson(testDynamicsBuildingInspectorProfessionalBodyMembership)
            .WithVerb(HttpMethod.Patch);

            Assert.Equal(testRegisterMembership, buildingInspectorProfessionalBodyMembership with { Id = dynamicsBuildingInspectorProfessionalBodyMembership.bsr_biprofessionalmembershipid });
            Assert.Equal(testRegisterMembership.MembershipNumber, buildingInspectorProfessionalBodyMembership.MembershipNumber);
            Assert.Equal(testRegisterMembership.MembershipNumber, buildingInspectorProfessionalBodyMembership.MembershipNumber);
        }




    }
}
