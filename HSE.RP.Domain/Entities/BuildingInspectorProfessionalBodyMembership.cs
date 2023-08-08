using System.Numerics;
using System.Text.Json.Serialization;

namespace HSE.RP.Domain.Entities;


public record BuildingInspectorProfessionalBodyMembership(
    string Id = null,
    string Name = null
    ) : Entity(Id);

public record DynamicsBuildingInspectorProfessionalBodyMembership(
    string bsr_name = null,
    string bsr_biprofessionalmembershipid = null,
    [property: JsonPropertyName("bsr_biapplicationid@odata.bind")]
    string buidingProfessionApplicationReferenceId = null,
    [property: JsonPropertyName("bsr_buildinginspectorid@odata.bind")]
    string contactRefId = null,
    [property: JsonPropertyName("bsr_professionalbodyid@odata.bind")]
    string professionalBodyRefId = null,
    string bsr_membershipnumber = null,
    string bsr_currentmembershiplevelortype = null,
    [property: JsonPropertyName("bsr_yearid@odata.bind")]
    string yearRefId = null
) : DynamicsEntity<BuildingInspectorProfessionalBodyMembership>;