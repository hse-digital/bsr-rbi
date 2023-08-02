using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{
    public record ProfessionalBodiesMember
	{
		public string Membership { get; set; } = string.Empty;

        public ComponentCompletionState CompletionState { get; set; } = ComponentCompletionState.NotStarted;
    }
}

