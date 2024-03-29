using System.Text.Json.Serialization; 
namespace HSE.RP.API.Models.DynamicsDataExport{ 

    public class DynamicsCountry
    {

        [JsonPropertyName("bsr_countryid")]
        public required string CountryId { get; set; }

        [JsonPropertyName("bsr_name")]
        public required string CountryName { get; set; }

    }

}