import { ComponentCompletionState } from "./component-completion-state.enum";
import { IComponentModel } from "./component. interface";
import { EmploymentType } from "./employment-type.enum";

export class EmploymentTypeSelection implements IComponentModel {
    EmploymentType?: EmploymentType = EmploymentType.None;
    CompletionState?: ComponentCompletionState = ComponentCompletionState.NotStarted;
}
