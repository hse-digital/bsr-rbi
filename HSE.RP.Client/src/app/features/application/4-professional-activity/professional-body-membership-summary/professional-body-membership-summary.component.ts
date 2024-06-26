import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../../services/application.service';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import {
  ApplicantProfessionBodyMemberships,
  ApplicantProfessionBodyMembershipsHelper,
} from '../../../../models/applicant-professional-body-membership';
import { ComponentCompletionState } from '../../../../models/component-completion-state.enum';
import { ProfessionalActivityRoutes } from '../../application-routes';
import { ProfessionalActivityEmploymentTypeComponent } from '../employment-type/professional-activity-employment-type.component';
import { ApplicationSummaryComponent } from '../../5-application-submission/application-summary/application-summary.component';
import { ProfessionalBodySelectionComponent } from '../professional-body-selection/professional-body-selection.component';
import { ProfessionalMembershipAndEmploymentSummaryComponent } from '../professional-membership-and-employment-summary/professional-membership-and-employment-summary.component';

@Component({
  selector: 'hse-professional-body-membership-summary',
  templateUrl: './professional-body-membership-summary.component.html',
})
export class ProfessionalBodyMembershipSummaryComponent extends PageComponent<ApplicantProfessionBodyMemberships> {
  static title: string =
    'Professional activity - Professional body membership summary - Register as a building inspector - GOV.UK';
  public static route: string = 'professional-body-membership-summary';
  //public static route: string = ProfessionalActivityRoutes.PROFESSIONAL_ACTIVITY_SUMMARY;
  readonly ComponentCompletionState = ComponentCompletionState;
  readonly ApplicantProfessionBodyMembershipsHelper =
    ApplicantProfessionBodyMembershipsHelper;
  production: boolean = environment.production;
  modelValid: boolean = false;
  summaryHasErrors = false;
  selectedOption: string = '';
  override model?: ApplicantProfessionBodyMemberships;
  errorMessage: string = '';
  queryParam?: string = '';

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService
  ) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {
    this.activatedRoute.queryParams.subscribe((params) => {
      this.queryParam = params['queryParam'];
    });
    this.model = applicationService.model.ProfessionalMemberships;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {

    if (this.model?.CompletionState == ComponentCompletionState.Complete && this.selectedOption == 'yes') {
      this.model.CompletionState = ComponentCompletionState.InProgress;
    }
    this.applicationService.model.ProfessionalMemberships = this.model!;
  }

  optionClicked(value: string) {
    this.selectedOption = value;
  }
  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return true;
    //return (FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.firstName) || FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.lastName));
  }

  override isValid(): boolean {
    this.summaryHasErrors = false;
    this.errorMessage = '';
    const memberships = this.applicationService.model.ProfessionalMemberships;

    if (memberships.RICS.CompletionState == ComponentCompletionState.Complete &&
      memberships.CABE.CompletionState == ComponentCompletionState.Complete &&
      memberships.CIOB.CompletionState == ComponentCompletionState.Complete &&
      memberships.OTHER.CompletionState == ComponentCompletionState.Complete) {
      this.summaryHasErrors = false;
    }
    else {
      if (this.selectedOption === '') {
        this.summaryHasErrors = true;
        this.errorMessage =
          'Select whether you want to tell us about additional memberships you hold or not';
      }
    }

    return !this.summaryHasErrors;

  }

  override navigateNext(): Promise<boolean> {
    if (
      this.queryParam != null &&
      this.queryParam != undefined &&
      this.queryParam != ''
    ) {
      const queryParam = this.queryParam;
      if (this.queryParam == 'application-summary') {
        if (this.selectedOption === 'no') {
          return this.navigationService.navigateRelative(
            `../application-submission/${ApplicationSummaryComponent.route}`,
            this.activatedRoute
          );
        }
        if (this.selectedOption === 'yes') {
          return this.navigationService.navigateRelative(
            ProfessionalBodySelectionComponent.route,
            this.activatedRoute,
            { queryParam: this.queryParam }
          );
        } else if (
          ApplicantProfessionBodyMembershipsHelper.AllCompleted(this.model!)
        ) {
          return this.navigationService.navigateRelative(
            `../application-submission/${ApplicationSummaryComponent.route}`,
            this.activatedRoute
          );
        }
      }
      else if (this.queryParam == 'professional-membership-and-employment-summary') {
        if (this.selectedOption === 'no') {
          return this.navigationService.navigateRelative(
            ProfessionalMembershipAndEmploymentSummaryComponent.route,
            this.activatedRoute
          );
        }
        if (this.selectedOption === 'yes') {
          return this.navigationService.navigateRelative(
            ProfessionalBodySelectionComponent.route,
            this.activatedRoute,
            { queryParam: this.queryParam }
          );
        } else if (
          ApplicantProfessionBodyMembershipsHelper.AllCompleted(this.model!)
        ) {
          return this.navigationService.navigateRelative(
            `../application-submission/${ApplicationSummaryComponent.route}`,
            this.activatedRoute
          );
        }
      }
    }

    if (this.selectedOption === 'no') {
      this.model!.CompletionState = ComponentCompletionState.Complete;
      return this.navigationService.navigateRelative(
        ProfessionalActivityEmploymentTypeComponent.route,
        this.activatedRoute
      );
    }
    if (this.selectedOption === 'yes') {

      return this.navigateTo('professional-body-selection'); // To professional body selection page.
    } else if (
      ApplicantProfessionBodyMembershipsHelper.AllCompleted(this.model!)
    ) {
      return this.navigationService.navigateRelative(
        ProfessionalActivityEmploymentTypeComponent.route,
        this.activatedRoute
      );
    }
    return this.navigateTo(`application/${this.applicationService.model.id}`); // Back to the task list.
  }

  public navigateTo(route: string) {
    return this.navigationService.navigateRelative(
      `${route}`,
      this.activatedRoute
    );
  }

  public navigateToChange(membershipCode: string) {
    return this.navigationService.navigateRelative(
      `professional-membership-information`,
      this.activatedRoute,
      { membershipCode: membershipCode, queryParam: this.queryParam }
    );
  }
  public navigateToRemove(membershipCode: string) {
    return this.navigationService.navigateRelative(
      `professional-confirmation-membership-removal`,
      this.activatedRoute,
      { membershipCode: membershipCode, queryParam: this.queryParam }
    );
  }
  public emptyActionText(): string {
    return '';
  }
  public changeActionText(): string {
    return 'change';
  }
  public removeActionText(): string {
    return 'remove';
  }

  async SyncAndContinue() {
    if (!this.processing) {
      this.processing = true;
      await this.applicationService.syncProfessionalBodyMemberships();
      this.saveAndContinue();
    }
  }

  canAddNewMembership(): boolean {
    const memberships = this.applicationService.model.ProfessionalMemberships;

    if (
      memberships.RICS.CompletionState === ComponentCompletionState.Complete &&
      memberships.CABE.CompletionState === ComponentCompletionState.Complete &&
      memberships.CIOB.CompletionState === ComponentCompletionState.Complete &&
      memberships.OTHER.CompletionState === ComponentCompletionState.Complete
    ) {
      return false;
    } else {
      return true;
    }
  }
}
