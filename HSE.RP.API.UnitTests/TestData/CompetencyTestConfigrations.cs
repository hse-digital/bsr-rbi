using HSE.RP.API.Enums;
using HSE.RP.API.Models;
using HSE.RP.Domain.Entities;
using BuildingInspectorClass = HSE.RP.API.Models.BuildingInspectorClass;

namespace HSE.RP.API.UnitTests.TestData
{
    public class CompetencyTestConfigrations
    {
        public Competency CABE;
        public Competency TDD;
        public Competency BSCF;
        public Competency RICS;
        public Competency Other;

        public DynamicsBuildingProfessionApplication CABEDynamicsBuildingProfessionApplicationNewApplication;
        public DynamicsBuildingProfessionApplication TDDDynamicsBuildingProfessionApplicationNewApplication;
        public DynamicsBuildingProfessionApplication BSCFDynamicsBuildingProfessionApplicationNewApplication;

        public CompetencyTestConfigrations()
        {


            CABE = new Competency
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
            };
            TDD = new Competency
            {
                CompetencyIndependentAssessmentStatus = new CompetencyIndependentAssessmentStatus
                {
                    IAStatus = "yes",
                    CompletionState = ComponentCompletionState.Complete
                },
                CompetencyAssessmentOrganisation = new CompetencyAssessmentOrganisation
                {
                    ComAssessmentOrganisation = "TDD",
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
                    CertificateNumber = "TDD1262IJSBFAHS840",
                    CompletionState = ComponentCompletionState.Complete
                }
            };
            BSCF = new Competency
            {
                CompetencyIndependentAssessmentStatus = new CompetencyIndependentAssessmentStatus
                {
                    IAStatus = "yes",
                    CompletionState = ComponentCompletionState.Complete
                },
                CompetencyAssessmentOrganisation = new CompetencyAssessmentOrganisation
                {
                    ComAssessmentOrganisation = "BSCF",
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
                    CertificateNumber = "BSCF1262IJSBFAHS840",
                    CompletionState = ComponentCompletionState.Complete
                }
            };


            CABEDynamicsBuildingProfessionApplicationNewApplication = new DynamicsBuildingProfessionApplication
            {
                bsr_applicantid = "contacts(123456789)",
                bsr_applicantid_contact = null,
                bsr_buildingprofessiontypecode = BuildingProfessionType.BuildingInspector,
                bsr_buildingproappid = null,
                bsr_buildingprofessionapplicationid = "21ca463a-8988-ee11-be36-0022481b5210",
                bsr_assessmentorganisationid = AssessmentOrganisationNames.Ids["CABE"],
                _bsr_applicantid_value = "123456789",
                CosmosId = "6EC5ACFE-7055-4978-BF24-CE0F2E00400E",
                bsr_buildingprofessionalapplicationstage = null,
                statuscode = 760810009,
                bsr_assessmentcertnumber = "CABE1262IJSBFAHS840",
                bsr_assessmentdate = new DateOnly(2022,3,31),
                bsr_hasindependentassessment = true
            };

            TDDDynamicsBuildingProfessionApplicationNewApplication = new DynamicsBuildingProfessionApplication
            {
                bsr_applicantid = "contacts(123456789)",
                bsr_applicantid_contact = null,
                bsr_buildingprofessiontypecode = BuildingProfessionType.BuildingInspector,
                bsr_buildingproappid = null,
                bsr_buildingprofessionapplicationid = "21ca463a-8988-ee11-be36-0022481b5210",
                bsr_assessmentorganisationid = AssessmentOrganisationNames.Ids["TDD"],
                _bsr_applicantid_value = "123456789",
                CosmosId = "6EC5ACFE-7055-4978-BF24-CE0F2E00400E",
                bsr_buildingprofessionalapplicationstage = null,
                statuscode = 760810009,
                bsr_assessmentcertnumber = "TDD1262IJSBFAHS840",
                bsr_assessmentdate = new DateOnly(2022, 3, 31),
                bsr_hasindependentassessment = true
            };

            BSCFDynamicsBuildingProfessionApplicationNewApplication = new DynamicsBuildingProfessionApplication
            {
                bsr_applicantid = "contacts(123456789)",
                bsr_applicantid_contact = null,
                bsr_buildingprofessiontypecode = BuildingProfessionType.BuildingInspector,
                bsr_buildingproappid = null,
                bsr_buildingprofessionapplicationid = "21ca463a-8988-ee11-be36-0022481b5210",
                bsr_assessmentorganisationid = AssessmentOrganisationNames.Ids["BSCF"],
                _bsr_applicantid_value = "123456789",
                CosmosId = "6EC5ACFE-7055-4978-BF24-CE0F2E00400E",
                bsr_buildingprofessionalapplicationstage = null,
                statuscode = 760810009,
                bsr_assessmentcertnumber = "BSCF1262IJSBFAHS840",
                bsr_assessmentdate = new DateOnly(2022, 3, 31),
                bsr_hasindependentassessment = true
            };
        }
    }
}
