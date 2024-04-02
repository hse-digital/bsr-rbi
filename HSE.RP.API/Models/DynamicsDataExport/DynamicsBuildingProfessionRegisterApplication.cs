using System.Text.Json.Serialization;
using HSE.RP.API.Models.Enums;
using System.Collections.Generic;

namespace HSE.RP.API.Models.DynamicsDataExport
{
    public class DynamicsBuildingProfessionRegisterApplication
    {
        [JsonPropertyName("bsr_buildingprofessionapplicationid")]
        public required string BuildingProfessionApplicationDynamicsId { get; set; }

        [JsonPropertyName("bsr_buildingproappid")]
        public required string ApplicationId { get; set; }

        [JsonPropertyName("bsr_buildingprofessiontypecode")]
        public BuildingProfessionType BuildingProfessionType { get; set; }

        [JsonPropertyName("bsr_registrationcommencementdate")]
        public required string DecisionDate { get; set; }

        [JsonPropertyName("bsr_decisioncondition")]
        public string? DecisionCondition { get; set; }

        [JsonPropertyName("bsr_regulatorydecisionstatus")]
        public BuildingProfessionRegulatoryDecisionStatus BuildingProfessionRegulatoryDecisionStatus { get; set; }

        [JsonPropertyName("bsr_reviewdecision")]
        public string? ReviewDecision { get; set; }

        [JsonPropertyName("bsr_applicantid_contact")]
        public required DynamicsApplicant Applicant { get; set; }

        [JsonPropertyName("statuscode")]
        public BuildingProfessionApplicationStatus ApplicationStatus { get; set; }

        [JsonPropertyName("bsr_bsr_biregcountry_buildingprofessionapplic")]
        public List<ApplicantCountryDetails>? ApplicantCountryDetails { get; set; }

        [JsonPropertyName("bsr_biemploymentdetail_buildingprofessionappl")]
        public List<ApplicantEmploymentDetail>? ApplicantEmploymentDetails { get; set; }

        [JsonPropertyName("bsr_bsr_biregclass_buildingprofessionapplicat")]
        public List<ApplicantClassDetails>? ApplicantClassDetails { get; set; }

        [JsonPropertyName("bsr_biregactivity_buildingprofessionapplicati")]
        public List<ApplicantActivityDetails>? ApplicantActivityDetails { get; set; }

    }
}
