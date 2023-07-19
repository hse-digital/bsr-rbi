import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { PageComponent } from '../../../helpers/page.component';
import { FieldValidations } from '../../../helpers/validators/fieldvalidations';
import { ApplicantName, ApplicationService, BuildingProfessionalModel, ComponentCompletionState, StageCompletionState} from '../../../services/application.service';
import { ApplicantEmailComponent } from '../applicant-email/applicant-email.component';

@Component({
  selector: 'hse-applicant-name',
  templateUrl: './new-applicant-name.component.html',
})
export class NewApplicantNameComponent extends PageComponent<BuildingProfessionalModel> {
  public static route: string = "new-applicant-name";
  static title: string = "Your Name - Apply for building control approval for a higher-risk building - GOV.UK";
  production: boolean = environment.production;
  FirstNameValid: boolean = false;
  LastNameValid: boolean = false;

  override model: BuildingProfessionalModel = new BuildingProfessionalModel;


  constructor(activatedRoute: ActivatedRoute, applicationService: ApplicationService) {
    super(activatedRoute);
    this.updateOnSave = true;
  }

  override onInit(applicationService: ApplicationService): void {
    if(!applicationService.model.PersonalDetails)
    {
      applicationService.model.PersonalDetails = {};
    }
    if(applicationService.model.PersonalDetails?.ApplicantName == null)
    {
      applicationService.model.PersonalDetails!.ApplicantName = new ApplicantName();
      applicationService.model.PersonalDetails!.ApplicantName.FirstName= '';
      applicationService.model.PersonalDetails!.ApplicantName.LastName= '';
    }
    if(applicationService.model.StageStatus == null)
    {
      applicationService.model.StageStatus = {
        "EmailVerification": StageCompletionState.Incomplete,
        "PhoneVerification": StageCompletionState.Incomplete,
        "PersonalDetails": StageCompletionState.Incomplete,
        "BuildingInspectorClass": StageCompletionState.Incomplete,
        "Competency": StageCompletionState.Incomplete,
        "ProfessionalActivity": StageCompletionState.Incomplete,
        "Declaration": StageCompletionState.Incomplete,
        "Payment": StageCompletionState.Incomplete,
      };
    }
    this.model = applicationService.model;

  }

  override DerivedIsComplete(value: boolean) {
    this.applicationService.model.PersonalDetails!.ApplicantName!.CompletionState = value ? ComponentCompletionState.Complete : ComponentCompletionState.InProgress;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    applicationService.model.PersonalDetails!.ApplicantName!.FirstName = this.model.PersonalDetails!.ApplicantName!.FirstName;
    applicationService.model.PersonalDetails!.ApplicantName!.LastName = this.model.PersonalDetails!.ApplicantName!.LastName;
  }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return true;
  }



  override isValid(): boolean {
    this.FirstNameValid = FieldValidations.IsNotNullOrWhitespace(this.model.PersonalDetails!.ApplicantName!.FirstName)
    this.LastNameValid = FieldValidations.IsNotNullOrWhitespace(this.model.PersonalDetails!.ApplicantName!.LastName)

    if(!this.FirstNameValid && !this.LastNameValid)
    {

    }

    return this.FirstNameValid && this.LastNameValid;
  }

  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(ApplicantEmailComponent.route, this.activatedRoute);
  }
}

