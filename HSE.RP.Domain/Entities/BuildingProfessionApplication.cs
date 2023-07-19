using System.Numerics;
using System.Text.Json.Serialization;

namespace HSE.RP.Domain.Entities;

public record BuildingProfessionApplication(
    string ContactId,
    string ApplicationReturnId = null,
    BuildingProfessionType? BuildingProfessionTypeCode = null,
    string Id = null
    ) : Entity(Id);

public record DynamicsBuildingProfessionApplication(
    [property: JsonPropertyName("bsr_applicantid_contact@odata.bind")]
    string bsr_applicantid = null,
    DynamicsContact bsr_applicantid_contact = null,
    BuildingProfessionType? bsr_buildingprofessiontypecode = null,
    string bsr_buildingproappid = null,
    string bsr_buildingprofessionapplicationid = null
) : DynamicsEntity<BuildingProfessionApplication>;

public enum BuildingProfessionType
{
    BuildingInspector = 760_810_000
}