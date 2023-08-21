import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../../services/application.service';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { ProfessionalActivityEmploymentDetailsComponent } from '../employment-details/professional-activity-employment-details.component';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { EmploymentTypeSelection } from 'src/app/models/employment-type-selection.model';
import { EmploymentType } from 'src/app/models/employment-type.enum';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { EmploymentOtherNameComponent } from '../employment-other-name/employment-other-name.component';
import { EmploymentPublicSectorBodyNameComponent } from '../employment-public-sector-body-name/employment-public-sector-body-name.component';
import { TaskListItemComponent } from 'src/app/components/task-list-item/task-list-item.component';
import { GovukTaskListComponent } from 'hse-angular';


@Component({
  selector: 'hse-professional-activity-employment-type',
  templateUrl: './professional-activity-employment-type.component.html',
})
export class ProfessionalActivityEmploymentTypeComponent extends PageComponent<EmploymentTypeSelection> {
  DerivedIsComplete(value: boolean): void {

  }

  public static route: string = "professional-activity-employment-type";
  static title: string = "Professional activity - Register as a building inspector - GOV.UK";
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  // override model?: EmploymentType;
  selectedOptionError: boolean = false;
  errorMessage: string = "";
  EmploymentType = EmploymentType;

  constructor(activatedRoute: ActivatedRoute, applicationService: ApplicationService) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {

    this.model = applicationService.model.ProfessionalActivity.EmploymentDetails?.EmploymentTypeSelection;
    // if the user visits this page for the first time, set status to in progress until user saves and continues
    if (applicationService.model.ProfessionalActivity.EmploymentDetails?.EmploymentTypeSelection?.EmploymentType === EmploymentType.None) {
      applicationService.model.ProfessionalActivity.EmploymentDetails.EmploymentTypeSelection = { EmploymentType: EmploymentType.None, CompletionState: ComponentCompletionState.InProgress };
    }
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    applicationService.model.ProfessionalActivity.EmploymentDetails!.EmploymentTypeSelection = this.model;
  }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return true;
    //return (FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.firstName) || FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.lastName));

  }


  override isValid(): boolean {
    if (!this.model?.EmploymentType) {
      this.selectedOptionError = true;
      this.errorMessage = "Select your current employment status";
      return false;
    }

    return true;
  }

  override navigateNext(): Promise<boolean> {

    if(this.model?.EmploymentType == EmploymentType.Other)
    {
      return this.navigationService.navigateRelative(EmploymentOtherNameComponent.route, this.activatedRoute);
    }
    if(this.model?.EmploymentType == EmploymentType.PublicSector)
    {
      return this.navigationService.navigateRelative(EmploymentPublicSectorBodyNameComponent.route, this.activatedRoute);
    }
    if(this.model?.EmploymentType == EmploymentType.PrivateSector)
    {
      return this.navigationService.navigateRelative(ProfessionalActivityEmploymentTypeComponent.route, this.activatedRoute);
    }
    else{
      return this.navigationService.navigateRelative(ApplicationTaskListComponent.route, this.activatedRoute); //TODO update to summary
    }

  }

}
