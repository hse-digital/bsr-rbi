using System;
using HSE.RP.API.Enums;

namespace HSE.RP.API.Models
{
	public record Competency
	{
		public string IndependentAssessmentStatus { get; set; } = string.Empty;
        public string CompetencyAssesesmentOrganisation { get; set; } = string.Empty;
        public NoCompetencyAssessment NoCompetencyAssessment { get; set; }
        public ComponentCompletionState CompletionState { get; set; }
    }
}

