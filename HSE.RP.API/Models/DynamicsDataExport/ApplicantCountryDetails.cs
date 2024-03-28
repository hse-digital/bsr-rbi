using System.Text.Json.Serialization; 
namespace HSE.RP.API.Models.DynamicsDataExport{ 

    public class ApplicantCountryDetails
    {
        [JsonPropertyName("bsr_biregcountryid")]
        public required string CountryDetailsId { get; set; }

        [JsonPropertyName("bsr_countryid")]
        public required DynamicsCountry Country { get; set; }
    }

}