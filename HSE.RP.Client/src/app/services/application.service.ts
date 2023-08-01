import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { LocalStorage } from 'src/app/helpers/local-storage';
import { BuildingProfessionalModel } from '../models/building-professional.model';
import { ApplicationStatus } from '../models/application-status.enum';

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
        OTPToken: OTPToken,
        Data: Data.toLowerCase(),
      })
    );
    var xx = response;
  }

  async checkIsTokenExpired(OTPToken: string, Data: string): Promise<boolean> {
    return await firstValueFrom(
      this.httpClient.post<boolean>('api/IsOTPTokenExpired', {
        OTPToken: OTPToken,
        Data: Data.toLowerCase(),
      })
    );
  }

  async registerNewBuildingProfessionApplication(): Promise<void> {
    this.model.ApplicationStatus = ApplicationStatus.PhoneVerified;
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
          `api/GetApplicationPhone/${ApplicationNumber.toUpperCase()}/${PhoneNumber}/${OTPToken}`
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
            `api/ValidateApplicationNumberPhone/${PhoneNumber!}/${ApplicationNumber.toUpperCase()}`
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

  async syncDeclaration(): Promise<void> {
    await firstValueFrom(
      this.httpClient.post(`api/SyncDeclaration`, this.model)
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
}
