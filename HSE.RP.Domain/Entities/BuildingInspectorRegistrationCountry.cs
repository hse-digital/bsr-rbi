using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;

namespace HSE.RP.Domain.Entities;


public record BuildingInspectorRegistrationCountry(
    string Id = null,
    string Name = null,
    string CountryID = null,
    string BuildingInspectorId = null,
    string BuildingProfessionApplicationId = null,
    int? StatusCode = null,
    int? StateCode = null
    ) : Entity(Id);

public record DynamicsBuildingInspectorRegistrationCountry(
    string bsr_biregcountryid = null,
    [property: JsonPropertyName("bsr_countryid@odata.bind")]
    string countryRefId = null,
    [property: JsonPropertyName("bsr_biapplicationid@odata.bind")]
    string buidingProfessionApplicationReferenceId = null,
    [property: JsonPropertyName("bsr_buildinginspectorid@odata.bind")]
    string contactRefId = null,
    string bsr_name = null,
    string _bsr_biapplicationid_value = null,
    string _bsr_buildinginspectorid_value = null,
    string _bsr_countryid_value = null,
    int? statuscode = null,
    int? statecode = null
) : DynamicsEntity<BuildingInspectorRegistrationCountry>;