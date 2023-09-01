import { Component, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { PageComponent } from 'src/app/helpers/page.component';
import { ApplicationService } from 'src/app/services/application.service';
import { CompetencyRoutes } from '../CompetencyRoutes';
import { environment } from 'src/environments/environment';
import { NoCompetencyAssessment } from 'src/app/models/no-competency-assessment.model';
import { CompetencySummaryComponent } from '../competency-summary/competency-summary.component';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { ApplicationSummaryComponent } from '../../5-application-submission/application-summary/application-summary.component';
import { BuildingInspectorClassType } from 'src/app/models/building-inspector-classtype.enum';

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
  queryParam?: string = '';


  @Output() onClicked = new EventEmitter();
  @Output() onKeyupEnter = new EventEmitter();

  constructor(activatedRoute: ActivatedRoute) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
    this.updateOnSave = true;
    this.activatedRoute.queryParams.subscribe((params) => {
      this.queryParam = params['queryParam'];
    });
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



    //this.applicationService.model.InspectorClass!.ClassType.Class = BuildingInspectorClassType.Class1;

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

    if (this.queryParam === 'application-summary') {
      return this.navigationService.navigateRelative(
        `../application-submission/${ApplicationSummaryComponent.route}`,
        this.activatedRoute
      );
    }
    return this.navigationService.navigateRelative(
      CompetencySummaryComponent.route,
      this.activatedRoute
    );

  }

  // override navigateNext(): Promise<boolean> {
  //   if (this.queryParam === 'personal-details-change') {
  //     return this.personalDetailRouter.navigateTo(
  //       this.applicationService.model,
  //       PersonalDetailRoutes.SUMMARY
  //     );
  //   } else if (this.queryParam === 'application-summary') {
  //     return this.navigationService.navigateRelative(
  //       `../application-submission/${ApplicationSummaryComponent.route}`,
  //       this.activatedRoute
  //     );
  //   }
  //   return this.personalDetailRouter.navigateTo(
  //     this.applicationService.model,
  //     PersonalDetailRoutes.ADDRESS
  //   );
  // }

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
