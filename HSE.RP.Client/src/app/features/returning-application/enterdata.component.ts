import { Component, EventEmitter, Input, Output, QueryList, ViewChildren } from "@angular/core";
import { TitleService } from 'src/app/services/title.service';
import { GovukErrorSummaryComponent } from "hse-angular";
import { ApplicationService } from "src/app/services/application.service";
import { EmailValidator } from '../../helpers/validators/email-validator';
import { PhoneNumberValidator } from "src/app/helpers/validators/phone-number-validator";

@Component({
  selector: 'application-enterdata',
  templateUrl: './enterdata.component.html'
})
export class ReturningApplicationEnterDataComponent {

  hasErrors = false;
  sendingRequest = false;
  errors = {
    emailAddress: { hasError: false, errorText: '' },
    applicationNumber: { hasError: false, errorText: '' },
    phoneNumber: { hasError: false, errorText: '' },
    validationOption: { hasError: false, errorText: '' }
  }

  validationOption?: string;


  @Input() emailAddress: string | undefined;
  @Output() emailAddressChange = new EventEmitter<string | undefined>();

  @Input() applicationNumber: string | undefined;
  @Output() applicationNumberChange = new EventEmitter<string | undefined>();


  @Input() phoneNumber: string | undefined;
  @Output() phoneNumberChange = new EventEmitter<string | undefined>();

  @Output()
  onContinue = new EventEmitter<{ emailAddress: string, applicationNumber: string, phoneNumber: string }>();

  @ViewChildren("summaryError") summaryError?: QueryList<GovukErrorSummaryComponent>;

  constructor(private applicationService: ApplicationService, private titleService: TitleService) { }


  getErrorDescription(showError: boolean, errorMessage: string): string | undefined {
    return this.hasErrors && showError ? errorMessage : undefined;
  }



  async validateAndContinue(): Promise<void> {
    this.errors.emailAddress.hasError = false;
    this.errors.phoneNumber.hasError = false;
    this.errors.applicationNumber.hasError = false;
    this.errors.validationOption.hasError = false;

    this.sendingRequest = true;

    if(!this.validationOption) {
      this.errors.validationOption.hasError = true;

      this.errors.validationOption.errorText = 'Select how you want to receive your 6-digit verification code, via text message or email';
    }
    if(this.validationOption == 'email-option') {
    this.isEmailAddressValid();
    }
    else if(this.validationOption == 'phone-option') {
      this.isPhoneNumberValid()
    }
    await this.isApplicationNumberValid();

    this.hasErrors = this.errors.emailAddress.hasError || this.errors.applicationNumber.hasError || this.errors.phoneNumber.hasError;

    if (!this.hasErrors) {
      // await this.applicationService.sendVerificationEmail(this.emailAddress!);
      // this.onContinue.emit({ emailAddress: this.emailAddress!, applicationNumber: this.applicationNumber!, phoneNumber: this.phoneNumber! });
      console.log(this.validationOption + " Selected")
    } else {
      console.log("error")

      this.sendingRequest
      this.summaryError?.first?.focus();
      this.titleService.setTitleError();
    }

    this.sendingRequest = false;
  }

  async isApplicationNumberValid() {
    this.errors.applicationNumber.errorText = ''
    if (!this.applicationNumber || this.applicationNumber.length != 12) {
      this.errors.applicationNumber.errorText = 'You must enter your 12 digit application code';
    } else {
      var result = await this.applicationService.validateReturningApplicationDetails(this.emailAddress!, this.applicationNumber!);
      if (result.isValidApplicationNumber && result.isValidEmail) {
        // Do nothing, this is a valid condition
      } else if (!result.isValidApplicationNumber && result.isValidEmail) {
        this.errors.applicationNumber.errorText = 'Application number does not match this email address. Enter the correct 12 digit application code';
      } else if (result.isValidApplicationNumber && !result.isValidEmail) {
        this.errors.applicationNumber.errorText = 'Your email does not match this application. Enter the correct email address'
      } else {
        this.errors.applicationNumber.errorText = 'Application number does not match this email address. Enter the correct 12 digit application code';
      }
    }

    this.errors.applicationNumber.hasError = this.errors.applicationNumber.errorText != '';
  }

  isEmailAddressValid() {

    if (!this.emailAddress) {
      this.errors.emailAddress.errorText = 'Enter your email address';
      this.errors.emailAddress.hasError = true;
    }
    else if (!EmailValidator.isValid(this.emailAddress!)) {
      this.errors.emailAddress.errorText = "Enter a real email address";
      this.errors.emailAddress.hasError = true;
    }
  }

  isPhoneNumberValid() {
    if (!this.phoneNumber) {
      this.errors.phoneNumber.errorText = 'Enter your phone number';
      this.errors.phoneNumber.hasError = true;
    }
    else if (!PhoneNumberValidator.isValid(
      this.phoneNumber!.toString() ?? ''
    )){
      this.errors.phoneNumber.errorText = "Enter a valid telephone number";
      this.errors.phoneNumber.hasError = true;
    }
  }

}
