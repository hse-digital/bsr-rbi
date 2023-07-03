import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { PageComponent } from '../../../helpers/page.component';
import { EmailValidator } from '../../../helpers/validators/email-validator';
import { FieldValidations } from '../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../services/application.service';
import { ApplicantProofOfIdentityComponent } from '../../application/1-personal-details/applicant-proof-of-identity/applicant-proof-of-identity.component';
import { ApplicationTaskListComponent } from '../../application/task-list/task-list.component';
import { ApplicantEmailVerifyComponent } from './applicant-email-verify.component';

@Component({
  selector: 'hse-applicant-email',
  templateUrl: './applicant-email.component.html',
})
export class ApplicantEmailComponent extends PageComponent<string>  {

  public static route: string = "applicant-email";
  static title: string = "Personal details - Register as a building inspector - GOV.UK";
  production: boolean = environment.production;
  emailHasErrors: boolean = false;
  emailErrorMessage: string = "Enter a valid email address";
  modelValid: boolean = false;
  override model?: string;

  constructor(activatedRoute: ActivatedRoute, applicationService: ApplicationService) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {
    if(!applicationService.model.PersonalDetails)
    {
      applicationService.model.PersonalDetails = {};
    }
    this.model = applicationService.model.PersonalDetails?.ApplicantEmail ?? '';
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    applicationService.model.PersonalDetails!.ApplicantEmail = this.model;
    await applicationService.sendVerificationEmail(this.model!)
  }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return FieldValidations.IsNotNullOrWhitespace(applicationService.model.PersonalDetails?.ApplicantName?.FirstName) && FieldValidations.IsNotNullOrWhitespace(applicationService.model.PersonalDetails?.ApplicantName?.LastName);
  }


  override isValid(): boolean {
    this.emailHasErrors = false;
    if (this.model == null || this.model == '')
    {
      this.emailErrorMessage = "Enter an email address";
      this.emailHasErrors = true;
    }
    else{
      this.emailHasErrors = !EmailValidator.isValid(this.model ?? '');
      this.emailErrorMessage = "Enter a valid email address";
    }
    return !this.emailHasErrors;
  }

  navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(ApplicantEmailVerifyComponent.route, this.activatedRoute);
  }


}
