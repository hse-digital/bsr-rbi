import { ComponentCompletionState } from './component-completion-state.enum';
import { NoCompetencyAssessment } from './no-competency-assessment.model';

export class Competency {
  IndependentAssessmentStatus?: string;
  CompetencyAssesesmentOrganisation?: string;
  NoCompetencyAssessment?: NoCompetencyAssessment = { Declaration: false };
  CompletionState?: ComponentCompletionState =
    ComponentCompletionState.NotStarted;
}
