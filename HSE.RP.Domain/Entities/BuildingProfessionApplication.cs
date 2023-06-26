using System.Numerics;
using System.Text.Json.Serialization;

namespace HSE.RP.Domain.Entities;

public record BuildingProfessionApplication(
    string ContactId,
    string Id = null) : Entity(Id);

public record DynamicsBuildingProfessionApplication(
    [property: JsonPropertyName("_bsr_applicantid_value@odata.bind")]
    string contactReferenceId = null,
    int? bsr_buildingprofessiontypecode = null,
    string bsr_buildingproappid = null
) : DynamicsEntity<BuildingProfessionApplication>;

public static class DynamicsBuildingProfessionTypeCode
{
    public static readonly IDictionary<string, int> Ids = new Dictionary<string, int>
    {
        ["Building Inspector"] = 760810000
    };
}