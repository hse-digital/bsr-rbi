using System;
using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{
    public record ApplicantDateOfBirth
    {
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public ComponentCompletionState CompletionState { get; set; } = ComponentCompletionState.NotStarted;

    }
}

