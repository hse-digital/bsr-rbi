import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { PageComponent } from '../../../helpers/page.component';
import { EmailValidator } from '../../../helpers/validators/email-validator';
import { FieldValidations } from '../../../helpers/validators/fieldvalidations';
import {ApplicationService} from '../../../services/application.service';
import { ApplicationTaskListComponent } from '../../application/task-list/task-list.component';
import { ApplicantEmailVerifyComponent } from './applicant-email-verify.component';
import { ApplicantEmail } from 'src/app/models/applicant-email.model';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';

@Component({
  selector: 'hse-applicant-email',
  templateUrl: './applicant-email.component.html',
})
export class ApplicantEmailComponent extends PageComponent<ApplicantEmail>  {

  public static route: string = "applicant-email";
  static title: string = "Personal details - Register as a building inspector - GOV.UK";
  production: boolean = environment.production;
  emailHasErrors: boolean = false;
  duplicateApplication: boolean = false;
  duplicateApplicationMessage: string = "There is already an application associated with your email address. Contact BSR [link to contact BSR service]"
  emailErrorMessage: string = "Enter a valid email address";
  modelValid: boolean = false;

  constructor(activatedRoute: ActivatedRoute, applicationService: ApplicationService) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {
    if (!applicationService.model.PersonalDetails?.ApplicantEmail) {
      applicationService.model.PersonalDetails!.ApplicantEmail = new ApplicantEmail();
    }
    this.model = applicationService.model.PersonalDetails?.ApplicantEmail;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    applicationService.model.PersonalDetails!.ApplicantEmail = this.model;
    await applicationService.sendVerificationEmail(this.model!.Email!)
  }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return applicationService.model.PersonalDetails?.ApplicantName?.CompletionState == ComponentCompletionState.Complete;
  }

  override isValid(): boolean {
    console.log(this.duplicateApplication);

    this.emailHasErrors = false;

    if (this.model == null || this.model == '')
    {
      this.emailHasErrors = true;
      this.emailErrorMessage = "Enter an email address";
    }
    else if (this.duplicateApplication == true){
      this.emailHasErrors = true;
      this.emailErrorMessage = this.duplicateApplicationMessage;
    }
    else {
      this.emailHasErrors = !EmailValidator.isValid(this.model!.Email ?? '');
      this.emailErrorMessage = "Enter a valid email address";
    }


    return !this.emailHasErrors && !this.duplicateApplication;
  }

  async DuplicateApplicationCheck(): Promise<void> {

    this.duplicateApplication = false;

    var duplicateApplicationResponse: boolean = await this.applicationService.CheckDuplicateBuildingProfessionalApplication();

    this.duplicateApplication = duplicateApplicationResponse;

    this.saveAndContinue();

  }

  navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(ApplicantEmailVerifyComponent.route, this.activatedRoute);
  }



}
