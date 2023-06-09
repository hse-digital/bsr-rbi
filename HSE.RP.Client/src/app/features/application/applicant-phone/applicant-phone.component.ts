import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { PageComponent } from '../../../helpers/page.component';
import { FieldValidations } from '../../../helpers/validators/fieldvalidations';
import { PhoneNumberValidator } from '../../../helpers/validators/phone-number-validator';
import { ApplicationService, BuildingControlModel } from '../../../services/application.service';

@Component({
  selector: 'hse-applicant-phone',
  templateUrl: './applicant-phone.component.html',
})
export class ApplicantPhoneComponent extends PageComponent<string> {
  public static route: string = "applicant-phone";
  static title: string = "Your telephone number - Apply for building control approval for a higher-risk building - GOV.UK";
  production: boolean = environment.production;
  modelValid: boolean = false;
  phoneNumberHasErrors = false;
  override model?: string;

  constructor(activatedRoute: ActivatedRoute, applicationService: ApplicationService) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {
    this.model = applicationService.model.PhoneNumber;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    applicationService.model.PhoneNumber = this.model;
  }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return (FieldValidations.IsNotNullOrWhitespace(applicationService.model?.FirstName) || FieldValidations.IsNotNullOrWhitespace(applicationService.model?.LastName));
  }


  override isValid(): boolean {
    this.phoneNumberHasErrors = !PhoneNumberValidator.isValid(this.model?.toString() ?? '');
    return !this.phoneNumberHasErrors;
  }

  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigate('application/applicant-email');
  }

}
