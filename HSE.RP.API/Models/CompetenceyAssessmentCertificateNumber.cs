using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{
    public record CompetencyAssessmentCertificateNumber
    {
        public string CertificateNumber { get; set; } = string.Empty;
        public ComponentCompletionState CompletionState { get; set; }
    }
}
