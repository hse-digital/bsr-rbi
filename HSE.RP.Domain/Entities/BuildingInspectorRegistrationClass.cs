using System.Numerics;
using System.Text.Json.Serialization;

namespace HSE.RP.Domain.Entities;


public record BuildingInspectorRegistrationClass(
    string Id = null,
    string BuildingProfessionApplicationId = null,
    string ApplicantId = null,
    string ClassId = null,
    string StatusCode = null,
    string Name = null
    ) : Entity(Id);

public record DynamicsBuildingInspectorRegistrationClass(
    string bsr_biregclassId = null,
    [property: JsonPropertyName("bsr_biapplicationid@odata.bind")]
    string buidingProfessionApplicationReferenceId = null,
    [property: JsonPropertyName("bsr_buildinginspectorid@odata.bind")]
    string contactRefId = null,
    [property: JsonPropertyName("bsr_biclassid@odata.bind")]
    string classRefId = null,
    string bsr_name = null,
    int? statuscode = null
) : DynamicsEntity<BuildingInspectorRegistrationClass>;

public enum BuildingInspectorRegistrationClassStatus
{
    Applied = 760_810_001,
    Registered = 760_810_002,
}