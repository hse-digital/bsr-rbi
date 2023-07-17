using System.Text.Json.Serialization;

namespace HSE.RP.Domain.Entities;

public record Contact(
    string Id = null,
    string FirstName = null,
    string LastName = null,
    string PhoneNumber = null,
    string AlternativePhoneNumber = null,
    string Email = null,
    string AlternativeEmail = null,
    string NationalInsuranceNumber = null,
    BuildingAddress Address = null,
    DateOnly? birthdate = null,
    string buildingProfessionApplicationReferenceId = null,
    string jobRoleReferenceId = null
    ) : Entity(Id);

public record DynamicsContact(
    string firstname = null, 
    string lastname = null, 
    string telephone1 = null, 
    string telephone2 = null, 
    string emailaddress2 = null, 
    string emailaddress1 = null, 
    string contactid = null,
    string address1_line1 = null, 
    string address1_line2 = null, 
    string address1_city = null, 
    string address1_county = null, 
    string address1_country = null, 
    string address1_postalcode = null, 
    YesNoOption? bsr_manualaddress = null,
    [property: JsonPropertyName("bsr_JobRole@odata.bind")]
    string jobRoleReferenceId = null,
    DynamicsContactType[] bsr_contacttype_contact = null,
    [property: JsonPropertyName("bsr_Address1CountryCode@odata.bind")]
    string countryReferenceId = null,
    [property: JsonPropertyName("bsr_buildingprofessionapplicationid@odata.bind")]
    string bsr_buildingprofessionapplicationid = null,
    string bsr_nationalinsuranceno = null,
    DateOnly? birthdate = null
    ) : DynamicsEntity<Contact>;

public record DynamicsContactType(
    string bsr_name = null, 
    string bsr_contacttypeid = null,
    [property: JsonPropertyName("@odata.id")]
    string contactTypeReferenceId = null);

public static class DynamicsJobRole
{
    public static readonly IDictionary<string, string> Ids = new Dictionary<string, string>
    {
        ["director"] = "c76070f7-09bc-ed11-83fe-000d3a86ecac",
        ["administrative_worker"] = "37c5ab12-0abc-ed11-83fe-000d3a86ecac",
        ["building_manager"] = "671f4426-0abc-ed11-83fe-000d3a86ecac",
        ["building_inspector"] = "c77d5afd-e006-ee11-8f6e-0022481b5210",
        ["building_director"] = "d5808a32-0abc-ed11-83fe-000d3a86ecac",
        ["other"] = "eee3fe38-0abc-ed11-83fe-000d3a86ecac"
    };
}

public static class DynamicsContactTypes
{
    public const string AccountablePerson = "6ca72c01-31b8-ed11-b597-0022481b5e4f";
    public const string PrincipalAccountablePerson = "05aa2b0d-31b8-ed11-b597-0022481b5e4f";
    public const string HRBRegistrationApplicant = "e3dc7003-8bc1-ed11-b597-6045bd0d4376";
    public const string PAPOrganisationLeadContact = "e1390d10-8bc1-ed11-b597-6045bd0d4376";
    public const string APOrganisationLeadContact = "d0e58e53-67c2-ed11-b597-6045bd0d4376";
    public const string BIApplicant = "761d55e5-e006-ee11-8f6e-0022481b5210";
}

public static class DynamicsCountryCodes
{
    public static readonly IDictionary<string, string> Ids = new Dictionary<string, string>
    {
        ["E"] = "65eeb151-30b8-ed11-b597-0022481b5e4f",
        ["W"] = "ab22b657-30b8-ed11-b597-0022481b5e4f",
    };
}
