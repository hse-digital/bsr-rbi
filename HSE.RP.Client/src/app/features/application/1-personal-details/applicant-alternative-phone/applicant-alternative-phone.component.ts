import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { PhoneNumberValidator } from '../../../../helpers/validators/phone-number-validator';
import {
  ApplicationService,
  ApplicationStatus,
  BuildingProfessionalModel,
} from '../../../../services/application.service';
import { ApplicantNationalInsuranceNumberComponent } from '../applicant-national-insurance-number/applicant-national-insurance-number.component';
import { PersonalDetailRoutes } from '../PersonalDetailRoutes'

@Component({
  selector: 'hse-applicant-alternative-phone',
  templateUrl: './applicant-alternative-phone.component.html',
})
export class ApplicantAlternativePhoneComponent extends PageComponent<string> {
  public static route: string = PersonalDetailRoutes.ALT_PHONE;
  static title: string = "Personal details - Register as a building inspector - GOV.UK";
  production: boolean = environment.production;
  modelValid: boolean = false;
  phoneNumberHasErrors = false;
  selectedOption: string = "";
  selectedOptionError: boolean = false;
  errorMessage: string = "";
  override model?: string;

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService
  ) {
    super(activatedRoute);
    this.updateOnSave = true;
  }

  override onInit(applicationService: ApplicationService): void {    
    this.model = applicationService.model.PersonalDetails?.ApplicantAlternativePhone;

    if (this.model === "") {
      this.selectedOption = "no";
    }
    else if (this.model) {
      this.selectedOption = "yes";
    }
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    this.applicationService.model.PersonalDetails!.ApplicantAlternativePhone = this.model;
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return this.applicationService.model.ApplicationStatus >= ApplicationStatus.PhoneVerified && this.applicationService.model.id != null;
  }

  override isValid(): boolean {
    // if no option is selected, skip phone validation and save number as empty string
    if (this.selectedOption === "") {
      this.selectedOptionError = true;
      this.errorMessage = "Select yes if you want to provide an alternative telephone number";
      console.log("no option selected")
      return false;
    } else if (this.selectedOption === "no") {
      this.model = ""; 
      this.modelValid = true;
      return this.modelValid;
    } else { 
      this.phoneNumberHasErrors = !PhoneNumberValidator.isValid(
        this.model?.toString() ?? ''
      );
      this.modelValid = false;
      this.errorMessage = "Enter a UK telephone number"
      return !this.phoneNumberHasErrors;
    }
  }

  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(
      ApplicantNationalInsuranceNumberComponent.route,
      this.activatedRoute
    );
  }
}
