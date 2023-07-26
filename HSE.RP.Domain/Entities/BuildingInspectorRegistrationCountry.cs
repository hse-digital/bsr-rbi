using System.Numerics;
using System.Text.Json.Serialization;

namespace HSE.RP.Domain.Entities;


public record BuildingInspectorRegistrationCountry(
    string Id = null,
    string CountryID = null,
    BuildingProfessionApplication Application = null,
    string Status = null
    ) : Entity(Id);

public record DynamicsBuildingInspectorRegistrationCountry(
    [property: JsonPropertyName("bsr_Country@odata.bind")]
    string countryReferenceId = null,
    string bsr_buildingprofessionapplicationid = null
) : DynamicsEntity<BuildingInspectorRegistrationCountry>;