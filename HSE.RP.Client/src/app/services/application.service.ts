import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { firstValueFrom } from "rxjs";
import { LocalStorage } from "src/app/helpers/local-storage";

@Injectable()
export class ApplicationService {
  // replace this any to a specific type
  model: BuildingControlModel;

  constructor(private httpClient: HttpClient) {
    this.model = LocalStorage.getJSON('application_data') ?? {};
  }

  newApplication() {
    LocalStorage.remove('application_data');
    this.model = new BuildingControlModel();
  }

  updateLocalStorage() {
    LocalStorage.setJSON('application_data', this.model)
  }

  clearApplication() {
    this.model = new BuildingControlModel();
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

export class BuildingControlModel {
  id?: String;
  ApplicationType?: ApplicationType
  ApplicationName?: string
  FirstName?: string
  LastName?: string
  PhoneNumber?: string
  Email?: string
}

  export enum ApplicationType{
    NewHRB,
    ExistingHRB,
    ExisitngBuildingToHRB
  }

export class ApplicantName {
  firstName?: string
  lastName?: string
}
