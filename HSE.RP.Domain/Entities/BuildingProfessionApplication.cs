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
    DateOnly? AssessmentdDate = null

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
    int? statuscode = null,
    string bsr_assessmentcertnumber = null,
    DateOnly? bsr_assessmentdate = null,
    bool? bsr_hasindependentassessment = null
) : DynamicsEntity<BuildingProfessionApplication>;




public enum BuildingProfessionType
{
    BuildingInspector = 760_810_000
}

public static class AssessmentOrganisationNames
{
    public static readonly IDictionary<string, string> Ids = new Dictionary<string, string>
    {
        ["BSCF"] = "4fb54d8a-262a-ee11-9965-0022481b59de",
        ["CABE"] = "f36d4890-262a-ee11-9965-0022481b59de",
        ["RICS"] = "48f60522-932c-ee11-9965-0022481b59de",
        ["CIOB"] = "689d678f-962c-ee11-9965-0022481b59de",
        ["Other"] = "65f5b095-962c-ee11-9965-0022481b59de",

    };
}