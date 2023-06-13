import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { firstValueFrom } from "rxjs";
import { LocalStorage } from "src/app/helpers/local-storage";
import { AddressModel } from "./address.service";

@Injectable()
export class ApplicationService {
  // replace this any to a specific type
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
  applicationName?: string //TODO review if required
  personalDetails?: PersonalDetails = {};
  applicationEmail?: string
  registrationStatus: ApplicationStatus = ApplicationStatus.None
}

export class PersonalDetails {
  applicantName?: ApplicantName = {};
  applicantPhoto?: string //Blob
  applicantAddress?: AddressModel;
  applicantPhone?: string;
  applicantAlternativeEmail?: string;
  applicantProofOfIdentity?: string;

}

export class ApplicantName {
  firstName?: string
  lastName?: string
}

export enum ApplicationStatus {
  None = 0,
  BlocksInBuildingInProgress = 1,
  BlocksInBuildingComplete = 2,
  AccountablePersonsInProgress = 4,
  AccountablePersonsComplete = 8,
  PaymentInProgress = 16,
  PaymentComplete = 32,
/*   KbiCheckBeforeInProgress = 64,
  KbiCheckBeforeComplete = 128,
  KbiStructureInformationInProgress = 256,
  KbiStructureInformationComplete = 512,
  KbiConnectionsInProgress = 1024,
  KbiConnectionsComplete = 2048,
  KbiSubmitInProgress = 4096,
  KbiSubmitComplete = 8192 */

}
