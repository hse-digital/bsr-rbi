using System.Numerics;
using System.Text.Json.Serialization;

namespace HSE.RP.Domain.Entities;


public record BuildingInspectorProfessionalBodyMembership(
    string Id = null,
    string Name = null,
    string BuildingProfessionApplicationId = null,
    string BuildingInspectorId = null,
    string ProfessionalBodyId = null,
    string MembershipNumber = null,
    string CurrentMembershipLevelOrType = null,
    string YearId = null
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


public static class BuildingInspectorProfessionalBodyIds
{
    public static readonly IDictionary<string, string> Ids = new Dictionary<string, string>
    {
        ["RICS"] = "48f60522-932c-ee11-9965-0022481b59de",
        ["CABE"] = "f36d4890-262a-ee11-9965-0022481b59de",
        ["CIOB"] = "689d678f-962c-ee11-9965-0022481b59de",
        ["OTHER"]= "65f5b095-962c-ee11-9965-0022481b59de"
    };
}