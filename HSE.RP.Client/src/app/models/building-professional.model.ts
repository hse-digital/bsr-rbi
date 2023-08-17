import { ApplicantEmploymentDetails } from './applicant-employment-details';
import { ApplicantProfessionBodyMemberships } from './applicant-professional-body-membership';
import { ApplicationStatus } from './application-status.enum';
import { BuildingInspectorClass } from './building-inspector-class.model';
import { Competency } from './competency.model';
import { ComponentCompletionState } from './component-completion-state.enum';
import { IComponentModel } from './component. interface';
import { PersonalDetails } from './personal-details.model';
import { ProfessionalActivity } from './professional-activity.model';
import { StageCompletionState } from './stage-completion-state.enum';

export class BuildingProfessionalModel implements IComponentModel {
  id?: string;
  PersonalDetails?: PersonalDetails = new PersonalDetails();
  InspectorClass?: BuildingInspectorClass = new BuildingInspectorClass();
  Competency?: Competency = new Competency();
  ProfessionalActivity: ProfessionalActivity = new ProfessionalActivity();
  ApplicationStatus: ApplicationStatus = ApplicationStatus.None;
  ProfessionalMemberships: ApplicantProfessionBodyMemberships =
    new ApplicantProfessionBodyMemberships();
  EmploymentDetails: ApplicantEmploymentDetails = new ApplicantEmploymentDetails();
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
  set CompletionState(value: ComponentCompletionState) {
    this._completionState = value;
  }
}
