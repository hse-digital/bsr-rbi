import { ComponentCompletionState } from "./component-completion-state.enum";

export class BuildingAssessingPlansBase {
    [key: string]: any;
    CategoryA?: boolean = false;
    CategoryB?: boolean = false;
    CategoryC?: boolean = false;
    CategoryD?: boolean = false;
    CategoryE?: boolean = false;
    CategoryF?: boolean = false;
    CompletionState?: ComponentCompletionState = ComponentCompletionState.NotStarted;;
  }
