import { ComponentCompletionState } from "./component-completion-state.enum";
import { IComponentModel } from "./component. interface";

export class CompetencyAssesesmentOrganisation implements IComponentModel {
    ComAssesesmentOrganisation: string = '';
    CompletionState?: ComponentCompletionState = ComponentCompletionState.NotStarted;
}