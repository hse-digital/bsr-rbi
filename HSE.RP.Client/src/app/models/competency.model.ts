import { CompetencyDateOfAssessment } from './competency-date-of-assessment.model';
import { ComponentCompletionState } from './component-completion-state.enum';
import { NoCompetencyAssessment } from './no-competency-assessment.model';
import { CompetenceyAssessmentCertificateNumber } from './competency-assessment-certificate-number.model';
import { IndependentAssessmentStatus } from './independent-assessment-status.model';
import { CompetencyAssesesmentOrganisation } from './competency-assesesment-organisation.model';

export class Competency {
  IndependentAssessmentStatus?: IndependentAssessmentStatus =
    new IndependentAssessmentStatus();
  CompetencyAssesesmentOrganisation?: CompetencyAssesesmentOrganisation =
    new CompetencyAssesesmentOrganisation();
  CompetencyDateOfAssessment: CompetencyDateOfAssessment = {};
  NoCompetencyAssessment?: NoCompetencyAssessment = { Declaration: false };
  CompetencyAssessmentCertificateNumber?: CompetenceyAssessmentCertificateNumber =
    new CompetenceyAssessmentCertificateNumber();

  // CompletionState?: ComponentCompletionState =
  //   ComponentCompletionState.NotStarted;
}
