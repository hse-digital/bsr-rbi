import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom, lastValueFrom } from 'rxjs';
import { LocalStorage } from 'src/app/helpers/local-storage';
import { BuildingProfessionalModel } from '../models/building-professional.model';
import { ApplicationStatus } from '../models/application-status.enum';
import { StageCompletionState } from '../models/stage-completion-state.enum';
import { Sanitizer } from '../helpers/sanitizer';
import { ComponentCompletionState } from '../models/component-completion-state.enum';
import { BuildingInspectorClass } from '../models/building-inspector-class.model';
import { ProfessionalActivity } from '../models/professional-activity.model';
import { ApplicantHasProfessionBodyMemberships, ApplicantProfessionBodyMembershipsHelper } from '../models/applicant-professional-body-membership';
import { ProfessionalActivityHelper } from '../helpers/professional-activity-helper.component';
import { ProfessionalBodyMembershipStep } from '../models/professional-body-membership-step.enum';

@Injectable()
export class ApplicationService {
  model: BuildingProfessionalModel;

  constructor(private httpClient: HttpClient) {
    this.model = LocalStorage.getJSON('application_data') ?? {};
  }

  newApplication() {
    LocalStorage.remove('application_data');
    this.model = new BuildingProfessionalModel();
    LocalStorage.setJSON('application_data', this.model);
  }

  updateLocalStorage() {
    LocalStorage.setJSON('application_data', this.model);
  }

  clearApplication() {
    this.model = new BuildingProfessionalModel();
    this.updateLocalStorage();
  }

  clearSession() {
    sessionStorage.clear();
  }

  async sendVerificationEmail(EmailAddress: string): Promise<void> {
    await firstValueFrom(
      this.httpClient.post('api/SendVerificationEmail', Sanitizer.sanitize({
        EmailAddress: EmailAddress,
      }))
    );
  }

  async sendVerificationSms(PhoneNumber: string): Promise<void> {
    await firstValueFrom(
      this.httpClient.post('api/SendVerificationSms', Sanitizer.sanitize({
        PhoneNumber: PhoneNumber,
      }))
    );
  }

  async validateOTPToken(OTPToken: string, Data: string): Promise<void> {
    var response = await firstValueFrom(
      this.httpClient.post('api/ValidateOTPToken', Sanitizer.sanitize({
        OTPToken: OTPToken.trim(),
        Data: Data.toLowerCase().trim().replace("+44", "0"),
      }))
    );
    var xx = response;
  }

  async registerNewBuildingProfessionApplication(): Promise<void> {
    this.model.StageStatus['PhoneVerification'] = StageCompletionState.Complete;
    this.model.StageStatus['EmailVerification'] = StageCompletionState.Complete;
    this.model = await firstValueFrom(
      this.httpClient.post<BuildingProfessionalModel>(
        'api/NewBuildingProfessionalApplication',
        Sanitizer.sanitize(this.model)
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
          Sanitizer.sanitize(this.model)
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
          `api/GetApplicationPhone/${ApplicationNumber.toUpperCase()}/${PhoneNumber?.trim().replace("+44", "0")}/${OTPToken}`
        )
      );
      this.model = application;
    }
    this.model.ReturningApplication = true;

    //If issue in model where class or professional activity is null set them to new #21710 #21821

    if (!this.model.InspectorClass) {
      this.model.InspectorClass = new BuildingInspectorClass()
    }
    if (!this.model.ProfessionalActivity) {
      this.model.ProfessionalActivity = new ProfessionalActivity();
    }

    //If application does not have inspector class completion state required for #9319 add it
    if (!this.model.InspectorClass?.CompletionState) {

      switch (this.model.InspectorClass?.ClassType?.CompletionState) {
        case ComponentCompletionState.Complete:
          this.model.InspectorClass!.CompletionState = ComponentCompletionState.Complete;
          break;
        case ComponentCompletionState.InProgress:
          this.model.InspectorClass!.CompletionState = ComponentCompletionState.InProgress;
          break;
        default:
          this.model.InspectorClass!.CompletionState = ComponentCompletionState.NotStarted;
          break;
      }

    }

    //If application does not have ApplicantHasProfessionBodyMemberships  required for #9319 add it
    if (!this.model.ProfessionalMemberships.ApplicantHasProfessionBodyMemberships) {
      this.model.ProfessionalMemberships.ApplicantHasProfessionBodyMemberships = new ApplicantHasProfessionBodyMemberships();
      if (ApplicantProfessionBodyMembershipsHelper.AnyCompleted(this.model.ProfessionalMemberships)) {
        this.model.ProfessionalMemberships.ApplicantHasProfessionBodyMemberships.CompletionState = ComponentCompletionState.Complete;
        this.model.ProfessionalMemberships.ApplicantHasProfessionBodyMemberships.IsProfessionBodyRelevantYesNo = "yes";
      }
    }

    if(!this.model.ProfessionalActivity.ApplicantProfessionalBodyMembership?.RemoveOptionSelected)
    {
      this.model.ProfessionalActivity.ApplicantProfessionalBodyMembership!.RemoveOptionSelected = "";
    }

