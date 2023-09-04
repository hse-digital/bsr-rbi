import { ComponentCompletionState } from "./component-completion-state.enum";
import { IComponentModel } from "./component. interface";

export class BuildingInspectorCountryOfWork implements IComponentModel{
    [key: string]: any;
    England?: boolean;
    Wales?: boolean;
    CompletionState?: ComponentCompletionState;
  }
