import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { ApplicationService } from '../../../../services/application.service';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { CompetencyRoutes } from '../CompetencyRoutes';
import { DateFormatHelper } from 'src/app/helpers/date-format-helper';

@Component({
  selector: 'hse-competency-summary',
  templateUrl: './competency-summary.component.html',
})
export class CompetencySummaryComponent extends PageComponent<string> {
  DerivedIsComplete(value: boolean): void {}
  CompetencyRoutes = CompetencyRoutes;

  public static route: string = CompetencyRoutes.SUMMARY;
  static title: string =
    'Competency - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;

  override model?: string;

  constructor(
    activatedRoute: ActivatedRoute,
  ) {
    super(activatedRoute);
    this.updateOnSave = true;
  }

  override onInit(applicationService: ApplicationService): void {
    //this.model = applicationService.model.personalDetails?.applicantPhoto?.toString() ?? '';
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    applicationService.model.ApplicationStatus =
      ApplicationStatus.CompetencyComplete;
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return true;
    //return (FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.firstName) || FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.lastName));
  }

  override isValid(): boolean {
    return true;
    /*     this.phoneNumberHasErrors = !PhoneNumberValidator.isValid(this.model?.toString() ?? '');
    return !this.phoneNumberHasErrors; */
  }

  async SyncAndContinue() {
    await this.applicationService.syncCompetency();
    this.saveAndContinue();
  }

  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(
      `../${ApplicationTaskListComponent.route}`,
      this.activatedRoute
    );
  }

  public navigateTo(route: string) {
    return this.navigationService.navigateRelative(
      `${route}`,
      this.activatedRoute
    );
  }

  public GetFormattedDateofBirth(): string {
    return DateFormatHelper.LongMonthFormat(
      this.applicationService.model.Competency?.CompetencyDateOfAssessment
        ?.Year,
      this.applicationService.model.Competency?.CompetencyDateOfAssessment
        ?.Month,
      this.applicationService.model.Competency?.CompetencyDateOfAssessment?.Day
    );
  }

  public getIndependentAssessmentStatus(): string {
    return this.applicationService.model.Competency
      ?.CompetencyIndependentAssessmentStatus?.IAStatus === 'yes'
      ? 'Yes'
      : 'No';
  }

  public isCompetencyAssessmentStatusYes(): boolean {
    return this.applicationService.model.Competency?.CompetencyIndependentAssessmentStatus?.IAStatus === 'yes';
  }

  public getCompetencyAssessmentOrg(): string {
    return this.applicationService.model.Competency
      ?.CompetencyAssessmentOrganisation?.ComAssessmentOrganisation === 'BSCF'
      ? 'Building Safety Competence Foundation (BSCF)'
      : 'Chartered Association of Building Engineers (CABE)';
  }

  public getCompetencyAssessmentCertificateNo(): string {
    return (
      this.applicationService.model.Competency
        ?.CompetencyAssessmentCertificateNumber?.CertificateNumber || 'No'
    );
  }
}
