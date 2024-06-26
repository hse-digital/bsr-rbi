﻿namespace HSE.RP.API.Models
{
    public record Competency
    {

        public CompetencyIndependentAssessmentStatus CompetencyIndependentAssessmentStatus { get; set; } = null;
        public CompetencyAssessmentOrganisation CompetencyAssessmentOrganisation { get; set; } = null;
        public NoCompetencyAssessment NoCompetencyAssessment { get; set; } = null;
        public CompetencyDateOfAssessment CompetencyDateOfAssessment { get; set; } = null;
        public CompetencyAssessmentCertificateNumber CompetencyAssessmentCertificateNumber { get; set; } = null;


    }
}

