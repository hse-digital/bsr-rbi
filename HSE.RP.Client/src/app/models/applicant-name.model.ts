import { ComponentCompletionState } from "./component-completion-state.enum";
import { IComponentModel } from "./component. interface";

export class ApplicantName implements IComponentModel {
    FirstName?: string;
    LastName?: string;
    CompletionState?: ComponentCompletionState;
  }