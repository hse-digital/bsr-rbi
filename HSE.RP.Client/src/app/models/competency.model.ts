import { CompetencyDateOfAssessment } from './competency-date-of-assessment.model';
import { ComponentCompletionState } from './component-completion-state.enum';
import { NoCompetencyAssessment } from './no-competency-assessment.model';
import { CompetenceyAssessmentCertificateNumber } from "./competency-assessment-certificate-number.model";

export class Competency {
  IndependentAssessmentStatus?: string;
  CompetencyAssesesmentOrganisation?: string;
  CompetencyDateOfAssessment: CompetencyDateOfAssessment = {};
  NoCompetencyAssessment?: NoCompetencyAssessment = { Declaration: false };
  CompetencyAssessmentCertificateNumber?: CompetenceyAssessmentCertificateNumber = new CompetenceyAssessmentCertificateNumber();
  
  CompletionState?: ComponentCompletionState =
    ComponentCompletionState.NotStarted;
}
