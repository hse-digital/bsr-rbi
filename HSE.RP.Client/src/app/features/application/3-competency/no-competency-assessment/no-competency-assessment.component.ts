import { Component, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { PageComponent } from 'src/app/helpers/page.component';
import { ApplicationService } from 'src/app/services/application.service';
import { CompetencyRoutes } from '../CompetencyRoutes';
import { environment } from 'src/environments/environment';
import { NoCompetencyAssessment } from 'src/app/models/no-competency-assessment.model';
import { CompetencySummaryComponent } from '../competency-summary/competency-summary.component';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';

@Component({
  selector: 'hse-no-competency-assessment',
  templateUrl: './no-competency-assessment.component.html',
  styles: [],
})
export class NoCompetencyAssessmentComponent extends PageComponent<NoCompetencyAssessment> {
  public static route: string = CompetencyRoutes.NO_COMPETENCY_ASSESSMENT;
  public id: string = '';
  static title: string =
    'Competency - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  public hint = '';
  errorMessage: string = '';
  override model?: NoCompetencyAssessment;

  @Output() onClicked = new EventEmitter();
  @Output() onKeyupEnter = new EventEmitter();

  constructor(activatedRoute: ActivatedRoute) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
    this.updateOnSave = true;

    if (!applicationService.model.Competency?.NoCompetencyAssessment) {
      applicationService.model.Competency!.NoCompetencyAssessment =
        new NoCompetencyAssessment();
    }

    this.model = applicationService.model.Competency?.NoCompetencyAssessment;

    const demandModel = this.DemandModel();
    const declarationKeys = ['Declaration'];
    
    this.applicationService = applicationService;
  }
  override async onSave(applicationService: ApplicationService): Promise<void> {
    const demandModel = this.DemandModel();
    demandModel.Declaration = true;

    if (this.model?.CompletionState !== ComponentCompletionState.InProgress) {
      applicationService.model.Competency!.NoCompetencyAssessment!.CompletionState =
        ComponentCompletionState.Complete;
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
    return true;
  }

  override async navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(
      CompetencySummaryComponent.route,
      this.activatedRoute
    );
  }

  DerivedIsComplete(value: boolean): void {
    this.applicationService.model.Competency!.NoCompetencyAssessment!.CompletionState =
      value
        ? ComponentCompletionState.Complete
        : ComponentCompletionState.InProgress;
  }

  public DemandModel(): NoCompetencyAssessment {
    if (this.model === undefined || this.model === null) {
      throw new Error('Model is undefined');
    }
    return this.model;
  }

  optionClicked() {}
}
