import { AddressModel } from "./address.model";
import { ApplicantDateOfBirth } from "./applicant-date-of-birth.model";
import { ApplicantEmail } from "./applicant-email.model";
import { ApplicantName } from "./applicant-name.model";
import { ApplicantNationalInsuranceNumber } from "./applicant-national-insurance-number.model";
import { ApplicantPhone } from "./applicant-phone-model";

export class PersonalDetails {
  ApplicantName?: ApplicantName = new ApplicantName();
  ApplicantDateOfBirth?: ApplicantDateOfBirth = new ApplicantDateOfBirth();
    ApplicantAddress?: AddressModel; // Don't initialise this as there is a requirement it is null
    ApplicantPhone?: ApplicantPhone= new ApplicantPhone();
    ApplicantAlternativePhone?: ApplicantPhone = new ApplicantPhone();
    ApplicantEmail?: ApplicantEmail = new ApplicantEmail();
    ApplicantAlternativeEmail?: ApplicantEmail = new ApplicantEmail();
  ApplicantNationalInsuranceNumber?: ApplicantNationalInsuranceNumber = new ApplicantNationalInsuranceNumber();
  }

