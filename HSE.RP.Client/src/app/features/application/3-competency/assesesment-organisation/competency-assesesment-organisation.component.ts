import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../../services/application.service';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { CompetencyAssessmentDateComponent } from '../assesesment-date/competency-assesesment-date.component';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { CompetencyRoutes } from '../CompetencyRoutes';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';

@Component({
  selector: 'hse-competency-assesesment-organisation',
  templateUrl: './competency-assesesment-organisation.component.html',
})
export class CompetencyAssessmentOrganisationComponent extends PageComponent<string> {
  public static route: string =
    CompetencyRoutes.COMPETENCY_ASSESSMENT_ORGANISATION;
  static title: string =
    'Competency - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  override model?: string;
  errorMessage: string = '';
  selectedOption: string = 'no';

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService
  ) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
    this.updateOnSave = true;
    this.model =
      applicationService.model.Competency?.CompetencyAssesesmentOrganisation;

    this.selectedOption = this.model ? this.model : 'no';

    this.applicationService = applicationService;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    this.applicationService.model.Competency!.CompetencyAssesesmentOrganisation =
      this.model;

    if (['no', 'yes'].includes(this.selectedOption)) {
      applicationService.model.Competency!.CompetencyAssesesmentOrganisation =
        this.selectedOption;
    }

    applicationService.model.ApplicationStatus =
      ApplicationStatus.CompetencyComplete;
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return true;
  }

  override isValid(): boolean {
    return true;
  }

  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(
      CompetencyAssessmentDateComponent.route,
      this.activatedRoute
    );
  }

  DerivedIsComplete(value: boolean): void {
    this.applicationService.model.Competency!.CompletionState = value
      ? ComponentCompletionState.Complete
      : ComponentCompletionState.InProgress;
  }
}
