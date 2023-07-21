import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { LocalStorage } from 'src/app/helpers/local-storage';
import { AddressModel } from './address.service';

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
    await firstValueFrom(
      this.httpClient.post('api/ValidateOTPToken', {
        OTPToken: OTPToken,
        Data: Data,
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
    PhoneNumber: string,
    OTPToken: string
  ): Promise<void> {
    let application: BuildingProfessionalModel = await firstValueFrom(
      this.httpClient.get<BuildingProfessionalModel>(
        `api/GetApplication/${ApplicationNumber}/${PhoneNumber}/${OTPToken}`
      )
    );
    this.model = application;
    this.model.ReturningApplication = true;
    this.updateLocalStorage();
  }

  async validateReturningApplicationDetails(
    EmailAddress: string,
    ApplicationNumber: string
  ): Promise<{ isValidEmail: boolean; isValidApplicationNumber: boolean }> {
    return await firstValueFrom(
      this.httpClient.get<{
        isValidEmail: boolean;
        isValidApplicationNumber: boolean;
      }>(
        `api/ValidateApplicationNumber/${EmailAddress.toLowerCase()}/${ApplicationNumber}`
      )
    );
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
}

export enum ComponentCompletionState {
  NotStarted = 0,
  InProgress = 1,
  Complete = 2,
}

export enum StageCompletionState {
  Incomplete = 0,
  Complete = 1,
}

export interface IComponentModel {
  CompletionState?: ComponentCompletionState;
}

export class StringModel implements IComponentModel {
  stringValue?: string;
  CompletionState?: ComponentCompletionState;
}

export class NumberModel implements IComponentModel {
  numberValue?: number;
  CompletionState?: ComponentCompletionState;
}

export class BuildingProfessionalModel implements IComponentModel {
  id?: string;
  PersonalDetails?: PersonalDetails = {};
  InspectorClass?: BuildingInspectorClass = new BuildingInspectorClass();
  ApplicationStatus: ApplicationStatus = ApplicationStatus.None;

  //TODO test StageStatus and replace ApplicationStatus
  StageStatus: Record<string, StageCompletionState> = {
    EmailVerification: StageCompletionState.Incomplete,
    PhoneVerification: StageCompletionState.Incomplete,
    PersonalDetails: StageCompletionState.Incomplete,
    BuildingInspectorClass: StageCompletionState.Incomplete,
    Competency: StageCompletionState.Incomplete,
    ProfessionalActivity: StageCompletionState.Incomplete,
    Declaration: StageCompletionState.Incomplete,
    Payment: StageCompletionState.Incomplete,
  };

  ReturningApplication: boolean = false;
  get CompletionState(): ComponentCompletionState {
    return this!.ApplicationStatus! ==
      ApplicationStatus.ApplicationSubmissionComplete
      ? ComponentCompletionState.Complete
      : ComponentCompletionState.InProgress;
  }
}

export class PersonalDetails {
  ApplicantName?: ApplicantName = {};
  ApplicantDateOfBirth?: ApplicantDateOfBirth = {};
  ApplicantAddress?: AddressModel;
  ApplicantPhone?: ApplicantPhone;
  ApplicantAlternativePhone?: ApplicantPhone;
  ApplicantEmail?: ApplicantEmail;
  ApplicantAlternativeEmail?: ApplicantEmail;
  ApplicantNationalInsuranceNumber?: ApplicantNationalInsuranceNumber;
}

export class ApplicantPhone implements IComponentModel {
  PhoneNumber?: string;
  CompletionState?: ComponentCompletionState;
}

export class ApplicantEmail implements IComponentModel {
  Email?: string;
  CompletionState?: ComponentCompletionState;
}

export class ApplicantNationalInsuranceNumber implements IComponentModel {
  NationalInsuranceNumber?: string;
  CompletionState?: ComponentCompletionState;
}

export class ApplicantName implements IComponentModel {
  FirstName?: string;
  LastName?: string;
  CompletionState?: ComponentCompletionState;
}

export class ApplicantDateOfBirth implements IComponentModel {
  Day?: string;
  Month?: string;
  Year?: string;
  CompletionState?: ComponentCompletionState;
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
  PaymentInProgress = 128,
  PaymentComplete = 256,
}

export class PaymentModel {
  CreatedDate?: string;
  Status?: string;
  Finished?: boolean;
  PaymentLink!: string;
  Amount?: number;
  Email?: string;
  Reference?: string;
  Description?: string;
  ReturnURL?: string;
  PaymentId?: string;
  PaymentProvider?: string;
  ProviderId?: string;
  ReconciliationStatus?: number;
}

export enum PaymentStatus {
  Started,
  Pending,
  Success,
  Failed,
}

export class BuildingInspectorClass {
  ClassType: ClassSelection = {
    Class: BuildingInspectorClassType.ClassNone,
    CompletionState: ComponentCompletionState.NotStarted,
  };
  Activities: BuildingInspectorRegulatedActivies = {
    AssessingPlans: false,
    Inspection: false,
    CompletionState: ComponentCompletionState.NotStarted,
  };
  BuildingAssessingPlansCategoriesClass2: BuildingAssessingPlansCategoriesClass2 = new BuildingAssessingPlansCategoriesClass2();
  BuildingAssessingPlansCategoriesClass3: BuildingAssessingPlansCategoriesClass3 = new BuildingAssessingPlansCategoriesClass3();
  ClassTechnicalManager?: string;
  InspectorCountryOfWork?: BuildingInspectorCountryOfWork = { England: false, Wales: false };
  Class2InspectBuildingCategories: Class2InspectBuildingCategories = new Class2InspectBuildingCategories();
  Class3InspectBuildingCategories: Class3InspectBuildingCategories = new Class3InspectBuildingCategories();
}

export class ClassSelection implements IComponentModel {
  Class?: BuildingInspectorClassType;
  CompletionState?: ComponentCompletionState;
}

export enum BuildingInspectorClassType {
  ClassNone = 0,
  Class1 = 1,
  Class2 = 2,
  Class3 = 3,
}

export class BuildingInspectorRegulatedActivies {
  [key: string]: any;
  AssessingPlans: boolean = false;
  Inspection: boolean = false;
  CompletionState: ComponentCompletionState = ComponentCompletionState.NotStarted;
}
export class BuildingAssessingPlansCategoriesClass2 {
  [key: string]: any;
  CategoryA?: boolean = false;
  CategoryB?: boolean = false;
  CategoryC?: boolean = false;
  CategoryD?: boolean = false;
  CategoryE?: boolean = false;
  CategoryF?: boolean = false;
  CompletionState?: ComponentCompletionState = ComponentCompletionState.NotStarted;
}

export class BuildingAssessingPlansCategoriesClass3 {
  [key: string]: any;
  CategoryA?: boolean = false;
  CategoryB?: boolean = false;
  CategoryC?: boolean = false;
  CategoryD?: boolean = false;
  CategoryE?: boolean = false;
  CategoryF?: boolean = false;
  CategoryG?: boolean = false;
  CategoryH?: boolean = false;
  CompletionState?: ComponentCompletionState = ComponentCompletionState.NotStarted;
}

export class Class2InspectBuildingCategories {
  [key: string]: any;
  CategoryA: boolean = false;
  CategoryB: boolean = false;
  CategoryC: boolean = false;
  CategoryD: boolean = false;
  CategoryE: boolean = false;
  CategoryF: boolean = false;
  CompletionState?: ComponentCompletionState = ComponentCompletionState.NotStarted;
}


export class Class3InspectBuildingCategories {
  [key: string]: any;
  CategoryA: boolean = false;
  CategoryB: boolean = false;
  CategoryC: boolean = false;
  CategoryD: boolean = false;
  CategoryE: boolean = false;
  CategoryF: boolean = false;
  CategoryG: boolean = false;
  CategoryH: boolean = false;
  CompletionState?: ComponentCompletionState = ComponentCompletionState.NotStarted;
}




export class BuildingInspectorCountryOfWork {
  [key: string]: any;
  England?: boolean;
  Wales?: boolean;
  CompletionState?: ComponentCompletionState;
}
