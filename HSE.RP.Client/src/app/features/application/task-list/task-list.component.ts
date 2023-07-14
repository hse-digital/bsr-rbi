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
  ApplicantDateOfBirth,
} from 'src/app/services/application.service';
import { NavigationService } from 'src/app/services/navigation.service';
import { PageComponent } from 'src/app/helpers/page.component';
import { environment } from 'src/environments/environment';
import { ApplicantDateOfBirthComponent } from '../1-personal-details/applicant-date-of-birth/applicant-date-of-birth.component';
import { ApplicantAlternativeEmailComponent } from '../1-personal-details/applicant-alternative-email/applicant-alternative-email.component';
import { ApplicantAlternativePhoneComponent } from '../1-personal-details/applicant-alternative-phone/applicant-alternative-phone.component';
import { ApplicantNameComponent } from '../1-personal-details/applicant-name/applicant-name.component';
import { ApplicantNationalInsuranceNumberComponent } from '../1-personal-details/applicant-national-insurance-number/applicant-national-insurance-number.component';

import { PersonalDetailRoutes, PersonalDetailRouter } from '../1-personal-details/PersonalDetailRoutes';

import { PaymentDeclarationComponent } from '../5-application-submission/payment/payment-declaration/payment-declaration.component';
// import { PaymentDeclarationComponent } from "../payment/payment-declaration/payment-declaration.component";
// import { PaymentModule } from "../payment/payment.module";
// import { BuildingSummaryNavigation } from "src/app/features/application/building-summary/building-summary.navigation";
// import { AccountablePersonNavigation } from "src/app/features/application/accountable-person/accountable-person.navigation";

@Component({
  templateUrl: './task-list.component.html',
})
export class ApplicationTaskListComponent extends PageComponent<BuildingProfessionalModel> {
  public PersonalDetailRouter: PersonalDetailRouter;
  PersonalDetailRoutes = PersonalDetailRoutes;


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
  ) {
    super(activatedRoute);
    this.updateOnSave = false;
    this.activatedRoute.params.subscribe(params => {
      this.QueryApplicationId=params['id']
    })
    this.ModelApplicationId = applicationService.model.id!;
    this.PersonalDetailRouter = personalDetailsRouter;
  }

  private GetModel(): BuildingProfessionalModel {
    if (this.model !== undefined) {
      return this.model;
    }
    else {
      return new BuildingProfessionalModel();
    }
  }

  override onInit(applicationService: ApplicationService): void {
    this.model = applicationService.model;


/*     if (this.containsFlag(ApplicationStatus.PersonalDetailsComplete)) this.completedSections++;
    if (this.containsFlag(ApplicationStatus.BuildingInspectorClassComplete)) this.completedSections++;
    if (this.containsFlag(ApplicationStatus.CompetencyComplete)) this.completedSections++;
    if (this.containsFlag(ApplicationStatus.ProfessionalActivityComplete)) this.completedSections++;
    if (this.containsFlag(ApplicationStatus.ApplicationOverviewComplete)) this.completedSections++;
    if (this.containsFlag(ApplicationStatus.PayAndSumbitComplete)) this.completedSections++; */

  }


