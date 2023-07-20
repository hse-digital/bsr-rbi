import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { TitleService } from 'src/app/services/title.service';
import {
  ActivatedRoute,
  ActivatedRouteSnapshot,
  Router,
} from '@angular/router';
import { GovukErrorSummaryComponent } from 'hse-angular';
import {
  ApplicationService,
  BuildingProfessionalModel,
  ApplicationStatus, /* PaymentStatus */
  IComponentModel,
  ComponentCompletionState,
} from 'src/app/services/application.service';
import { NavigationService } from 'src/app/services/navigation.service';
import { PageComponent } from 'src/app/helpers/page.component';
//import { PersonalDetailsPlaceholderComponent } from '../1-personal-details/personal-details-placeholder/personal-details-placeholder.component';
//import { BuildingInspectorClassPlaceholderComponent } from '../2-building-inspector-class/building-inspector-class-placeholder/building-inspector-class-placeholder.component';
//import { CompetencyPlaceholderComponent } from '../3-competency/competency-placeholder/competency-placeholder.component';
//import { ProfessionalActivityPlaceholderComponent } from '../4-professional-activity/professional-activity-placeholder/professional-activity-placeholder.component';
//import { ApplicationSubmissionPlaceholderComponent } from '../5-application-submission/application-submission-placeholder/application-submission-placeholder.component';
import { environment } from 'src/environments/environment';
//import { PayAndSubmitComponent } from '../5-application-submission/pay-and-submit-application/pay-and-submit.component';
import { ApplicantDateOfBirthComponent } from '../1-personal-details/applicant-date-of-birth/applicant-date-of-birth.component';
import { ApplicantAddressComponent } from '../1-personal-details/applicant-address/applicant-address.component';
import { ApplicantNameComponent } from '../1-personal-details/applicant-name/applicant-name.component';
import { ApplicantEmailComponent } from '../../new-application/applicant-email/applicant-email.component';
import { ApplicantPhoneComponent } from '../../new-application/applicant-phone/applicant-phone.component';
import { FieldValidations } from '../../../helpers/validators/fieldvalidations';
import { ApplicantAlternativePhoneComponent } from '../1-personal-details/applicant-alternative-phone/applicant-alternative-phone.component';
import { ApplicantAlternativeEmailComponent } from '../1-personal-details/applicant-alternative-email/applicant-alternative-email.component';
import { ApplicantNationalInsuranceNumberComponent } from '../1-personal-details/applicant-national-insurance-number/applicant-national-insurance-number.component';
import { ApplicantSummaryComponent } from '../1-personal-details/applicant-summary/applicant-summary.component';
import { ApplicationPersonalDetailsModule } from '../1-personal-details/application.personal-details.module';
import { BuildingInspectorClassModule } from '../2-building-inspector-class/application.building-inspector-class.module';
import { CompetencyModule } from '../3-competency/application.competency.module';
import { BuildingInspectorClassSelectionComponent } from '../2-building-inspector-class/class-selection/building-inspector-class-selection.component';
import { BuildingInspectorSummaryComponent } from '../2-building-inspector-class/building-inspector-summary/building-inspector-summary.component';
import { BuildingInspectorCountryComponent } from '../2-building-inspector-class/country/building-inspector-country.component';
import { CompetencyIndependentStatusComponent } from '../3-competency/independent-competency-status/independent-competency-status.component';
import { CompetencyCertificateCodeComponent } from '../3-competency/certificate-code/competency-certificate-code.component';
import { CompetencyAssessmentOrganisationComponent } from '../3-competency/assesesment-organisation/competency-assesesment-organisation.component';
import { CompetencyAssessmentDateComponent } from '../3-competency/assesesment-date/competency-assesesment-date.component';
import { CompetencySummaryComponent } from '../3-competency/competency-summary/competency-summary.component';
import { ProfessionalBodyMembershipsComponent } from '../4-professional-activity/professional-body-memberships/professional-body-memberships.component';
import { ProfessionalActivityEmploymentTypeComponent } from '../4-professional-activity/employment-type/professional-activity-employment-type.component';
import { ProfessionalActivityEmploymentDetailsComponent } from '../4-professional-activity/employment-details/professional-activity-employment-details.component';
import { ProfessionalActivitySummaryComponent } from '../4-professional-activity/professional-activity-summary/professional-activity-summary.component';
import { ApplicationSummaryComponent } from '../5-application-submission/application-summary/application-summary.component';
import { PaymentConfirmationComponent } from '../5-application-submission/payment/payment-confirmation/payment-confirmation.component';

