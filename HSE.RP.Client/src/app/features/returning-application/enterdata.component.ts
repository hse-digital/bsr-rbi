import {
  Component,
  EventEmitter,
  Input,
  Output,
  QueryList,
  ViewChildren,
} from '@angular/core';
import { TitleService } from 'src/app/services/title.service';
import { GovukErrorSummaryComponent } from 'hse-angular';
import { ApplicationService } from 'src/app/services/application.service';
import { EmailValidator } from '../../helpers/validators/email-validator';
import { PhoneNumberValidator } from 'src/app/helpers/validators/phone-number-validator';

@Component({
  selector: 'application-enterdata',
  templateUrl: './enterdata.component.html',
})
export class ReturningApplicationEnterDataComponent {
  hasErrors = false;
  sendingRequest = false;
  errors = {
    emailAddress: { hasError: false, errorText: '', anchorId: '' },
    applicationNumber: { hasError: false, errorText: '', anchorId: '' },
    phoneNumber: { hasError: false, errorText: '', anchorId: '' },
    validationOption: { hasError: false, errorText: '', anchorId: '' },
  };

  verificationEmail?: string;
  verificationPhone?: string;

  @Input() emailAddress: string | undefined;
  @Output() emailAddressChange = new EventEmitter<string | undefined>();

  @Input() applicationNumber: string | undefined;
  @Output() applicationNumberChange = new EventEmitter<string | undefined>();

  @Input() phoneNumber: string | undefined;
  @Output() phoneNumberChange = new EventEmitter<string | undefined>();

  @Input() validationOption: string | undefined;
  @Output() validationOptionChange = new EventEmitter<string | undefined>();




  @Output()
  onContinue = new EventEmitter<{
    emailAddress?: string;
    applicationNumber: string;
    phoneNumber?: string;
    validationOption: string;
  }>();

  @ViewChildren('summaryError')
  summaryError?: QueryList<GovukErrorSummaryComponent>;

  constructor(
    private applicationService: ApplicationService,
    private titleService: TitleService
  ) {}

  getErrorDescription(
    showError: boolean,
    errorMessage: string
  ): string | undefined {
    return this.hasErrors && showError ? errorMessage : undefined;
  }

  async validateAndContinue(): Promise<void> {
    this.errors.emailAddress.hasError = false;
    this.errors.phoneNumber.hasError = false;
    this.errors.applicationNumber.hasError = false;
    this.errors.validationOption.hasError = false;

    this.sendingRequest = true;

    await this.isApplicationNumberValid();

    if (!this.validationOption && !this.applicationNumber) {
      this.errors.applicationNumber.errorText =
        'Enter your 12-digit application reference number and select a verification option';
      this.errors.applicationNumber.anchorId = 'input-application-number';
    } else if (!this.validationOption) {
      this.errors.validationOption.hasError = true;
      this.errors.validationOption.errorText =
        'Select how you want to receive your 6-digit verification code, via text message or email';
    } else {
      if (this.validationOption == 'phone-option') {
        this.isEmailAddressValid();
      } else if (this.validationOption == 'email-option') {
        this.isPhoneNumberValid();
      }
    }

    this.hasErrors =
      this.errors.emailAddress.hasError ||
      this.errors.applicationNumber.hasError ||
      this.errors.phoneNumber.hasError ||
      this.errors.validationOption.hasError;
    if (!this.hasErrors) {
      if(this.validationOption == 'phone-option')
      {
        await this.applicationService.sendVerificationSms(this.emailAddress!);

      }
      else if(this.validationOption == 'email-option')
      {
        await this.applicationService.sendVerificationEmail(this.verificationEmail!);
      }
      this.onContinue.emit({ emailAddress: this.verificationEmail, applicationNumber: this.applicationNumber!, phoneNumber: this.verificationPhone, validationOption: this.validationOption! });
    } else {
      this.sendingRequest = false;
      this.summaryError?.first?.focus();
      this.titleService.setTitleError();
    }

    this.sendingRequest = false;
  }

  async isApplicationNumberValid() {
    this.errors.applicationNumber.errorText = '';
    if (!this.applicationNumber || this.applicationNumber.length != 12) {
      this.errors.applicationNumber.errorText =
        'You must enter your 12 digit application code';
      this.errors.applicationNumber.anchorId = 'input-application-number';
    } else {
      var result =
        await this.applicationService.validateReturningApplicationDetails(
          this.applicationNumber!,
          this.validationOption!,
          this.emailAddress,
          this.phoneNumber
        );

      if (this.validationOption == 'email-option') {
        if (result.IsValidApplicationNumber && result.IsValid) {
          this.verificationEmail = result.EmailAddress;
        } else if (!result.IsValidApplicationNumber && result.IsValid) {
          this.errors.applicationNumber.errorText =
            'Application number does not match this telephone number. Enter the correct 12 digit application code';
          this.errors.applicationNumber.anchorId = 'input-phone-number';
        } else if (result.IsValidApplicationNumber && !result.IsValid) {
          this.errors.applicationNumber.errorText =
            'Your telephone number does not match this application. Enter the correct telephone number';
        } else {
          this.errors.applicationNumber.errorText =
            'Your mobile phone number does not match this application. Enter the correct mobile telephone number';
        }
      } else if (this.validationOption == 'phone-option') {
        if (result.IsValidApplicationNumber && result.IsValid) {
          this.phoneNumber = result.PhoneNumber;
        } else if (!result.IsValidApplicationNumber && result.IsValid) {
          this.errors.applicationNumber.errorText =
            'Application number does not match this email address. Enter the correct 12 digit application code';
          this.errors.applicationNumber.anchorId = 'input-email-address';
        } else if (result.IsValidApplicationNumber && !result.IsValid) {
          this.errors.applicationNumber.errorText =
            'Your email does not match this application. Enter the correct email address';
        }
      }
    }

    this.errors.applicationNumber.hasError =
      this.errors.applicationNumber.errorText != '';
  }

  isEmailAddressValid() {
    if (!this.emailAddress) {
      this.errors.emailAddress.errorText = 'Enter your email address';
      this.errors.emailAddress.hasError = true;
    } else if (!EmailValidator.isValid(this.emailAddress!)) {
      this.errors.emailAddress.errorText = 'Enter a real email address';
      this.errors.emailAddress.hasError = true;
    }
  }

  isPhoneNumberValid() {
    if (!this.phoneNumber) {
      this.errors.phoneNumber.errorText = 'Enter your phone number';
      this.errors.phoneNumber.hasError = true;
    } else if (
      !PhoneNumberValidator.isValid(this.phoneNumber!.toString() ?? '')
    ) {
      this.errors.phoneNumber.errorText = 'Enter a UK mobile telephone number';
      this.errors.phoneNumber.hasError = true;
    }
  }
}
