import { ComponentCompletionState } from "./component-completion-state.enum";
import { IComponentModel } from "./component. interface";

export class ApplicantNationalInsuranceNumber implements IComponentModel {
    NationalInsuranceNumber?: string = '';
    CompletionState?: ComponentCompletionState;
  }
