import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { ApplicationService } from '../../../../services/application.service';
import { CompetencyRoutes } from '../CompetencyRoutes';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { CompetencyAssessmentCertificateNumberComponent } from '../assessment-certificate-number/competency-assessment-certificate-number.component';
import { CompetencyAssessmentOrganisation } from 'src/app/models/competency-assessment-organisation.model';
import { CompetencyAssessmentCertificateNumber } from 'src/app/models/competency-assessment-certificate-number.model';
import { CompetencyDateOfAssessment } from 'src/app/models/competency-date-of-assessment.model';

@Component({
  selector: 'hse-competency-assesesment-organisation',
  templateUrl: './competency-assesesment-organisation.component.html',
})
export class CompetencyAssessmentOrganisationComponent extends PageComponent<CompetencyAssessmentOrganisation> {
  public static route: string =
    CompetencyRoutes.COMPETENCY_ASSESSMENT_ORGANISATION;
  static title: string =
    'Competency - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  errorMessage: string = '';
  //selectedOption?: string = '';
  queryParam?: string = '';

  constructor(activatedRoute: ActivatedRoute) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
    this.updateOnSave = true;
    this.activatedRoute.queryParams.subscribe((params) => {
      this.queryParam = params['queryParam'];
    });
    if (
      !applicationService.model.Competency?.CompetencyAssessmentOrganisation
    ) {
      applicationService.model.Competency!.CompetencyAssessmentOrganisation =
        new CompetencyAssessmentOrganisation();
      applicationService.model.Competency!.CompetencyAssessmentOrganisation.ComAssessmentOrganisation =
        '';
    }

    // applicationService.model.Competency!.CompetencyAssessmentOrganisation!.CompletionState =
    //   ComponentCompletionState.InProgress;

    // this.selectedOption = applicationService.model.Competency!
    //   .CompetencyAssessmentOrganisation.ComAssessmentOrganisation
    //   ? applicationService.model.Competency!.CompetencyAssessmentOrganisation
    //       .ComAssessmentOrganisation
    //   : '';

      this.model = applicationService.model.Competency!.CompetencyAssessmentOrganisation;


    this.applicationService = applicationService;
  }

  override async saveAndComeBack(): Promise<void> {
    this.processing = true;

    const STATUS = this.applicationService.model.InspectorClass?.ClassType.CompletionState;

    if (this.modelImplementsIComponent(this.model)) {
      var componentModel = this.model;
      if (componentModel.CompletionState === ComponentCompletionState.Complete) {
        if(this.originalModelStringified !== JSON.stringify(this.model)) {
          componentModel.CompletionState = ComponentCompletionState.InProgress;
          this.applicationService.model.Competency!.CompetencyAssessmentCertificateNumber = new CompetencyAssessmentCertificateNumber();
          this.applicationService.model.Competency!.CompetencyDateOfAssessment = new CompetencyDateOfAssessment();
        }
      }
    }

    if (!this.hasErrors) {
      this.triggerScreenReaderNotification();
      this.applicationService.updateLocalStorage();
      await this.applicationService.updateApplication();
      this.navigationService.navigate(
        `application/${this.applicationService.model.id}`
      );
    } else {
      this.focusAndUpdateErrors();
    }

    this.processing = false;

  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    // if (['CABE', 'BSCF'].includes(this.selectedOption!)) {
    //   applicationService.model.Competency!.CompetencyAssessmentOrganisation!.ComAssessmentOrganisation =
    //     this.selectedOption!;

    this.applicationService.model.Competency!.CompetencyAssessmentOrganisation! = this.model!;

    if(this.originalModelStringified !== JSON.stringify(this.model)) {
      this.applicationService.model.Competency!.CompetencyAssessmentCertificateNumber = new CompetencyAssessmentCertificateNumber();
      this.applicationService.model.Competency!.CompetencyDateOfAssessment = new CompetencyDateOfAssessment();
    }

    // if (this.model?.CompletionState !== ComponentCompletionState.InProgress) {
    //   applicationService.model.Competency!.CompetencyAssessmentOrganisation!.CompletionState =
    //     ComponentCompletionState.Complete;
     }



  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return this.applicationService.model.Competency?.CompetencyIndependentAssessmentStatus?.IAStatus==='yes' && this.applicationService.model.Competency?.CompetencyIndependentAssessmentStatus.CompletionState===ComponentCompletionState.Complete;
  }

  override isValid(): boolean {
    this.hasErrors = false;
    this.errorMessage = '';

    if (this.model?.ComAssessmentOrganisation === '') {
      this.hasErrors = true;
      this.errorMessage = 'Select an assessment organisation';
    }

    return !this.hasErrors;
  }

  override navigateNext(): Promise<boolean> {
    if (
      this.queryParam != null &&
      this.queryParam != undefined &&
      this.queryParam != ''
    ) {
      const queryParam = this.queryParam;
      return this.navigationService.navigateRelative(
        CompetencyAssessmentCertificateNumberComponent.route,
        this.activatedRoute,
        { queryParam }
      );
    }

    return this.navigationService.navigateRelative(
      CompetencyAssessmentCertificateNumberComponent.route,
      this.activatedRoute
    );
  }

  DerivedIsComplete(value: boolean): void {
    this.applicationService.model.Competency!.CompetencyAssessmentOrganisation!.CompletionState =
      value
        ? ComponentCompletionState.Complete
        : ComponentCompletionState.InProgress;
  }
}
