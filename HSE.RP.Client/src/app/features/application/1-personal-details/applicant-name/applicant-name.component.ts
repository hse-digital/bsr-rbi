import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicantName, ApplicationService, BuildingInspectorModel } from '../../../../services/application.service';

@Component({
  selector: 'hse-applicant-name',
  templateUrl: './applicant-name.component.html',
})
export class ApplicantNameComponent extends PageComponent<ApplicantName> {
  public static route: string = "applicant-name";
  static title: string = "Your Name - Apply for building control approval for a higher-risk building - GOV.UK";
  production: boolean = environment.production;
  firstNameValid: boolean = false;
  lastNameValid: boolean = false;
  override model: ApplicantName = new ApplicantName;


  constructor(activatedRoute: ActivatedRoute, applicationService: ApplicationService) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {
    this.model.firstName = applicationService.model.FirstName;
    this.model.lastName = applicationService.model.LastName;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    applicationService.model.FirstName = this.model.firstName;
    applicationService.model.LastName = this.model.lastName;
  }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return FieldValidations.IsNotNullOrWhitespace(applicationService.model.ApplicationName);
  }



  override isValid(): boolean {
    this.firstNameValid = FieldValidations.IsNotNullOrWhitespace(this.model.firstName)
    this.lastNameValid = FieldValidations.IsNotNullOrWhitespace(this.model.lastName)
    return this.firstNameValid && this.lastNameValid;
  }

  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigate('application/applicant-phone');
  }
}

