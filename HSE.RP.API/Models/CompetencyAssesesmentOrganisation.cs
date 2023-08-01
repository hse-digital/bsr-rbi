using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{
    public record CompetencyAssesesmentOrganisation
	{
		public string ComAssesesmentOrganisation { get; set; }
        public ComponentCompletionState CompletionState { get; set; } = ComponentCompletionState.NotStarted;
    }
}

