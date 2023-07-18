//BuildingInspectorRegulatedActivies
import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService, ApplicationStatus, BuildingInspectorClass, BuildingInspectorRegulatedActivies } from '../../../../services/application.service';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { BuildingInspectorSummaryComponent } from '../building-inspector-summary/building-inspector-summary.component';
import { BuildingInspectorRoutes, BuildingInspectorRouter } from '../BuildingInspectorRoutes'; 

@Component({
  selector: 'hse-building-inspector-regulated-activities',
  templateUrl: './building-inspector-regulated-activities.component.html',
})
export class BuildingInspectorRegulatedActivitiesComponent extends PageComponent<BuildingInspectorRegulatedActivies> {

  public static route: string = "building-inspector-country";
  public id: string = BuildingInspectorSummaryComponent.route;
  static title: string = "Building inspector class - Register as a building inspector - GOV.UK";
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  override model?: BuildingInspectorRegulatedActivies;

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService,
    private buildingInspectorRouter : BuildingInspectorRouter) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {
    if (!applicationService.model?.InspectorClass) {
      applicationService.model.InspectorClass = new BuildingInspectorClass();
    }
    if (!applicationService.model.InspectorClass.Activities) {
      applicationService.model.InspectorClass.Activities = new BuildingInspectorRegulatedActivies();
    }
    this.model = applicationService.model.InspectorClass?.Activities;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
  //  applicationService.model.ApplicationStatus = ApplicationStatus.BuildingInspectorClassComplete;
  }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return true;
    //return (FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.firstName) || FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.lastName));

  }


  override isValid(): boolean {
    var validityState: boolean = false;
    if (this.model !== undefined) {
      if (this.model.AssessingPlans !== undefined && this.model.AssessingPlans === true) {
        validityState = true;
      }
      if (this.model.Inspection !== undefined && this.model.Inspection === true) {
        validityState = true;
      }
    }


    return validityState;
  }

  override navigateNext(): Promise<boolean> {
    return this.buildingInspectorRouter.navigateTo(this.applicationService.model, BuildingInspectorRoutes.SUMMARY);
  }
}

