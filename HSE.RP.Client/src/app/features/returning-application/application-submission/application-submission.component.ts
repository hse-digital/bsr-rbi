import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot, CanActivate } from '@angular/router';
import { NotFoundComponent } from 'src/app/components/not-found/not-found.component';
import { ApplicationStage } from 'src/app/models/application-stage.enum';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { PaymentModel } from 'src/app/models/payment.model';
import { StageCompletionState } from 'src/app/models/stage-completion-state.enum';
import { ApplicationService } from 'src/app/services/application.service';
import { NavigationService } from 'src/app/services/navigation.service';
import { PaymentService } from 'src/app/services/payment.service';

@Component({
  selector: 'hse-application-submission',
  templateUrl: './application-submission.component.html',
})
export class ApplicationSubmissionComponent implements OnInit, CanActivate {
  static route: string = "application-submission";
  static title: string = "Application submitted - Payment - GOV.UK";
  payment?: PaymentModel;
  shouldRender = false;
  paymentReference?: string;
  applicationNumber?: string;

  constructor(public applicationService: ApplicationService, public paymentService: PaymentService, private navigationService: NavigationService, private activatedRoute: ActivatedRoute) {
  }

  async ngOnInit() {

    this.activatedRoute.queryParams.subscribe(async query => {
      this.paymentReference = query['reference']
      this.applicationNumber = this.applicationService.model.id;
      if (!this.paymentReference) {
        this.navigationService.navigate(`/application/${this.applicationService.model.id}`);
        return;
      }

      this.payment = await this.paymentService.GetPayment(this.paymentReference);
      if (this.payment?.Status == 'success') {
        this.applicationService.model.StageStatus['Payment'] = StageCompletionState.Complete;
        this.applicationService.model.ApplicationStage = ApplicationStage.ApplicationSubmitted;
        await this.applicationService.updateApplication();
        await this.applicationService.syncApplicationStage();
        await this.applicationService.clearSession();
        await this.applicationService.clearApplication();
        this.shouldRender = true;
      } else {
        this.applicationService.model.StageStatus['Payment'] = StageCompletionState.Incomplete;
        this.applicationService.model.ApplicationStage = ApplicationStage.ApplicationSummary;
        await this.applicationService.updateApplication();
        await this.applicationService.syncApplicationStage();
        this.navigationService.navigate(`/application/${this.applicationService.model.id}`);
      }


    });
  }

  canContinue(): boolean {
    return this.applicationService.model.id != undefined;
  }


  canActivate(_: ActivatedRouteSnapshot) {
    return this.applicationService.model.StageStatus!["ApplicationConfirmed"] == StageCompletionState.Complete

  }

  // async continueToKbi() {
  //   await this.navigationService.navigateRelative("../kbi", this.activatedRoute);
  // }
}
