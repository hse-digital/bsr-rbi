import { Component, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { PageComponent } from 'src/app/helpers/page.component';
import {
  ApplicationService,
  BuildingAssessingPlansCategoriesClass2,
  BuildingInspectorClass,
  BuildingInspectorClassType,
  Class3InspectBuildingCategories,
  ClassSelection,
  ComponentCompletionState,
} from 'src/app/services/application.service';
import { BuildingInspectorRoutes } from '../BuildingInspectorRoutes';
import { environment } from 'src/environments/environment';
import { BuildingInspectorSummaryComponent } from '../building-inspector-summary/building-inspector-summary.component';

@Component({
  selector: 'hse-building-class3-inspect-building-categories',
  templateUrl: './building-class3-inspect-building-categories.component.html',
})
export class Class3InspectBuildingCategoriesComponent extends PageComponent<Class3InspectBuildingCategories> {
  public static route: string = BuildingInspectorRoutes.CLASS3_INSPECT_BUILDING_CATEGORIES;
  public id: string = BuildingInspectorSummaryComponent.route;
  static title =
    'Building inspector class - Register as a building inspector - GOV.UK';
  production = environment.production;
  modelValid = false;
  photoHasErrors = false;
  public hint = 'Select all that apply';
  public errorText = '';

  override model?: Class3InspectBuildingCategories;
  public selections: string[] = [];

  @Output() onClicked = new EventEmitter();
  @Output() onKeyupEnter = new EventEmitter();

  constructor(activatedRoute: ActivatedRoute) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
    this.updateOnSave = true;

    if (applicationService.model?.InspectorClass)
      applicationService.model.InspectorClass.ClassType = new ClassSelection();
    if (!applicationService.model?.InspectorClass) {
      applicationService.model.InspectorClass = new BuildingInspectorClass();
    }

    if (!applicationService.model.InspectorClass.Class3InspectBuildingCategories) {
      applicationService.model.InspectorClass.Class3InspectBuildingCategories =
        new Class3InspectBuildingCategories();
    }

    this.model = applicationService.model.InspectorClass?.Class3InspectBuildingCategories;

    const demandModel = this.DemandModel();
    const categoryKeys = [
      'CategoryA',
      'CategoryB',
      'CategoryC',
      'CategoryD',
      'CategoryE',
      'CategoryF',
      'CategoryG',
      'CategoryH',
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
    demandModel.CategoryG = false;
    demandModel.CategoryH = false;

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
    return this.navigationService.navigateRelative(
      BuildingInspectorSummaryComponent.route,
      this.activatedRoute
    );
  }

  DerivedIsComplete(value: boolean): void {
    this.DemandModel().CompletionState = value
      ? ComponentCompletionState.Complete
      : ComponentCompletionState.InProgress;
  }

  public DemandModel(): Class3InspectBuildingCategories {
    if (this.model === undefined || this.model === null) {
      throw new Error('Model is undefined');
    }
    return this.model;
  }

  optionClicked() {}
}
