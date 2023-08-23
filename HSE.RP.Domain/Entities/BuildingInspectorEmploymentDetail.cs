using System.Numerics;
using System.Text.Json.Serialization;

namespace HSE.RP.Domain.Entities;


public record BuildingInspectorEmploymentDetail(
    string Id = null,
    string Name = null,
    string BuildingProfessionApplicationId = null,
    string BuildingInspectorId = null,
    string EmployerIdContact = null,
    string EmployerIdAccount = null,
    string EmploymentTypeId = null,
    bool IsCurrent = false,
    int? StateCode = null,
    int? StatusCode = null
    ) : Entity(Id);

public record DynamicsBuildingInspectorEmploymentDetail(
    string bsr_biemploymentdetailid = null,
    string bsr_name = null,
    [property: JsonPropertyName("bsr_biapplicationid@odata.bind")]
    string buildingProfessionApplicationReferenceId = null,
    [property: JsonPropertyName("bsr_buildinginspectorid@odata.bind")]
    string contactRefId = null,
    [property: JsonPropertyName("bsr_biemployerid_contact@odata.bind")]
    string employerIdContact = null,
    [property: JsonPropertyName("bsr_biemployerid_account@odata.bind")]
    string employerIdAccount = null,
    [property: JsonPropertyName("bsr_employmenttypeid@odata.bind")]
    string employmentTypeId = null,
    bool bsr_iscurrent = false,
    string _bsr_employmenttypeid_value = null,
    string _bsr_buildinginspectorid_value = null,
    string _bsr_biemployerid_value = null,
    string _bsr_biapplicationid_value = null,
    int? statuscode = null,
    int? statecode = null

) : DynamicsEntity<BuildingInspectorBuildingCategory>;

public static class BuildingInspectorEmploymentTypeSelection
{
    public static readonly IDictionary<int, string> Ids = new Dictionary<int, string>
    {
        [1] = "e5a761f1-0932-ee11-bdf3-0022481b56d1", //Public
        [2] = "f6d565f7-0932-ee11-bdf3-0022481b56d1", //Private
        [3] = "05d665f7-0932-ee11-bdf3-0022481b56d1", //Other
        [4] = "6a3f65fd-0932-ee11-bdf3-0022481b56d1" //Unemployed
    };
}
