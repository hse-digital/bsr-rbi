import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { PageComponent } from 'src/app/helpers/page.component';
import { ApplicationService } from 'src/app/services/application.service';
import {
  BuildingInspectorRouter,
  BuildingInspectorRoutes,
} from '../BuildingInspectorRoutes';
import { environment } from 'src/environments/environment';
import { BuildingInspectorSummaryComponent } from '../building-inspector-summary/building-inspector-summary.component';

@Component({
  selector: 'hse-building-class-technical-manager',
  templateUrl: './building-class-technical-manager.component.html',
  styles: [],
})
export class BuildingClassTechnicalManagerComponent extends PageComponent<string> {
  public static route: string = BuildingInspectorRoutes.CLASS_TECHNICAL_MANAGER;
  public id: string = BuildingInspectorSummaryComponent.route;
  static title =
    'Building inspector class - Register as a building inspector - GOV.UK';
  production = environment.production;
  errorMessage: string = '';
  modelValid: boolean = false;
  selectedOption: string = 'no';

  constructor(
    activatedRoute: ActivatedRoute,
    private buildingInspectorRouter: BuildingInspectorRouter
  ) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
    this.updateOnSave = true;

    this.model = applicationService.model.InspectorClass?.ClassTechnicalManager;

    this.selectedOption = this.model ? this.model : 'no';

    this.applicationService = applicationService;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    this.applicationService.model.InspectorClass!.ClassTechnicalManager =
      this.model;

    /**
     * Below block of code condition: "if (['no', 'yes'].includes(this.selectedOption))"
     * is equal to condition: "if (this.selectedOption === 'no' || this.selectedOption === 'yes')""
     **/

    if (['no', 'yes'].includes(this.selectedOption)) {
      applicationService.model.InspectorClass!.ClassTechnicalManager =
        this.selectedOption;
    }
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return true;
  }

  override isValid(): boolean {
    this.hasErrors = false;

    if (this.selectedOption === '') {
      this.hasErrors = true;
      this.errorMessage = 'Select one option';
    }

    return !this.hasErrors;
  }

  override async navigateNext(): Promise<boolean> {
    return this.buildingInspectorRouter.navigateTo(
      this.applicationService.model,
      BuildingInspectorRoutes.INSPECTOR_COUNTRY
    );
  }

  override DerivedIsComplete(value: boolean): void {}
}
