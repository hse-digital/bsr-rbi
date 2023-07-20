import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService, ApplicationStatus, StringModel, BuidlingInspectorAssessingPlansClass3, BuildingInspectorClassType, BuildingInspectorClass, ComponentCompletionState } from '../../../../services/application.service';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { BuildingInspectorCountryComponent } from '../country/building-inspector-country.component';
import { BuildingInspectorRoutes, BuildingInspectorRouter } from '../BuildingInspectorRoutes'; 
import { SelectMultipleControlValueAccessor } from '@angular/forms';

interface AssessingPlans {
  value: string,
  hint: string
}

@Component({
  selector: 'hse-building-inspector-assessing-plans',
  templateUrl: './building-inspector-assessing-plans.component-class3.html',
})
export class BuildingInspectorAssessingPlansClass3Component extends PageComponent<BuidlingInspectorAssessingPlansClass3> {

  public static route: string = BuildingInspectorRoutes.ASSESSING_PLANS_CLASS_3;
  static title: string = "Building inspector class - Register as a building inspector - GOV.UK";
  production: boolean = environment.production;
  public hint: string = "Select all that apply";
  public errorText: string = "";
  public options: AssessingPlans[] = [
    {value: "CategoryA", hint: "Content to be finalised"},
    {value: "CategoryB", hint: "Content to be finalised"},
    {value: "CategoryC", hint: "Content to be finalised"},
    {value: "CategoryD", hint: "Content to be finalised"},
    {value: "CategoryE", hint: "Content to be finalised"},
    {value: "CategoryF", hint: "Content to be finalised"},
    {value: "CategoryG", hint: "Content to be finalised"},
    {value: "CategoryH", hint: "Content to be finalised"},
  ]

  override model?: BuidlingInspectorAssessingPlansClass3;
  public selections: string[] = [];


  constructor(activatedRoute: ActivatedRoute) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
    this.updateOnSave = true;
    if (applicationService.model?.InspectorClass)
      applicationService.model.InspectorClass.Class =
        BuildingInspectorClassType.ClassNone;
    if (!applicationService.model?.InspectorClass) {
      applicationService.model.InspectorClass = new BuildingInspectorClass();
    }
    if (!applicationService.model.InspectorClass.AssessingPlansClass3) {
      applicationService.model.InspectorClass.AssessingPlansClass3 =
        new BuidlingInspectorAssessingPlansClass3();
    }
    applicationService.model.InspectorClass!.Class =
      BuildingInspectorClassType.Class3;
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
      ...categoryKeys.filter((key) => demandModel[key] === true));
    this.applicationService = applicationService;
  }

  public DemandModel(): BuidlingInspectorAssessingPlansClass3 {
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
    this.selections.forEach((value: keyof typeof demandModel) => {
      demandModel[value] = true;
    });
  }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return true;
  }


  override isValid(): boolean {
    if (this.selections.length == 0)
      this.errorText = 'You must select at least one option';
    return this.selections.length > 0;
  }

  DerivedIsComplete(value: boolean): void {
    this.DemandModel().CompletionState = value
      ? ComponentCompletionState.Complete
      : ComponentCompletionState.InProgress;
  }

  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(BuildingInspectorCountryComponent.route, this.activatedRoute);
  }

}
