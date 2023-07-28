﻿using System;
using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{
    public record ApplicantDateOfBirth : DateBase
    {
        public ComponentCompletionState IsComplete { get; set; } = ComponentCompletionState.NotStarted;
    }
}

