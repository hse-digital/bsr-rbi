import { Component, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { PageComponent } from 'src/app/helpers/page.component';
import { ApplicationService } from 'src/app/services/application.service';
import { CompetencyRoutes } from '../CompetencyRoutes';
import { environment } from 'src/environments/environment';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { NoCompetencyAssessment } from 'src/app/models/no-competency-assessment.model';
import { CompetencySummaryComponent } from '../competency-summary/competency-summary.component';

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
  public selections: string[] = [];
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

    this.selections.push(
      ...declarationKeys.filter((key) => demandModel[key] === true)
    );

    this.applicationService = applicationService;
  }
  override async onSave(applicationService: ApplicationService): Promise<void> {
    const demandModel = this.DemandModel();
    demandModel.Declaration = false;

    this.selections.forEach((value: keyof typeof demandModel) => {
      demandModel[value] = true;
    });

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
    if (this.selections.length == 0)
      this.errorMessage =
        'You must select the declaration checkbox to agree to be registered as a Class 1 building inpsector';
    return this.selections.length > 0;
  }

  override async navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(
      CompetencySummaryComponent.route,
      this.activatedRoute
    );
  }

  DerivedIsComplete(value: boolean): void {}

  public DemandModel(): NoCompetencyAssessment {
    if (this.model === undefined || this.model === null) {
      throw new Error('Model is undefined');
    }
    return this.model;
  }

  optionClicked() {}
}
