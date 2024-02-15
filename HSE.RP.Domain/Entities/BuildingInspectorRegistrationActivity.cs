using System.Numerics;
using System.Text.Json.Serialization;

namespace HSE.RP.Domain.Entities;


public record BuildingInspectorRegistrationActivity(
    string Id = null,
    string Status = null,
    string ActivityId = null,
    string BuildingCategoryId = null,
    string BuildingProfessionApplicationId = null,
    string BuildingInspectorId = null,
    int? StatusCode = null,
    int? StateCode = null
    ) : Entity(Id);

public record DynamicsBuildingInspectorRegistrationActivity(
    string bsr_biregactivityId = null,
    [property: JsonPropertyName("bsr_biactivityid@odata.bind")]
    string buildingActivityReferenceId = null,
    [property: JsonPropertyName("bsr_biapplicationid@odata.bind")]
    string buidingProfessionApplicationReferenceId = null,
    [property: JsonPropertyName("bsr_bibuildingcategoryid@odata.bind")]
    string buidingCategoryReferenceId = null,
    [property: JsonPropertyName("bsr_buildinginspectorid@odata.bind")]
    string contactRefId = null,

    string _bsr_biapplicationid_value = null,
    string _bsr_biactivityid_value = null,
    string _bsr_buildinginspectorid_value = null,
    string _bsr_bibuildingcategoryid_value = null,

    int? statuscode = null,
    int? statecode = null
) : DynamicsEntity<BuildingInspectorRegistrationActivity>;

public enum BuildingInspectorRegistrationActivityStatus
{
    Applied = 760_810_001,
    Registered = 760_810_002,
    Considered = 760_810_003,
}