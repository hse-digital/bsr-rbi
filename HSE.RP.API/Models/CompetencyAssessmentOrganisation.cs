using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{
    public record CompetencyAssessmentOrganisation
	{
		public string ComAssessmentOrganisation { get; set; }
        public ComponentCompletionState CompletionState { get; set; } = ComponentCompletionState.NotStarted;
    }
}

