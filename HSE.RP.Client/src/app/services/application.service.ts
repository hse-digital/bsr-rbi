import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { firstValueFrom } from "rxjs";
import { LocalStorage } from "src/app/helpers/local-storage";
import { AddressModel } from "./address.service";

@Injectable()
export class ApplicationService {

  model: BuildingInspectorModel;

  constructor(private httpClient: HttpClient) {
    this.model = LocalStorage.getJSON('application_data') ?? {};
  }

  newApplication() {
    LocalStorage.remove('application_data');
    this.model = new BuildingInspectorModel();
  }

  updateLocalStorage() {
    LocalStorage.setJSON('application_data', this.model)
  }

  clearApplication() {
    this.model = new BuildingInspectorModel();
    this.updateLocalStorage();
  }

  async sendVerificationEmail(emailAddress: string): Promise<void> {
    await firstValueFrom(this.httpClient.post('api/SendVerificationEmail', { "EmailAddress": emailAddress }));
  }

  async validateOTPToken(otpToken: string, emailAddress: string): Promise<void> {
    await firstValueFrom(this.httpClient.post('api/ValidateOTPToken', {
      "OTPToken": otpToken,
      "EmailAddress": emailAddress
    }));
  }


  async updateApplication(): Promise<void> {
    this.updateLocalStorage();

    if (this.model?.id) {
      await firstValueFrom(this.httpClient.put(`api/UpdateApplication/${this.model.id}`, this.model));
    }
  }

}

export class BuildingInspectorModel {
  id?: String;
  personalDetails?: PersonalDetails = {};
  applicationStatus: ApplicationStatus = ApplicationStatus.None
}

export class PersonalDetails {
  applicantName?: ApplicantName = {};
  applicantPhoto?: string //Blob
  applicantAddress?: AddressModel;
  applicantPhone?: string;
  applicantAlternativePhone?: string;
  applicantEmail?: string;
  applicantAlternativeEmail?: string;
  applicantProofOfIdentity?: string; //Blob

}

export class ApplicantName {
  firstName?: string
  lastName?: string
}

export enum ApplicationStatus {
  None = 0,
  PersonalDetailsComplete = 1,
  BuildingInspectorClassComplete = 2,
  CompetencyComplete = 4,
  ProfessionalActivityComplete = 8,
  ApplicationOverviewComplete = 16,
  PayAndSumbitComplete = 32,
}
