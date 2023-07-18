import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { PageComponent } from '../../../helpers/page.component';
import { FieldValidations } from '../../../helpers/validators/fieldvalidations';
import { PhoneNumberValidator } from '../../../helpers/validators/phone-number-validator';
import {
    ApplicantPhone,
  ApplicationService,
  ApplicationStatus,
  BuildingProfessionalModel,
  ComponentCompletionState,
} from '../../../services/application.service';
import { ApplicationTaskListComponent } from '../../application/task-list/task-list.component';
import { ApplicantPhoneVerifyComponent } from './applicant-phone-verify.component';

@Component({
  selector: 'hse-applicant-phone',
  templateUrl: './applicant-phone.component.html',
})
export class ApplicantPhoneComponent extends PageComponent<string> {
  public static route: string = 'applicant-phone';
  static title: string = "Personal details - Register as a building inspector - GOV.UK";
  production: boolean = environment.production;
  modelValid: boolean = false;
  phoneNumberHasErrors = false;
  phoneNumberErrorMessage = "Enter your telephone number";  

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService
  ) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {
    if(!applicationService.model.PersonalDetails?.ApplicantPhone)
    {
      applicationService.model.PersonalDetails!.ApplicantPhone = { PhoneNumber: '', CompletionState: ComponentCompletionState.InProgress };
    }
    this.model = applicationService.model.PersonalDetails?.ApplicantPhone.PhoneNumber;
  }

  override DerivedIsComplete(value: boolean): void {
    this.applicationService.model.PersonalDetails!.ApplicantPhone!.CompletionState = value ? ComponentCompletionState.Complete : ComponentCompletionState.InProgress;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    this.applicationService.model.PersonalDetails!.ApplicantPhone!.PhoneNumber = this.model;
    await applicationService.sendVerificationSms(this.model!)
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return this.applicationService.model.PersonalDetails?.ApplicantEmail?.CompletionState === ComponentCompletionState.Complete
      && (this.applicationService.model.ApplicationStatus >= ApplicationStatus.EmailVerified ?? false);
  }

  override isValid(): boolean {
    this.phoneNumberHasErrors = !PhoneNumberValidator.isValid(
      this.model?.toString() ?? ''
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
