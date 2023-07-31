import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { ApplicationService } from '../../../../services/application.service';
import { CompetencyRoutes } from '../CompetencyRoutes';
import { CompetencyAssessmentOrganisationComponent } from '../assesesment-organisation/competency-assesesment-organisation.component';
import { NoCompetencyAssessmentComponent } from '../no-competency-assessment/no-competency-assessment.component';

@Component({
  selector: 'hse-independent-competency-status',
  templateUrl: './independent-competency-status.component.html',
})
export class CompetencyIndependentStatusComponent extends PageComponent<string> {
  public static route: string = CompetencyRoutes.INDEPENDENT_COMPETENCY_STATUS;
  static title: string =
    'Competency - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
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

    if(!applicationService.model.Competency?.IndependentAssessmentStatus) {
      applicationService.model.Competency!.IndependentAssessmentStatus = 'no'
    } 

    console.log(applicationService.model.Competency?.IndependentAssessmentStatus)

    this.selectedOption = applicationService.model.Competency?.IndependentAssessmentStatus ? applicationService.model.Competency?.IndependentAssessmentStatus : 'no'

    this.applicationService = applicationService;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    this.applicationService.model.Competency!.IndependentAssessmentStatus =
      this.model;

    if (['no', 'yes'].includes(this.selectedOption)) {
      applicationService.model.Competency!.IndependentAssessmentStatus =
        this.selectedOption;
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

  DerivedIsComplete(value: boolean): void {}
}
