using HSE.RP.API.Models.Enums;
using System.Text.Json.Serialization;
namespace HSE.RP.API.Models.DynamicsDataExport
{

    public class ApplicantActivityDetails
    {
        [JsonPropertyName("bsr_biregactivityid")]
        public required string ApplicantActivityDetailsId { get; set; }

        [JsonPropertyName("bsr_biactivityid")]
        public required DynamicsActivity Activity { get; set; }

        [JsonPropertyName("bsr_bibuildingcategoryid")]
        public required DynamicsCategory Category { get; set; }

        [JsonPropertyName("statuscode")]
        public required BuildingInspectorRegistrationActivityStatus ActivityStatus { get; set; }
    }

}