import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { EmailValidator } from '../../../../helpers/validators/email-validator';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../../services/application.service';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { ApplicantEmail } from '../../../../models/applicant-email.model';
import { SearchEmploymentOrganisationAddressComponent } from '../search-employment-organisation-address/search-employment-organisation-address.component';
import { EmployerName } from 'src/app/models/employment-employer-name';
import { EmploymentType } from 'src/app/models/employment-type.enum';

@Component({
  selector: 'hse-employment-other-name',
  templateUrl: './employment-other-name.component.html',
})
export class EmploymentOtherNameComponent extends PageComponent<EmployerName> {
  public static route: string = 'employment-other-name';
  static title: string =
    'Employment - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  otherNameHasErrors = false;
  invalidNameError = false;
  noOptionSelectedError = false;
  invalidNameErrorMessage: string = 'Enter the name of your business';
  invalidSelectionErrorMessage: string = 'Select yes, if you want to provide a business name';
  errorMessage: string = 'Select yes, if you want to provide a business name';

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService
  ) {
    super(activatedRoute);
    this.updateOnSave = true;
  }

  override onInit(applicationService: ApplicationService): void {



    if (
      !this.applicationService.model.ProfessionalActivity.EmploymentDetails
        ?.EmployerName
    ) {
      this.applicationService.model.ProfessionalActivity.EmploymentDetails!.EmployerName =
        new EmployerName();
    }

    this.model =
      this.applicationService.model.ProfessionalActivity.EmploymentDetails!.EmployerName;


  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    this.applicationService.model.ProfessionalActivity.EmploymentDetails!.EmployerName =
      this.model;
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return (
      this.applicationService.model.ProfessionalActivity.EmploymentDetails
        ?.EmploymentTypeSelection?.CompletionState ==
        ComponentCompletionState.Complete &&
      this.applicationService.model.ProfessionalActivity.EmploymentDetails
        .EmploymentTypeSelection?.EmploymentType == EmploymentType.Other
    );
  }

  override isValid(): boolean {
    this.noOptionSelectedError = false;
    this.invalidNameError = false;

    if(!FieldValidations.IsNotNullOrWhitespace(
      this.model?.OtherBusinessSelection
    ))
    {
      this.otherNameHasErrors=true;
      this.noOptionSelectedError = true;
      this.errorMessage = this.invalidSelectionErrorMessage;
      console.log(this.noOptionSelectedError);
      return false;
    }

    if (this.model?.OtherBusinessSelection == 'yes') {
      this.otherNameHasErrors=true;
      this.invalidNameError = true;
      this.errorMessage = this.invalidNameErrorMessage;
      console.log(this.invalidNameError);
      return false;
    }

    return true;

  }

  navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(
      SearchEmploymentOrganisationAddressComponent.route,
      this.activatedRoute
    ); //update to address
  }

  clearName() {
    this.model!.FullName = '';
    this.invalidNameError = false;
    this.otherNameHasErrors = false;
  }
}
