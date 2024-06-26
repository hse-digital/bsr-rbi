﻿using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{
    public record ApplicantNationalInsuranceNumber
    {
        public string NationalInsuranceNumber { get; set; }
        public ComponentCompletionState CompletionState { get; set; } = ComponentCompletionState.NotStarted;
    }
}

