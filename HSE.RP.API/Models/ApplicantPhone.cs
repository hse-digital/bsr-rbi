using System;
using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{
    public record ApplicantPhone
    {
        public string PhoneNumber { get; set; }
        public ComponentCompletionState IsComplete { get; set; } = ComponentCompletionState.NotStarted;
    }
}

