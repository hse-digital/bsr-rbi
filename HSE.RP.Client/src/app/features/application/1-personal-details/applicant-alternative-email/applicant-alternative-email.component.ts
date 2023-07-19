import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { EmailValidator } from '../../../../helpers/validators/email-validator';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { PersonalDetailRoutes, PersonalDetailRouter } from '../PersonalDetailRoutes'
import { ApplicantEmail, ApplicationService, ApplicationStatus, ComponentCompletionState } from '../../../../services/application.service';
import { ApplicantAlternativePhoneComponent } from '../applicant-alternative-phone/applicant-alternative-phone.component';

@Component({
  selector: 'hse-applicant-alternative-email',
  templateUrl: './applicant-alternative-email.component.html',
})
export class ApplicantAlternativeEmailComponent extends PageComponent<string>  {

  public static route: string = PersonalDetailRoutes.ALT_EMAIL;
  static title: string = "Personal details - Register as a building inspector - GOV.UK";
  production: boolean = environment.production;
  emailHasErrors: boolean = false;
  emailErrorMessage: string = "";
  modelValid: boolean = false;
  selectedOption: string = "";

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService,
    private personalDetailRouter: PersonalDetailRouter) {
    super(activatedRoute);
    this.updateOnSave = true;
  }

  override onInit(applicationService: ApplicationService): void {
    if (!applicationService.model.PersonalDetails?.ApplicantAlternativeEmail) {
      applicationService.model.PersonalDetails!.ApplicantAlternativeEmail = { Email: '', CompletionState: ComponentCompletionState.InProgress };
    }
    this.model = applicationService.model.PersonalDetails?.ApplicantAlternativeEmail?.Email;
    if (this.model === "") {
      this.selectedOption = "no"
    } else if (this.model) {
      this.selectedOption = "yes"
    }
  }

  override DerivedIsComplete(value: boolean) {
    if(value)
      this.applicationService.model.PersonalDetails!.ApplicantAlternativeEmail!.CompletionState = ComponentCompletionState.Complete;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    this.applicationService.model.PersonalDetails!.ApplicantAlternativeEmail!.Email = this.model;
    if (this.selectedOption === "no") {
        this.applicationService.model.PersonalDetails!.ApplicantAlternativeEmail!.Email = ""
    }
  }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return this.applicationService.model.ApplicationStatus >= ApplicationStatus.PhoneVerified && this.applicationService.model.id != null;
    // return true
  }

  override isValid(): boolean {
    this.emailHasErrors = false;
    if(this.selectedOption === "") {
      this.emailHasErrors = true
      this.emailErrorMessage = "Select yes if you want to provide an alternative email address"
    }
    else if (this.selectedOption === "no") {
      return !this.emailHasErrors
    }
    else if (this.model == null || this.model == '')
    {
      this.emailErrorMessage = "Select yes if you want to provide an alternative email address";
      this.emailHasErrors = true;
    }
    else{
      this.emailHasErrors = !EmailValidator.isValid(this.model ?? '');
      this.emailErrorMessage = "Enter a valid email address";
    }
    return !this.emailHasErrors;
  }

  navigateNext(): Promise<boolean> {
    return this.personalDetailRouter.navigateTo(this.applicationService.model, PersonalDetailRoutes.ALT_PHONE);
  }

}
