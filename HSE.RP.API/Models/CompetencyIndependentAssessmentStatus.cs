using System;
using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{
	public record CompetencyIndependentAssessmentStatus
    {
        public string IAStatus { get; set; }
        public ComponentCompletionState CompletionState { get; set; } = ComponentCompletionState.NotStarted;
    }
}

