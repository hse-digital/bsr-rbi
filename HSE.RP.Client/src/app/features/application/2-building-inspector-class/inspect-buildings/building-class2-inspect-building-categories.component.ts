import { Component, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { PageComponent } from 'src/app/helpers/page.component';
import { ApplicationService } from 'src/app/services/application.service';
import { BuildingInspectorRoutes } from '../BuildingInspectorRoutes';
import { environment } from 'src/environments/environment';
import { BuildingInspectorSummaryComponent } from '../building-inspector-summary/building-inspector-summary.component';
import { Class2InspectBuildingCategories } from 'src/app/models/class2-inspect-building-categories.model';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { BuildingClassTechnicalManagerComponent } from '../class-technical-manager/building-class-technical-manager.component';

@Component({
  selector: 'hse-building-class2-inspect-building-categories',
  templateUrl: './building-class2-inspect-building-categories.component.html',
})
export class Class2InspectBuildingCategoriesComponent extends PageComponent<Class2InspectBuildingCategories> {
  public static route: string =
    BuildingInspectorRoutes.CLASS2_INSPECT_BUILDING_CATEGORIES;
  public id: string = BuildingInspectorSummaryComponent.route;
  static title =
    'Building inspector class - Class 2 inspect buildings categories - Register as a building inspector - GOV.UK';
  production = environment.production;
  modelValid = false;
  public hint = 'Select all that apply';
  public errorText = '';
  selectedOptionError: boolean = false;
  queryParam?: string = '';
  resetIA? : boolean = false;

  override model?: Class2InspectBuildingCategories;
  public selections: string[] = [];

  @Output() onClicked = new EventEmitter();
  @Output() onKeyupEnter = new EventEmitter();

  constructor(activatedRoute: ActivatedRoute) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
    this.activatedRoute.queryParams.subscribe((params) => {
      this.queryParam = params['queryParam'];
      this.resetIA = params['resetIA'] == 'true' ? true : false;

    });
    this.updateOnSave = true;

    this.model =
      applicationService.model.InspectorClass?.Class2InspectBuildingCategories;

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

    // this.applicationService.model.InspectorClass!.ClassType.CompletionState =
    //   ComponentCompletionState.InProgress;
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return true;
  }

  override isValid(): boolean {
    if (this.selections.length == 0) {
      this.selectedOptionError = true;
      this.errorText = 'Select a category';
    }
    return this.selections.length > 0;
  }

  override navigateNext(): Promise<boolean> {

    if (
      this.queryParam != null &&
      this.queryParam != undefined &&
      this.queryParam != ''
    ) {
      const queryParam = this.queryParam;

      return this.navigationService.navigateRelative(
        BuildingClassTechnicalManagerComponent.route,
        this.activatedRoute,
        { resetIA: this.resetIA, queryParam: queryParam }
      );
    }

    return this.navigationService.navigateRelative(
      BuildingClassTechnicalManagerComponent.route,
      this.activatedRoute
    );
  }

  DerivedIsComplete(value: boolean): void {
    this.DemandModel().CompletionState = value
      ? ComponentCompletionState.Complete
      : ComponentCompletionState.InProgress;
  }

  public DemandModel(): Class2InspectBuildingCategories {
    if (this.model === undefined || this.model === null) {
      throw new Error('Model is undefined');
    }
    return this.model;
  }

  optionClicked() {}
}
