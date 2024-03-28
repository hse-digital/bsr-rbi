using HSE.RP.API.Models.Enums;
using System.Text.Json.Serialization;
namespace HSE.RP.API.Models.DynamicsDataExport
{

    public class DynamicsClass
    {
        [JsonPropertyName("bsr_biclassid")]
        public required string ClassId { get; set; }
        [JsonPropertyName("bsr_name")]
        public required string ClassName { get; set; }

    }

}