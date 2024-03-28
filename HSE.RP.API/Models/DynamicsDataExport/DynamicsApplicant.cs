using System.Text.Json.Serialization; 
namespace HSE.RP.API.Models.DynamicsDataExport{ 

    public class DynamicsApplicant
    {
        [JsonPropertyName("firstname")]
        public string? ApplicantFirstName { get; set; }

        [JsonPropertyName("lastname")]
        public string? ApplicantLastName { get; set; }

        [JsonPropertyName("contactid")]
        public string? ApplicantContactId { get; set; }


        [JsonPropertyName("address2_composite")]
        public string? BusinessAddress { get; set; }
    }

}