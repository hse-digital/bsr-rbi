import { ComponentCompletionState } from "./component-completion-state.enum";
import { IComponentModel } from "./component. interface";

export class EmploymentType implements IComponentModel {
    PublicSector?: boolean;
    PrivateSector?: boolean;
    Other?: boolean;
    Unemployed?: boolean;
    CompletionState?: ComponentCompletionState = ComponentCompletionState.NotStarted;
}
