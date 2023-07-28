import { ComponentCompletionState } from './component-completion-state.enum';
import { NoCompetencyAssessment } from './no-competency-assessment.model';
import { CompetenceyAssessmentCertificateNumber } from "./competency-assessment-certificate-number.model";

export class Competency {
  IndependentAssessmentStatus?: string;
  CompetencyAssesesmentOrganisation?: string;
  NoCompetencyAssessment?: NoCompetencyAssessment = { Declaration: false };
  CompetencyAssessmentCertificateNumber?: CompetenceyAssessmentCertificateNumber = new CompetenceyAssessmentCertificateNumber();
  CompletionState?: ComponentCompletionState =
    ComponentCompletionState.NotStarted;
}
