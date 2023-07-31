import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { ApplicationService } from '../../../../services/application.service';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { CompetencyAssessmentDateComponent } from '../assesesment-date/competency-assesesment-date.component';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { CompetencyRoutes } from '../CompetencyRoutes';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { error } from 'console';
import { CompetenceyAssessmentCertificateNumber } from 'src/app/models/competency-assessment-certificate-number.model';
import { FieldValidations } from 'src/app/helpers/validators/fieldvalidations';

@Component({
  selector: 'hse-competency-assesesment-certificate-number',
  templateUrl: './competency-assessment-certificate-number.component.html',
})
export class CompetencyAssessmentCertificateNumberComponent extends PageComponent<CompetenceyAssessmentCertificateNumber> {
  public static route: string =
    CompetencyRoutes.COMPETENCY_ASSESSMENT_CERTIFICATE_NUMBER;
  static title: string =
    'Competency - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  override model?: CompetenceyAssessmentCertificateNumber;
  errorMessage: string = '';
  organisationPrefix: string = '';
  certificateNumber: string = '';
  override hasErrors: boolean = false;

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService
  ) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
    this.updateOnSave = true;

    if(!applicationService.model.Competency!.CompetencyAssessmentCertificateNumber) {
      applicationService.model.Competency!.CompetencyAssessmentCertificateNumber = new CompetenceyAssessmentCertificateNumber();
    } else {
      this.certificateNumber = applicationService.model.Competency!.CompetencyAssessmentCertificateNumber.CertificateNumber!;
    }
    
    this.organisationPrefix = applicationService.model.Competency!.CompetencyAssesesmentOrganisation!;
    applicationService.model.Competency!.CompetencyAssessmentCertificateNumber!.CompletionState = ComponentCompletionState.InProgress;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {

    applicationService.model.Competency!.CompetencyAssessmentCertificateNumber!.CertificateNumber! = this.certificateNumber;

    if (this.model?.CompletionState !== ComponentCompletionState.InProgress) {
      applicationService.model.Competency!.CompetencyAssessmentCertificateNumber!.CompletionState = ComponentCompletionState.Complete;
    }
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return true;
  }

  override isValid(): boolean {
    if (!FieldValidations.IsNotNullOrWhitespace(this.certificateNumber))
    {
      this.errorMessage = "Enter your assessment certificate number";
      this.hasErrors = true;
      return false;
    }

    // 3-4 character prefix (CABE or BSCF) with a 20 character max string so CABE1262IJSBFAHS840.
    let prefix = this.certificateNumber.slice(0, 4);

    console.log()

    if (prefix !== this.organisationPrefix) {
      this.errorMessage = "You must enter an assessment certificate number in the correct format";
      this.hasErrors = true;

      return false;
    }

    this.hasErrors = false;
    return true;
  }

  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(
      CompetencyAssessmentDateComponent.route,
      this.activatedRoute
    );
  }

  DerivedIsComplete(value: boolean): void {
    this.applicationService.model.Competency!.CompetencyAssessmentCertificateNumber!.CompletionState = value
      ? ComponentCompletionState.Complete
      : ComponentCompletionState.InProgress;
  }
}
