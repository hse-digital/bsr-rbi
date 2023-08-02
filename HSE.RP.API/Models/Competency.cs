using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{
    public record Competency
	{
         IndependentAssessmentStatus IndependentAssessmentStatus { get; set; }
         CompetencyAssesesmentOrganisation CompetencyAssesesmentOrganisation { get; set; }
         NoCompetencyAssessment NoCompetencyAssessment { get; set; }
         CompetencyDateOfAssessment CompetencyDateOfAssessment { get; set; }
         CompetencyAssessmentCertificateNumber CompetencyAssessmentCertificateNumber { get; set; }

    }
}

