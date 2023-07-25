import { ComponentCompletionState } from "./component-completion-state.enum";
import { IComponentModel } from "./component. interface";

export class NumberModel implements IComponentModel {
  numberValue?: number;
  CompletionState?: ComponentCompletionState;
}