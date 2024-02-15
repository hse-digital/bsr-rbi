using System.Numerics;
using System.Text.Json.Serialization;

namespace HSE.RP.Domain.Entities;


public record BuildingInspectorRegistrationClass(
    string Id = null,
    string BuildingProfessionApplicationId = null,
    string ApplicantId = null,
    string ClassId = null,
    string Name = null,
    int? StatusCode = null,
    int? StateCode = null
    ) : Entity(Id);

public record DynamicsBuildingInspectorRegistrationClass(
    string bsr_biregclassid = null,
    [property: JsonPropertyName("bsr_biapplicationid@odata.bind")]
    string buidingProfessionApplicationReferenceId = null,
    [property: JsonPropertyName("bsr_buildinginspectorid@odata.bind")]
    string contactRefId = null,
    [property: JsonPropertyName("bsr_biclassid@odata.bind")]
    string classRef = null,
    string _bsr_biapplicationid_value = null,
    string _bsr_biclassid_value = null,
    string _bsr_buildinginspectorid_value = null,
    string bsr_name = null,
    int? statuscode = null,
    int? statecode = null
) : DynamicsEntity<BuildingInspectorRegistrationClass>;

public enum BuildingInspectorRegistrationClassStatus
{
    Applied = 760_810_001,
    Registered = 760_810_002,
    Considered = 760_810_003,
}