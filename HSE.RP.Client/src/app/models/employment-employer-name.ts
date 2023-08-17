import { ComponentCompletionState } from "./component-completion-state.enum";
import { IComponentModel } from "./component. interface";

export class EmployerName implements IComponentModel {
  FullName?: string = '';
  CompletionState?: ComponentCompletionState = ComponentCompletionState.NotStarted;
  }
