using System.Text.Json.Serialization;

namespace HSE.RP.Domain.Entities;

public record DynamicsAddress(
    string bsr_addressId = null,
    string bsr_line1 = null, 
    string bsr_line2 = null, 
    string bsr_city = null, 
    string bsr_postcode = null, 
    string bsr_uprn = null, 
    string bsr_usrn = null,
    [property: JsonPropertyName("bsr_Country@odata.bind")]
    string countryReferenceId = null,
    AddressType? bsr_addresstypecode = null, 
    YesNoOption? bsr_manualaddress = null,
    [property: JsonPropertyName("bsr_independentsectionid@odata.bind")]
    string structureReferenceId = null,
    string bsr_address1lacode = null,
    string bsr_address1ladescription = null,
    int? statuscode = null,
    int? statecode = null
    );