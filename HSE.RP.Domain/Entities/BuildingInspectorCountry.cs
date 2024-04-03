using System.Numerics;
using System.Text.Json.Serialization;

namespace HSE.RP.Domain.Entities;


public record BuildingInspectorCountry(
    string Id = null,
    string Name = null
    ) : Entity(Id);

public record DynamicsBuildingInspectorCountry(
    string bsr_countryid = null,
    string bsr_name = null
) : DynamicsEntity<BuildingInspectorCountry>;

public static class BuildingInspectorCountryNames
{
    public static readonly IDictionary<string, string> Ids = new Dictionary<string, string>
    {
        ["England"] = "65eeb151-30b8-ed11-b597-0022481b5e4f",
        ["Wales"] = "ab22b657-30b8-ed11-b597-0022481b5e4f",
        ["Scotland"] = "fb55923f-99f1-ee11-904b-002248c72690",
        ["Northern Ireland"] = "522c9343-99f1-ee11-904c-0022481b5210",
        
    };
}