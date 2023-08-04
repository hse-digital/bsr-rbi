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
import { VerificationData } from './returning-application.component';

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
    verificationOption: { hasError: false, errorText: '', anchorId: '' },
    serviceError: { hasError: false, errorText: '', anchorId: '' },
  };

  verificationEmail: string = '';
  verificationPhone: string = '';
  VerificationData?: VerificationData;

  @Input() emailAddress: string | undefined;
  @Output() emailAddressChange = new EventEmitter<string | undefined>();

  @Input() applicationNumber: string | undefined;
  @Output() applicationNumberChange = new EventEmitter<string | undefined>();

  @Input() phoneNumber: string | undefined;
  @Output() phoneNumberChange = new EventEmitter<string | undefined>();

  @Input() verificationOption: string | undefined;
  @Output() verificationOptionChange = new EventEmitter<string | undefined>();

  @Output()
  onContinue = new EventEmitter<VerificationData>();

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
    this.errors.verificationOption.hasError = false;
    this.errors.serviceError.hasError = false;
    this.sendingRequest = false;



    if (!this.verificationOption && !this.applicationNumber) {
      this.errors.applicationNumber.errorText =
        'Enter your 12-digit application reference number and select a verification option';
      this.errors.applicationNumber.anchorId = 'input-application-number';
    }
    else if (!this.verificationOption) {
      this.errors.verificationOption.hasError = true;
      this.errors.verificationOption.errorText =
        'Select how you want to receive your 6-digit verification code, via text message or email';
    }
    else {
      if (this.verificationOption == 'phone-option') {
        this.isEmailAddressValid();
      } else if (this.verificationOption == 'email-option') {
        this.isPhoneNumberValid();
      }
    }

    try{
      await this.isApplicationNumberValid();
      }catch(error){
        this.errors.serviceError.hasError = true;
        this.errors.serviceError.errorText = "There was a problem with the service. Try again later.";
        this.sendingRequest = false;
        throw error;
      }

    this.hasErrors =
      this.errors.emailAddress.hasError ||
      this.errors.applicationNumber.hasError ||
      this.errors.phoneNumber.hasError ||
      this.errors.verificationOption.hasError ||
      this.errors.serviceError.hasError;

    if (!this.hasErrors) {
      if (this.verificationOption == 'phone-option') {
        try{
        await this.applicationService.sendVerificationSms(
          this.verificationPhone!)
        }catch(error){
          this.errors.serviceError.hasError = true;
          this.errors.serviceError.errorText = "There was a problem with the service. Try again later.";
          this.sendingRequest = false;
          throw error;

        };
      } else if (this.verificationOption == 'email-option') {
        try{
        await this.applicationService.sendVerificationEmail(
          this.verificationEmail!
        );
        }catch(error){
          this.errors.serviceError.hasError = true;
          this.errors.serviceError.errorText = "There was a problem with the service. Try again later.";
          this.sendingRequest = false;
          throw error;
        }
      }

      if(!this.errors.serviceError.hasError)
      {
        this.VerificationData = {
          verificationEmail: this.verificationEmail,
          verificationPhone: this.verificationPhone,
        };
        this.onContinue.emit(this.VerificationData);
      }
      else
      {
        this.sendingRequest = false;
        this.summaryError?.first?.focus();
        this.titleService.setTitleError();

      }
    } else {
      this.sendingRequest = false;
      this.summaryError?.first?.focus();
      this.titleService.setTitleError();
    }
  }

  async isApplicationNumberValid() {
    this.errors.applicationNumber.errorText = '';
    if (!this.applicationNumber || this.applicationNumber.length != 12) {
      this.errors.applicationNumber.errorText =
        'You must enter your 12 digit application number';
      this.errors.applicationNumber.anchorId = 'input-application-number';
    } else {
      var result =
        await this.applicationService.validateReturningApplicationDetails(
          this.applicationNumber!,
          this.verificationOption!,
          this.emailAddress,
          this.phoneNumber
        );
      if (this.verificationOption == 'email-option') {
        if(this.errors.phoneNumber.hasError==false){
        if (result.IsValidApplicationNumber && result.IsValid) {
          this.verificationEmail = result.EmailAddress;
        } else if (!result.IsValidApplicationNumber && result.IsValid) {
          this.errors.applicationNumber.errorText =
            'Application number does not match this telephone number. Enter the correct 12 digit application number';
          this.errors.applicationNumber.anchorId = 'input-phone-number';
        } else if (result.IsValidApplicationNumber && !result.IsValid) {
          this.errors.applicationNumber.errorText =
            'Your telephone number does not match this application. Enter the correct telephone number';
        }
      }
      } else if (this.verificationOption == 'phone-option') {
        if(this.errors.emailAddress.hasError==false){
          if (result.IsValidApplicationNumber && result.IsValid) {
            this.verificationPhone = result.PhoneNumber;
          } else if (!result.IsValidApplicationNumber && result.IsValid ) {
            this.errors.applicationNumber.errorText =
              'Application number does not match this email address. Enter the correct 12 digit application number';
            this.errors.applicationNumber.anchorId = 'input-email-address';
          } else if (result.IsValidApplicationNumber && !result.IsValid ) {
           this.errors.applicationNumber.errorText = 'Your email does not match this application. Enter the correct email address';
          }
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
      this.errors.phoneNumber.errorText = 'Enter your mobile telephone number';
      this.errors.phoneNumber.hasError = true;
    } else if (
      !PhoneNumberValidator.isValid(this.phoneNumber!.toString() ?? '')
    ) {
      this.errors.phoneNumber.errorText = 'Enter a UK mobile telephone number';
      this.errors.phoneNumber.hasError = true;
    }
  }
}
