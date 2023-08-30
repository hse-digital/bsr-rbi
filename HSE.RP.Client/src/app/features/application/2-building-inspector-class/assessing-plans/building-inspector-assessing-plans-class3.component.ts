import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../../services/application.service';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { BuildingInspectorCountryComponent } from '../country/building-inspector-country.component';
import {
  BuildingInspectorRoutes,
  BuildingInspectorRouter,
} from '../BuildingInspectorRoutes';
import { SelectMultipleControlValueAccessor } from '@angular/forms';
import { BuildingAssessingPlansCategoriesClass3 } from 'src/app/models/buidling-assessing-plans-categories-class3.model';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';

interface AssessingPlans {
  value: string;
  text: string;
  hint: string;
}

@Component({
  selector: 'hse-building-inspector-assessing-plans',
  templateUrl: './building-inspector-assessing-plans.component-class3.html',
})
export class BuildingInspectorAssessingPlansClass3Component extends PageComponent<BuildingAssessingPlansCategoriesClass3> {
  public static route: string =
    BuildingInspectorRoutes.CLASS3_ACCESSING_PLANS_CATEGORIES;
  static title: string =
    'Building inspector class - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  public hint: string = 'Select all that apply';
  errors: boolean = false;
  public errorText: string = '';
  public options: AssessingPlans[] = [
    {
      value: 'CategoryA',
      text: 'Category A',
      hint: 'Residential dwelling houses (single household) less than 7.5m in height',
    },
    {
      value: 'CategoryB',
      text: 'Category B',
      hint: 'Residential flats and dwelling houses, less than 11m in height',
    },
    {
      value: 'CategoryC',
      text: 'Category C',
      hint: 'Residential flats and dwelling houses, 11m or more but less than 18m in height',
    },
    {
      value: 'CategoryD',
      text: 'Category D',
      hint: 'All building types and uses less than 7.5m in height',
    },
    {
      value: 'CategoryE',
      text: 'Category E',
      hint: 'All building types 7.5m or more but less than 11m in height',
    },
    {
      value: 'CategoryF',
      text: 'Category F',
      hint: 'All building types 11m or more but less than 18m in height',
    },
    {
      value: 'CategoryG',
      text: 'Category G',
      hint: 'All building types, including standard and non-standard but excluding high-risk, with no height limit',
    },
    {
      value: 'CategoryH',
      text: 'Category H',
      hint: 'All building types, including high-risk',
    },
  ];

  override model?: BuildingAssessingPlansCategoriesClass3;
  public selections: string[] = [];
  queryParam?: string = '';

  constructor(
    activatedRoute: ActivatedRoute,
    private buildingInspectorRouter: BuildingInspectorRouter
  ) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
    this.activatedRoute.queryParams.subscribe((params) => {
      this.queryParam = params['queryParam'];
    });
    this.updateOnSave = true;
    this.model = applicationService.model.InspectorClass?.AssessingPlansClass3;
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

  public DemandModel(): BuildingAssessingPlansCategoriesClass3 {
    if (this.model === undefined || this.model === null) {
      throw new Error('Model is undefined');
    }
    return this.model;
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
    if (this.selections.length == 0) {
      this.errorText = 'Select the categories you are applying for';
      this.errors = true;
    } else {
      this.errors = false;
    }
    return this.selections.length > 0;
  }

  DerivedIsComplete(value: boolean): void {
    this.DemandModel().CompletionState = value
      ? ComponentCompletionState.Complete
      : ComponentCompletionState.InProgress;
  }

  override navigateNext(): Promise<boolean> {
    if (
      this.applicationService.model.InspectorClass?.Activities.Inspection ===
        true &&
      this.applicationService.model.InspectorClass?.Activities
        .AssessingPlans === true
    ) {

      if (
        this.queryParam != null &&
        this.queryParam != undefined &&
        this.queryParam != ''
      ) {
        const queryParam = this.queryParam;

        return this.navigationService.navigateRelative(
          BuildingInspectorRoutes.CLASS3_INSPECT_BUILDING_CATEGORIES,
          this.activatedRoute,
          { queryParam }
        );
      }


      return this.buildingInspectorRouter.navigateTo(
        this.applicationService.model,
        BuildingInspectorRoutes.CLASS3_INSPECT_BUILDING_CATEGORIES
      );
    }
    // redirect to the Class 3 Inspection Categories once that page has been made

    if (
      this.queryParam != null &&
      this.queryParam != undefined &&
      this.queryParam != ''
    ) {
      const queryParam = this.queryParam;

      return this.navigationService.navigateRelative(
        BuildingInspectorRoutes.CLASS_TECHNICAL_MANAGER,
        this.activatedRoute,
        { queryParam }
      );

    }
    return this.buildingInspectorRouter.navigateTo(
      this.applicationService.model,
      BuildingInspectorRoutes.CLASS_TECHNICAL_MANAGER
    );
  }
}
