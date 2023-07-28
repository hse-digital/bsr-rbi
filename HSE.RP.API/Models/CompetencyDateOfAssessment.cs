using System;
using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{
	public record CompetencyDateOfAssessment : DateBase
    {
        public ComponentCompletionState IsComplete { get; set; } = ComponentCompletionState.NotStarted;
    }
}

