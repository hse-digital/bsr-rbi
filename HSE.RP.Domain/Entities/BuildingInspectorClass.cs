using System.Numerics;
using System.Text.Json.Serialization;

namespace HSE.RP.Domain.Entities;


public record BuildingInspectorClass(
    string Id = null,
    string Name = null
    ) : Entity(Id);

public record DynamicsBuildingInspectorClass(
    string bsr_biactivityId = null,
    string bsr_name = null
) : DynamicsEntity<BuildingInspectorClass>;

public static class BuildingInspectorClassNames
{
    public static readonly IDictionary<string, string> Ids = new Dictionary<string, string>
    {
        ["class-1-trainee-building-inspector"] = "d5a3e4b0-8624-ee11-9965-0022481b56d1",
        ["class-2-building-inspector"] = "a269b113-8724-ee11-9965-0022481b5210",
        ["class-3-specialist-building-inspector"] = "e30de119-8724-ee11-9965-0022481b5210",
        ["class-4-technical-manager"] = "4379fa25-8724-ee11-9965-0022481b5210",

    };
}