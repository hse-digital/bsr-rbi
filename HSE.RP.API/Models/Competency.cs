using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{
    public record Competency
	{
        IndependentAssessmentStatus IndependentAssessmentStatus { get; set; } = null;
        CompetencyAssesesmentOrganisation CompetencyAssesesmentOrganisation { get; set; } = null;
        NoCompetencyAssessment NoCompetencyAssessment { get; set; } = null;
        CompetencyDateOfAssessment CompetencyDateOfAssessment { get; set; } = null;
        CompetencyAssessmentCertificateNumber CompetencyAssessmentCertificateNumber { get; set; } = null;

    }
}

