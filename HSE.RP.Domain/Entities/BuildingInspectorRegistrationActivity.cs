using System.Numerics;
using System.Text.Json.Serialization;

namespace HSE.RP.Domain.Entities;


public record BuildingInspectorRegistrationActivity(
    string Id = null,
    string Status = null,
    string ActivityId = null,
    string BuildingCategoryId = null,
    string BuildingProfessionApplicationId = null,
    string ApplicantId = null
    ) : Entity(Id);

public record DynamicsBuildingInspectorRegistrationActivity(
    string bsr_biregactivityId = null,
    [property: JsonPropertyName("bsr_biactivityid.bind")]
    string buildingActivityReferenceId = null,
    [property: JsonPropertyName("bsr_biapplicationid@odata.bind")]
    string buidingProfessionApplicationReferenceId = null,
    [property: JsonPropertyName("bsr_bibuildingcategoryid@odata.bind")]
    string buidingCategoryReferenceId = null,
    string statuscode = null
) : DynamicsEntity<BuildingInspectorRegistrationActivity>;

public enum BuildingInspectorRegistrationActivityStatus
{
    Applied = 760_810_001,
    Registered = 760_810_002,
}