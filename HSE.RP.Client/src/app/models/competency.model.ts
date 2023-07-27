import { CompetenceyAssessmentCertificateNumber } from "./competency-assessment-certificate-number.model";
import { ComponentCompletionState } from "./component-completion-state.enum";

export class Competency {
  IndependentAssessmentStatus?: string = "";
  CompetencyAssesesmentOrganisation?: string = "";
  CompetencyAssessmentCertificateNumber?: CompetenceyAssessmentCertificateNumber = new CompetenceyAssessmentCertificateNumber();
  CompletionState?: ComponentCompletionState = ComponentCompletionState.NotStarted;
}
