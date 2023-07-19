import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService, ApplicationStatus, StringModel, ComponentCompletionState } from '../../../../services/application.service';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { BuildingInspectorCountryComponent } from '../country/building-inspector-country.component';
import { BuildingInspectorRoutes, BuildingInspectorRouter } from '../BuildingInspectorRoutes'; 
import { application } from 'express';

@Component({
  selector: 'hse-building-inspector-class-selection',
  templateUrl: './building-inspector-class-selection.component.html',
})
export class BuildingInspectorClassSelectionComponent extends PageComponent<string> {
  DerivedIsComplete(value: boolean): void {
    // this.applicationService.model.BuildingInspectorClass!.ClassSelection!.CompletionState = value ? ComponentCompletionState.Complete : ComponentCompletionState.InProgress;
  }

  public static route: string = BuildingInspectorRoutes.CLASS_SELECTION;
  static title: string = "Building inspector class - Register as a building inspector - GOV.UK";
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  selectedOption: string = "";
  selectedOptionError: boolean = false;
  errorMessage: string = "";

  constructor(activatedRoute: ActivatedRoute, applicationService: ApplicationService) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {
    // if the user visits this page for the first time, set status to in progress until user saves and continues
    if (!applicationService.model.BuildingInspectorClass?.ClassSelection) {
      applicationService.model.BuildingInspectorClass!.ClassSelection = { ClassType: '', CompletionState: ComponentCompletionState.InProgress };
    }

    this.model = applicationService.model.BuildingInspectorClass!.ClassSelection!.ClassType; 
    
    if (this.model) { this.selectedOption = this.model; }
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    if (this.selectedOption === "1") {
      this.applicationService.model.BuildingInspectorClass!.ClassSelection = { ClassType: this.selectedOption, CompletionState: ComponentCompletionState.Complete };
    }
    else {
      this.applicationService.model.BuildingInspectorClass!.ClassSelection = { ClassType: this.selectedOption, CompletionState: ComponentCompletionState.InProgress };
    }
  }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    if (this.applicationService.model.ApplicationStatus === ApplicationStatus.PersonalDetailsComplete) {
      return true;
    }

    return false;    
  }

  override isValid(): boolean {
    if (!this.selectedOption) {
      this.selectedOptionError = true;
      this.errorMessage = "Select a class of building inspector you are applying for";
      return false;
    }

    this.model = this.selectedOption;
    return true;
  }

  override navigateNext(): Promise<boolean> {
    if (this.selectedOption === "1") {
      return this.navigationService.navigateRelative(
        BuildingInspectorCountryComponent.route,
        this.activatedRoute
      );
    }
    else {
      return this.navigationService.navigateRelative(
        //TODO replace this route with activities page when completed
        BuildingInspectorCountryComponent.route,
        this.activatedRoute
      )
    }
  }
}