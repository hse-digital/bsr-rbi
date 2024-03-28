using HSE.RP.API.Models.Enums;
using System.Text.Json.Serialization;
namespace HSE.RP.API.Models.DynamicsDataExport
{

    public class DynamicsActivity
    {
        [JsonPropertyName("bsr_name")]
        public required string ActivityName { get; set; }

        [JsonPropertyName("bsr_biactivityid")]
        public required string ActivityId { get; set; }


    }

}