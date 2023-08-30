import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { ApplicationService } from '../../../../services/application.service';
import { CompetencyRoutes } from '../CompetencyRoutes';
import { CompetencyAssessmentOrganisationComponent } from '../assesesment-organisation/competency-assesesment-organisation.component';
import { NoCompetencyAssessmentComponent } from '../no-competency-assessment/no-competency-assessment.component';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { CompetencyIndependentAssessmentStatus } from 'src/app/models/competency-independent-assessment-status.model';
import { CompetencyAssessmentOrganisation } from 'src/app/models/competency-assessment-organisation.model';
import { CompetencyAssessmentCertificateNumber } from 'src/app/models/competency-assessment-certificate-number.model';
import { CompetencyDateOfAssessment } from 'src/app/models/competency-date-of-assessment.model';
import { NoCompetencyAssessment } from 'src/app/models/no-competency-assessment.model';

@Component({
  selector: 'hse-independent-competency-status',
  templateUrl: './independent-competency-status.component.html',
})
export class CompetencyIndependentStatusComponent extends PageComponent<CompetencyIndependentAssessmentStatus> {
  public static route: string = CompetencyRoutes.INDEPENDENT_COMPETENCY_STATUS;
  static title: string =
    'Competency - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  errorMessage: string = '';
  selectedOption: string = '';
  queryParam?: string = '';

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService
  ) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
    this.activatedRoute.queryParams.subscribe((params) => {
      this.queryParam = params['queryParam'];
    });
    this.updateOnSave = true;

    if (!applicationService.model.Competency) {
      applicationService.model.Competency = {};
    }

    if (
      applicationService.model.Competency
        ?.CompetencyIndependentAssessmentStatus == null
    ) {
      applicationService.model.Competency!.CompetencyIndependentAssessmentStatus =
        new CompetencyIndependentAssessmentStatus();
      this.selectedOption = '';
    }

    this.selectedOption = applicationService.model.Competency
      ?.CompetencyIndependentAssessmentStatus
      ? applicationService.model.Competency
          ?.CompetencyIndependentAssessmentStatus.IAStatus!
      : 'no';
  }

  override async saveAndComeBack(): Promise<void> {
    this.processing = true;

    const STATUS =
      this.applicationService.model.Competency!
        .CompetencyIndependentAssessmentStatus!.CompletionState;

    const IASTATUS =
      this.applicationService.model.Competency!
        .CompetencyIndependentAssessmentStatus!.IAStatus;

    if (this.selectedOption === IASTATUS && STATUS === 2) {
      this.applicationService.model.Competency!.CompetencyIndependentAssessmentStatus!.CompletionState =
        ComponentCompletionState.Complete;
    } else {
      this.applicationService.model.Competency!.CompetencyIndependentAssessmentStatus!.CompletionState =
        ComponentCompletionState.InProgress;
    }

    this.applicationService.model.Competency!.CompetencyIndependentAssessmentStatus!.IAStatus =
      this.selectedOption;

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
    if (['no', 'yes'].includes(this.selectedOption)) {
      applicationService.model.Competency!.CompetencyIndependentAssessmentStatus!.IAStatus =
        this.selectedOption;
    }

    if (this.selectedOption === 'no') {
      applicationService.model.Competency!.CompetencyAssessmentOrganisation =
        new CompetencyAssessmentOrganisation();
      applicationService.model.Competency!.CompetencyAssessmentCertificateNumber =
        new CompetencyAssessmentCertificateNumber();
      applicationService.model.Competency!.CompetencyDateOfAssessment =
        new CompetencyDateOfAssessment();
      this.applicationService.model.Competency!.CompetencyIndependentAssessmentStatus!.CompletionState =
        ComponentCompletionState.InProgress;
    } else if (this.selectedOption === 'yes') {
      this.applicationService.model.Competency!.NoCompetencyAssessment =
        new NoCompetencyAssessment();
      this.applicationService.model.Competency!.CompetencyIndependentAssessmentStatus!.CompletionState =
        ComponentCompletionState.Complete;
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
    this.errorMessage = '';

    if (this.selectedOption === '') {
      this.hasErrors = true;
      this.errorMessage =
        'Select yes if you have completed a BSR-approved competency assessment';
    }

    return !this.hasErrors;
  }

  override navigateNext(): Promise<boolean> {
    if (
      this.queryParam != null &&
      this.queryParam != '' &&
      this.queryParam != undefined
    ) {
      const queryParam = this.queryParam;
      if (this.selectedOption === 'yes') {
        return this.navigationService.navigateRelative(
          CompetencyAssessmentOrganisationComponent.route,
          this.activatedRoute,
          {queryParam}
        );
      } else {
        return this.navigationService.navigateRelative(
          NoCompetencyAssessmentComponent.route,
          this.activatedRoute,
          {queryParam}
        );
      }
    } else {
      if (this.selectedOption === 'yes') {
        return this.navigationService.navigateRelative(
          CompetencyAssessmentOrganisationComponent.route,
          this.activatedRoute
        );
      } else {
        return this.navigationService.navigateRelative(
          NoCompetencyAssessmentComponent.route,
          this.activatedRoute
        );
      }
    }
  }
  DerivedIsComplete(value: boolean): void {
    this.applicationService.model.Competency!.CompetencyIndependentAssessmentStatus!.CompletionState =
      value
        ? ComponentCompletionState.Complete
        : ComponentCompletionState.InProgress;
  }
}
