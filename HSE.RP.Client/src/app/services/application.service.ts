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

  async sendVerificationSms(PhoneNumber: string): Promise<void> {
    await firstValueFrom(this.httpClient.post('api/SendVerificationSms', { "PhoneNumber": PhoneNumber }));
  }

  async validateOTPToken(OTPToken: string, Data: string): Promise<void> {
    await firstValueFrom(this.httpClient.post('api/ValidateOTPToken', {
      "OTPToken": OTPToken,
      "Data": Data
    }));
  }

  async registerNewBuildingProfessionApplication(): Promise<void> {
    this.model.ApplicationStatus = ApplicationStatus.PhoneVerified;
    this.model = await firstValueFrom(this.httpClient.post<BuildingProfessionalModel>('api/NewBuildingProfessionalApplication', this.model));
    this.updateLocalStorage();
  }

  async updateApplication(): Promise<void> {
    this.updateLocalStorage();

    if (this.model?.id) {
      await firstValueFrom(this.httpClient.put(`api/UpdateApplication/${this.model.id}`, this.model));
    }
  }

  async continueApplication(ApplicationNumber: string, PhoneNumber: string, OTPToken: string): Promise<void> {
    let application: BuildingProfessionalModel = await firstValueFrom(this.httpClient.get<BuildingProfessionalModel>(`api/GetApplication/${ApplicationNumber}/${PhoneNumber}/${OTPToken}`));
    this.model = application;
    this.model.ReturningApplication = true;
    this.updateLocalStorage();
  }

  async isApplicationNumberValid(EmailAddress: string, ApplicationNumber: string): Promise<boolean> {
    try {
      await firstValueFrom(this.httpClient.get(`api/ValidateApplicationNumber/${EmailAddress.toLowerCase()}/${ApplicationNumber}`));
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
  ApplicantDateOfBirth?: ApplicantDateOfBirth = {};
  ApplicantAddress?: AddressModel;
  ApplicantPhone?: string;
  ApplicantAlternativePhone?: string;
  ApplicantEmail?: string;
  ApplicantAlternativeEmail?: string;
}

export class ApplicantName {
  FirstName?: string
  LastName?: string
}

export class ApplicantDateOfBirth {
   Day?: string;
   Month?: string;
   Year?: string;
}

export enum ApplicationStatus {
  None = 0,
  EmailVerified = 1,
  PhoneVerified = 2,
  PersonalDetailsComplete = 4,
  BuildingInspectorClassComplete = 8,
  CompetencyComplete = 16,
  ProfessionalActivityComplete = 23,
  ApplicationSubmissionComplete = 64,
  PayAndSubmitComplete = 128,
}

