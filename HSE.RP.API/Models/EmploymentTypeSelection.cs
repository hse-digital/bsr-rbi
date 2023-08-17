using System;
using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{
    public record EmploymentTypeSelection
    {
        public EmploymentType EmploymentType { get; set; } = EmploymentType.None;

        public ComponentCompletionState CompletionState { get; set; } = ComponentCompletionState.NotStarted;
    }
}

