import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService, ApplicationStatus, BuildingInspectorClassType } from '../../../../services/application.service';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { BuildingInspectorRoutes, BuildingInspectorRouter } from '../BuildingInspectorRoutes';

@Component({
  selector: 'hse-building-inspector-summary',
  templateUrl: './building-inspector-summary.component.html',
})
export class BuildingInspectorSummaryComponent extends PageComponent<string> {
  DerivedIsComplete(value: boolean): void {
    
  }

  BuildingInspectorRoutes = BuildingInspectorRoutes;
  BuildingInspectorClassType = BuildingInspectorClassType;

  public static route: string = BuildingInspectorRoutes.SUMMARY;
  static title: string = "Building inspector class - Register as a building inspector - GOV.UK";
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  override model?: string;

  assessPlansCategories: string = "";
  assessPlansLink: string = "";

  inspectionCategories: string = "";
  inspectionLink: string = "";

  constructor(activatedRoute: ActivatedRoute, applicationService: ApplicationService) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {
    //this.model = applicationService.model.personalDetails?.applicantPhoto?.toString() ?? '';

    if (applicationService.model.InspectorClass?.ClassType.Class === BuildingInspectorClassType.Class2)
    {
      this.assessPlansLink = BuildingInspectorRoutes.CLASS2_ACCESSING_PLANS_CATEGORIES;
      this.inspectionLink = BuildingInspectorRoutes.CLASS2_INSPECT_BUILDING_CATEGORIES;
      this.inspectionCategories = this.categoriesToString(applicationService.model.InspectorClass?.Class2InspectBuildingCategories)
      this.assessPlansCategories = this.categoriesToString(applicationService.model.InspectorClass?.BuildingAssessingPlansCategoriesClass2)
    }

    if (applicationService.model.InspectorClass?.ClassType.Class === BuildingInspectorClassType.Class3)
    {
      this.assessPlansLink = BuildingInspectorRoutes.CLASS3_ACCESSING_PLANS_CATEGORIES;
      this.inspectionLink = BuildingInspectorRoutes.CLASS3_INSPECT_BUILDING_CATEGORIES;
      
      this.inspectionCategories = this.categoriesToString(applicationService.model.InspectorClass?.Class3InspectBuildingCategories)
      this.assessPlansCategories = this.categoriesToString(applicationService.model.InspectorClass?.BuildingAssessingPlansCategoriesClass3)
    }
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    applicationService.model.ApplicationStatus = ApplicationStatus.BuildingInspectorClassComplete;
  }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return true;
    //! check if previous section is comeplete
    //return (FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.firstName) || FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.lastName));
  }


  override isValid(): boolean {
    return true;
/*     this.phoneNumberHasErrors = !PhoneNumberValidator.isValid(this.model?.toString() ?? '');
    return !this.phoneNumberHasErrors; */
  }

  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(`../${ApplicationTaskListComponent.route}`, this.activatedRoute);
  }

  public navigateTo(route: string) {
    return this.navigationService.navigateRelative(`${route}`, this.activatedRoute);
  }

  categoriesToString(categories: any): string {
    // loop through keys in object
    // if keys value is true, add the last character of the key name to string with a comma
    // when the loop is complete, remove the trailing comma and space
    let newString = "";

    Object.keys(categories).forEach(key => {
      if (categories[key] === true) {
        newString = newString + key.slice(-1) + ", ";
      }
    });

    return newString.slice(0, -2);
  }
}
