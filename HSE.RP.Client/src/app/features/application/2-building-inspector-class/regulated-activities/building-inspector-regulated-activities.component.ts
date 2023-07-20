//BuildingInspectorRegulatedActivies
import { Component, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService, ApplicationStatus, BuildingInspectorClass, BuildingInspectorClassType, BuildingInspectorRegulatedActivies, ComponentCompletionState } from '../../../../services/application.service';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { BuildingInspectorSummaryComponent } from '../building-inspector-summary/building-inspector-summary.component';
import { BuildingInspectorRoutes, BuildingInspectorRouter } from '../BuildingInspectorRoutes'; 

@Component({
  selector: 'hse-building-inspector-regulated-activities',
  templateUrl: './building-inspector-regulated-activities.component.html',
})
export class BuildingInspectorRegulatedActivitiesComponent extends PageComponent<BuildingInspectorRegulatedActivies> {
  DerivedIsComplete(value: boolean): void {
    this.DemandModel().CompletionState = value ? ComponentCompletionState.Complete : ComponentCompletionState.InProgress;

  }

  public static route: string = BuildingInspectorRoutes.REGULATED_ACTIVITIES;
  public id: string = BuildingInspectorSummaryComponent.route;
  static title: string = "Building inspector class - Register as a building inspector - GOV.UK";
  production: boolean = environment.production;
  public hint: string = "Select all that apply";
  public errorText: string = "";

  override model?: BuildingInspectorRegulatedActivies;
  public selections: string[] = [];

  @Output() onClicked = new EventEmitter();
  @Output() onKeyupEnter = new EventEmitter();

  public DemandModel(): BuildingInspectorRegulatedActivies {
    if (this.model === undefined || this.model === null) {
      throw new Error("Model is undefined");
    }
    return this.model;
  }


  public getClassType(): BuildingInspectorClassType
  {
    return this.applicationService?.model?.InspectorClass?.ClassType.Class ?? 0;
  }

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService,
    private buildingInspectorRouter : BuildingInspectorRouter) {
    super(activatedRoute);
    this.updateOnSave = true;
    if (applicationService.model?.InspectorClass)
      applicationService.model.InspectorClass.ClassType.Class = BuildingInspectorClassType.ClassNone;
    if (!applicationService.model?.InspectorClass) {
      applicationService.model.InspectorClass = new BuildingInspectorClass();
    }
    if (!applicationService.model.InspectorClass.Activities) {
      applicationService.model.InspectorClass.Activities = new BuildingInspectorRegulatedActivies();
    }
    this.model = applicationService.model.InspectorClass?.Activities;
    if (this.DemandModel().AssessingPlans === true)
      this.selections.push("AssessingPlans");
    if (this.DemandModel().Inspection === true)
      this.selections.push("Inspection");
    this.applicationService = applicationService;

  }

  override onInit(applicationService: ApplicationService): void {


  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    this.DemandModel().AssessingPlans = false;
    this.DemandModel().Inspection = false;
    this.selections.forEach((value: any) => {
      this.DemandModel()[value] = true;
    });
  }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return true;
    //return (FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.firstName) || FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.lastName));

  }
  //override async saveAndContinue(): Promise<void> {
  //  await super.saveAndContinue();
  //}

  override isValid(): boolean {
    this.errorText = "";
    if (this.selections.length == 0)
      this.errorText = "You must select at least one BSR-regulated activity";
    return this.selections.length > 0;
  }

  override navigateNext(): Promise<boolean> {
    return this.buildingInspectorRouter.navigateTo(this.applicationService.model, BuildingInspectorRoutes.SUMMARY);
  }

  optionClicked() {
  }

}

