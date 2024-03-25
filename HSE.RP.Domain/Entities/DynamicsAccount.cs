using System.Text.Json.Serialization;

namespace HSE.RP.Domain.Entities;

public record DynamicsAccount(
    string name = null, 
    string accountid = null,
    int? address1_addresstypecode = null,
    string address1_line1 = null,
    string address1_line2 = null,
    string address1_city = null,
    string address1_county = null,
    string address1_country = null,
    string address1_postalcode = null,
    string bsr_address1uprn = null,
    string bsr_address1usrn = null,
    string bsr_address1lacode = null,
    string bsr_address1ladescription = null,
    [property: JsonPropertyName("bsr_Address1CountryCode@odata.bind")]
    string countryReferenceId = null,
    [property: JsonPropertyName("bsr_accounttype_accountId@odata.bind")]
    string accountTypeReferenceId = null,
    string bsr_otherorganisationtype = null,
    string _bsr_accounttype_accountid_value = null,
    YesNoOption? bsr_manualaddress = null);

public static class DynamicsAccountType
{
    public static readonly IDictionary<string, string> Ids = new Dictionary<string, string>
    {
        ["commonhold-association"] = "a45704fb-1cad-ed11-83ff-0022481b5e4f",
        ["housing-association"] = "64866e32-1dad-ed11-83ff-0022481b5e4f",
        ["local-authority"] = "db305f3e-1dad-ed11-83ff-0022481b5e4f",
        ["management-company"] = "92c98d4a-1dad-ed11-83ff-0022481b5e4f",
        ["rmc-or-organisation"] = "20df1269-1dad-ed11-83ff-0022481b5e4f",
        ["rtm-or-organisation"] = "50f0899f-1dad-ed11-83ff-0022481b5e4f",
        ["other"] = "453982a5-1dad-ed11-83ff-0022481b5e4f",
        ["building-inspector-professional-body"]= "1c9430d4-922c-ee11-9965-0022481b59de",
        ["private-sector-building-control-body"] = "42d1914c-fc31-ee11-bdf3-0022481b5210",
        ["public-sector-building-control-body"] = "729b10d9-8e36-ee11-bdf4-0022481b5210"
        
    };
}

public record DynamicsAccountTypeRelationship(
    string bsr_name = null,
    string bsr_accounttypeid = null,
    [property: JsonPropertyName("@odata.id")]
    string accountTypeReferenceId = null);