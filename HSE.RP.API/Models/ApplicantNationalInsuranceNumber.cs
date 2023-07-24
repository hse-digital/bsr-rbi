using System;
using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{
    public record ApplicantNationalInsuranceNumber
    {
        public string NationalInsuranceNumber { get; set; }
        public ComponentCompletionState IsComplete { get; set; } = ComponentCompletionState.NotStarted;
    }
}

