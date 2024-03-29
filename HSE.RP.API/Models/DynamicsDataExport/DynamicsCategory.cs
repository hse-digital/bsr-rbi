using System.Text.Json.Serialization;
namespace HSE.RP.API.Models.DynamicsDataExport
{

    public class DynamicsCategory
    {

        [JsonPropertyName("bsr_bibuildingcategoryid")]
        public required string CategoryId { get; set; }

        [JsonPropertyName("bsr_name")]
        public required string CategoryName { get; set; }

    }

}