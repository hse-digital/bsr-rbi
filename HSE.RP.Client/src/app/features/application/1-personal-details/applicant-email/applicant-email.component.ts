import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { EmailValidator } from '../../../../helpers/validators/email-validator';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../../services/application.service';
import { ApplicantProofOfIdentityComponent } from '../applicant-proof-of-identity/applicant-proof-of-identity.component';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';

@Component({
  selector: 'hse-applicant-email',
  templateUrl: './applicant-email.component.html',
})
export class ApplicantEmailComponent extends PageComponent<string>  {

  public static route: string = "applicant-email";
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
    if(!applicationService.model.personalDetails)
    {
      applicationService.model.personalDetails = {};
    }
    this.model = applicationService.model.personalDetails?.applicantEmail ?? '';
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    applicationService.model.personalDetails!.applicantEmail = this.model;
    await applicationService.sendVerificationEmail(this.model!)
  }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return true;
  }


  override isValid(): boolean {
    this.emailHasErrors = !EmailValidator.isValid(this.model ?? '');
    return !this.emailHasErrors;
  }

  navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative('applicant-email-verify', this.activatedRoute)

  }


}