/*   async ngOnInit(applicationService: ApplicationService): Promise<void> {
    if (this.containsFlag(BuildingApplicationStatus.BlocksInBuildingComplete)) this.completedSections++;
    if (this.containsFlag(BuildingApplicationStatus.AccountablePersonsComplete)) {
      await this.getPaymentStatus();
      this.completedSections++;
    }
    if (this.containsFlag(BuildingApplicationStatus.PaymentComplete)) this.completedSections++;

    this.checkingStatus = false;
  } */

  checkKeyExistsAndNotNull(obj: any, key: any) {
    if (obj.hasOwnProperty(key) && obj[key] !== null) {
      return true;
    }
    return false;
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

  isNullOrEmpty(s: string | null | undefined): boolean {
    return s === null || s === undefined || s.length === 0;
  }

  // paymentEnum = PaymentStatus;
  // paymentStatus?: PaymentStatus;

  containsFlag(flag: ApplicationStatus) {
    return (this.model!.ApplicationStatus & flag) == flag;
  }

  navigateToSummary() : Promise<boolean> {
    return this.PersonalDetailRouter.navigateTo(this.GetModel(), PersonalDetailRoutes.SUMMARY);
  }

  navigateToApplicantName() {
    return this.PersonalDetailRouter.navigateTo(this.GetModel(), PersonalDetailRoutes.NAME);
  }

  navigateToNationalInsuranceNumber() {
    return this.PersonalDetailRouter.navigateTo(this.GetModel(), PersonalDetailRoutes.NATIONAL_INS_NUMBER);
  }


  navigateToPersonalDetailsDateOfBirth() {
    return this.PersonalDetailRouter.navigateTo(this.GetModel(), PersonalDetailRoutes.DATE_OF_BIRTH);
  }

  navigateToPersonalDetailsAlternativeEmailAddress() {
    this.containsFlag(ApplicationStatus.PhoneVerified)
    return this.PersonalDetailRouter.navigateTo(this.GetModel(), PersonalDetailRoutes.ALT_EMAIL);
  }

  navigateToPersonalDetailsAlternativePhone() {
    return this.PersonalDetailRouter.navigateTo(this.GetModel(), PersonalDetailRoutes.ALT_PHONE);
  }

  navigateToSummaryPage() {
    return this.PersonalDetailRouter.navigateTo(this.GetModel(), PersonalDetailRoutes.SUMMARY);
  }

  navigateToBuildingInspectorClass() {
    throw new Error('Method not implemented.');
  }
  navigateToCompetency() {
    throw new Error('Method not implemented.');
  }
  navigateToProfessionalActivity() {
    throw new Error('Method not implemented.');
  }
  navigateToApplicationOverview() {
    throw new Error('Method not implemented.');
  }
  navigateToPayAndSubmit() {
    return this.navigationService.navigateRelative(`${this.ModelApplicationId}/application-submission/payment/${PaymentDeclarationComponent.route}`, this.activatedRoute);
  }
  navigateToPayment() {
    throw new Error('Method not implemented.');
  }

  //@ViewChildren("summaryError") override summaryError?: QueryList<GovukErrorSummaryComponent>;

  /*   constructor(router: Router, applicationService: ApplicationService, navigationService: NavigationService, activatedRoute: ActivatedRoute, titleService: TitleService,
    private buildingNavigation: BuildingSummaryNavigation, private apNavigation: AccountablePersonNavigation) {
    super(router, applicationService, navigationService, activatedRoute, titleService);
  } */

  /*   async ngOnInit(): Promise<void> {
    if (this.containsFlag(RegistrationStatus.BlocksInBuildingComplete)) this.completedSections++;
    if (this.containsFlag(RegistrationStatus.AccountablePersonsComplete)) {
      await this.getPaymentStatus();
      this.completedSections++;
    }
    if (this.containsFlag(RegistrationStatus.PaymentComplete)) this.completedSections++;

    this.checkingStatus = false;
  } */

  /*
  override canAccess(routeSnapshot: ActivatedRouteSnapshot): boolean {
    return this.applicationService.model?.id !== undefined && this.applicationService.model?.id == routeSnapshot.params['id'];
  } */

  /*   async navigateToSections() {
    const route = this.buildingNavigation.getNextRoute();
    await this.navigationService.navigateAppend(route, this.activatedRoute);
  } */

  /*   async navigateToPap() {
    const route = this.apNavigation.getNextRoute();
    await this.navigationService.navigateAppend(route, this.activatedRoute);
  } */

  /*   navigateToPayment() {
    let appendRoute = PaymentModule.baseRoute;
    appendRoute = `${appendRoute}/${PaymentDeclarationComponent.route}`;

    this.navigationService.navigateAppend(appendRoute, this.activatedRoute);
  } */

  /*   containsFlag(flag: RegistrationStatus) {
    return (this.applicationService.model.ApplicationStatus & flag) == flag;
  } */

  /*   async getPaymentStatus(): Promise<void> {
    var payments = await this.applicationService.getApplicationPayments();

    if (payments?.length > 0) {
      var successfulPayments = payments.filter(x => x.bsr_govukpaystatus == 'success');

      if (successfulPayments?.length > 0) {
        var sucesssfulpayment = successfulPayments.find(x => x.bsr_paymentreconciliationstatus !== 760_810_002 && x.bsr_paymentreconciliationstatus !== 760_810_003 && x.bsr_paymentreconciliationstatus !== 760_810_004);
        this.paymentStatus = sucesssfulpayment ? PaymentStatus.Success : PaymentStatus.Failed;
      } else {
        this.paymentStatus = PaymentStatus.Failed;
      }
    } else if (this.containsFlag(RegistrationStatus.PaymentInProgress)) {
      this.paymentStatus = PaymentStatus.Started;
    }
  } */
}
