using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;

namespace HSE.RP.Domain.Entities;


public record BuildingInspectorRegistrationCountry(
    string Id = null,
    string CountryID = null,
    string Name = null,
    string BuildingProfessionApplicationId = null,
    string Status = null
    ) : Entity(Id);

public record DynamicsBuildingInspectorRegistrationCountry(
    string bsr_biregcountryid = null,
    [property: JsonPropertyName("bsr_countryid@odata.bind")]
    string countryReferenceId = null,
    [property: JsonPropertyName("bsr_biapplicationid@odata.bind")]
    string bsr_buildingprofessionapplicationid = null,
    string bsr_name = null
) : DynamicsEntity<BuildingInspectorRegistrationCountry>;