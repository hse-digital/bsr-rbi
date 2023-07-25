import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { ApplicationService } from '../../../../services/application.service';
import { CompetencyCertificateCodeComponent } from '../certificate-code/competency-certificate-code.component';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { CompetencyRoutes } from '../CompetencyRoutes';

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
  // override model?: string;

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService
  ) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
    this.updateOnSave = true;

    this.model =
      applicationService.model.Competency?.IndependentAssessmentStatus;

    this.selectedOption = this.model ? this.model : 'no';

    this.applicationService = applicationService;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {

    this.applicationService.model.Competency!.IndependentAssessmentStatus =
      this.model;

    if (['no', 'yes'].includes(this.selectedOption)) {
      applicationService.model.Competency!.IndependentAssessmentStatus =
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
      CompetencyCertificateCodeComponent.route,
      this.activatedRoute
    );
  }

  DerivedIsComplete(value: boolean): void {}
}
