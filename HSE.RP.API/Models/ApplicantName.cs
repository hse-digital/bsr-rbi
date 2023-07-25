﻿using System;
using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{
    public record ApplicantName
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ComponentCompletionState IsComplete { get; set; } = ComponentCompletionState.NotStarted;
    }

}

