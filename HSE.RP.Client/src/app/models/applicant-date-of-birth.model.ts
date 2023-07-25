import { ComponentCompletionState } from "./component-completion-state.enum";
import { IComponentModel } from "./component. interface";

export class ApplicantDateOfBirth implements IComponentModel {
    Day?: string;
    Month?: string;
    Year?: string;
    CompletionState?: ComponentCompletionState;
  }