import { ComponentCompletionState } from "./component-completion-state.enum";

export class Competency {
  IndependentAssessmentStatus?: string;
  CompletionState?: ComponentCompletionState = ComponentCompletionState.NotStarted;
}
