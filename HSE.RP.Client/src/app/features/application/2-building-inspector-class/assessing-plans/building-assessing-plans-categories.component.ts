import { Component, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { PageComponent } from 'src/app/helpers/page.component';
import {
  ApplicationService,
  BuildingAssessingPlansCategories,
  BuildingInspectorClass,
  BuildingInspectorClassType,
  ComponentCompletionState,
} from 'src/app/services/application.service';
import { BuildingInspectorRouter, BuildingInspectorRoutes } from '../BuildingInspectorRoutes';
import { environment } from 'src/environments/environment';
import { BuildingClassTechnicalManagerComponent } from '../class-technical-manager/building-class-technical-manager.component';

@Component({
  selector: 'hse-building-assessing-plans-categories',
  templateUrl: './building-assessing-plans-categories.component.html',
})
export class BuildingAssessingPlansCategoriesComponent extends PageComponent<BuildingAssessingPlansCategories> {
  public static route: string = BuildingInspectorRoutes.PLANS_CATEGORIES;
  public id: string = BuildingClassTechnicalManagerComponent.route;
  static title =
    'Building inspector class - Register as a building inspector - GOV.UK';
  production = environment.production;
  modelValid = false;
  photoHasErrors = false;
  public hint = 'Select all that apply';
  public errorText = '';

  override model?: BuildingAssessingPlansCategories;
  public selections: string[] = [];

  @Output() onClicked = new EventEmitter();
  @Output() onKeyupEnter = new EventEmitter();

  constructor(activatedRoute: ActivatedRoute, private buildingInspectorRouter : BuildingInspectorRouter) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
    this.updateOnSave = true;
    if (!applicationService.model?.InspectorClass) {
      applicationService.model.InspectorClass = new BuildingInspectorClass();
    }

    if (!applicationService.model.InspectorClass.BuildingPlanCategories) {
      applicationService.model.InspectorClass.BuildingPlanCategories =
        new BuildingAssessingPlansCategories();
    }

    applicationService.model.InspectorClass!.ClassType.Class =
      BuildingInspectorClassType.Class2;
    this.model = applicationService.model.InspectorClass?.BuildingPlanCategories;

    const demandModel = this.DemandModel();
    const categoryKeys = [
      'CategoryA',
      'CategoryB',
      'CategoryC',
      'CategoryD',
      'CategoryE',
      'CategoryF',
    ];

    this.selections.push(
      ...categoryKeys.filter((key) => demandModel[key] === true)
    );

    this.applicationService = applicationService;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    const demandModel = this.DemandModel();

    demandModel.CategoryA = false;
    demandModel.CategoryB = false;
    demandModel.CategoryC = false;
    demandModel.CategoryD = false;
    demandModel.CategoryE = false;
    demandModel.CategoryF = false;

    this.selections.forEach((value: keyof typeof demandModel) => {
      demandModel[value] = true;
    });
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return true;
  }

  override isValid(): boolean {
    if (this.selections.length == 0)
      this.errorText = 'You must select at least one option';
    return this.selections.length > 0;
  }

  override navigateNext(): Promise<boolean> {
    if (this.applicationService.model.InspectorClass?.Activities.Inspection === false && this.applicationService.model.InspectorClass?.Activities.AssessingPlans === true) {
      return this.buildingInspectorRouter.navigateTo(this.applicationService.model, BuildingInspectorRoutes.CLASS_TECHNICAL_MANAGER);
    }
    // redirect to the Class 2 Inspection Categories once that page has been made
    return this.buildingInspectorRouter.navigateTo(this.applicationService.model, BuildingInspectorRoutes.SUMMARY);
  }

  DerivedIsComplete(value: boolean): void {
    this.DemandModel().CompletionState = value
      ? ComponentCompletionState.Complete
      : ComponentCompletionState.InProgress;
  }

  public DemandModel(): BuildingAssessingPlansCategories {
    if (this.model === undefined || this.model === null) {
      throw new Error('Model is undefined');
    }
    return this.model;
  }

  optionClicked() {}
}
