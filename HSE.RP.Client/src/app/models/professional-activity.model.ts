import { ApplicantProfessionalBodyMembership } from "./applicant-professional-body-membership";
import { EmploymentType } from "./employment-type";

export class ProfessionalActivity {
  ApplicantProfessionalBodyMembership?: ApplicantProfessionalBodyMembership = new ApplicantProfessionalBodyMembership()
  EmploymentType?: EmploymentType = new EmploymentType();
}
