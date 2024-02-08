import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { TitleService } from 'src/app/services/title.service';
import {
  ActivatedRoute,
  ActivatedRouteSnapshot,
  Params,
  Router,
} from '@angular/router';
import { GovukErrorSummaryComponent } from 'hse-angular';
import { ApplicationService } from 'src/app/services/application.service';
import { NavigationService } from 'src/app/services/navigation.service';
import { PageComponent } from 'src/app/helpers/page.component';
import { environment } from 'src/environments/environment';
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
import { CompetencyAssessmentOrganisationComponent } from '../3-competency/assesesment-organisation/competency-assesesment-organisation.component';
import { CompetencyAssessmentDateComponent } from '../3-competency/assesesment-date/competency-assesesment-date.component';
import { CompetencySummaryComponent } from '../3-competency/competency-summary/competency-summary.component';
import { ProfessionalBodyMembershipsComponent } from '../4-professional-activity/professional-body-memberships/professional-body-memberships.component';
import { ProfessionalActivityEmploymentTypeComponent } from '../4-professional-activity/employment-type/professional-activity-employment-type.component';
import { ProfessionalBodyMembershipSummaryComponent } from '../4-professional-activity/professional-body-membership-summary/professional-body-membership-summary.component';
import { ApplicationSummaryComponent } from '../5-application-submission/application-summary/application-summary.component';
import { PaymentConfirmationComponent } from '../5-application-submission/payment/payment-confirmation/payment-confirmation.component';

import {
  PersonalDetailRoutes,
  PersonalDetailRouter,
} from '../1-personal-details/PersonalDetailRoutes';

import { PaymentDeclarationComponent } from '../5-application-submission/payment/payment-declaration/payment-declaration.component';
import { Validators } from '@angular/forms';
import { PaymentReconciliationStatus } from 'src/app/services/payment.service';
import { BuildingProfessionalModel } from 'src/app/models/building-professional.model';
import { IComponentModel } from 'src/app/models/component. interface';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { StageCompletionState } from 'src/app/models/stage-completion-state.enum';
import { PaymentStatus } from 'src/app/models/payment-status.enum';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { CompetencyAssessmentCertificateNumberComponent } from '../3-competency/assessment-certificate-number/competency-assessment-certificate-number.component';
import { ProfessionalActivityModule } from '../4-professional-activity/application.professional-activity.module';
import { BuildingInspectorClassType } from 'src/app/models/building-inspector-classtype.enum';
import { PersonalDetails } from 'src/app/models/personal-details.model';
import { BuildingInspectorClass } from 'src/app/models/building-inspector-class.model';
import { Competency } from 'src/app/models/competency.model';
import { ApplicantProfessionBodyMemberships } from 'src/app/models/applicant-professional-body-membership';
import { ApplicantEmploymentDetails } from 'src/app/models/applicant-employment-details';
import { ProfessionalMembershipAndEmploymentSummaryComponent } from '../4-professional-activity/professional-membership-and-employment-summary/professional-membership-and-employment-summary.component';
import { ClassSelection } from 'src/app/models/class-selection.model';



interface ITaskListParent {
  prompt: string;
  show: boolean;
  relativeRoute: string;
  id: string;
  children: ITaskListChild[];
}
interface ITaskListChild {
  show?: boolean;
  prompt: string;
  relativeRoute(): TaskListRoute;
  getStatus(model: BuildingProfessionalModel): TaskStatus;
}

type TaskListRoute = {
  route: string;
  queryParams?: Params;
};

export enum TaskStatus {
  NotStarted = 0,
  Complete = 1,
  InProgress = 2,
  CannotStart = 3,
  None = 4,
  Immutable = 5,
  SummaryCanStart = 6,
  SummaryCannotStart = 7
}

@Component({
  templateUrl: './task-list.component.html',
})
export class ApplicationTaskListComponent extends PageComponent<BuildingProfessionalModel> {
  public PersonalDetailRouter: PersonalDetailRouter;
  PersonalDetailRoutes = PersonalDetailRoutes;

