import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService, ApplicationStatus } from '../../../../services/application.service';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { BuildingInspectorCountryComponent } from '../country/building-inspector-country.component';
import { application } from 'express';

@Component({
  selector: 'hse-building-inspector-class-selection',
  templateUrl: './building-inspector-class-selection.component.html',
})
export class BuildingInspectorClassSelectionComponent extends PageComponent<string> {
  public static route: string = "building-inspector-class-selection";
  static title: string = "Building inspector class - Register as a building inspector - GOV.UK";
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  override model?: string;
  selectedOption?: string = "";
  selectedOptionError: boolean = false;
  errorMessage: string = "";

  constructor(activatedRoute: ActivatedRoute, applicationService: ApplicationService) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {
    //this.model = applicationService.model.personalDetails?.applicantPhoto?.toString() ?? '';
    console.log(applicationService.model)
    console.log(applicationService)
    // this.model = applicationService.model.BuildingInspectorClass.ClassSelection; 
    
    if (this.model === "1") {
      this.selectedOption = "1"
    }
    if (this.model === "2") {
      this.selectedOption = "2"
    }
    if (this.model === "3") {
      this.selectedOption = "3"
    }
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    this.model = applicationService.model.BuildingInspectorClass!.ClassSelection;
    console.log(this.model)

    // applicationService.model.ApplicationStatus = ApplicationStatus.BuildingInspectorClassComplete;
  }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return true;
    //! uncomment this when done, but check if it is the correct way first
    //return (FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.firstName) || FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.lastName));
  }

  override isValid(): boolean {
    if (!this.selectedOption) {
      this.selectedOptionError = true;
      this.errorMessage = "Select a class of building inspector you are applying for";
      return false;
    }

    return true;
  }

  override navigateNext(): Promise<boolean> {
    console.log(this.model)
    // if (this.selectedOption === "1") {
      return this.navigationService.navigateRelative(
        BuildingInspectorCountryComponent.route,
        this.activatedRoute
      );
    //}

    //TODO the next page is missing so will have to create a shell for it then do navigation
    // if (this.selectedOption === "2" || "3"){
    //   return this.navigationService.navigateRelative(
    //     BuildingInspect
    //   )
    // }
  }
}