import { PersonalDetailRoutes, PersonalDetailRouter } from '../1-personal-details/PersonalDetailRoutes';

import { PaymentDeclarationComponent } from '../5-application-submission/payment/payment-declaration/payment-declaration.component';


interface ITaskListParent {
  prompt: string;
  number: string;
  relativeRoute: string;
  children: ITaskListChild[];
}
interface ITaskListChild {
  prompt: string;
  relativeRoute: string;
  getStatus(model: BuildingProfessionalModel): TaskStatus;
}

export enum TaskStatus {
  NotStarted = 0,
  Complete = 1,
  InProgress = 2,
  CannotStart = 3,
  None = 4,
}

@Component({
  templateUrl: './task-list.component.html',
})

export class ApplicationTaskListComponent extends PageComponent<BuildingProfessionalModel> {

  public PersonalDetailRouter: PersonalDetailRouter;
  PersonalDetailRoutes = PersonalDetailRoutes;

  valueNotStarted(): TaskStatus { return TaskStatus.NotStarted; }
  valueComplete(): TaskStatus { return TaskStatus.Complete; }
  valueInProgress(): TaskStatus { return TaskStatus.InProgress; }
  valueCannotStart(): TaskStatus { return TaskStatus.CannotStart; }
  
  DerivedIsComplete(value: boolean): void {
    throw new Error('Method not implemented.');
  }

