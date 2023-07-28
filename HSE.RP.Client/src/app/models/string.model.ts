import { ComponentCompletionState } from './component-completion-state.enum';
import { IComponentModel } from './component. interface';

export class StringModel implements IComponentModel {
  stringValue?: string;
  CompletionState?: ComponentCompletionState;
}
