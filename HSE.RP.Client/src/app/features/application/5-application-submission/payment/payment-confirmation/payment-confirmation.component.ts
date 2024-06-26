import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot, CanActivate } from '@angular/router';
import { NotFoundComponent } from 'src/app/components/not-found/not-found.component';
import { ApplicationStage } from 'src/app/models/application-stage.enum';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { BuildingProfessionalModel } from 'src/app/models/building-professional.model';
import { PaymentModel } from 'src/app/models/payment.model';
import { StageCompletionState } from 'src/app/models/stage-completion-state.enum';
import { ApplicationService } from 'src/app/services/application.service';
import { NavigationService } from 'src/app/services/navigation.service';
import { PaymentService } from 'src/app/services/payment.service';

@Component({
  selector: 'hse-confirmation',
  templateUrl: './payment-confirmation.component.html',
})
export class PaymentConfirmationComponent implements OnInit, CanActivate {
  static route: string = "confirm";
  static title: string = "Register as a building inspector - Payment - GOV.UK";
  payment?: PaymentModel;
  shouldRender = false;
  paymentReference?: string;
  applicationNumber?: string;


  constructor(public applicationService: ApplicationService, public paymentService: PaymentService, private navigationService: NavigationService, private activatedRoute: ActivatedRoute) {
  }

  async ngOnInit() {
    await this.applicationService.syncPayment();

    this.activatedRoute.queryParams.subscribe(async query => {
      this.paymentReference = query['reference'] ?? await this.getApplicationPaymentReference();
      this.applicationNumber = this.applicationService.model.id;

      if (!this.paymentReference) {
        this.navigationService.navigate(`/application/${this.applicationService.model.id}`);
        return;
      }

      this.payment = await this.paymentService.GetPayment(this.paymentReference);
      if (this.payment?.Status == 'success') {
        this.applicationService.model.StageStatus['Payment'] = StageCompletionState.Complete;
        this.applicationService.model.ApplicationStage =ApplicationStage.ApplicationSubmitted;
        await this.applicationService.updateApplication();
        await this.applicationService.syncApplicationStage();
        await this.applicationService.clearSession();
        await this.applicationService.clearApplication();
        this.shouldRender = true;
      } else {
        this.navigationService.navigate(`/application/${this.applicationService.model.id}`);
      }

      this.applicationService.model = new BuildingProfessionalModel();

    });
  }



  private async getApplicationPaymentReference() {
    var payments = await this.applicationService.getApplicationPayments()
    return await payments.find(x => x.bsr_govukpaystatus == "success")?.bsr_transactionid;
  }

  canContinue(): boolean {
    return true;
  }

  navigateToNextPage(navigationService: NavigationService, activatedRoute: ActivatedRoute): Promise<boolean> {
    return navigationService.navigateRelative(PaymentConfirmationComponent.route, activatedRoute);
  }


  canActivate(_: ActivatedRouteSnapshot) {
    return this.applicationService.model.id != undefined && this.applicationService.model.id != null && this.applicationService.model.id != '' && this.applicationService.model.StageStatus!["ApplicationConfirmed"] == StageCompletionState.Complete;
  }

}
