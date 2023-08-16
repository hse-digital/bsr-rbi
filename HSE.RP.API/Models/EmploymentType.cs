using System;
using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{
    public record EmploymentType
    {
        public bool? PublicSector { get; set; }
        public bool? PrivateSector { get; set; }
        public bool? Other { get; set; }
        public bool? Unemployed { get; set; }

        public ComponentCompletionState CompletionState { get; set; } = ComponentCompletionState.NotStarted;
    }
}