  valueNotStarted(): TaskStatus {
    return TaskStatus.NotStarted;
  }
  valueImmutable(): TaskStatus {
    return TaskStatus.Immutable;
  }
  valueComplete(): TaskStatus {
    return TaskStatus.Complete;
  }
  valueInProgress(): TaskStatus {
    return TaskStatus.InProgress;
  }
  valueCannotStart(): TaskStatus {
    return TaskStatus.CannotStart;
  }
  valueSummaryCannotStartTask(): TaskStatus {
    return TaskStatus.SummaryCannotStart;
  }
  valueSummaryCanStartTask(): TaskStatus {
    return TaskStatus.SummaryCanStart;
  }



  DerivedIsComplete(value: boolean): void {
    throw new Error('Method not implemented.');
  }

  static route: string = '';
  static title: string =
    'Register as a building inspector - Application task list - Register a high-rise building - GOV.UK';
  ApplicationStatus = ApplicationStatus;
  completedSections: number = 0;
  checkingStatus = true;
  QueryApplicationId!: String;
  ModelApplicationId!: String;
  production: boolean = environment.production;
  paymentEnum = PaymentStatus;
  paymentStatus?: PaymentStatus;
  paymentRoute?: TaskListRoute;

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService,
    private personalDetailsRouter: PersonalDetailRouter,
    personalDetailRouter: PersonalDetailRouter,
  ) {
    super(activatedRoute);
    this.updateOnSave = false;
    this.activatedRoute.params.subscribe((params) => {
      this.QueryApplicationId = params['id'];
    });


    this.getPaymentStatus();
    this.ModelApplicationId = applicationService.model.id!;
    this.PersonalDetailRouter = personalDetailRouter;

  }



  override async onInit(applicationService: ApplicationService): Promise<void> {

    this.model = applicationService.model;
    this.checkingStatus = true;
    if (this.isInspectorClassOne()) this.hideCompetencySection();

    this.showCompetencyAssement();

  }



  ngAfterViewInit() {
    this.activatedRoute.fragment.subscribe((fragment) => {
      if (fragment) {

          const element = document.querySelector('#' + fragment);
          if (element) {
            setTimeout(() => {
              element.scrollIntoView({ behavior: 'smooth', block: 'start', inline: 'nearest' }); 
            }, 100);
          }
      }
    });
  }
  



  private isInspectorClassOne() {
    return (
      this.model?.InspectorClass?.ClassType.Class == BuildingInspectorClassType.Class1 && this.model?.InspectorClass?.ClassType.CompletionState==ComponentCompletionState.Complete
    );
  }

  private showCompetencyAssement(): void {
    if (
      (this.applicationService.model.Competency?.CompetencyIndependentAssessmentStatus?.IAStatus === 'no')
    ) {
      this.taskItems[2].children
        .filter(
          (x) => x.prompt !== 'Independent assessment' && x.prompt !== 'Summary'
        )
        .map((x) => (x.show = false));
    }
  }

  getModelStatus(model?: IComponentModel): TaskStatus {
    if (!model?.CompletionState) {
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
  }

  getClassSelectionModelStatus(
    model?: IComponentModel,
    buildingModel?: BuildingProfessionalModel
  ): TaskStatus {
    const classSelection = buildingModel?.InspectorClass?.ClassType.Class;
    const competencyStatus =
      buildingModel?.Competency?.CompetencyIndependentAssessmentStatus
        ?.IAStatus;
    const competencyCompletion =
      buildingModel?.Competency?.CompetencyIndependentAssessmentStatus
        ?.CompletionState;

    if (
      classSelection !== 1 &&
      competencyStatus === 'no' &&
      competencyCompletion === ComponentCompletionState.Complete
    ) {
      return TaskStatus.Immutable;
    } else if (!model?.CompletionState) {
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
  }

  determineTaskStatus(
    model?: IComponentModel,
    countryModel?: IComponentModel
  ): TaskStatus {

    if(this.applicationService.model.InspectorClass?.ClassType.CompletionState!==ComponentCompletionState.Complete){
      return TaskStatus.CannotStart;
    }

    if (!model?.CompletionState) {
      return TaskStatus.CannotStart;
    } else if (
      model?.CompletionState == ComponentCompletionState.Complete &&
      countryModel?.CompletionState == ComponentCompletionState.NotStarted
    ) {
      return TaskStatus.NotStarted;
    } else if (
      model?.CompletionState == ComponentCompletionState.InProgress &&
      countryModel?.CompletionState == ComponentCompletionState.NotStarted
    ) {
      return TaskStatus.CannotStart;
    } else if (
      model?.CompletionState == ComponentCompletionState.Complete &&
      countryModel?.CompletionState == ComponentCompletionState.NotStarted
    ) {
      return TaskStatus.NotStarted;
    } else if (
      model?.CompletionState == ComponentCompletionState.InProgress &&
      countryModel?.CompletionState == ComponentCompletionState.InProgress
    ) {
      return TaskStatus.CannotStart;
    } else if (
      model?.CompletionState == ComponentCompletionState.InProgress &&
      countryModel?.CompletionState == ComponentCompletionState.Complete
    ) {
      return TaskStatus.CannotStart;
    } else if (
      model?.CompletionState == ComponentCompletionState.Complete &&
      countryModel?.CompletionState == ComponentCompletionState.InProgress
    ) {
      return TaskStatus.InProgress;
    } else if (
      model?.CompletionState == ComponentCompletionState.Complete &&
      countryModel?.CompletionState == ComponentCompletionState.Complete
    ) {
      return TaskStatus.Complete;
    } else {
      return TaskStatus.None;
    }
  }

  determineCompetencyTaskStatus(
    model?: IComponentModel,
    countryModel?: IComponentModel
  ): TaskStatus {

    if(this.applicationService.model.InspectorClass?.ClassType.CompletionState!==ComponentCompletionState.Complete || this.applicationService.model.InspectorClass?.InspectorCountryOfWork?.CompletionState!==ComponentCompletionState.Complete){
      return TaskStatus.CannotStart;
    }

    if (!model?.CompletionState) {
      return TaskStatus.CannotStart;
    } else if (
      model?.CompletionState == ComponentCompletionState.Complete &&
      countryModel?.CompletionState == ComponentCompletionState.NotStarted
    ) {
      return TaskStatus.NotStarted;
    } else if (
      model?.CompletionState == ComponentCompletionState.InProgress &&
      countryModel?.CompletionState == ComponentCompletionState.NotStarted
    ) {
      return TaskStatus.CannotStart;
    } else if (
      model?.CompletionState == ComponentCompletionState.Complete &&
      countryModel?.CompletionState == ComponentCompletionState.NotStarted
    ) {
      return TaskStatus.NotStarted;
    } else if (
      model?.CompletionState == ComponentCompletionState.InProgress &&
      countryModel?.CompletionState == ComponentCompletionState.InProgress
    ) {
      return TaskStatus.CannotStart;
    } else if (
      model?.CompletionState == ComponentCompletionState.InProgress &&
      countryModel?.CompletionState == ComponentCompletionState.Complete
    ) {
      return TaskStatus.CannotStart;
    } else if (
      model?.CompletionState == ComponentCompletionState.Complete &&
      countryModel?.CompletionState == ComponentCompletionState.InProgress
    ) {
      return TaskStatus.InProgress;
    } else if (
      model?.CompletionState == ComponentCompletionState.Complete &&
      countryModel?.CompletionState == ComponentCompletionState.Complete
    ) {
      return TaskStatus.Complete;
    } else {
      return TaskStatus.None;
    }
  }

  determinePersonalSummaryTask(model?: PersonalDetails): TaskStatus {
    if (
      model?.ApplicantName!.CompletionState! ===
        ComponentCompletionState.Complete &&
      model?.ApplicantPhone?.CompletionState ===
        ComponentCompletionState.Complete &&
      model?.ApplicantAlternativePhone?.CompletionState ===
        ComponentCompletionState.Complete &&
      model?.ApplicantEmail?.CompletionState ===
        ComponentCompletionState.Complete &&
      model?.ApplicantAlternativeEmail?.CompletionState ===
        ComponentCompletionState.Complete &&
      model?.ApplicantDateOfBirth?.CompletionState ===
        ComponentCompletionState.Complete &&
      model?.ApplicantNationalInsuranceNumber?.CompletionState ===
        ComponentCompletionState.Complete
    ) {
      return TaskStatus.SummaryCanStart;
    } else return TaskStatus.SummaryCannotStart;
  }

  determineCompetencySummaryTask(model?: Competency): TaskStatus {
    if (
      this.applicationService.model.Competency?.CompetencyIndependentAssessmentStatus?.IAStatus === 'no' &&
      model?.CompetencyIndependentAssessmentStatus!.CompletionState! === ComponentCompletionState.Complete
      && this.determineClassSummaryTask(this.applicationService.model.InspectorClass) === TaskStatus.SummaryCanStart
    ) {
      return TaskStatus.SummaryCanStart;
    } else if (
      model?.CompetencyIndependentAssessmentStatus!.CompletionState! ===
        ComponentCompletionState.Complete &&
      model?.CompetencyAssessmentOrganisation!.CompletionState! ===
        ComponentCompletionState.Complete &&
      model?.CompetencyDateOfAssessment!.CompletionState! ===
        ComponentCompletionState.Complete &&
      model?.CompetencyAssessmentCertificateNumber!.CompletionState! ===
        ComponentCompletionState.Complete
        &&
        this.determineClassSummaryTask(this.applicationService.model.InspectorClass) === TaskStatus.SummaryCanStart
    ) {
      return TaskStatus.SummaryCanStart;
    } else return TaskStatus.SummaryCannotStart;
  }

  determineClassSummaryTask(model?: BuildingInspectorClass): TaskStatus {
    if (
      model?.ClassType!.CompletionState! ===
        ComponentCompletionState.Complete &&
      model?.InspectorCountryOfWork!.CompletionState! ===
        ComponentCompletionState.Complete
    ) {
      return TaskStatus.SummaryCanStart;
    } else return TaskStatus.SummaryCannotStart;
  }

  determineProfessionalMembershipSummaryTask(
    model?: ApplicantProfessionBodyMemberships,
    employeeModel?: ApplicantEmploymentDetails
  ): TaskStatus {
    if (
      model?.CompletionState === ComponentCompletionState.Complete &&
      employeeModel?.CompletionState === ComponentCompletionState.Complete
    ) {
      return TaskStatus.SummaryCanStart;
    } else {
      return TaskStatus.SummaryCannotStart;
    }
  }

  determineApplicationSummaryTask(
    model?: BuildingProfessionalModel
  ): TaskStatus {
    if (
      this.determinePersonalSummaryTask(model!.PersonalDetails) === TaskStatus.SummaryCanStart &&
      this.determineClassSummaryTask(model!.InspectorClass) === TaskStatus.SummaryCanStart &&
      (this.determineCompetencySummaryTask(model!.Competency) === TaskStatus.SummaryCanStart || model!.InspectorClass?.ClassType.Class == BuildingInspectorClassType.Class1) &&
      this.determineProfessionalMembershipSummaryTask(model!.ProfessionalMemberships, model!.ProfessionalActivity.EmploymentDetails) === TaskStatus.SummaryCanStart
    ) {
      return TaskStatus.SummaryCanStart;
    } else return TaskStatus.SummaryCannotStart;
  }

  determineDeclarationPaymentTask(
    model?: BuildingProfessionalModel
  ): TaskStatus {
    if (
      model?.StageStatus!['ApplicationConfirmed'] ===
      StageCompletionState.Complete
    ) {
      return TaskStatus.None;
    } else return TaskStatus.CannotStart;
  }

  hideCompetencySection() {
    this.taskItems[2].show = false;
  }

  get items() {
    return this.taskItems.filter((x) => x.show);
  }

  //#9044 Pattern updated to allow routes to also include query params
  private taskItems: ITaskListParent[] = [
    {
      prompt: 'Personal details',
      relativeRoute: ApplicationPersonalDetailsModule.baseRoute,
      show: true,
      id: 'personal-details',
      children: [
        {
          show: true,
          prompt: 'Name',
          relativeRoute: (): TaskListRoute => {
            return {
              route: PersonalDetailRoutes.NAME,
            };
          },
          getStatus: (aModel: BuildingProfessionalModel): TaskStatus =>
            TaskStatus.Complete,
        },
        {
          show: true,
          prompt: 'Email address',
          relativeRoute: (): TaskListRoute => {
            return { route: '' };
          },
          getStatus: (aModel: BuildingProfessionalModel): TaskStatus =>
            TaskStatus.Immutable,
        },
        {
          show: true,
          prompt: 'Telephone number',
          relativeRoute: (): TaskListRoute => {
            return { route: '' };
          },
          getStatus: (aModel: BuildingProfessionalModel): TaskStatus =>
            TaskStatus.Immutable,
        },
        {
          show: true,
          prompt: 'Date of birth',
          relativeRoute: (): TaskListRoute => {
            return { route: PersonalDetailRoutes.DATE_OF_BIRTH };
          },
          getStatus: (aModel: BuildingProfessionalModel): TaskStatus =>
            this.getModelStatus(aModel.PersonalDetails?.ApplicantDateOfBirth),
        },
        {
          show: true,
          prompt: 'Home address',
          relativeRoute: (): TaskListRoute => {
            return { route: PersonalDetailRoutes.ADDRESS };
          },
          getStatus: (aModel: BuildingProfessionalModel): TaskStatus =>
            this.getModelStatus(aModel.PersonalDetails?.ApplicantAddress),
        },
        {
          show: true,
          prompt: 'Alternative email address',
          relativeRoute: (): TaskListRoute => {
            return { route: PersonalDetailRoutes.ALT_EMAIL };
          },
          getStatus: (aModel: BuildingProfessionalModel): TaskStatus =>
            this.getModelStatus(
              aModel.PersonalDetails?.ApplicantAlternativeEmail
            ),
        },
        {
          show: true,
          prompt: 'Alternative telephone number',
          relativeRoute: (): TaskListRoute => {
            return { route: PersonalDetailRoutes.ALT_PHONE };
          },
          getStatus: (aModel: BuildingProfessionalModel): TaskStatus =>
            this.getModelStatus(
              aModel.PersonalDetails?.ApplicantAlternativePhone
            ),
        },
        {
          show: true,
          prompt: 'National Insurance number',
          relativeRoute: (): TaskListRoute => {
            return { route: PersonalDetailRoutes.NATIONAL_INS_NUMBER };
          },
          getStatus: (aModel: BuildingProfessionalModel): TaskStatus =>
            this.getModelStatus(
              aModel.PersonalDetails?.ApplicantNationalInsuranceNumber
            ),
        },
        {
          show: true,
          prompt: 'Summary',
          relativeRoute: (): TaskListRoute => {
            return { route: PersonalDetailRoutes.SUMMARY };
          },
          getStatus: (aModel: BuildingProfessionalModel): TaskStatus =>
            this.determinePersonalSummaryTask(aModel.PersonalDetails),
        },
      ],
    },
    {
      prompt: 'Building inspector class',
      relativeRoute: BuildingInspectorClassModule.baseRoute,
      show: true,
      id: 'building-inspector-class',
      children: [
        {
          show: true,
          prompt: 'Class selection',
          relativeRoute: (): TaskListRoute => {
            return { route: BuildingInspectorClassSelectionComponent.route };
          },
          getStatus: (aModel: BuildingProfessionalModel): TaskStatus =>
            this.getClassSelectionModelStatus(
              aModel.InspectorClass?.ClassType,
              aModel
            ),
        },
        {
          show: true,
          prompt: 'Country',
          relativeRoute: (): TaskListRoute => {
            return { route: BuildingInspectorCountryComponent.route };
          },
          getStatus: (aModel: BuildingProfessionalModel): TaskStatus =>
            this.determineTaskStatus(
              aModel.InspectorClass?.ClassType,
              aModel.InspectorClass?.InspectorCountryOfWork
            ),
        },
        {
          show: true,
          prompt: 'Summary',
          relativeRoute: (): TaskListRoute => {
            return { route: BuildingInspectorSummaryComponent.route };
          },
          getStatus: (aModel: BuildingProfessionalModel): TaskStatus =>
            this.determineClassSummaryTask(aModel.InspectorClass),
        },
      ],
    },
    {
      prompt: 'Competency',
      relativeRoute: CompetencyModule.baseRoute,
      show: true,
      id: 'competency',
      children: [
        {
          show: true,
          prompt: 'Independent assessment',
          relativeRoute: (): TaskListRoute => {
            return { route: CompetencyIndependentStatusComponent.route };
          },
          getStatus: (aModel: BuildingProfessionalModel): TaskStatus =>
            this.determineCompetencyTaskStatus(
              aModel.InspectorClass?.InspectorCountryOfWork,
              aModel.Competency?.CompetencyIndependentAssessmentStatus
            ),

          // this.getModelStatus(aModel.Competency?.CompetencyIndependentAssessmentStatus),
        },
        {
          show: true,
          prompt: 'Assessment organisation',
          relativeRoute: (): TaskListRoute => {
            return { route: CompetencyAssessmentOrganisationComponent.route };
          },
          getStatus: (aModel: BuildingProfessionalModel): TaskStatus =>
            this.determineCompetencyTaskStatus(
              aModel.Competency?.CompetencyIndependentAssessmentStatus,
              aModel.Competency?.CompetencyAssessmentOrganisation
            ),
        },
        {
          show: true,
          prompt: 'Assessment certificate number',
          relativeRoute: (): TaskListRoute => {
            return {
              route: CompetencyAssessmentCertificateNumberComponent.route,
            };
          },
          getStatus: (aModel: BuildingProfessionalModel): TaskStatus =>
            this.determineCompetencyTaskStatus(
              aModel.Competency?.CompetencyAssessmentOrganisation,
              aModel.Competency?.CompetencyAssessmentCertificateNumber
            ),
        },
        {
          show: true,
          prompt: 'Date of assessment',
          relativeRoute: (): TaskListRoute => {
            return { route: CompetencyAssessmentDateComponent.route };
          },
          getStatus: (aModel: BuildingProfessionalModel): TaskStatus =>
            this.determineCompetencyTaskStatus(
              aModel.Competency?.CompetencyAssessmentCertificateNumber,
              aModel.Competency?.CompetencyDateOfAssessment
            ),
        },
        {
          show: true,
          prompt: 'Summary',
          relativeRoute: (): TaskListRoute => {
            return { route: CompetencySummaryComponent.route };
          },
          getStatus: (aModel: BuildingProfessionalModel): TaskStatus =>
            this.determineCompetencySummaryTask(aModel.Competency),
        },
      ],
    },
    {
      prompt: 'Professional memberships and employment',
      relativeRoute: ProfessionalActivityModule.baseRoute,
      show: true,
      id: 'professional-memberships-and-employment',
      children: [
        {
          show: true,
          prompt: 'Membership of professional bodies',
          relativeRoute: (): TaskListRoute => {
            return { route: ProfessionalBodyMembershipsComponent.route };
          },
          getStatus: (aModel: BuildingProfessionalModel): TaskStatus =>
            this.getModelStatus(aModel.ProfessionalMemberships),
        },
        {
          show: true,
          prompt: 'Employment',
          relativeRoute: (): TaskListRoute => {
            return { route: ProfessionalActivityEmploymentTypeComponent.route };
          },
          getStatus: (aModel: BuildingProfessionalModel): TaskStatus =>
            this.getModelStatus(aModel.ProfessionalActivity?.EmploymentDetails),
        },
        {
          show: true,
          prompt: 'Summary',
          relativeRoute: (): TaskListRoute => {
            return {
              route: ProfessionalMembershipAndEmploymentSummaryComponent.route,
            };
          },
          getStatus: (aModel: BuildingProfessionalModel): TaskStatus =>
            this.determineProfessionalMembershipSummaryTask(
              aModel.ProfessionalMemberships,
              aModel.ProfessionalActivity.EmploymentDetails
            ),
        },
      ],
    },
    {
      prompt: 'Application summary',
      relativeRoute: 'application-submission',
      show: true,
      id: 'application-summary',
      children: [
        {
          show: true,
          prompt: 'Application summary',
          relativeRoute: (): TaskListRoute => {
            return { route: ApplicationSummaryComponent.route };
          },
          getStatus: (aModel: BuildingProfessionalModel): TaskStatus =>
            this.determineApplicationSummaryTask(aModel),
        },
        {
          show: true,
          prompt: 'Pay your fee and submit your application',
          relativeRoute: (): TaskListRoute => this.paymentRoute!,
          getStatus: (aModel: BuildingProfessionalModel): TaskStatus => {
            if (this.checkingStatus == true) {
              return TaskStatus.CannotStart;
            } else {
              if (
                this.model?.StageStatus['ApplicationConfirmed'] ==
                StageCompletionState.Complete
              ) {
                if (this.paymentStatus == this.paymentEnum.Success) {
                  return TaskStatus.Complete;
                } else if (this.paymentStatus == this.paymentEnum.Started || this.paymentStatus == this.paymentEnum.Failed) {
                  return TaskStatus.InProgress;
                } else {
                  return TaskStatus.NotStarted;
                }
              } else {
                return TaskStatus.CannotStart;
              }
            }
          },
        },
      ],
    },
  ];

  navigateTo(parent: ITaskListParent, child: ITaskListChild): void {
    if (
      child.getStatus(this.model!) !== TaskStatus.CannotStart &&
      child.relativeRoute().route != ''
    ) {
      this.navigationService.navigateRelative(
        `${this.ModelApplicationId}/${parent.relativeRoute}/${
          child.relativeRoute().route
        }`,
        this.activatedRoute,
        child.relativeRoute().queryParams
      );
    }
  }

  override onSave(applicationService: ApplicationService): Promise<void> {
    throw new Error('Method not implemented.');
  }
  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return (
      this.applicationService.model?.StageStatus['EmailVerification'] == StageCompletionState.Complete &&
      this.applicationService.model?.StageStatus['PhoneVerification'] == StageCompletionState.Complete &&
      this.applicationService.model.StageStatus['Payment'] != StageCompletionState.Complete &&
      FieldValidations.IsNotNullOrWhitespace(this.applicationService.model.id)
    );
  }
  override isValid(): boolean {
    throw new Error('Method not implemented.');
  }
  override navigateNext(): Promise<boolean> {
    throw new Error('Method not implemented.');
  }

  navigateToSummary(): Promise<boolean> {
    return this.PersonalDetailRouter.navigateTo(
      this.model!,
      PersonalDetailRoutes.SUMMARY
    );
  }

  navigateToApplicantName() {
    throw new Error('Method not implemented.');
  }

  async getPaymentStatus(): Promise<void> {
    var payments = await this.applicationService.getApplicationPayments();

    //Check for all payments for application
    if (payments?.length > 0) {
      //Filter for successful and newly created payments
      var successfulPayments = payments.filter(
        (x) => x.bsr_govukpaystatus == 'success'
      );

      if (successfulPayments?.length > 0) {
        //Check if these payments are not failed or refunded
        var successsfulpayment = successfulPayments.find(
          (x) =>
            x.bsr_paymentreconciliationstatus !==
              PaymentReconciliationStatus.FAILED_RECONCILIATION &&
            x.bsr_paymentreconciliationstatus !==
              PaymentReconciliationStatus.FAILED_PAYMENT &&
            x.bsr_paymentreconciliationstatus !==
              PaymentReconciliationStatus.REFUNDED
        );
        this.paymentStatus = successsfulpayment
          ? PaymentStatus.Success
          : PaymentStatus.Failed;
        this.paymentRoute = successsfulpayment
          ? {
              route: 'payment/' + PaymentConfirmationComponent.route,
              queryParams: { reference: successsfulpayment?.bsr_transactionid },
            }
          : { route: 'payment/' + PaymentDeclarationComponent.route };
      } else {
        this.paymentStatus = PaymentStatus.Failed;
        this.paymentRoute = {
          route: 'payment/' + PaymentDeclarationComponent.route,
        };
      }
    } else if (
      this.model?.StageStatus['Declaration'] == StageCompletionState.Complete
    ) {
      this.paymentStatus = PaymentStatus.Started;
      this.paymentRoute = {
        route: 'payment/' + PaymentDeclarationComponent.route,
      };
    } else {
      this.paymentStatus = PaymentStatus.Pending;
      this.paymentRoute = {
        route: 'payment/' + PaymentDeclarationComponent.route,
      };
    }
    this.checkingStatus = false;
  }

}
