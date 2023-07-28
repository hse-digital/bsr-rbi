import { CompetencyDateOfAssessment } from './competency-date-of-assessment.model';
import { ComponentCompletionState } from './component-completion-state.enum';
import { NoCompetencyAssessment } from './no-competency-assessment.model';

export class Competency {
  IndependentAssessmentStatus?: string;
  CompetencyAssesesmentOrganisation?: string;
  CompetencyDateOfAssessment: CompetencyDateOfAssessment = {};
  NoCompetencyAssessment?: NoCompetencyAssessment = { Declaration: false };
  
  CompletionState?: ComponentCompletionState =
    ComponentCompletionState.NotStarted;
}
