import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService, ApplicationStatus } from '../../../../services/application.service';
import { ApplicantAddressComponent } from '../applicant-address/applicant-address.component';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { ApplicantSummaryComponent } from '../applicant-summary/applicant-summary.component';
import { NationalInsuranceNumberValidator } from '../../../../helpers/validators/national-insurance-number-validator';
import { PersonalDetailRoutes, PersonalDetailRouter } from '../PersonalDetailRoutes'

@Component({
  selector: 'hse-applicant-national-insurance-number',
  templateUrl: './applicant-national-insurance-number.component.html',
})
export class ApplicantNationalInsuranceNumberComponent extends PageComponent<string> {

  public static route: string = PersonalDetailRoutes.NATIONAL_INS_NUMBER;
  static title: string = "Personal details - Register as a building inspector - GOV.UK";
  production: boolean = environment.production;
  nsiHasErrors: boolean = false;
  nsiIsNullOrWhiteSpace: boolean = false;
  nsiIsInvalidFormat: boolean = false;
  override model?: string;

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService,
    private personalDetailRouter: PersonalDetailRouter) {
    super(activatedRoute);
    this.updateOnSave = true;
  }

  override onInit(applicationService: ApplicationService): void {
    this.model = applicationService.model.PersonalDetails?.ApplicantNationalInsuranceNumber ?? '';
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    this.applicationService.model.PersonalDetails!.ApplicantNationalInsuranceNumber = this.model;

   }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return this.applicationService.model.ApplicationStatus >= ApplicationStatus.PhoneVerified && this.applicationService.model.id != null;
    
  }

  getErrorMessage(): string {
    if (this.nsiIsNullOrWhiteSpace) {
      return "You must enter your National Insurance number to proceed.";
    }
    if (this.nsiIsInvalidFormat) {
      return "Please enter a properly formated National Insurance number";
    }
    return "";
  }

  override isValid(): boolean {
    this.nsiIsNullOrWhiteSpace = !FieldValidations.IsNotNullOrWhitespace(this.model);
    this.nsiIsInvalidFormat = !NationalInsuranceNumberValidator.isValid(this.model ?? '');
    this.nsiHasErrors = this.nsiIsNullOrWhiteSpace || this.nsiIsInvalidFormat;
    return !this.nsiHasErrors; 
  }

  override navigateNext(): Promise<boolean> {
    return this.personalDetailRouter.navigateTo(this.applicationService.model, PersonalDetailRoutes.SUMMARY)
  }

}
