import { Component, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { PageComponent } from 'src/app/helpers/page.component';
import {
  ApplicationService,
  ApplicationStatus,
  BuildingAssessingPlansCategories,
} from 'src/app/services/application.service';
import {
  BuildingInspectorRouter,
  BuildingInspectorRoutes,
} from '../BuildingInspectorRoutes';
import { environment } from 'src/environments/environment';
import { BuildingInspectorSummaryComponent } from '../building-inspector-summary/building-inspector-summary.component';

@Component({
  selector: 'hse-building-assessing-plans-categories',
  templateUrl: './building-assessing-plans-categories.component.html',
})
export class BuildingAssessingPlansCategoriesComponent extends PageComponent<BuildingAssessingPlansCategories> {
  public static route: string = BuildingInspectorRoutes.PLANS_CATEGARIES;
  public id: string = BuildingInspectorSummaryComponent.route;
  static title: string = 'Assessing Plans - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  public hint: string = 'Select all that apply';
  public errorText: string = '';

  override model?: BuildingAssessingPlansCategories;
  public selections: string[] = [];

  @Output() onClicked = new EventEmitter();
  @Output() onKeyupEnter = new EventEmitter();

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService,
    private buildingInspectorRouter: BuildingInspectorRouter
  ) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {}

  override async onSave(applicationService: ApplicationService): Promise<void> {
    applicationService.model.ApplicationStatus = ApplicationStatus.BuildingInspectorClassComplete;
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
    return this.navigationService.navigateRelative(BuildingInspectorSummaryComponent.route, this.activatedRoute);
  }
  
  override DerivedIsComplete(value: boolean): void {}

  optionClicked() {
   
  }
}
