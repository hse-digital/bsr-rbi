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

  async registerNewBuildingProfessionApplication(): Promise<void> {
    this.model.ApplicationStatus = ApplicationStatus.EmailVerified;
    this.model = await firstValueFrom(this.httpClient.post<BuildingProfessionalModel>('api/NewBuildingProfessionalApplication', this.model));
    this.updateLocalStorage();
  }

  async updateApplication(): Promise<void> {
    this.updateLocalStorage();

    if (this.model?.id) {
      await firstValueFrom(this.httpClient.put(`api/UpdateApplication/${this.model.id}`, this.model));
    }
  }

  async continueApplication(ApplicationNumber: string, EmailAddress: string, OTPToken: string): Promise<void> {
    let application: BuildingProfessionalModel = await firstValueFrom(this.httpClient.get<BuildingProfessionalModel>(`api/GetApplication/${ApplicationNumber}/${EmailAddress}/${OTPToken}`));
    this.model = application;
    this.model.ReturningApplication = true;
    this.updateLocalStorage();
  }

  async isApplicationNumberValid(emailAddress: string, applicationNumber: string): Promise<boolean> {
    try {
      await firstValueFrom(this.httpClient.get(`api/ValidateApplicationNumber/${emailAddress.toLowerCase()}/${applicationNumber}`));
      return true;
    } catch {
      return false;
    }
  }
}

export class BuildingProfessionalModel {
  id?: String;
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
  EmailVerified = 1,
  PersonalDetailsComplete = 2,
  BuildingInspectorClassComplete = 4,
  CompetencyComplete = 8,
  ProfessionalActivityComplete = 16,
  ApplicationOverviewComplete = 32,
  PayAndSubmitComplete = 64,
}
