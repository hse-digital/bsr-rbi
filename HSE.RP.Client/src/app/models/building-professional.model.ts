import { ApplicationStatus } from "./application-status.enum";
import { BuildingInspectorClass } from "./building-inspector-class.model";
import { ComponentCompletionState } from "./component-completion-state.enum";
import { IComponentModel } from "./component. interface";
import { PersonalDetails } from "./personal-details.model";
import { StageCompletionState } from "./stage-completion-state.enum";

export class BuildingProfessionalModel implements IComponentModel {
  id?: string;
  PersonalDetails?: PersonalDetails = {};
  InspectorClass?: BuildingInspectorClass = new BuildingInspectorClass();
  ApplicationStatus: ApplicationStatus = ApplicationStatus.None;

  //TODO test StageStatus and replace ApplicationStatus
  StageStatus: Record<string, StageCompletionState> = {
    EmailVerification: StageCompletionState.Incomplete,
    PhoneVerification: StageCompletionState.Incomplete,
    PersonalDetails: StageCompletionState.Incomplete,
    BuildingInspectorClass: StageCompletionState.Incomplete,
    Competency: StageCompletionState.Incomplete,
    ProfessionalActivity: StageCompletionState.Incomplete,
    Declaration: StageCompletionState.Incomplete,
    Payment: StageCompletionState.Incomplete,
  };

  ReturningApplication: boolean = false;
  get CompletionState(): ComponentCompletionState {
    return this!.ApplicationStatus! ==
      ApplicationStatus.ApplicationSubmissionComplete
      ? ComponentCompletionState.Complete
      : ComponentCompletionState.InProgress;
  }
}