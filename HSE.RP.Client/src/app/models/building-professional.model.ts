import { ApplicantProfessionBodyMemberships } from './applicant-professional-body-membership';
import { ApplicationStage } from './application-stage.enum';
import { BuildingInspectorClass } from './building-inspector-class.model';
import { Competency } from './competency.model';
import { ComponentCompletionState } from './component-completion-state.enum';
import { IComponentModel } from './component. interface';
import { PersonalDetails } from './personal-details.model';
import { ProfessionalActivity } from './professional-activity.model';
import { StageCompletionState } from './stage-completion-state.enum';

export class BuildingProfessionalModel implements IComponentModel {
  id?: string;
  CosmosId?: string;
  PersonalDetails?: PersonalDetails = new PersonalDetails();
  InspectorClass?: BuildingInspectorClass = new BuildingInspectorClass();
  Competency?: Competency = new Competency();
  ProfessionalActivity: ProfessionalActivity = new ProfessionalActivity();
  //ApplicationStatus: ApplicationStatus = ApplicationStatus.None;
  ProfessionalMemberships: ApplicantProfessionBodyMemberships = new ApplicantProfessionBodyMemberships();

  ApplicationStage: ApplicationStage = ApplicationStage.NotStarted;

  private _completionState: ComponentCompletionState =
    ComponentCompletionState.NotStarted;



  //TODO test StageStatus and replace ApplicationStatus
  StageStatus: Record<string, StageCompletionState> = {
    EmailVerification: StageCompletionState.Incomplete,
    PhoneVerification: StageCompletionState.Incomplete,
    PersonalDetails: StageCompletionState.Incomplete,
    BuildingInspectorClass: StageCompletionState.Incomplete,
    Competency: StageCompletionState.Incomplete,
    ProfessionalActivity: StageCompletionState.Incomplete,
    ApplicationConfirmed: StageCompletionState.Incomplete,
    Declaration: StageCompletionState.Incomplete,
    Payment: StageCompletionState.Incomplete,
  };



  ReturningApplication: boolean = false;

  get CompletionState(): ComponentCompletionState {
    return this!.StageStatus!["Payment"] ==
      StageCompletionState.Complete
      ? ComponentCompletionState.Complete
      : ComponentCompletionState.InProgress;
  }
  set CompletionState(value: ComponentCompletionState) {
    this._completionState = value;
  }
}
