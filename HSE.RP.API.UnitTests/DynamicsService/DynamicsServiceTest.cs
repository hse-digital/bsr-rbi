using HSE.RP.API.Enums;
using HSE.RP.API.Models;
using HSE.RP.API.Models.DynamicsSynchronisation;
using HSE.RP.API.Services;
using HSE.RP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE.RP.API.UnitTests.DynamicsService
{
    public class DynamicsServiceTest : UnitTestBase
    {
        private readonly IDynamicsService _dynamicsService;
        private BuildingProfessionApplicationModel buildingProfessionApplicationModel;

        private DynamicsContact dynamicsContact;
        private DynamicsAccount dynamicsAccount;


        public DynamicsServiceTest()
        {
            _dynamicsService = DynamicsService;

            buildingProfessionApplicationModel = new BuildingProfessionApplicationModel
            {
                Id = "RBCP00509D2P",
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

            dynamicsContact = new DynamicsContact
            {
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
                birthdate = new DateOnly(1990,06,06),
                bsr_nationalinsuranceno = "QQ 12 34 56 C"
            };
        }
    }
 
}
