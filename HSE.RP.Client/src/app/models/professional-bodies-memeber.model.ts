import { ComponentCompletionState } from './component-completion-state.enum';
import { IComponentModel } from './component. interface';

export class ProfessionalBodiesMember implements IComponentModel {
  Membership?: string = 'no';
  CompletionState?: ComponentCompletionState =
    ComponentCompletionState.NotStarted;
}
