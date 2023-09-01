import { Component, OnInit } from '@angular/core';
import {
  ActivatedRoute,
  ActivatedRouteSnapshot,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { PageComponent } from 'src/app/helpers/page.component';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { BuildingProfessionalModel } from 'src/app/models/building-professional.model';
import { PaymentStatus } from 'src/app/models/payment-status.enum';
import { StageCompletionState } from 'src/app/models/stage-completion-state.enum';
import { ApplicationService } from 'src/app/services/application.service';
import { NavigationService } from 'src/app/services/navigation.service';
import {
  PaymentReconciliationStatus,
  PaymentService,
} from 'src/app/services/payment.service';
import { TitleService } from 'src/app/services/title.service';
import { ApplicationSummaryComponent } from '../../application-summary/application-summary.component';
import { PaymentConfirmationComponent } from '../payment-confirmation/payment-confirmation.component';

@Component({
  selector: 'hse-payment-declaration',
  templateUrl: './payment-declaration.component.html',
})
export class PaymentDeclarationComponent extends PageComponent<BuildingProfessionalModel> {
  DerivedIsComplete(value: boolean): void {}
  static route: string = 'declaration';
  static title: string = 'Register as a building inspector - GOV.UK';
  paymentEnum = PaymentStatus;
  paymentStatus?: PaymentStatus;
  paymentReference?: '';
  loading = false;

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService,
    public paymentService: PaymentService
  ) {
    super(activatedRoute);
    this.updateOnSave = true;
  }

  override async onInit(applicationService: ApplicationService): Promise<void> {
    try{
    this.loading = true;
    await this.applicationService.updateApplication();
    await this.getPaymentStatus();
    }catch(error){
      this.loading = false;
    }
  }

  override async onSave(
    applicationService: ApplicationService
  ): Promise<void> {}

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return true;
  }
  override isValid(): boolean {
    return true;
  }
  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(
      PaymentConfirmationComponent.route,
      this.activatedRoute,
      { reference: this.paymentReference }
    );
  }

  override async saveAndContinue() {
    if (this.paymentStatus != PaymentStatus.Success) {
      this.loading = true;
      this.screenReaderNotification();

      await this.applicationService.syncDeclaration();
      this.applicationService.model.StageStatus['Declaration'] =
        StageCompletionState.Complete;
      var paymentResponse = await this.paymentService.InitialisePayment(
        this.applicationService.model
      );
      this.applicationService.updateApplication();

      if (typeof window !== 'undefined') {
        window.location.href = paymentResponse.PaymentLink;
      }
    } else {
      this.navigateNext();
    }
  }

  screenReaderNotification(message: string = 'Sending success') {
    var alertContainer = document!.getElementById('hiddenAlertContainer');
    if (alertContainer) {
      alertContainer.innerHTML = message;
    }
  }

  async getPaymentStatus(): Promise<void> {
    try {

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
          this.paymentReference = successsfulpayment?.bsr_paymentreference;
          this.loading = false;
        } else {
          this.paymentStatus = PaymentStatus.Pending;
          this.loading = false;
        }
      }
      this.loading = false;
    } catch (error) {
      this.loading = false;
    }
  }
}
