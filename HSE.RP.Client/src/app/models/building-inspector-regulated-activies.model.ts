import { ComponentCompletionState } from "./component-completion-state.enum";

export class BuildingInspectorRegulatedActivies {
    [key: string]: any;
    AssessingPlans: boolean = false;
    Inspection: boolean = false;
    CompletionState: ComponentCompletionState = ComponentCompletionState.NotStarted;
  }