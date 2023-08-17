import { AddressModel } from "./address.model";
import { ApplicantDateOfBirth } from "./applicant-date-of-birth.model";
import { ApplicantEmail } from "./applicant-email.model";
import { ApplicantName } from "./applicant-name.model";
import { ApplicantNationalInsuranceNumber } from "./applicant-national-insurance-number.model";
import { ApplicantPhone } from "./applicant-phone-model";
import { ComponentCompletionState } from "./component-completion-state.enum";
import { EmployerName } from "./employment-employer-name";
import { EmploymentTypeSelection } from "./employment-type-selection.model";

export class ApplicantEmploymentDetails {
  EmploymentTypeSelection?: EmploymentTypeSelection = new EmploymentTypeSelection();
  EmployerName?: EmployerName = new EmployerName();
  EmployerAddress?: AddressModel = new AddressModel();
  CompletionState?: ComponentCompletionState = ComponentCompletionState.NotStarted;

}

