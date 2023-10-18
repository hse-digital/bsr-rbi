using System.Text.Json.Serialization;

namespace HSE.RP.API.Models
{
    public class DynamicsAuthenticationModel
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
