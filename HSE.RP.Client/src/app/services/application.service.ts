import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { firstValueFrom } from "rxjs";
import { LocalStorage } from "src/app/helpers/local-storage";
import { AddressModel } from "./address.service";

@Injectable()
export class ApplicationService {

  model: BuildingProfessionalModel;


  constructor(private httpClient: HttpClient) {
    this.model = LocalStorage.getJSON('application_data') ?? {};
  }

  newApplication() {
    LocalStorage.remove('application_data');
    this.model = new BuildingProfessionalModel();
  }

  updateLocalStorage() {
    LocalStorage.setJSON('application_data', this.model)
  }

  clearApplication() {
    this.model = new BuildingProfessionalModel();
    this.updateLocalStorage();
  }

  async sendVerificationEmail(EmailAddress: string): Promise<void> {
    await firstValueFrom(this.httpClient.post('api/SendVerificationEmail', { "EmailAddress": EmailAddress }));
  }

  async validateOTPToken(OTPToken: string, EmailAddress: string): Promise<void> {
    await firstValueFrom(this.httpClient.post('api/ValidateOTPToken', {
      "OTPToken": OTPToken,
      "EmailAddress": EmailAddress
    }));
  }


  async updateApplication(): Promise<void> {
    this.updateLocalStorage();

    if (this.model?.Id) {
      await firstValueFrom(this.httpClient.put(`api/UpdateApplication/${this.model.Id}`, this.model));
    }
  }

  async continueApplication(ApplicationNumber: string, EmailAddress: string, OTPToken: string): Promise<void> {
    let application: BuildingProfessionalModel = await firstValueFrom(this.httpClient.get<BuildingProfessionalModel>(`api/GetApplication/${ApplicationNumber}/${EmailAddress}/${OTPToken}`)); //TODO update when information in Dynamics
    this.clearApplication();
    this.model = new BuildingProfessionalModel();
    this.model.Id = ApplicationNumber;
    this.model.PersonalDetails = {};
    this.model.PersonalDetails!.ApplicantEmail=EmailAddress;
    this.model.ReturningApplication = true;
    this.updateLocalStorage();
  }

}

export class BuildingProfessionalModel {
  Id?: String;
  PersonalDetails?: PersonalDetails = {};
  ApplicationStatus: ApplicationStatus = ApplicationStatus.None
  ReturningApplication: boolean = false;
}

export class PersonalDetails {
  ApplicantName?: ApplicantName = {};
  ApplicantPhoto?: string //Blob
  ApplicantAddress?: AddressModel;
  ApplicantPhone?: string;
  ApplicantAlternativePhone?: string;
  ApplicantEmail?: string;
  ApplicantAlternativeEmail?: string;
  ApplicantProofOfIdentity?: string; //Blob
}

export class ApplicantName {
  FirstName?: string
  LastName?: string
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
