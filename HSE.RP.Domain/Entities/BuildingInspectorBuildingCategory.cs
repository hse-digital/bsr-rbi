using System.Numerics;
using System.Text.Json.Serialization;

namespace HSE.RP.Domain.Entities;


public record BuildingInspectorBuildingCategory(
    string Id = null,
    string Name = null
    ) : Entity(Id);

public record DynamicsBuildingInspectorBuildingCategory(
    string bsr_bibuildingcategoryId = null,
    string bsr_name = null
) : DynamicsEntity<BuildingInspectorBuildingCategory>;

public static class BuildingInspectorBuildingCategoryNames
{
    public static readonly IDictionary<string, string> Ids = new Dictionary<string, string>
    {
        ["CategoryAClass2"] = "8aa8d113-9624-ee11-9965-0022481b56d1",
        ["CategoryAClass3"] = "e0bd3e1a-9624-ee11-9965-0022481b56d1",
        ["CategoryBClass2"] = "f9bd3e1a-9624-ee11-9965-0022481b56d1",
        ["CategoryBClass3"] = "60273e20-9624-ee11-9965-0022481b56d1",
        ["CategoryCClass2"] = "67fa3c2c-9624-ee11-9965-0022481b56d1",
        ["CategoryCClass3"] = "55af8832-9624-ee11-9965-0022481b56d1",
        ["CategoryDClass2"] = "46268838-9624-ee11-9965-0022481b56d1",
        ["CategoryDClass3"] = "b0c7c03e-9624-ee11-9965-0022481b56d1",
        ["CategoryEClass2"] = "d943cb50-9624-ee11-9965-0022481b56d1",
        ["CategoryEClass3"] = "14c0dd56-9624-ee11-9965-0022481b56d1",
        ["CategoryFClass2"] = "72550f5d-9624-ee11-9965-0022481b56d1",
        ["CategoryFClass3"] = "096e3e63-9624-ee11-9965-0022481b56d1",
        ["CategoryGClass3"] = "c0e4cc69-9624-ee11-9965-0022481b56d1",
        ["CategoryHClass3"] = "0813d16f-9624-ee11-9965-0022481b56d1",
    };
}