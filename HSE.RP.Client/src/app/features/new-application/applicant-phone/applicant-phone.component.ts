import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { PageComponent } from '../../../helpers/page.component';
import { FieldValidations } from '../../../helpers/validators/fieldvalidations';
import { PhoneNumberValidator } from '../../../helpers/validators/phone-number-validator';
import { ApplicationService } from '../../../services/application.service';
import { ApplicationTaskListComponent } from '../../application/task-list/task-list.component';
import { ApplicantPhoneVerifyComponent } from './applicant-phone-verify.component';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { ApplicantPhone } from '../../../models/applicant-phone-model';

@Component({
  selector: 'hse-applicant-phone',
  templateUrl: './applicant-phone.component.html',
})
export class ApplicantPhoneComponent extends PageComponent<ApplicantPhone> {
  public static route: string = 'applicant-phone';
  static title: string =
    'Personal details - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  phoneNumberHasErrors = false;
  phoneNumberErrorMessage = 'Enter your telephone number';

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService
  ) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {
    if (!applicationService.model.PersonalDetails?.ApplicantPhone) {
      applicationService.model.PersonalDetails!.ApplicantPhone = new ApplicantPhone();
    }
    this.model = applicationService.model.PersonalDetails?.ApplicantPhone;
  }


  override async onSave(applicationService: ApplicationService): Promise<void> {
    //Force the phone number to be in the correct format
    this.model!.PhoneNumber = this.model?.PhoneNumber?.trim().replace("+44", "0");
    this.applicationService.model.PersonalDetails!.ApplicantPhone = this.model;
    await applicationService.sendVerificationSms(this.model!.PhoneNumber ?? "");
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return (
      this.applicationService.model.PersonalDetails?.ApplicantEmail
        ?.CompletionState === ComponentCompletionState.Complete &&
      (this.applicationService.model.ApplicationStatus >=
        ApplicationStatus.EmailVerified ??
        false)
    );
  }

  override isValid(): boolean {
    this.phoneNumberHasErrors = !PhoneNumberValidator.isValid(
      this.model?.PhoneNumber ?? ''
    );
    return !this.phoneNumberHasErrors;
  }

  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(
      ApplicantPhoneVerifyComponent.route,
      this.activatedRoute
    );
  }
}
