using System.Text.Json.Serialization;

namespace HSE.RP.API.Model;

public class DynamicsResponse<T>
{
    public List<T> value { get; set; }

    [JsonPropertyName("@odata.nextLink")]
    public string nextLink { get; set; }

}