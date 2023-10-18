using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{
    public record CompetencyDateOfAssessment : DateBase
    {
        public ComponentCompletionState CompletionState { get; set; } = ComponentCompletionState.NotStarted;
    }
}

