import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { ApplicationService } from '../../../../services/application.service';
import { DateFormatHelper } from 'src/app/helpers/date-format-helper';
import { BuildingProfessionalModel } from 'src/app/models/building-professional.model';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { StageCompletionState } from 'src/app/models/stage-completion-state.enum';
import { AddressModel } from 'src/app/models/address.model';
import { PersonalDetailRouter, PersonalDetailRoutes } from '../../1-personal-details/PersonalDetailRoutes';
import { ApplicationSubmissionModule } from '../application.application-submission.module';
import { BuildingInspectorRoutes } from '../../application-routes';
import { BuildingInspectorClassType } from '../../../../models/building-inspector-classtype.enum';

@Component({
  selector: 'hse-application-summary',
  templateUrl: './application-summary.component.html',
})
export class ApplicationSummaryComponent extends PageComponent<string> {
  DerivedIsComplete(value: boolean): void {

  }
  PersonalDetailRoutes = PersonalDetailRoutes;

  BuildingInspectorRoutes = BuildingInspectorRoutes;
  BuildingInspectorClassType = BuildingInspectorClassType;

  public static route: string = "application-summary";
  static title: string = "Register as a building inspector - Summary of your application - GOV.UK";
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  private personalDetailRouter: PersonalDetailRouter;
  override model?: string;
  override processing = false;

  // assessPlansCategories: string = "";
  // assessPlansLink: string = "";

  // inspectionCategories: string = "";
  // inspectionLink: string = "";



  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService,
    personalDetailRouter: PersonalDetailRouter) {
    super(activatedRoute);
    this.personalDetailRouter = personalDetailRouter;
    this.updateOnSave = true;

  }


  override onInit(applicationService: ApplicationService): void {
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    // this.applicationService.model.StageStatus['PersonalDetails'] = StageCompletionState.Complete;
    // this.applicationService.model.ApplicationStatus = ApplicationStatus.PersonalDetailsComplete;
  }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return true;
  }


  override isValid(): boolean {
    return true;
  }

  override navigateNext(): Promise<boolean> {
    return this.personalDetailRouter.navigateTo(this.applicationService.model, PersonalDetailRoutes.TASK_LIST);
  }

  public navigateToPersonalDetailsRoute(namedRoute: string) {
    return this.navigateTo('personal-details/'+PersonalDetailRoutes.MapNameToRoute(namedRoute));
  }

  public navigateToBuildingInspectorClassDetailsRoute(route: string) {
    return this.navigateTo('building-inspector-class/'+route);
  }

  public navigateToBuildingInspectorCompetencyDetailsRoute(route: string) {
    return this.navigateTo('competency/'+route);
  }

  public navigateToBuildingInspectorProfessionalActivityDetailsRoute(route: string) {
    return this.navigateTo('professional-activity/'+route);
  }

  public navigateTo(route: string) {
    return this.navigationService.navigateRelative(`../${route}`, this.activatedRoute);
  }

  public GetFormattedDateofBirth(): string {
    return DateFormatHelper.LongMonthFormat(
      this.applicationService.model.PersonalDetails?.ApplicantDateOfBirth?.Year,
      this.applicationService.model.PersonalDetails?.ApplicantDateOfBirth?.Month,
      this.applicationService.model.PersonalDetails?.ApplicantDateOfBirth?.Day
    );
  }

  public getAlternativePhone(): string {
    return this.applicationService.model.PersonalDetails?.ApplicantAlternativePhone?.PhoneNumber || 'none';
  }

  public getAlternativeEmail(): string {
    return this.applicationService.model.PersonalDetails?.ApplicantAlternativeEmail?.Email || 'none';
  }

  async SyncAndContinue() {
    await this.applicationService.syncPersonalDetails();
    this.applicationService.model.StageStatus['PersonalDetails'] = StageCompletionState.Complete;
    this.saveAndContinue();
  }
}
