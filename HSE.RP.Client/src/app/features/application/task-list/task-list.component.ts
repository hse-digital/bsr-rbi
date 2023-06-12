import { Component, OnInit, QueryList, ViewChildren } from "@angular/core";
import { TitleService } from 'src/app/services/title.service';
import { ActivatedRoute, ActivatedRouteSnapshot, Router } from "@angular/router";
import { GovukErrorSummaryComponent } from "hse-angular";
import { ApplicationService, BuildingInspectorModel, ApplicationStatus, /* PaymentStatus */ } from "src/app/services/application.service";
import { NavigationService } from "src/app/services/navigation.service";
import { PageComponent } from "src/app/helpers/page.component";
// import { PaymentDeclarationComponent } from "../payment/payment-declaration/payment-declaration.component";
// import { PaymentModule } from "../payment/payment.module";
// import { BuildingSummaryNavigation } from "src/app/features/application/building-summary/building-summary.navigation";
// import { AccountablePersonNavigation } from "src/app/features/application/accountable-person/accountable-person.navigation";

@Component({
  templateUrl: './task-list.component.html'
})
export class ApplicationTaskListComponent extends PageComponent<BuildingInspectorModel> {
navigateToSections() {
throw new Error('Method not implemented.');
}
navigateToPap() {
throw new Error('Method not implemented.');
}
navigateToPayment() {
throw new Error('Method not implemented.');
}
paymentStatus: any;
paymentEnum: any;
override model?: BuildingInspectorModel;



    constructor(activatedRoute: ActivatedRoute, applicationService: ApplicationService) {
    super(activatedRoute);
    this.updateOnSave = false;
  }
  override onInit(applicationService: ApplicationService): void {
/*     if (this.containsFlag(ApplicationStatus.BlocksInBuildingComplete, applicationService)) this.completedSections++;
      await this.getPaymentStatus();
      this.completedSections++;

    if (this.containsFlag(RegistrationStatus.PaymentComplete)) this.completedSections++;

    this.checkingStatus = false; */

    this.model = applicationService.model;
  }

  override onSave(applicationService: ApplicationService): Promise<void> {
    throw new Error("Method not implemented.");
  }
  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return true;
  }
  override isValid(): boolean {
    throw new Error("Method not implemented.");
  }
  override navigateNext(): Promise<boolean> {
    throw new Error("Method not implemented.");
  }

  static route: string = 'task-list';
  static title: string = "Register as a building inspector - Register a high-rise building - GOV.UK";

  applicationStatus = ApplicationStatus;
  completedSections: number = 0;
  checkingStatus = true;
  // paymentEnum = PaymentStatus;
  // paymentStatus?: PaymentStatus;


  containsFlag(flag: ApplicationStatus, applicationService: ApplicationService) {
    return (applicationService.model.RegistrationStatus & flag) == flag;
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
