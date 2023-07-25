namespace HSE.RP.Domain.Entities;

public class BuildingAddress
{
    public string UPRN { get; init; }
    public string USRN { get; init; }
    public string Address { get; init; }
    public string AddressLineTwo { get; init; }
    public string BuildingName { get; init; }
    public string Number { get; init; }
    public string Street { get; init; }
    public string Town { get; init; }
    public string Country { get; init; }
    public string AdministrativeArea { get; init; }
    public string Postcode { get; init; }
    /// <summary>
    /// This needs to be nullable, for the address control to work
    /// </summary>
    public bool? IsManual { get; init; }
    public string ClassificationCode { get; init; }
    public string CustodianCode { get; init; }
    public string CustodianDescription { get; init; }


}