import { ComponentCompletionState } from './component-completion-state.enum';
import { IComponentModel } from './component. interface';

export class CompetencyIndependentAssessmentStatus implements IComponentModel {
  IAStatus?: string = 'no';
  CompletionState?: ComponentCompletionState =
    ComponentCompletionState.NotStarted;
}
