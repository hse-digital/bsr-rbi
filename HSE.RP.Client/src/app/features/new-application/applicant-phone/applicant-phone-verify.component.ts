import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';

import { environment } from '../../../../environments/environment';
import { NotFoundComponent } from '../../../components/not-found/not-found.component';
import { PageComponent } from '../../../helpers/page.component';
import { EmailValidator } from '../../../helpers/validators/email-validator';
import { FieldValidations } from '../../../helpers/validators/fieldvalidations';
import { ApplicationService} from '../../../services/application.service';
import { ApplicantPhoneComponent } from '../applicant-phone/applicant-phone.component';
import { StageCompletionState } from 'src/app/models/stage-completion-state.enum';
import { ComponentCompletionState } from "src/app/models/component-completion-state.enum";
import { IComponentModel } from "src/app/models/component. interface";

export class NumberComponent implements IComponentModel {
  Number: string = '';
  PhoneNumber: string = '';
  CompletionState: ComponentCompletionState = ComponentCompletionState.NotStarted;
}



@Component({
  selector: 'hse-applicant-phone-verify',
  templateUrl: './applicant-phone-verify.component.html',
})
export class ApplicantPhoneVerifyComponent extends PageComponent<NumberComponent> {
  DerivedIsComplete(value: boolean): void {
  }
  public static route: string = 'applicant-phone-verify';
  static title: string =
    'Verify phone number - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  optTimedOut:boolean = false;

  otpToken = '';
  otpError = false;
  isOtpNotNumber = false;
  isOtpInvalidLength = false;
  isOtpEmpty = false;
  dataSyncError = false;
  PhoneNumber?: string;
  override processing = false;


  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService
  ) {
    super(activatedRoute);
    this.updateOnSave = false;

  }

  override onInit(applicationService: ApplicationService): void {
    this.model = new NumberComponent();
    this.PhoneNumber = applicationService.model.PersonalDetails!.ApplicantPhone?.PhoneNumber;
    this.model.PhoneNumber = this.PhoneNumber ?? '';
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return FieldValidations.IsNotNullOrWhitespace(
      applicationService.model.PersonalDetails!.ApplicantPhone?.PhoneNumber
    );
  }

  override async onSave(
    applicationService: ApplicationService
  ): Promise<void> {

  }

  override isValid(): boolean {
    return !this.hasErrors;
  }

  async validateOTP() {
    this.processing=true;
    this.otpError = false;
    this.isOtpNotNumber = false;
    this.isOtpInvalidLength = false;
    this.isOtpEmpty = false;
    this.hasErrors = false;

    var otp = this.model?.Number ?? '';
    this.isOtpEmpty = otp.length == 0;
    this.isOtpInvalidLength = otp.trim().length < 6 || otp.trim().length > 6;
    this.isOtpNotNumber = isNaN(parseInt(this.model!.Number));
    if (!(this.isOtpNotNumber || this.isOtpInvalidLength || this.isOtpEmpty)) {
        try {
          await this.applicationService.validateOTPToken(
            this.model?.Number ?? '',
            this.PhoneNumber ?? ''
          );
        } catch (error: any) {

          if (error.status && error.status === 403) {
            this.optTimedOut= true;
          }

          this.otpError = true;
          this.hasErrors = true;
          this.processing = false;
          this.focusAndUpdateErrors();
          throw error;
        }
        this.applicationService.model.StageStatus['PhoneVerification'] = StageCompletionState.Complete;
        try {
          await this.applicationService.registerNewBuildingProfessionApplication();
        } catch (error) {
          this.dataSyncError = true;
          this.hasErrors = true;
          this.processing=false;

          this.focusAndUpdateErrors();
          throw error;
        }
        this.processing=false;
    }
    else
    {
      this.hasErrors = true;
      this.processing=false;
    }

    this.saveAndContinue();
  }

  navigateNext(): Promise<boolean> {
    return this.navigationService.navigate(`application/${this.applicationService.model.id}`);
  }

  getOtpError() {
    if (!this.isOtpEmpty && this.isOtpNotNumber) {
      return 'Your 6-digit security code must be a number, like 123456';
    } else if (this.isOtpInvalidLength) {
      return 'You must enter your 6 digit security code';
    } else if (this.otpError) {
      if (this.optTimedOut) {
        return 'Your 6-digit verification code has expired. Request a new verification code.';

      }
      else {
        return 'Enter the correct security code';

      }
    } else {
      return 'You must enter your 6 digit security code';
    }
  }
}
