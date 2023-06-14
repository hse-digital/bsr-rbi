import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { PhoneNumberValidator } from '../../../../helpers/validators/phone-number-validator';
import {
  ApplicationService,
  BuildingInspectorModel,
} from '../../../../services/application.service';
import { ApplicantAlternativeEmailComponent } from '../applicant-alternative-email/applicant-alternative-email.component';
import { ApplicantNationalInsuranceNumberComponent } from '../applicant-national-insurance-number/applicant-national-insurance-number.component';

@Component({
  selector: 'hse-applicant-alternative-phone',
  templateUrl: './applicant-alternative-phone.component.html',
})
export class ApplicantAlternativePhoneComponent extends PageComponent<string> {
  public static route: string = 'applicant-alternative-phone';
  static title: string = "Personal details - Register as a building inspector - GOV.UK";
  production: boolean = environment.production;
  modelValid: boolean = false;
  phoneNumberHasErrors = false;
  override model?: string;

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService
  ) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {
    this.model =
      applicationService.model.personalDetails?.applicantAlternativePhone?.toString() ??
      '';
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    /*     applicationService.model.personalDetails?.applicantPhone? = this.model;
     */
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return true;
    //return (FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicantName?.firstName) || FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicantName?.lastName));
  }

  override isValid(): boolean {
    this.phoneNumberHasErrors = !PhoneNumberValidator.isValid(
      this.model?.toString() ?? ''
    );
    return !this.phoneNumberHasErrors;
  }

  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(
      ApplicantNationalInsuranceNumberComponent.route,
      this.activatedRoute
    );
  }
}
