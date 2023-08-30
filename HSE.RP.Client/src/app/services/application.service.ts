import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom, lastValueFrom } from 'rxjs';
import { LocalStorage } from 'src/app/helpers/local-storage';
import { BuildingProfessionalModel } from '../models/building-professional.model';
import { ApplicationStatus } from '../models/application-status.enum';
import { StageCompletionState } from '../models/stage-completion-state.enum';

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
    LocalStorage.setJSON('application_data', this.model);
  }

  clearApplication() {
    this.model = new BuildingProfessionalModel();
    this.updateLocalStorage();
  }

  async sendVerificationEmail(EmailAddress: string): Promise<void> {
    await firstValueFrom(
      this.httpClient.post('api/SendVerificationEmail', {
        EmailAddress: EmailAddress,
      })
    );
  }

  async sendVerificationSms(PhoneNumber: string): Promise<void> {
    await firstValueFrom(
      this.httpClient.post('api/SendVerificationSms', {
        PhoneNumber: PhoneNumber,
      })
    );
  }

  async validateOTPToken(OTPToken: string, Data: string): Promise<void> {
    var response = await firstValueFrom(
      this.httpClient.post('api/ValidateOTPToken', {
        OTPToken: OTPToken.trim(),
        Data: Data.toLowerCase().trim().replace("+44", "0"),
      })
    );
    var xx = response;
  }

  async registerNewBuildingProfessionApplication(): Promise<void> {
    this.model.StageStatus['PhoneVerification'] = StageCompletionState.Complete;
    this.model.StageStatus['EmailVerification'] = StageCompletionState.Complete;
    this.model = await firstValueFrom(
      this.httpClient.post<BuildingProfessionalModel>(
        'api/NewBuildingProfessionalApplication',
        this.model
      )
    );
    this.updateLocalStorage();
  }

  async updateApplication(): Promise<void> {
    this.updateLocalStorage();

    if (this.model?.id) {
      await firstValueFrom(
        this.httpClient.put(
          `api/UpdateApplication/${this.model.id}`,
          this.model
        )
      );
    }
  }

  async continueApplication(
    ApplicationNumber: string,
    OTPToken: string,
    ValidationOption: string,
    EmailAddress?: string,
    PhoneNumber?: string
  ): Promise<void> {
    if (ValidationOption === 'email-option') {
      let application: BuildingProfessionalModel = await firstValueFrom(
        this.httpClient.get<BuildingProfessionalModel>(
          `api/GetApplicationEmail/${ApplicationNumber.toUpperCase()}/${EmailAddress}/${OTPToken}`
        )
      );
      this.model = application;
    } else if (ValidationOption === 'phone-option') {
      let application: BuildingProfessionalModel = await firstValueFrom(
        this.httpClient.get<BuildingProfessionalModel>(
          `api/GetApplicationPhone/${ApplicationNumber.toUpperCase()}/${PhoneNumber?.trim().replace("+44","0")}/${OTPToken}`
        )
      );
      this.model = application;
    }
    this.model.ReturningApplication = true;
    this.updateLocalStorage();
  }

  async validateReturningApplicationDetails(
    ApplicationNumber: string,
    ValidationOption: string,
    EmailAddress?: string,
    PhoneNumber?: string
  ): Promise<{
    IsValid: boolean;
    IsValidApplicationNumber: boolean;
    EmailAddress: string;
    PhoneNumber: string;
  }> {
    try {
      if ((ValidationOption === 'email-option')) {
        return await firstValueFrom(
          this.httpClient.get<{
            IsValid: boolean;
            IsValidApplicationNumber: boolean;
            EmailAddress: string;
            PhoneNumber: string;
          }>(
            `api/ValidateApplicationNumberPhone/${PhoneNumber!.trim().replace("+44","0")}/${ApplicationNumber.toUpperCase()}`
          )
        );
      } else if ((ValidationOption === 'phone-option')) {
        return await firstValueFrom(
          this.httpClient.get<{
            IsValid: boolean;
            IsValidApplicationNumber: boolean;
            EmailAddress: string;
            PhoneNumber: string;
          }>(
            `api/ValidateApplicationNumberEmail/${EmailAddress!.toLowerCase()}/${ApplicationNumber.toUpperCase()}`
          )
        );
      } else {
        return {
          IsValid: false,
          IsValidApplicationNumber: false,
          EmailAddress: '',
          PhoneNumber: '',
        };
      }
    } catch (error) {
      return {
        IsValid: false,
        IsValidApplicationNumber: false,
        EmailAddress: '',
        PhoneNumber: '',
      };
    }
  }

  async syncPayment(): Promise<void> {
    await firstValueFrom(this.httpClient.post(`api/SyncPayment`, this.model));
  }

  async getApplicationPayments(): Promise<any[]> {
    return await firstValueFrom(
      this.httpClient.get<any[]>(
        `api/GetApplicationPaymentStatus/${this.model.id}`
      )
    );
  }

  async getPublicSectorBodies(): Promise<string[]> {
    return await firstValueFrom(
      this.httpClient.get<string[]>(
        `api/GetPublicSectorBodies`
      )
    );
  }

  async syncDeclaration(): Promise<void> {
    await firstValueFrom(
      this.httpClient.post(`api/SyncDeclaration`, this.model)
    );
  }

  async syncCompetency(): Promise<void> {
    await firstValueFrom(
      this.httpClient.post(`api/SyncCompetency`, this.model)
    );
  }

  async syncPersonalDetails(): Promise<void> {
    await firstValueFrom(
      this.httpClient.post(`api/SyncPersonalDetails`, this.model)
    );
  }

  async syncBuildingInspectorClass(): Promise<void> {
    await firstValueFrom(
      this.httpClient.post(`api/SyncBuildingInspectorClass`, this.model)
    );
  }

  async syncProfessionalBodyMemberships(): Promise<void> {
    await firstValueFrom(
      this.httpClient.post(`api/SyncProfessionalBodyMemberships`, this.model)
    );
  }

  async syncEmploymentDetails(): Promise<void> {
    await firstValueFrom(
      this.httpClient.post(`api/SyncEmploymentDetails`, this.model)
    );
  }

  async syncFullApplication(): Promise<void> {
    await firstValueFrom(
      this.httpClient.post(`api/SyncFullApplication`, this.model)
    );
  }

  async CheckDuplicateBuildingProfessionalApplication(): Promise<boolean> {
   return await lastValueFrom(
      this.httpClient.post<boolean>(`api/CheckDuplicateBuildingProfessionalApplication`, this.model)
    );
  }

}
