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
    DateOnly? AssessmentdDate = null

    ) : Entity(Id);

public record DynamicsBuildingProfessionApplication(
    [property: JsonPropertyName("bsr_applicantid_contact@odata.bind")]
    string bsr_applicantid = null,
    DynamicsContact bsr_applicantid_contact = null,
    BuildingProfessionType? bsr_buildingprofessiontypecode = null,
    string bsr_buildingproappid = null,
    string bsr_buildingprofessionapplicationid = null,
    bool? bsr_hasindependentassessment = null,
    [property: JsonPropertyName("bsr_assessmentorganisationid@odata.bind")]
    string bsr_assessmentorganisationid = null,
    string statuscode = null,
    string bsr_assessmentcertnumber = null,
    DateOnly? bsr_assessmentdate = null
) : DynamicsEntity<BuildingProfessionApplication>;

public enum BuildingProfessionType
{
    BuildingInspector = 760_810_000
}