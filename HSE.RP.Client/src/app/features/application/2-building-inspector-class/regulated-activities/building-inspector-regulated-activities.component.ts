//BuildingInspectorRegulatedActivies
import { Component, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../../services/application.service';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { BuildingInspectorSummaryComponent } from '../building-inspector-summary/building-inspector-summary.component';
import { BuildingInspectorRoutes, BuildingInspectorRouter } from '../BuildingInspectorRoutes'; 
import { BuildingInspectorRegulatedActivies } from 'src/app/models/building-inspector-regulated-activies.model';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { BuildingInspectorClass } from 'src/app/models/building-inspector-class.model';
import { BuildingInspectorClassType } from 'src/app/models/building-inspector-classtype.enum';
import { BuildingAssessingPlansCategoriesClass2 } from 'src/app/models/building-assessing-plans-categories-class2.model';
import { BuildingAssessingPlansCategoriesClass3 } from 'src/app/models/buidling-assessing-plans-categories-class3.model';
import { Class2InspectBuildingCategories } from 'src/app/models/class2-inspect-building-categories.model';
import { Class3InspectBuildingCategories } from 'src/app/models/class3-inspect-building-categories.model';

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
  }

  override onInit(applicationService: ApplicationService): void {
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

  override async onSave(applicationService: ApplicationService): Promise<void> {
    // check if original options have changed, if they have reset state for the relevant next pages
    if (this.model?.AssessingPlans === true && this.selections.includes("AssessingPlans") === false)
    {
      this.applicationService.model.InspectorClass!.AssessingPlansClass2 = new BuildingAssessingPlansCategoriesClass2();
      this.applicationService.model.InspectorClass!.AssessingPlansClass3 = new BuildingAssessingPlansCategoriesClass3();
    }

    if (this.model?.Inspection === true && this.selections.includes("Inspection") === false)
    {
      this.applicationService.model.InspectorClass!.Class2InspectBuildingCategories = new Class2InspectBuildingCategories();
      this.applicationService.model.InspectorClass!.Class3InspectBuildingCategories = new Class3InspectBuildingCategories();
    }

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
    if (this.applicationService.model.InspectorClass?.Activities.AssessingPlans === true) {
      if (this.applicationService.model.InspectorClass.ClassType.Class === BuildingInspectorClassType.Class2) {
        return this.buildingInspectorRouter.navigateTo(this.applicationService.model, BuildingInspectorRoutes.CLASS2_ACCESSING_PLANS_CATEGORIES);
      }
      if (this.applicationService.model.InspectorClass.ClassType.Class === BuildingInspectorClassType.Class3) {
        return this.buildingInspectorRouter.navigateTo(this.applicationService.model, BuildingInspectorRoutes.CLASS3_ACCESSING_PLANS_CATEGORIES);
      }
    }

    if (this.applicationService.model.InspectorClass?.ClassType.Class === BuildingInspectorClassType.Class2) {
          // redirect to the Class 2 Inspection Categories once that page has been made
      return this.buildingInspectorRouter.navigateTo(this.applicationService.model, BuildingInspectorRoutes.CLASS2_INSPECT_BUILDING_CATEGORIES);
    }
          // redirect to the Class 3 Inspection Categories once that page has been made
      return this.buildingInspectorRouter.navigateTo(this.applicationService.model, BuildingInspectorRoutes.CLASS3_INSPECT_BUILDING_CATEGORIES);
  
  }

  optionClicked() {
  }

}

