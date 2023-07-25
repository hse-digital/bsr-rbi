using System;
using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{

    public record BuildingInspectorRegulatedActivies
    {
        public bool? AssessingPlans { get; set; }
        public bool? Inspection { get; set; }
        public ComponentCompletionState CompletionState { get; set; }

    }
}

