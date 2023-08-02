import { Component, EventEmitter, Input, OnInit, Output, QueryList, ViewChildren } from "@angular/core";
import { GovukErrorSummaryComponent } from "hse-angular";
import { StageCompletionState } from "src/app/models/stage-completion-state.enum";
import { ApplicationService } from "src/app/services/application.service";
import { NavigationService } from "src/app/services/navigation.service";
import { TitleService } from 'src/app/services/title.service';

@Component({
  selector: 'application-verify',
  templateUrl: './verify.component.html'
})
export class ReturningApplicationVerifyComponent implements OnInit {

  static title: string = "Enter security code - Register as a building inspector - GOV.UK";

  @Input() emailAddress?: string;
  @Input() applicationNumber!: string;
  @Input() verificationOption!: string;
  @Input() phoneNumber?: string;

  @Output() onResendClicked = new EventEmitter();
  @Output() onShowChangeVerificationStepClicked = new EventEmitter();

  sendingRequest = false;
  hasErrors = false;
  securityCode?: string;
  errors = {
    securityCode: { hasError: false, errorText: '' }
  }

  @ViewChildren("summaryError") summaryError?: QueryList<GovukErrorSummaryComponent>;

  constructor(private applicationService: ApplicationService, private navigationService: NavigationService, private titleService: TitleService) { }

  ngOnInit() {
    this.titleService.setTitle(ReturningApplicationVerifyComponent.title);

  }

  getErrorDescription(showError: boolean, errorMessage: string): string | undefined {
    return this.hasErrors && showError ? errorMessage : undefined;
  }

  async validateAndContinue() {
    this.sendingRequest = true;

    this.errors.securityCode.hasError = true;
    if (!this.securityCode) {
      this.errors.securityCode.errorText = 'You must enter your 6 digit security code';
    } else if (!Number(this.securityCode) || this.securityCode.length != 6) {
      this.errors.securityCode.errorText = 'You must enter your 6 digit security code';
    } else if (!(await this.doesSecurityCodeMatch())) {
      this.errors.securityCode.errorText = 'Your 6-digit verification code is incorrect or has expired. Request a new verification code by clicking "Generate a new security code" link on this page.';
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
      if(this.verificationOption == "email-option"){
        //await this.applicationService.validateOTPToken(this.securityCode!, this.emailAddress!); --Redundant as token validated in continueApplication
        await this.applicationService.continueApplication( this.applicationNumber, this.securityCode!, this.verificationOption, this.emailAddress!, undefined);
        this.applicationService.model.StageStatus["EmailVerification"] == StageCompletionState.Complete
        this.applicationService.model.StageStatus["PhoneVerification"] == StageCompletionState.Complete
        this.navigationService.navigate(`application/${this.applicationNumber}`);
        return true;
      }
      else if(this.verificationOption == "phone-option"){
        //await this.applicationService.validateOTPToken(this.securityCode!, this.phoneNumber!); --Redundant as token validated in continueApplication
        await this.applicationService.continueApplication(this.applicationNumber, this.securityCode!, this.verificationOption, undefined, this.phoneNumber!);
        this.applicationService.model.StageStatus["EmailVerification"] == StageCompletionState.Complete
        this.applicationService.model.StageStatus["PhoneVerification"] == StageCompletionState.Complete
        this.navigationService.navigate(`application/${this.applicationNumber}`);
        return true;
      }
      else{
        return false;
      }
    } catch {
      return false;
    }
  }
}
