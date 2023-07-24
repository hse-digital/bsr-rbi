import { Component, EventEmitter, Input, OnInit, Output, QueryList, ViewChildren } from "@angular/core";
import { GovukErrorSummaryComponent } from "hse-angular";
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
      this.errors.securityCode.errorText = 'Enter the correct security code';
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

  private async doesSecurityCodeMatch(): Promise<boolean> {
    try {
      if(this.verificationOption == "email-option"){
        await this.applicationService.validateOTPToken(this.securityCode!, this.emailAddress!);
        await this.applicationService.continueApplication(this.applicationNumber, this.emailAddress!, this.securityCode!);
        this.navigationService.navigate(`application/${this.applicationNumber}`);
        return true;
      }
      else if(this.verificationOption == "phone-option"){
        await this.applicationService.validateOTPToken(this.securityCode!, this.phoneNumber!);
        await this.applicationService.continueApplication(this.applicationNumber, this.phoneNumber!, this.securityCode!);
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
