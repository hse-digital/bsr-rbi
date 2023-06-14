import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicantName, ApplicationService} from '../../../../services/application.service';
import { ApplicantPhotoComponent } from '../applicant-photo/applicant-photo.component';
import { ApplicantDateOfBirthComponent } from '../applicant-date-of-birth/applicant-date-of-birth.component';

@Component({
  selector: 'hse-applicant-name',
  templateUrl: './applicant-name.component.html',
})
export class ApplicantNameComponent extends PageComponent<ApplicantName> {
  public static route: string = "applicant-name";
  static title: string = "Your Name - Apply for building control approval for a higher-risk building - GOV.UK";
  production: boolean = environment.production;
  firstNameValid: boolean = false;
  lastNameValid: boolean = false;
  override model: ApplicantName = new ApplicantName;


  constructor(activatedRoute: ActivatedRoute, applicationService: ApplicationService) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {
    if(applicationService.model.personalDetails?.applicantName == null)
    {
      applicationService.model.personalDetails!.applicantName = new ApplicantName();
    }
    this.model.firstName = applicationService.model.personalDetails?.applicantName?.firstName ?? '';
    this.model.lastName = applicationService.model.personalDetails?.applicantName?.lastName ?? '';
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    applicationService.model.personalDetails!.applicantName!.firstName = this.model.firstName;
    applicationService.model.personalDetails!.applicantName!.lastName = this.model.lastName;
  }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    //return FieldValidations.IsNotNullOrWhitespace(applicationService.model.personalDetails!.applicantName?.firstName) && FieldValidations.IsNotNullOrWhitespace(applicationService.model.personalDetails!.applicantName?.lastName);
    return true;
  }



  override isValid(): boolean {
    this.firstNameValid = FieldValidations.IsNotNullOrWhitespace(this.model.firstName)
    this.lastNameValid = FieldValidations.IsNotNullOrWhitespace(this.model.lastName)
    return this.firstNameValid && this.lastNameValid;
  }

  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(ApplicantDateOfBirthComponent.route, this.activatedRoute);
  }
}