  static route: string = '';
  static title: string =
    'Register as a building inspector - Register a high-rise building - GOV.UK';
  paymentStatus: any;
  paymentEnum: any;
  ApplicationStatus = ApplicationStatus;
  completedSections: number = 0;
  checkingStatus = true;
  QueryApplicationId!: String;
  ModelApplicationId!: String;
  production: boolean = environment.production;

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService,
    private personalDetailsRouter: PersonalDetailRouter,
    personalDetailRouter: PersonalDetailRouter
  ) {
    super(activatedRoute);
    this.updateOnSave = false;
    this.activatedRoute.params.subscribe(params => {
      this.QueryApplicationId = params['id']
    })
    this.ModelApplicationId = applicationService.model.id!;
    this.PersonalDetailRouter = personalDetailRouter;
  }

  override onInit(applicationService: ApplicationService): void {
    this.model = applicationService.model;
  }

  getModelStatus(model?: IComponentModel): TaskStatus {
    if (!(model?.CompletionState)) {
      return TaskStatus.NotStarted;
    } else if (model?.CompletionState == ComponentCompletionState.Complete) {
      return TaskStatus.Complete;
    } else if (model?.CompletionState == ComponentCompletionState.InProgress) {
      return TaskStatus.InProgress;
    } else if (model?.CompletionState == ComponentCompletionState.NotStarted) {
      return TaskStatus.NotStarted;
    } else {
      return TaskStatus.None;
    }
  };

  taskItems: ITaskListParent[] = [{
    prompt: "Personal details", number: "1.", relativeRoute: ApplicationPersonalDetailsModule.baseRoute, children: [{
      prompt: "Name", relativeRoute: PersonalDetailRoutes.NAME, getStatus: (aModel: BuildingProfessionalModel): TaskStatus => TaskStatus.Complete
    },
    {
      prompt: "Email address", relativeRoute: '', getStatus: (aModel: BuildingProfessionalModel): TaskStatus => TaskStatus.Complete
    },
    {
      prompt: "Telephone number", relativeRoute: '', getStatus: (aModel: BuildingProfessionalModel): TaskStatus => TaskStatus.Complete
    },
    {
      prompt: "Date of birth", relativeRoute: PersonalDetailRoutes.DATE_OF_BIRTH, getStatus: (aModel: BuildingProfessionalModel): TaskStatus => this.getModelStatus(aModel.PersonalDetails?.ApplicantDateOfBirth)
    },
    {
      prompt: "Home address", relativeRoute: PersonalDetailRoutes.ADDRESS, getStatus: (aModel: BuildingProfessionalModel): TaskStatus => this.getModelStatus(aModel.PersonalDetails?.ApplicantAddress)
    },
    {
      prompt: "Alternative email address", relativeRoute: PersonalDetailRoutes.ALT_EMAIL, getStatus: (aModel: BuildingProfessionalModel): TaskStatus => this.getModelStatus(aModel.PersonalDetails?.ApplicantAlternativeEmail)
    },
    {
      prompt: "Telephone number", relativeRoute: PersonalDetailRoutes.ALT_PHONE, getStatus: (aModel: BuildingProfessionalModel): TaskStatus => this.getModelStatus(aModel.PersonalDetails?.ApplicantAlternativePhone)
    },
    {
      prompt: "National Insurance number", relativeRoute: PersonalDetailRoutes.NATIONAL_INS_NUMBER, getStatus: (aModel: BuildingProfessionalModel): TaskStatus => this.getModelStatus(aModel.PersonalDetails?.ApplicantNationalInsuranceNumber)
    },
    {
      prompt: "Summary", relativeRoute: PersonalDetailRoutes.SUMMARY, getStatus: (aModel: BuildingProfessionalModel): TaskStatus => TaskStatus.None
    },
    ]
  },
  {
    prompt: "Building inspector class", number: "2.", relativeRoute: BuildingInspectorClassModule.baseRoute, children: [
      {
        prompt: "Class selection", relativeRoute: BuildingInspectorClassSelectionComponent.route, getStatus: (aModel: BuildingProfessionalModel): TaskStatus => this.getModelStatus(aModel.InspectorClass?.ClassType)
      },
      {
        prompt: "Class details", relativeRoute: BuildingInspectorClassSelectionComponent.route, getStatus: (aModel: BuildingProfessionalModel): TaskStatus => TaskStatus.CannotStart
      },
      {
        prompt: "Country", relativeRoute: BuildingInspectorCountryComponent.route, getStatus: (aModel: BuildingProfessionalModel): TaskStatus => TaskStatus.CannotStart
      },
      {
        prompt: "Summary", relativeRoute: BuildingInspectorSummaryComponent.route, getStatus: (aModel: BuildingProfessionalModel): TaskStatus => TaskStatus.None
      }]
  },
  {
    prompt: "Competency", number: "3.", relativeRoute: CompetencyModule.baseRoute, children: [
      {
        prompt: "Independent assessment", relativeRoute: CompetencyIndependentStatusComponent.route, getStatus: (aModel: BuildingProfessionalModel): TaskStatus => TaskStatus.CannotStart
      },
      {
        prompt: "Certificate code", relativeRoute: CompetencyCertificateCodeComponent.route, getStatus: (): TaskStatus => TaskStatus.CannotStart
      },
      {
        prompt: "Assessment organisation", relativeRoute: CompetencyAssessmentOrganisationComponent.route, getStatus: (): TaskStatus => TaskStatus.CannotStart
      },
      {
        prompt: "Date of assessment", relativeRoute: CompetencyAssessmentDateComponent.route, getStatus: (): TaskStatus => TaskStatus.CannotStart
      },
      {
        prompt: "Summary", relativeRoute: CompetencySummaryComponent.route, getStatus: (): TaskStatus => TaskStatus.None
      },
    ]
  },
  {
    prompt: "Professional Activity", number: "4.", relativeRoute: CompetencyModule.baseRoute, children: [
      {
        prompt: "Professional body membership", relativeRoute: ProfessionalBodyMembershipsComponent.route, getStatus: (aModel: BuildingProfessionalModel): TaskStatus => TaskStatus.CannotStart
      },
      {
        prompt: "Employment type", relativeRoute: ProfessionalActivityEmploymentTypeComponent.route, getStatus: (aModel: BuildingProfessionalModel): TaskStatus => TaskStatus.CannotStart
      },
      {
        prompt: "Employment details", relativeRoute: ProfessionalActivityEmploymentDetailsComponent.route, getStatus: (aModel: BuildingProfessionalModel): TaskStatus => TaskStatus.CannotStart
      },
      {
        prompt: "Summary", relativeRoute: ProfessionalActivitySummaryComponent.route, getStatus: (aModel: BuildingProfessionalModel): TaskStatus => TaskStatus.None
      },
    ]
  },
  {
    prompt: "Application summary", number: "5.", relativeRoute: CompetencyModule.baseRoute, children: [
      {
        prompt: "Application summary", relativeRoute: ApplicationSummaryComponent.route, getStatus: (aModel: BuildingProfessionalModel): TaskStatus => TaskStatus.None
      },
      {
        prompt: "Pay your fee and submit your application", relativeRoute: PaymentConfirmationComponent.route, getStatus: (aModel: BuildingProfessionalModel): TaskStatus => TaskStatus.CannotStart
      },
    ]
  }
  ];

  navigateTo(parent: ITaskListParent, child: ITaskListChild): void {
    if (child.getStatus(this.model!) !== TaskStatus.CannotStart && child.relativeRoute != '')
    {
      this.navigationService.navigateRelative(`${this.ModelApplicationId}/${parent.relativeRoute}/${child.relativeRoute}`, this.activatedRoute);
    }
  }

  override onSave(applicationService: ApplicationService): Promise<void> {
    throw new Error('Method not implemented.');
  }
  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {

    return true;
  }


  override isValid(): boolean {
    throw new Error('Method not implemented.');
  }
  override navigateNext(): Promise<boolean> {
    throw new Error('Method not implemented.');
  }



  // paymentEnum = PaymentStatus;
  // paymentStatus?: PaymentStatus;

  containsFlag(flag: ApplicationStatus) {
    return (this.model!.ApplicationStatus & flag) == flag;
  }

  navigateToSummary() : Promise<boolean> {
    return this.PersonalDetailRouter.navigateTo(this.model!, PersonalDetailRoutes.SUMMARY);
  }

  navigateToApplicantName() {
    throw new Error('Method not implemented.');
  }

