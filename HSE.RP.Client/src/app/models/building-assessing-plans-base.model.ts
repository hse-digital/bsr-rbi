import { ComponentCompletionState } from "./component-completion-state.enum";

export class BuildingAssessingPlansBase {
    [key: string]: any;
    CategoryA?: boolean;
    CategoryB?: boolean;
    CategoryC?: boolean;
    CategoryD?: boolean;
    CategoryE?: boolean;
    CategoryF?: boolean;
    CompletionState?: ComponentCompletionState = ComponentCompletionState.NotStarted;;
  }
  