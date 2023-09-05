import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../../services/application.service';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { EmploymentTypeSelection } from 'src/app/models/employment-type-selection.model';
import { EmploymentType } from 'src/app/models/employment-type.enum';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { EmploymentOtherNameComponent } from '../employment-other-name/employment-other-name.component';
import { EmploymentPublicSectorBodyNameComponent } from '../employment-public-sector-body-name/employment-public-sector-body-name.component';
import { TaskListItemComponent } from 'src/app/components/task-list-item/task-list-item.component';
import { GovukTaskListComponent } from 'hse-angular';
import { EmploymentPrivateSectorBodyNameComponent } from '../employment-private-sector-body-name/employment-private-sector-body-name.component';
import { ProfessionalMembershipAndEmploymentSummaryComponent } from '../professional-membership-and-employment-summary/professional-membership-and-employment-summary.component';

@Component({
  selector: 'hse-professional-activity-employment-type',
  templateUrl: './professional-activity-employment-type.component.html',
})
export class ProfessionalActivityEmploymentTypeComponent extends PageComponent<EmploymentTypeSelection> {
  DerivedIsComplete(value: boolean): void {}

  public static route: string = 'professional-activity-employment-type';
  static title: string =
    'Professional activity - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  // override model?: EmploymentType;
  selectedOptionError: boolean = false;
  errorMessage: string = '';
  EmploymentType = EmploymentType;
  existingEmploymentType?: EmploymentType =
    this.applicationService.model.ProfessionalActivity.EmploymentDetails
      ?.EmploymentTypeSelection?.EmploymentType;
  queryParam?: string = '';

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService
  ) {
    super(activatedRoute);
    this.updateOnSave = true;
  }

  override onInit(applicationService: ApplicationService): void {
    this.activatedRoute.queryParams.subscribe((params) => {
      this.queryParam = params['queryParam'];
    });

    this.model =
      applicationService.model.ProfessionalActivity.EmploymentDetails?.EmploymentTypeSelection;
    // if the user visits this page for the first time, set status to in progress until user saves and continues
    if (
      applicationService.model.ProfessionalActivity.EmploymentDetails
        ?.EmploymentTypeSelection?.EmploymentType === EmploymentType.None
    ) {
      applicationService.model.ProfessionalActivity.EmploymentDetails.EmploymentTypeSelection =
        {
          EmploymentType: EmploymentType.None,
          CompletionState: ComponentCompletionState.InProgress,
        };
    }
    if (
      applicationService.model.ProfessionalActivity.EmploymentDetails
        ?.CompletionState != ComponentCompletionState.Complete
    ) {
      applicationService.model.ProfessionalActivity.EmploymentDetails!.CompletionState =
        ComponentCompletionState.InProgress;
    }
  }

  override async saveAndComeBack(): Promise<void> {
    this.processing = true;

    const status =
      this.applicationService.model.ProfessionalActivity.EmploymentDetails
        ?.CompletionState;

    if (
      this.model?.EmploymentType === this.existingEmploymentType &&
      status === ComponentCompletionState.Complete
    ) {
      this.applicationService.model.ProfessionalActivity.EmploymentDetails!.CompletionState =
        ComponentCompletionState.Complete;
    } else {
      this.applicationService.model.ProfessionalActivity.EmploymentDetails!.CompletionState =
        ComponentCompletionState.InProgress;
    }

    this.applicationService.model.ProfessionalActivity.EmploymentDetails!.EmploymentTypeSelection!.EmploymentType =
      this.model?.EmploymentType;

    if (!this.hasErrors) {
      this.triggerScreenReaderNotification();
      this.applicationService.updateLocalStorage();
      await this.applicationService.updateApplication();
    } else {
      this.focusAndUpdateErrors();
    }

    this.processing = false;

    const taskListRoute: string = `application/${this.applicationService.model.id}`;
    this.navigationService.navigate(taskListRoute);
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    applicationService.model.ProfessionalActivity.EmploymentDetails!.EmploymentTypeSelection =
      this.model;

    const status =
      this.applicationService.model.ProfessionalActivity.EmploymentDetails
        ?.CompletionState;

    if (
      this.model?.EmploymentType === this.existingEmploymentType &&
      status === ComponentCompletionState.Complete
    ) {
      this.applicationService.model.ProfessionalActivity.EmploymentDetails!.CompletionState =
        ComponentCompletionState.Complete;
    } else {
      this.applicationService.model.ProfessionalActivity.EmploymentDetails!.CompletionState =
        ComponentCompletionState.InProgress;
    }
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return true;
    //return (FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.firstName) || FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.lastName));
  }

  override isValid(): boolean {
    if (!this.model?.EmploymentType) {
      this.selectedOptionError = true;
      this.errorMessage = 'Select your current employment status';
      return false;
    }

    return true;
  }

  override navigateNext(): Promise<boolean> {
    if (
      this.queryParam != null &&
      this.queryParam != undefined &&
      this.queryParam != ''
    ) {
      const queryParam = this.queryParam;

      if (this.queryParam == 'application-summary') {
        if (this.model?.EmploymentType == EmploymentType.Other) {
          return this.navigationService.navigateRelative(
            EmploymentOtherNameComponent.route,
            this.activatedRoute,
            { queryParam: this.queryParam }
          );
        }
        if (this.model?.EmploymentType == EmploymentType.PublicSector) {
          return this.navigationService.navigateRelative(
            EmploymentPublicSectorBodyNameComponent.route,
            this.activatedRoute,
            { queryParam: this.queryParam }
          );
        }
        if (this.model?.EmploymentType == EmploymentType.PrivateSector) {
          return this.navigationService.navigateRelative(
            EmploymentPrivateSectorBodyNameComponent.route,
            this.activatedRoute,
            { queryParam: this.queryParam }
          );
        }
        if (this.model?.EmploymentType == EmploymentType.Unemployed) {
          this.applicationService.model.ProfessionalActivity.EmploymentDetails!.CompletionState = ComponentCompletionState.Complete
          return this.navigationService.navigateRelative(
            ProfessionalMembershipAndEmploymentSummaryComponent.route,
            this.activatedRoute,
            { queryParam: this.queryParam }
          );
        } else {
          return this.navigationService.navigate(
            `application/${this.applicationService.model.id}`
          );
        }
      }
    }

    if (this.model?.EmploymentType == EmploymentType.Other) {
      return this.navigationService.navigateRelative(
        EmploymentOtherNameComponent.route,
        this.activatedRoute
      );
    }
    if (this.model?.EmploymentType == EmploymentType.PublicSector) {
      return this.navigationService.navigateRelative(
        EmploymentPublicSectorBodyNameComponent.route,
        this.activatedRoute
      );
    }
    if (this.model?.EmploymentType == EmploymentType.PrivateSector) {
      return this.navigationService.navigateRelative(
        EmploymentPrivateSectorBodyNameComponent.route,
        this.activatedRoute
      );
    }
    if (this.model?.EmploymentType == EmploymentType.Unemployed) {
      this.applicationService.model.ProfessionalActivity.EmploymentDetails!.CompletionState = ComponentCompletionState.Complete
      return this.navigationService.navigateRelative(
        ProfessionalMembershipAndEmploymentSummaryComponent.route,
        this.activatedRoute
      );
    } else {
      return this.navigationService.navigate(
        `application/${this.applicationService.model.id}`
      );
    }
  }
}
