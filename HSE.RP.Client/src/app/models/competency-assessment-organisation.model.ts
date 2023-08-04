import { ComponentCompletionState } from "./component-completion-state.enum";
import { IComponentModel } from "./component. interface";

export class CompetencyAssessmentOrganisation implements IComponentModel {
    ComAssessmentOrganisation: string = '';
    CompletionState?: ComponentCompletionState = ComponentCompletionState.NotStarted;
}
