using System;
using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{
    public record EmployerName
    {
        public string FullName { get; set; } = string.Empty;

        public ComponentCompletionState CompletionState { get; set; } = ComponentCompletionState.NotStarted;
    }
}