    if(!this.model.ProfessionalActivity.ApplicantProfessionalBodyMembership?.CurrentStep)
    {
      this.model.ProfessionalActivity.ApplicantProfessionalBodyMembership!.CurrentStep = ProfessionalBodyMembershipStep.NotStarted;
    }

    //If application does not have remove option in professional memberships required for #9319 add it
    if (!this.model.ProfessionalMemberships.CABE.RemoveOptionSelected) {
      this.model.ProfessionalMemberships.CABE.RemoveOptionSelected = '';
    }
    if (!this.model.ProfessionalMemberships.RICS.RemoveOptionSelected) {
      this.model.ProfessionalMemberships.RICS.RemoveOptionSelected = '';
    }
    if (!this.model.ProfessionalMemberships.CIOB.RemoveOptionSelected) {
      this.model.ProfessionalMemberships.CIOB.RemoveOptionSelected = '';
    }
    if (!this.model.ProfessionalMemberships.OTHER.RemoveOptionSelected) {
      this.model.ProfessionalMemberships.OTHER.RemoveOptionSelected = '';
    }

    //If application does not have professional membership current step  required for #9319 add it
    if (!this.model.ProfessionalMemberships.CABE.CurrentStep) {
      if (this.model.ProfessionalMemberships.CABE.CompletionState == ComponentCompletionState.Complete) {
        this.model.ProfessionalMemberships.CABE.CurrentStep = ProfessionalBodyMembershipStep.ConfirmDetails;
      }
      else {
        this.model.ProfessionalMemberships.CABE.CurrentStep = ProfessionalBodyMembershipStep.NotStarted;
      }
    }
    if (!this.model.ProfessionalMemberships.RICS.CurrentStep) {
      if (this.model.ProfessionalMemberships.RICS.CompletionState == ComponentCompletionState.Complete) {
        this.model.ProfessionalMemberships.RICS.CurrentStep = ProfessionalBodyMembershipStep.ConfirmDetails;
      }
      else {
        this.model.ProfessionalMemberships.RICS.CurrentStep = ProfessionalBodyMembershipStep.NotStarted;
      }
    }
    if (!this.model.ProfessionalMemberships.CIOB.CurrentStep) {
      if (this.model.ProfessionalMemberships.CIOB.CompletionState == ComponentCompletionState.Complete) {
        this.model.ProfessionalMemberships.CIOB.CurrentStep = ProfessionalBodyMembershipStep.ConfirmDetails;
      }
      else {
        this.model.ProfessionalMemberships.CIOB.CurrentStep = ProfessionalBodyMembershipStep.NotStarted;
      }
    }
    if (!this.model.ProfessionalMemberships.OTHER.CurrentStep) {
      if (this.model.ProfessionalMemberships.OTHER.CompletionState == ComponentCompletionState.Complete) {
        this.model.ProfessionalMemberships.OTHER.CurrentStep = ProfessionalBodyMembershipStep.ConfirmDetails;
      }
      else {
        this.model.ProfessionalMemberships.OTHER.CurrentStep = ProfessionalBodyMembershipStep.NotStarted;
      }
    }


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
            `api/ValidateApplicationNumberPhone/${PhoneNumber!.trim().replace("+44", "0")}/${ApplicationNumber.toUpperCase()}`
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
    await firstValueFrom(this.httpClient.post(`api/SyncPayment`, Sanitizer.sanitize(this.model)));
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

  async syncApplicationStage(): Promise<void> {
    await firstValueFrom(
      this.httpClient.post(`api/SyncApplicationStage`, Sanitizer.sanitize(this.model))
    );
  }

  async syncDeclaration(): Promise<void> {
    await firstValueFrom(
      this.httpClient.post(`api/SyncDeclaration`, Sanitizer.sanitize(this.model))
    );
  }

  async syncCompetency(): Promise<void> {
    await firstValueFrom(
      this.httpClient.post(`api/SyncCompetency`, Sanitizer.sanitize(this.model))
    );
  }

  async syncPersonalDetails(): Promise<void> {
    await firstValueFrom(
      this.httpClient.post(`api/SyncPersonalDetails`, Sanitizer.sanitize(this.model))
    );
  }

  async syncBuildingInspectorClass(): Promise<void> {
    await firstValueFrom(
      this.httpClient.post(`api/SyncBuildingInspectorClass`, Sanitizer.sanitize(this.model))
    );
  }

  async syncProfessionalBodyMemberships(): Promise<void> {
    await firstValueFrom(
      this.httpClient.post(`api/SyncProfessionalBodyMemberships`, Sanitizer.sanitize(this.model))
    );
  }

  async syncEmploymentDetails(): Promise<void> {
    await firstValueFrom(
      this.httpClient.post(`api/SyncEmploymentDetails`, Sanitizer.sanitize(this.model))
    );
  }

  async syncFullApplication(): Promise<void> {
    await firstValueFrom(
      this.httpClient.post(`api/SyncFullApplication`, Sanitizer.sanitize(this.model))
    );
  }

  async CheckDuplicateBuildingProfessionalApplication(): Promise<boolean> {
    return await lastValueFrom(
      this.httpClient.post<boolean>(`api/CheckDuplicateBuildingProfessionalApplication`, Sanitizer.sanitize(this.model))
    );
  }

}
