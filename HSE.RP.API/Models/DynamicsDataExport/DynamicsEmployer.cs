using System.Text.Json.Serialization;
namespace HSE.RP.API.Models.DynamicsDataExport
{

    public class DynamicsEmployer
    {
        [JsonPropertyName("accountid")]
        public required string EmployerAccountId { get; set; }

        [JsonPropertyName("name")]
        public required string EmployerName { get; set; }

        [JsonPropertyName("address1_composite")]
        public string? EmployerAddress { get; set; }


    }

}