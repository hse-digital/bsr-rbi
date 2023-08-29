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
import { StageCompletionState } from 'src/app/models/stage-completion-state.enum';
import {
  ApplicationService
} from 'src/app/services/application.service';
import { NavigationService } from 'src/app/services/navigation.service';
import { PaymentService } from 'src/app/services/payment.service';
import { TitleService } from 'src/app/services/title.service';

@Component({
  selector: 'hse-payment-declaration',
  templateUrl: './payment-declaration.component.html',
})
export class PaymentDeclarationComponent extends PageComponent<BuildingProfessionalModel> {
  DerivedIsComplete(value: boolean): void {

  }
  static route: string = 'declaration';
  static title: string =
    'Register as a building inspector - GOV.UK';

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


    await this.applicationService.updateApplication();
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
  }

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
    throw new Error('Method not implemented.');
  }

  override async saveAndContinue() {
    this.loading = true;
    this.screenReaderNotification();

    await this.applicationService.syncDeclaration();
    this.applicationService.model.StageStatus["Declaration"] = StageCompletionState.Complete;
    var paymentResponse = await this.paymentService.InitialisePayment(
      this.applicationService.model
    );
    this.applicationService.updateApplication();

    if (typeof window !== 'undefined') {
      window.location.href = paymentResponse.PaymentLink;
    }
  }

  screenReaderNotification(message: string = 'Sending success') {
    var alertContainer = document!.getElementById('hiddenAlertContainer');
    if (alertContainer) {
      alertContainer.innerHTML = message;
    }
  }

}
