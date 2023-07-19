using HSE.RP.Domain.Entities;

namespace HSE.RP.API.Models;

public class BuildingAddressSearchResponse
{
    public int Offset { get; init; }
    public int MaxResults { get; init; }
    public int TotalResults { get; init; }
    public BuildingAddress[] Results { get; set; }
}
