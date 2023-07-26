import { ComponentCompletionState } from "./component-completion-state.enum";

export class Competency {
  IndependentAssessmentStatus?: string;
  CompetencyAssesesmentOrganisation?: string;
  CompletionState?: ComponentCompletionState = ComponentCompletionState.NotStarted;
}
