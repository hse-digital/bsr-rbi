using System;
using HSE.RP.API.Enums;
using HSE.RP.Domain.Entities;

namespace HSE.RP.API.Models
{
	public record BuildingAssessingPlansBase
	{
        public bool? CategoryA { get; set; }
        public bool? CategoryB { get; set; }
        public bool? CategoryC { get; set; }
        public bool? CategoryD { get; set; }
        public bool? CategoryE { get; set; }
        public bool? CategoryF { get; set; }
        public ComponentCompletionState CompletionState { get; set; }
    }
}

