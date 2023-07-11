import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { EmailValidator } from '../../../../helpers/validators/email-validator';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService, ApplicationStatus } from '../../../../services/application.service';
import { ApplicantProofOfIdentityComponent } from '../applicant-proof-of-identity/applicant-proof-of-identity.component';

@Component({
  selector: 'hse-applicant-alternative-email',
  templateUrl: './applicant-alternative-email.component.html',
})
export class ApplicantAlternativeEmailComponent extends PageComponent<string>  {

  public static route: string = "applicant-alternative-email";
  static title: string = "Personal details - Register as a building inspector - GOV.UK";
  production: boolean = environment.production;
  emailHasErrors: boolean = false;
  emailErrorMessage: string = "";
  modelValid: boolean = false;
  selectedOption: string = "";
  override model?: string;

  constructor(activatedRoute: ActivatedRoute, applicationService: ApplicationService) {
    super(activatedRoute);
    this.updateOnSave = true;
  }

  override onInit(applicationService: ApplicationService): void {
    this.model = applicationService.model.PersonalDetails?.ApplicantAlternativeEmail;
    if (this.model === "") {
      this.selectedOption = "no"
    } else if (this.model) {
      this.selectedOption = "yes"
    }
  }


  override async onSave(applicationService: ApplicationService): Promise<void> {
    if (this.selectedOption === "no") {
      this.applicationService.model.PersonalDetails!.ApplicantAlternativeEmail = ""
    } else {
      this.applicationService.model.PersonalDetails!.ApplicantAlternativeEmail = this.model; 
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
    return this.navigationService.navigateRelative(ApplicantProofOfIdentityComponent.route, this.activatedRoute)
  }

}
