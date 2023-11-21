import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { PageComponent } from '../../../helpers/page.component';
import { FieldValidations } from '../../../helpers/validators/fieldvalidations';
import { ApplicationService} from '../../../services/application.service';
import { ApplicantEmailComponent } from '../applicant-email/applicant-email.component';
import { BuildingProfessionalModel } from 'src/app/models/building-professional.model';
import { ApplicantName } from 'src/app/models/applicant-name.model';
import { StageCompletionState } from 'src/app/models/stage-completion-state.enum';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { app } from '../../../../../server';
import { Competency } from 'src/app/models/competency.model';
import { v4 as uuidv4 } from 'uuid';
import { ApplicationStage } from 'src/app/models/application-stage.enum';

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
    if(!applicationService.model.CosmosId)
    {
      applicationService.model.CosmosId = uuidv4();
    }
    if(!applicationService.model.PersonalDetails)
    {
      applicationService.model.PersonalDetails = {};
    }
    if(applicationService.model.PersonalDetails?.ApplicantName == null)
    {
      applicationService.model.PersonalDetails!.ApplicantName = new ApplicantName();
      applicationService.model.PersonalDetails!.ApplicantName.FirstName= '';
      applicationService.model.PersonalDetails!.ApplicantName.LastName= '';
      applicationService.model.PersonalDetails!.ApplicantName.CompletionState = ComponentCompletionState.InProgress;
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
        "ApplicationConfirmed": StageCompletionState.Incomplete,
        "Declaration": StageCompletionState.Incomplete,
        "Payment": StageCompletionState.Incomplete,
      };
    }
    applicationService.model.Competency = new Competency();
    applicationService.model.ApplicationStage = ApplicationStage.PersonalDetails;
    this.model = applicationService.model;

  }


  override async onSave(applicationService: ApplicationService): Promise<void> {
    applicationService.model.PersonalDetails!.ApplicantName!.FirstName = this.model.PersonalDetails!.ApplicantName!.FirstName;
    applicationService.model.PersonalDetails!.ApplicantName!.LastName = this.model.PersonalDetails!.ApplicantName!.LastName;
    applicationService.model.PersonalDetails!.ApplicantName!.CompletionState = ComponentCompletionState.Complete;
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

  override async  navigateNext(): Promise<boolean> {
    var couldNavigate = await this.navigationService.navigateRelative(ApplicantEmailComponent.route, this.activatedRoute);
    return couldNavigate;
  }
}

