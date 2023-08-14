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

  constructor(activatedRoute: ActivatedRoute) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
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

  override async onSave(applicationService: ApplicationService): Promise<void> {
    if (['no', 'yes'].includes(this.selectedOption)) {
      applicationService.model.Competency!.CompetencyIndependentAssessmentStatus!.IAStatus =
        this.selectedOption;
    }

    if (this.selectedOption === 'no') {
      applicationService.model.Competency!.CompetencyIndependentAssessmentStatus!.CompletionState =
        ComponentCompletionState.InProgress;
    } else if (this.selectedOption === 'yes') {
      applicationService.model.Competency!.CompetencyIndependentAssessmentStatus!.CompletionState =
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
    if (this.selectedOption === 'yes') {
      return this.navigationService.navigateRelative(
        CompetencyAssessmentOrganisationComponent.route,
        this.activatedRoute
      );
    } else {
      // TODO - needs changing to US-9033 when complete
      return this.navigationService.navigateRelative(
        NoCompetencyAssessmentComponent.route,
        this.activatedRoute
      );
    }
  }

  DerivedIsComplete(value: boolean): void {
    this.applicationService.model.Competency!.CompetencyIndependentAssessmentStatus!.CompletionState =
      value
        ? ComponentCompletionState.Complete
        : ComponentCompletionState.InProgress;
  }
}
