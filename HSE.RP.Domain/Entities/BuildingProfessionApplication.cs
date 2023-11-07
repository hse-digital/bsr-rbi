using System.Numerics;
using System.Text.Json.Serialization;

namespace HSE.RP.Domain.Entities;

public record BuildingProfessionApplication(
    string Id = null,
    string ContactId = null,
    string ApplicationReturnId = null,
    BuildingProfessionType? BuildingProfessionTypeCode = null,
    string AssessmentOrganisationId = null,
    string AsessmentCertificateNumber = null,
    bool? HasIndependentAssessment = false,
    DateOnly? AssessmentdDate = null,
    BuildingProfessionApplicationStatus? StatusCode = null,
    BuildingProfessionApplicationStage? BuildingProfessionApplicationStage = null

    ) : Entity(Id);

public record DynamicsBuildingProfessionApplication(
    [property: JsonPropertyName("bsr_applicantid_contact@odata.bind")]
    string bsr_applicantid = null,
    DynamicsContact bsr_applicantid_contact = null,
    BuildingProfessionType? bsr_buildingprofessiontypecode = null,
    string bsr_buildingproappid = null,
    string bsr_buildingprofessionapplicationid = null,
    [property: JsonPropertyName("bsr_assessmentorganisationid@odata.bind")]
    string bsr_assessmentorganisationid = null,
    string _bsr_applicantid_value = null,
    BuildingProfessionApplicationStage? bsr_buildingprofessionalapplicationstage = null,
    int? statuscode = null,
    string bsr_assessmentcertnumber = null,
    DateOnly? bsr_assessmentdate = null,
    bool? bsr_hasindependentassessment = null
) : DynamicsEntity<BuildingProfessionApplication>;




public enum BuildingProfessionType
{
    BuildingInspector = 760_810_000
}

public enum BuildingProfessionApplicationStage
{
    PersonalDetails = 760_810_000,
    BuildingInspectorClass = 760_810_001,
    Competency = 760810002,
    ProfessionalMembershipsAndEmployment = 760_810_003,
    ApplicationSummary = 760_810_004,
    PayAndSubmit = 760_810_005,
    ApplicationSubmitted = 760_810_006,
}


public enum BuildingProfessionApplicationStatus
{
    Ready = 760_810_002,
    InProgress = 760_810_001,
    HoldPendingIASO = 760_810_003,
    HoldPendingInformationCorrection = 760_810_004,
    Completed = 760_810_005,
    Terminated = 760_810_007,
    EscalatedToAuditTeam = 760_810_006,
    AwaitingDecisionReview = 760_810_008,
    New = 760_810_009,
    Submitted = 760_810_010,
}
