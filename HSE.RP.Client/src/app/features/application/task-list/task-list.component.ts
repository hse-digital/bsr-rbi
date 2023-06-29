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
  ApplicationStatus /* PaymentStatus */,
} from 'src/app/services/application.service';
import { NavigationService } from 'src/app/services/navigation.service';
import { PageComponent } from 'src/app/helpers/page.component';
import { PersonalDetailsPlaceholderComponent } from '../1-personal-details/personal-details-placeholder/personal-details-placeholder.component';
import { BuildingInspectorClassPlaceholderComponent } from '../2-building-inspector-class/building-inspector-class-placeholder/building-inspector-class-placeholder.component';
import { CompetencyPlaceholderComponent } from '../3-competency/competency-placeholder/competency-placeholder.component';
import { ProfessionalActivityPlaceholderComponent } from '../4-professional-activity/professional-activity-placeholder/professional-activity-placeholder.component';
import { ApplicationSubmissionPlaceholderComponent } from '../5-application-submission/application-submission-placeholder/application-submission-placeholder.component';
import { environment } from 'src/environments/environment';
import { PayAndSubmitComponent } from '../5-application-submission/pay-and-submit-application/pay-and-submit.component';
// import { PaymentDeclarationComponent } from "../payment/payment-declaration/payment-declaration.component";
// import { PaymentModule } from "../payment/payment.module";
// import { BuildingSummaryNavigation } from "src/app/features/application/building-summary/building-summary.navigation";
// import { AccountablePersonNavigation } from "src/app/features/application/accountable-person/accountable-person.navigation";

@Component({
  templateUrl: './task-list.component.html',
})
export class ApplicationTaskListComponent extends PageComponent<BuildingProfessionalModel> {


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
    applicationService: ApplicationService
  ) {
    super(activatedRoute);
    this.updateOnSave = false;
    this.activatedRoute.params.subscribe(params => {
      this.QueryApplicationId=params['id']
    })
    this.ModelApplicationId=applicationService.model.id!;
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

  navigateToPersonalDetails() {
    return this.navigationService.navigateRelative(`${this.ModelApplicationId}/personal-details/${PersonalDetailsPlaceholderComponent.route}`, this.activatedRoute);
  }
  navigateToBuildingInspectorClass() {
    return this.navigationService.navigateRelative(`${this.ModelApplicationId}/building-inspector-class/${BuildingInspectorClassPlaceholderComponent.route}`, this.activatedRoute);
  }
  navigateToCompetency() {
    return this.navigationService.navigateRelative(`${this.ModelApplicationId}/competency/${CompetencyPlaceholderComponent.route}`, this.activatedRoute);
  }
  navigateToProfessionalActivity() {
    return this.navigationService.navigateRelative(`${this.ModelApplicationId}/professional-activity/${ProfessionalActivityPlaceholderComponent.route}`, this.activatedRoute);
  }
  navigateToApplicationOverview() {
    return this.navigationService.navigateRelative(`${this.ModelApplicationId}/application-submission/${ApplicationSubmissionPlaceholderComponent.route}`, this.activatedRoute);
  }
  navigateToPayAndSubmit() {
    return this.navigationService.navigateRelative(`${this.ModelApplicationId}/application-submission/${PayAndSubmitComponent.route}`, this.activatedRoute);
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
