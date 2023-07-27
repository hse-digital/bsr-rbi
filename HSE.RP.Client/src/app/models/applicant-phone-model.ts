import { ComponentCompletionState } from "./component-completion-state.enum";
import { IComponentModel } from "./component. interface";

export class ApplicantPhone implements IComponentModel {
    PhoneNumber?: string;
  CompletionState?: ComponentCompletionState = ComponentCompletionState.NotStarted;
  }
