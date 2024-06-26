﻿using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{
    public record ApplicantEmploymentDetails
    {
        public EmploymentTypeSelection EmploymentTypeSelection { get; set; }
        public EmployerName EmployerName { get; set; }
        public AddressModel EmployerAddress { get; set; }

        public ComponentCompletionState CompletionState { get; set; } = ComponentCompletionState.NotStarted;

    }
}

