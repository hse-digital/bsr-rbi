import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { ApplicationService } from '../../../../services/application.service';
import { CompetencyRoutes } from '../CompetencyRoutes';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { CompetencyAssessmentCertificateNumberComponent } from '../assessment-certificate-number/competency-assessment-certificate-number.component';
import { CompetencyAssesesmentOrganisation } from 'src/app/models/competency-assesesment-organisation.model';

@Component({
  selector: 'hse-competency-assesesment-organisation',
  templateUrl: './competency-assesesment-organisation.component.html',
})
export class CompetencyAssessmentOrganisationComponent extends PageComponent<CompetencyAssesesmentOrganisation> {
  public static route: string =
    CompetencyRoutes.COMPETENCY_ASSESSMENT_ORGANISATION;
  static title: string =
    'Competency - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  errorMessage: string = '';
  selectedOption?: string = '';

  constructor(activatedRoute: ActivatedRoute) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
    this.updateOnSave = true;

    if (
      !applicationService.model.Competency?.CompetencyAssesesmentOrganisation
    ) {
      applicationService.model.Competency!.CompetencyAssesesmentOrganisation =
        new CompetencyAssesesmentOrganisation();
      applicationService.model.Competency!.CompetencyAssesesmentOrganisation.ComAssesesmentOrganisation =
        'CABE';
    }

    applicationService.model.Competency!.CompetencyAssesesmentOrganisation!.CompletionState =
      ComponentCompletionState.InProgress;

    this.selectedOption =
      applicationService.model.Competency!.CompetencyAssesesmentOrganisation
        .ComAssesesmentOrganisation === 'BSCF'
        ? applicationService.model.Competency!.CompetencyAssesesmentOrganisation
            .ComAssesesmentOrganisation
        : 'CABE';

    this.applicationService = applicationService;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    if (['CABE', 'BSCF'].includes(this.selectedOption!)) {
      applicationService.model.Competency!.CompetencyAssesesmentOrganisation!.ComAssesesmentOrganisation =
        this.selectedOption!;
    }

    if (this.model?.CompletionState !== ComponentCompletionState.InProgress) {
      applicationService.model.Competency!.CompetencyAssesesmentOrganisation!.CompletionState =
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

  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(
      CompetencyAssessmentCertificateNumberComponent.route,
      this.activatedRoute
    );
  }

  DerivedIsComplete(value: boolean): void {
    this.applicationService.model.Competency!.CompetencyAssesesmentOrganisation!.CompletionState =
      value
        ? ComponentCompletionState.Complete
        : ComponentCompletionState.InProgress;
  }
}
