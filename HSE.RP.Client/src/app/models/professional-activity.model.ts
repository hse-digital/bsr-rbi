import { ApplicantProfessionalBodyMembership } from "./applicant-professional-body-membership";
import { EmploymentTypeSelection } from "./employment-type-selection.model";

export class ProfessionalActivity {
  ApplicantProfessionalBodyMembership?: ApplicantProfessionalBodyMembership = new ApplicantProfessionalBodyMembership();
  EmploymentTypeSelection?: EmploymentTypeSelection = new EmploymentTypeSelection();
}
