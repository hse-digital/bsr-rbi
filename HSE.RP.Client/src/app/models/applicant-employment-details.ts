import { AddressModel } from "./address.model";
import { ApplicantDateOfBirth } from "./applicant-date-of-birth.model";
import { ApplicantEmail } from "./applicant-email.model";
import { ApplicantName } from "./applicant-name.model";
import { ApplicantNationalInsuranceNumber } from "./applicant-national-insurance-number.model";
import { ApplicantPhone } from "./applicant-phone-model";
import { EmployerName } from "./employment-employer-name";

export class ApplicantEmploymentDetails {
  EmployerType?: string = '';
  EmployerName?: EmployerName = new EmployerName();
}

