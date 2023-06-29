import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { PageComponent } from '../../../helpers/page.component';
import { FieldValidations } from '../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../services/application.service';
import { ApplicantAddressComponent } from '../../application/1-personal-details/applicant-address/applicant-address.component';
import { ApplicantSummaryComponent } from '../../application/1-personal-details/applicant-summary/applicant-summary.component';
import { ApplicantEmailVerifyComponent } from './applicant-email-verify.component';

@Component({
  selector: 'hse-applicant-email-generate-new-security-code',
  templateUrl: './applicant-email-generate-new-security-code.component.html',
})
export class ApplicantGenerateNewSecurityCodeComponent extends PageComponent<string> {

  public static route: string = "applicant-email-generate-new-security-code";
  static title: string = "Personal details - Register as a building inspector - GOV.UK";
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  override model?: string;

  constructor(activatedRoute: ActivatedRoute, applicationService: ApplicationService) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {
    this.model = applicationService.model.PersonalDetails?.ApplicantEmail ?? '';
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    await applicationService.sendVerificationEmail(this.model!)
  }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return !!applicationService.model.PersonalDetails?.ApplicantEmail
  }


  override isValid(): boolean {
    return true;
  }

  navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(ApplicantEmailVerifyComponent.route, this.activatedRoute);
  }

}