//  navigateToPersonalDetailsAddress() {
//    return this.navigationService.navigateRelative(`${this.ModelApplicationId}/personal-details/${ApplicantAddressComponent.route}`, this.activatedRoute);
//  }

//  navigateToPersonalDetailsDateOfBirth() {
//    return this.navigationService.navigateRelative(`${this.ModelApplicationId}/personal-details/${ApplicantDateOfBirthComponent.route}`, this.activatedRoute);
//  }

//  navigateToPersonalDetails() {
//    return this.navigationService.navigateRelative(`${this.ModelApplicationId}/personal-details/${PersonalDetailsPlaceholderComponent.route}`, this.activatedRoute);
//  }
//  navigateToBuildingInspectorClass() {
//    return this.navigationService.navigateRelative(`${this.ModelApplicationId}/building-inspector-class/${BuildingInspectorClassPlaceholderComponent.route}`, this.activatedRoute);
//  }
//  navigateToCompetency() {
//    return this.navigationService.navigateRelative(`${this.ModelApplicationId}/competency/${CompetencyPlaceholderComponent.route}`, this.activatedRoute);
//  }
//  navigateToProfessionalActivity() {
//    return this.navigationService.navigateRelative(`${this.ModelApplicationId}/professional-activity/${ProfessionalActivityPlaceholderComponent.route}`, this.activatedRoute);
//  }
//  navigateToApplicationOverview() {
//    return this.navigationService.navigateRelative(`${this.ModelApplicationId}/application-submission/${ApplicationSubmissionPlaceholderComponent.route}`, this.activatedRoute);
//  }
//  navigateToPayAndSubmit() {
//    return this.navigationService.navigateRelative(`${this.ModelApplicationId}/application-submission/${PayAndSubmitComponent.route}`, this.activatedRoute);
//  }
//  navigateToPayment() {
//    throw new Error('Method not implemented.');
//  }
}
