import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';

import { environment } from '../../../../environments/environment';
import { NotFoundComponent } from '../../../components/not-found/not-found.component';
import { PageComponent } from '../../../helpers/page.component';
import { EmailValidator } from '../../../helpers/validators/email-validator';
import { FieldValidations } from '../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../services/application.service';
import { ApplicantPhoneComponent } from '../applicant-phone/applicant-phone.component';

@Component({
  selector: 'hse-applicant-phone-verify',
  templateUrl: './applicant-phone-verify.component.html',
})
export class ApplicantPhoneVerifyComponent extends PageComponent<number> {
  public static route: string = 'applicant-phone-verify';
  static title: string =
    'Apply for building control approval for a higher-risk building - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;

  override model!: number;
  otpToken = '';
  otpError = false;
  isOtpNotNumber = false;
  isOtpInvalidLength = false;
  isOtpEmpty = false;
  dataSyncError = false;
  PhoneNumber?: string;


  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService
  ) {
    super(activatedRoute);
    this.updateOnSave = false;

  }

  override onInit(applicationService: ApplicationService): void {
    this.PhoneNumber = applicationService.model.PersonalDetails!.ApplicantPhone;
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return FieldValidations.IsNotNullOrWhitespace(
      applicationService.model.PersonalDetails!.ApplicantPhone
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
    this.otpError = false;
    this.isOtpNotNumber = false;
    this.isOtpInvalidLength = false;
    this.isOtpEmpty = false;
    this.hasErrors = false;

    var otp = this.model?.toString() ?? '';
    this.isOtpEmpty = otp.length == 0;
    this.isOtpInvalidLength = otp.trim().length < 6 || otp.trim().length > 6;
    this.isOtpNotNumber = isNaN(this.model);
    if (!(this.isOtpNotNumber || this.isOtpInvalidLength || this.isOtpEmpty)) {
        try {
          await this.applicationService.validateOTPToken(
            this.model?.toString() ?? '',
            this.PhoneNumber ?? ''
          );
        } catch (error) {
          this.otpError = true;
          this.hasErrors = true;
          this.focusAndUpdateErrors();
          throw error;
        }
        try {
          await this.applicationService.registerNewBuildingProfessionApplication();
        } catch (error) {
          this.dataSyncError = true;
          this.hasErrors = true;
          this.focusAndUpdateErrors();
          throw error;
        }
    }
    else
    {
      this.hasErrors = true;
    }

    this.saveAndContinue();
  }

  navigateEnterPhoneNumber(): Promise<boolean> {
    return this.navigationService.navigate(ApplicantPhoneComponent.route);
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
      return 'Enter the correct security code';
    } else {
      return 'You must enter your 6 digit security code';
    }
  }
}
