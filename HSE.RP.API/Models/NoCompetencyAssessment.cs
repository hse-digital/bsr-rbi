using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{
    public record NoCompetencyAssessment()
    {

        public bool Declaration { get; set; } = false;

        public ComponentCompletionState CompletionState { get; set; } = ComponentCompletionState.NotStarted;
    }
}

