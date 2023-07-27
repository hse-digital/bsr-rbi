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
    public static readonly IDictionary<int, string> Ids = new Dictionary<int, string>
    {
        [1] = "d5a3e4b0-8624-ee11-9965-0022481b56d1",
        [2] = "a269b113-8724-ee11-9965-0022481b5210",
        [3] = "e30de119-8724-ee11-9965-0022481b5210",
        [4] = "4379fa25-8724-ee11-9965-0022481b5210",

    };
}