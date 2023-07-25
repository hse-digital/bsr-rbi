import { BuildingInspectorClassType } from "./building-inspector-classtype.enum";
import { ComponentCompletionState } from "./component-completion-state.enum";
import { IComponentModel } from "./component. interface";

export class ClassSelection implements IComponentModel {
    Class?: BuildingInspectorClassType;
    CompletionState?: ComponentCompletionState;
  }