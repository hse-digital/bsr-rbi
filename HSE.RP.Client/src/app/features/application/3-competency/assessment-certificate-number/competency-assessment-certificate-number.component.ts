import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { ApplicationService } from '../../../../services/application.service';
import { CompetencyAssessmentDateComponent } from '../assesesment-date/competency-assesesment-date.component';
import { CompetencyRoutes } from '../CompetencyRoutes';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { CompetencyAssessmentCertificateNumber } from 'src/app/models/competency-assessment-certificate-number.model';
import { FieldValidations } from 'src/app/helpers/validators/fieldvalidations';
import { CompetencySummaryComponent } from '../competency-summary/competency-summary.component';
import { ApplicationSummaryComponent } from '../../5-application-submission/application-summary/application-summary.component';

@Component({
  selector: 'hse-competency-assesesment-certificate-number',
  templateUrl: './competency-assessment-certificate-number.component.html',
})
export class CompetencyAssessmentCertificateNumberComponent extends PageComponent<CompetencyAssessmentCertificateNumber> {
  public static route: string =
    CompetencyRoutes.COMPETENCY_ASSESSMENT_CERTIFICATE_NUMBER;
  static title: string =
    'Competency - Certificate number - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  errorMessage: string = '';
  organisationPrefix: string = '';
  override hasErrors: boolean = false;
  queryParam?: string = '';

  constructor(activatedRoute: ActivatedRoute) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
    this.activatedRoute.queryParams.subscribe((params) => {
      this.queryParam = params['queryParam'];
    });

    this.updateOnSave = true;

    if (
      !applicationService.model.Competency!
        .CompetencyAssessmentCertificateNumber
    ) {
      applicationService.model.Competency!.CompetencyAssessmentCertificateNumber =
        new CompetencyAssessmentCertificateNumber();
    }

    this.organisationPrefix =
      applicationService.model.Competency!.CompetencyAssessmentOrganisation!.ComAssessmentOrganisation;

      this.model = applicationService.model.Competency!.CompetencyAssessmentCertificateNumber;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    applicationService.model.Competency!.CompetencyAssessmentCertificateNumber = this.model!;
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return true;
  }

  override isValid(): boolean {
    this.model!.CertificateNumber = this.model!.CertificateNumber!.trim().toUpperCase();

    if (!FieldValidations.IsNotNullOrWhitespace(this.model!.CertificateNumber)) {
      this.errorMessage = 'Enter your assessment certificate number';
      this.hasErrors = true;
      return false;
    }

    const validCertificateNumRegex = new RegExp(
      /^(CABE|BSCF|TTD)[a-zA-Z0-9]*$/
    ).test(this.model!.CertificateNumber);

    if (!validCertificateNumRegex) {
      this.errorMessage =
        'You must enter an assessment certificate number in the correct format';
      this.hasErrors = true;
      return false;
    }

    // 4 character prefix (CABE or BSCF) with a 20 character max string so CABE1262IJSBFAHS840.

    let prefix = this.organisationPrefix == "TTD" ? this.model!.CertificateNumber.slice(0, 3) : this.model!.CertificateNumber.slice(0, 4);
    if (prefix.toLowerCase() !== this.organisationPrefix.toLowerCase()) {
      this.errorMessage =
        'You must enter an assessment certificate number in the correct format';
      this.hasErrors = true;

      return false;
    }

    this.hasErrors = false;

    return !this.hasErrors;
  }

  override navigateNext(): Promise<boolean> {
    if (this.queryParam === 'competency-change') {
      return this.navigationService.navigateRelative(
        CompetencySummaryComponent.route,
        this.activatedRoute
      );
    } else if (this.queryParam === 'application-summary') {
      return this.navigationService.navigateRelative(`../application-submission/${ApplicationSummaryComponent.route}`, this.activatedRoute);
    }
    else{
      return this.navigationService.navigateRelative(
        CompetencyAssessmentDateComponent.route,
        this.activatedRoute
      );
    }

    // return this.navigationService.navigateRelative(
    //   CompetencyAssessmentDateComponent.route,
    //   this.activatedRoute
    // );
  }

  DerivedIsComplete(value: boolean): void {
    this.applicationService.model.Competency!.CompetencyAssessmentCertificateNumber!.CompletionState =
      value
        ? ComponentCompletionState.Complete
        : ComponentCompletionState.InProgress;
  }
}
