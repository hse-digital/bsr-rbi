using HSE.RP.API.Models.Enums;
using System.Text.Json.Serialization;
namespace HSE.RP.API.Models.DynamicsDataExport
{

    public class ApplicantClassDetails
    {
        [JsonPropertyName("bsr_biregclassid")]
        public required string ClassDetailsId { get; set; }

        [JsonPropertyName("bsr_biclassid")]
        public required DynamicsClass Class { get; set; }

        [JsonPropertyName("statuscode")]
        public BuildingInspectorRegistrationClassStatus StatusCode { get; set; }
    }

}