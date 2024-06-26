import { ComponentCompletionState } from './component-completion-state.enum';
import { IComponentModel } from './component. interface';

export class NoCompetencyAssessment implements IComponentModel {
  [key: string]: any;
  Declaration?: boolean;
  CompletionState?: ComponentCompletionState =
    ComponentCompletionState.NotStarted;
}
