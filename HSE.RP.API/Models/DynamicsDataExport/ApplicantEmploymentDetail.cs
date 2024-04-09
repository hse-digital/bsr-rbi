using System.Text.Json.Serialization;
namespace HSE.RP.API.Models.DynamicsDataExport
{

    public class ApplicantEmploymentDetail
    {
        [JsonPropertyName("bsr_biemploymentdetailid")]
        public required string EmploymentDetailId { get; set; }

        [JsonPropertyName("bsr_biemployerid_account")]
        public DynamicsEmployer? Employer { get; set; }

        [JsonPropertyName("_bsr_biapplicationid_value")]
        public string? applicationId { get; set; }

        [JsonPropertyName("_bsr_employmenttypeid_value")]
        public string? EmploymentType { get; set; }
    }

}