import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService, ApplicationStatus, StringModel, ComponentCompletionState, BuildingInspectorRegulatedActivies, ClassSelection, BuildingInspectorClassType, BuildingInspectorClass } from '../../../../services/application.service';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { BuildingInspectorCountryComponent } from '../country/building-inspector-country.component';
import { application } from 'express';
import { BuildingInspectorRegulatedActivitiesComponent } from '../regulated-activities/building-inspector-regulated-activities.component';
import { BuildingInspectorRoutes } from '../BuildingInspectorRoutes';

@Component({
  selector: 'hse-building-inspector-class-selection',
  templateUrl: './building-inspector-class-selection.component.html',
})
export class BuildingInspectorClassSelectionComponent extends PageComponent<ClassSelection> {
  DerivedIsComplete(value: boolean): void {
    // this.applicationService.model.BuildingInspectorClass!.ClassSelection!.CompletionState = value ? ComponentCompletionState.Complete : ComponentCompletionState.InProgress;
  }

  public static route: string = BuildingInspectorRoutes.CLASS_SELECTION;
  static title: string = "Building inspector class - Register as a building inspector - GOV.UK";
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  BuildingInspectorClassType = BuildingInspectorClassType;
  selectedOption: BuildingInspectorClassType = BuildingInspectorClassType.ClassNone;
  testSelect = BuildingInspectorClassType;
  selectedOptionError: boolean = false;
  errorMessage: string = "";
  override model?: ClassSelection;

  constructor(activatedRoute: ActivatedRoute, applicationService: ApplicationService) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {
    this.model = applicationService.model.InspectorClass?.ClassType
    // if the user visits this page for the first time, set status to in progress until user saves and continues
    if (applicationService.model.InspectorClass?.ClassType.Class === BuildingInspectorClassType.ClassNone) {
      applicationService.model.InspectorClass!.ClassType = { Class: BuildingInspectorClassType.ClassNone, CompletionState: ComponentCompletionState.InProgress };
    }
    
    if (this.model) { this.selectedOption = this.model.Class!; }
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    console.log(applicationService.model)
    if (this.selectedOption === BuildingInspectorClassType.Class1) {      
      this.applicationService.model.InspectorClass!.ClassType = { Class: this.selectedOption, CompletionState: ComponentCompletionState.Complete };
    }
    else {
      this.applicationService.model.InspectorClass!.ClassType = { Class: this.selectedOption, CompletionState: ComponentCompletionState.InProgress };
    }
  }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    // if (this.applicationService.model.ApplicationStatus === ApplicationStatus.PersonalDetailsComplete) {
    //   return true;
    // }

    // return false;    
    return true
  }

  override isValid(): boolean {
    if (!this.selectedOption) {
      this.selectedOptionError = true;
      this.errorMessage = "Select a class of building inspector you are applying for";
      return false;
    }

    this.model!.Class = this.selectedOption;
    return true;
  }

  override navigateNext(): Promise<boolean> {
    if (this.selectedOption === BuildingInspectorClassType.Class1) {
      return this.navigationService.navigateRelative(
        BuildingInspectorCountryComponent.route,
        this.activatedRoute
      );
    }
    else {
      return this.navigationService.navigateRelative(
        BuildingInspectorRegulatedActivitiesComponent.route,
        this.activatedRoute
      )
    }
  }
}