import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  QueryList,
  ViewChildren,
} from '@angular/core';
import { log } from 'console';
import { GovukErrorSummaryComponent } from 'hse-angular';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { PaymentStatus } from 'src/app/models/payment-status.enum';
import { StageCompletionState } from 'src/app/models/stage-completion-state.enum';
import { ApplicationService } from 'src/app/services/application.service';
import { NavigationService } from 'src/app/services/navigation.service';
import { PaymentReconciliationStatus } from 'src/app/services/payment.service';
import { TitleService } from 'src/app/services/title.service';
import { PaymentConfirmationComponent } from '../application/5-application-submission/payment/payment-confirmation/payment-confirmation.component';
import { ActivatedRoute } from '@angular/router';
import { ApplicationSubmissionComponent } from './application-submission/application-submission.component';
import { ApplicationStage } from 'src/app/models/application-stage.enum';

@Component({
  selector: 'application-verify',
  templateUrl: './verify.component.html',
})
export class ReturningApplicationVerifyComponent implements OnInit {
  static title: string =
    'Enter security code - Register as a building inspector - GOV.UK';

  @Input() emailAddress?: string;
  @Input() applicationNumber!: string;
  @Input() verificationOption!: string;
  @Input() phoneNumber?: string;

  @Output() onResendClicked = new EventEmitter();
  @Output() onShowChangeVerificationStepClicked = new EventEmitter();

  paymentEnum = PaymentStatus;
  paymentStatus?: PaymentStatus = PaymentStatus.Pending;
  paymentReference?: '';
  sendingRequest = false;
  checkingPaymentStatus = false;
  hasErrors = false;
  securityCode?: string;
  errors = {
    securityCode: { hasError: false, errorText: '' },
  };

  @ViewChildren('summaryError')
  summaryError?: QueryList<GovukErrorSummaryComponent>;



  constructor(
    private applicationService: ApplicationService,
    private navigationService: NavigationService,
    private titleService: TitleService,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit() {
    this.titleService.setTitle(ReturningApplicationVerifyComponent.title);
  }

  getErrorDescription(
    showError: boolean,
    errorMessage: string
  ): string | undefined {
    return this.hasErrors && showError ? errorMessage : undefined;
  }

  async validateAndContinue() {
    this.sendingRequest = true;

    this.errors.securityCode.hasError = true;
    if (!this.securityCode) {
      this.errors.securityCode.errorText =
        'You must enter your 6 digit security code';
    } else if (!Number(this.securityCode) || this.securityCode.length != 6) {
      this.errors.securityCode.errorText =
        'You must enter your 6 digit security code';
    } else if (!(await this.doesSecurityCodeMatch())) {
      this.errors.securityCode.errorText =
        'Your 6-digit verification code is incorrect or has expired. Request a new verification code by clicking "Generate a new security code" link on this page.';
    } else {
      this.errors.securityCode.hasError = false;
    }

    this.hasErrors = this.errors.securityCode.hasError;

    if (this.hasErrors) {
      this.summaryError?.first?.focus();
      this.titleService.setTitleError();
    }

    this.sendingRequest = false;
  }

  showResendStep() {
    this.onResendClicked.emit();
  }

  showChangeVerificationStep() {
    this.onShowChangeVerificationStepClicked.emit();
  }

  private async doesSecurityCodeMatch(): Promise<boolean> {
    try {
      if (
        this.verificationOption != 'email-option' &&
        this.verificationOption != 'phone-option'
      ) {
        return false;
      }

      if (this.verificationOption == 'email-option') {
        await this.applicationService.continueApplication(
          this.applicationNumber,
          this.securityCode!,
          this.verificationOption,
          this.emailAddress!,
          undefined
        );
      }
      if (this.verificationOption == 'phone-option') {
        await this.applicationService.continueApplication(
          this.applicationNumber,
          this.securityCode!,
          this.verificationOption,
          undefined,
          this.phoneNumber!
        );
      }

      this.applicationService.model.StageStatus['EmailVerification'] ==
        StageCompletionState.Complete;
      this.applicationService.model.StageStatus['PhoneVerification'] ==
        StageCompletionState.Complete;
      //Check payment status
      await this.getPaymentStatus();

      while(this.checkingPaymentStatus){
        await new Promise(resolve => setTimeout(resolve, 1000));
      }

      if (this.paymentStatus == PaymentStatus.Success) {
        return this.navigationService.navigateRelative(
          ApplicationSubmissionComponent.route,
          this.activatedRoute,
          { reference: this.paymentReference }
        );
      }
      else{
        this.applicationService.model.StageStatus['Payment'] = StageCompletionState.Incomplete;
        if(this.applicationService.model.ApplicationStage == ApplicationStage.ApplicationSubmitted)
        {
          this.applicationService.model.ApplicationStage = ApplicationStage.PayAndSubmit;
        }
        this.applicationService.updateApplication();
        await this.applicationService.syncApplicationStage();
        this.navigationService.navigate(`application/${this.applicationNumber}`);
      }

      return true;
    } catch (error) {
      return false;
    }
  }

  async getPaymentStatus(): Promise<void> {
    try {
      this.checkingPaymentStatus = true;

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
              x.bsr_paymentreconciliationstatus !== PaymentReconciliationStatus.FAILED_RECONCILIATION &&
              x.bsr_paymentreconciliationstatus !== PaymentReconciliationStatus.FAILED_PAYMENT &&
              x.bsr_paymentreconciliationstatus !== PaymentReconciliationStatus.REFUNDED
          );
          this.paymentStatus = successsfulpayment ? PaymentStatus.Success: PaymentStatus.Failed;
          this.paymentReference = successsfulpayment?.bsr_transactionid;
          this.checkingPaymentStatus = false;
        } else {
          this.paymentStatus = PaymentStatus.Pending;
          this.checkingPaymentStatus = false;
        }
      }
      this.checkingPaymentStatus = false;
    } catch (error) {
      this.checkingPaymentStatus = false;
    }
  }
}
