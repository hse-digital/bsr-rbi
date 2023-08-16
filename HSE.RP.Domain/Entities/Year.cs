namespace HSE.RP.Domain.Entities;

public record Year(string Value, string Id);

public record DynamicsYear(string bsr_name, string bsr_yearid);


public static class DynamicsYearIds
{
    public static readonly IDictionary<int, string> Ids = new Dictionary<int, string>
    {
        [2021] = "759bbf6b-6437-ee11-bdf4-0022481b59de"
    };
}