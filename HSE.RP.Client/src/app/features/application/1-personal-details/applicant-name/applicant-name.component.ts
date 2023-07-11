import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicantName, ApplicationService} from '../../../../services/application.service';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';

@Component({
  selector: 'hse-applicant-name',
  templateUrl: './applicant-name.component.html',
})
export class ApplicantNameComponent extends PageComponent<ApplicantName> {
  public static route: string = "applicant-name";
  static title: string = "Your Name - Apply for building control approval for a higher-risk building - GOV.UK";
  production: boolean = environment.production;
  FirstNameValid: boolean = false;
  LastNameValid: boolean = false;

  override model: ApplicantName = new ApplicantName;


  constructor(activatedRoute: ActivatedRoute, applicationService: ApplicationService) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {
    if(!applicationService.model.PersonalDetails)
    {
      applicationService.model.PersonalDetails = {};
    }
    if(applicationService.model.PersonalDetails?.ApplicantName == null)
    {
      applicationService.model.PersonalDetails!.ApplicantName = new ApplicantName();
    }
    this.model.FirstName = applicationService.model.PersonalDetails?.ApplicantName?.FirstName ?? '';
    this.model.LastName = applicationService.model.PersonalDetails?.ApplicantName?.LastName ?? '';
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    applicationService.model.PersonalDetails!.ApplicantName!.FirstName = this.model.FirstName;
    applicationService.model.PersonalDetails!.ApplicantName!.LastName = this.model.LastName;
  }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return true;
  }



  override isValid(): boolean {
    this.FirstNameValid = FieldValidations.IsNotNullOrWhitespace(this.model.FirstName)
    this.LastNameValid = FieldValidations.IsNotNullOrWhitespace(this.model.LastName)

    if(!this.FirstNameValid && !this.LastNameValid)
    {

    }

    return this.FirstNameValid && this.LastNameValid;
  }

  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(`../${ApplicationTaskListComponent.route}`, this.activatedRoute);
  }
}
