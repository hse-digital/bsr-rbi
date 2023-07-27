using System.Numerics;
using System.Text.Json.Serialization;

namespace HSE.RP.Domain.Entities;


public record BuildingInspectorActivity(
    string Id = null,
    string Name = null
    ) : Entity(Id);

public record DynamicsBuildingInspectorActivity(
    string bsr_biactivityId = null,
    string bsr_name = null
) : DynamicsEntity<BuildingInspectorActivity>;

public static class BuildingInspectorActivityNames
{
    public static readonly IDictionary<string, string> Ids = new Dictionary<string, string>
    {
        ["Inspect"] = "5f23c81c-8b24-ee11-9965-0022481b5210",
        ["AssessingPlans"] = "5723c81c-8b24-ee11-9965-0022481b5210",
    };
}