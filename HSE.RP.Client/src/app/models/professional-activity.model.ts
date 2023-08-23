import { ApplicantEmploymentDetails } from "./applicant-employment-details";
import { ApplicantProfessionalBodyMembership } from "./applicant-professional-body-membership";
import { ComponentCompletionState } from "./component-completion-state.enum";
import { EmploymentTypeSelection } from "./employment-type-selection.model";

export class ProfessionalActivity {
  ApplicantProfessionalBodyMembership?: ApplicantProfessionalBodyMembership = new ApplicantProfessionalBodyMembership();
  EmploymentDetails?: ApplicantEmploymentDetails = new ApplicantEmploymentDetails();
  CompletionState?: ComponentCompletionState = ComponentCompletionState.NotStarted;

}
