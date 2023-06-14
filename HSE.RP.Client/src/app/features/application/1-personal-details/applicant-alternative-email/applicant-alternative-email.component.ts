import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { EmailValidator } from '../../../../helpers/validators/email-validator';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../../services/application.service';
import { ApplicantProofOfIdentityComponent } from '../applicant-proof-of-identity/applicant-proof-of-identity.component';
import { ApplicantAlternativePhoneComponent } from '../applicant-alternative-phone/applicant-alternative-phone.component';

@Component({
  selector: 'hse-applicant-alternative-email',
  templateUrl: './applicant-alternative-email.component.html',
})
export class ApplicantAlternativeEmailComponent extends PageComponent<string>  {

  public static route: string = "applicant-alternative-email";
  static title: string = "Personal details - Register as a building inspector - GOV.UK";
  production: boolean = environment.production;
  emailHasErrors: boolean = false;
  modelValid: boolean = false;
  override model?: string;

  constructor(activatedRoute: ActivatedRoute, applicationService: ApplicationService) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {
/*     this.model = applicationService.model.Email;
 */  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
/*     applicationService.model.Email = this.model;

    await applicationService.sendVerificationEmail(this.model!) */
  }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {

   /*  return FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails.); */
   return true;
  }


  override isValid(): boolean {
    this.emailHasErrors = !EmailValidator.isValid(this.model ?? '');
    return !this.emailHasErrors;
  }

  navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(ApplicantAlternativePhoneComponent.route, this.activatedRoute)

  }


}
