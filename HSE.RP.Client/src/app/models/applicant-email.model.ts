import { ComponentCompletionState } from "./component-completion-state.enum";
import { IComponentModel } from "./component. interface";

export class ApplicantEmail implements IComponentModel {
  Email?: string = '';
  CompletionState?: ComponentCompletionState = ComponentCompletionState.NotStarted;
  }
