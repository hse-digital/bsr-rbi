using System;
using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{

    public record ApplicantEmail
    {
        public string Email { get; set; }

        public ComponentCompletionState IsComplete { get; set; } = ComponentCompletionState.NotStarted;
    }
}

