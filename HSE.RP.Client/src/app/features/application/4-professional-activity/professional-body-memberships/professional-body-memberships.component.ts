import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { ApplicationService } from '../../../../services/application.service';
import { ProfessionalActivityEmploymentTypeComponent } from '../employment-type/professional-activity-employment-type.component';
import { ApplicantHasProfessionBodyMemberships, ApplicantProfessionBodyMemberships, ApplicantProfessionBodyMembershipsHelper, ProfessionalBodies } from 'src/app/models/applicant-professional-body-membership';
import { ProfessionalBodySelectionComponent } from '../professional-body-selection/professional-body-selection.component';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { ProfessionalMembershipAndEmploymentSummaryComponent } from '../professional-membership-and-employment-summary/professional-membership-and-employment-summary.component';
import { ApplicationSummaryComponent } from '../../5-application-submission/application-summary/application-summary.component';




@Component({
  selector: 'hse-professional-body-memberships',
  templateUrl: './professional-body-memberships.component.html',
})
export class ProfessionalBodyMembershipsComponent extends PageComponent<ApplicantHasProfessionBodyMemberships> {

  public static route: string = 'professional-body-memberships';
  static title: string =
    'Professional activity - Professional body memberships - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  errorMessage: string = '';
  queryParam?: string = '';
  originalSelection?: string = '';

  constructor(activatedRoute: ActivatedRoute) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
    //this.applicationService.model.ProfessionalMemberships = new ApplicantProfessionBodyMemberships();
    this.updateOnSave = true;
    this.activatedRoute.queryParams.subscribe((params) => {
      this.queryParam = params['queryParam'];
    });
    if (!applicationService.model.ProfessionalMemberships) {
      applicationService.model.ProfessionalMemberships = new ApplicantProfessionBodyMemberships();
      applicationService.model.ProfessionalMemberships.ApplicantHasProfessionBodyMemberships = new ApplicantHasProfessionBodyMemberships();
      this.applicationService.model.ProfessionalMemberships.CompletionState = ComponentCompletionState.InProgress;
      this.model = applicationService.model.ProfessionalMemberships.ApplicantHasProfessionBodyMemberships;
    } else {
      if (!applicationService.model.ProfessionalMemberships.ApplicantHasProfessionBodyMemberships) {
        applicationService.model.ProfessionalMemberships.ApplicantHasProfessionBodyMemberships = new ApplicantHasProfessionBodyMemberships();
      }
      if (this.applicationService.model.ProfessionalMemberships.CompletionState == ComponentCompletionState.NotStarted) {
        this.applicationService.model.ProfessionalMemberships.CompletionState = ComponentCompletionState.InProgress;
      }
      this.model = applicationService.model.ProfessionalMemberships.ApplicantHasProfessionBodyMemberships;
    }

    if (this.model.IsProfessionBodyRelevantYesNo == null) {
      this.model.IsProfessionBodyRelevantYesNo = '';
    }

    this.originalSelection = applicationService.model.ProfessionalMemberships.ApplicantHasProfessionBodyMemberships.IsProfessionBodyRelevantYesNo;

  }

  override async saveAndComeBack(): Promise<void> {
    this.processing = true;

    if (this.model!.IsProfessionBodyRelevantYesNo === this.originalSelection) {
      this.applicationService.model.ProfessionalMemberships.ApplicantHasProfessionBodyMemberships = this.model!
    }
    else {
      this.model!.CompletionState = ComponentCompletionState.InProgress;
      this.applicationService.model.ProfessionalMemberships.ApplicantHasProfessionBodyMemberships = this.model!
      this.applicationService.model.ProfessionalMemberships.CompletionState = ComponentCompletionState.InProgress;
    }



    if (!this.hasErrors) {
      this.triggerScreenReaderNotification();
      this.applicationService.updateLocalStorage();
      await this.applicationService.updateApplication();
    } else {
      this.focusAndUpdateErrors();
    }
    this.processing = false;

    const taskListRoute: string = `application/${this.applicationService.model.id}`;
    this.navigationService.navigate(taskListRoute);

  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    if (['no', 'yes'].includes(this.model!.IsProfessionBodyRelevantYesNo)) {
      if (this.model?.IsProfessionBodyRelevantYesNo === 'no') {
        this.applicationService.model.ProfessionalMemberships.RICS = ApplicantProfessionBodyMembershipsHelper.Reset(ProfessionalBodies.RICS.BodyCode)
        this.applicationService.model.ProfessionalMemberships.CABE = ApplicantProfessionBodyMembershipsHelper.Reset(ProfessionalBodies.CABE.BodyCode)
        this.applicationService.model.ProfessionalMemberships.CIOB = ApplicantProfessionBodyMembershipsHelper.Reset(ProfessionalBodies.CIOB.BodyCode)
        this.applicationService.model.ProfessionalMemberships.OTHER = ApplicantProfessionBodyMembershipsHelper.Reset(ProfessionalBodies.OTHER.BodyCode)
      }
      if (this.model?.IsProfessionBodyRelevantYesNo === 'yes') {
        this.applicationService.model.ProfessionalMemberships.CompletionState = ComponentCompletionState.InProgress;
      }

      applicationService.model.ProfessionalMemberships.ApplicantHasProfessionBodyMemberships = this.model!;
    }

  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return true;
  }

  override isValid(): boolean {
    this.hasErrors = false;
    this.errorMessage = '';
    if (this.model!.IsProfessionBodyRelevantYesNo === '') {
      this.hasErrors = true;
      this.errorMessage =
        'Select whether you hold a membership with a professional body or not';
    }

    return !this.hasErrors;
  }

  override async navigateNext(): Promise<boolean> {
    const queryParam = this.queryParam;

    if (this.model!.IsProfessionBodyRelevantYesNo === 'no') {

      if (this.queryParam == 'professional-membership-and-employment-summary') {
        return this.navigationService.navigateRelative(
          ProfessionalMembershipAndEmploymentSummaryComponent.route,
          this.activatedRoute
        );
      }
      else if (this.queryParam == 'application-summary') {
        return this.navigationService.navigateRelative(
          `../application-submission/${ApplicationSummaryComponent.route}`,
          this.activatedRoute
        );
      }

      await this.applicationService.syncProfessionalBodyMemberships();
      this.model!.CompletionState = ComponentCompletionState.Complete;
      this.applicationService.model.ProfessionalMemberships.ApplicantHasProfessionBodyMemberships = this.model!;
      return this.navigationService.navigateRelative(
        ProfessionalActivityEmploymentTypeComponent.route,
        this.activatedRoute
      );
    }

    if (
      this.model!.IsProfessionBodyRelevantYesNo === 'yes' &&
      this.applicationService.model.ProfessionalMemberships.RICS.CompletionState === ComponentCompletionState.Complete &&
      this.applicationService.model.ProfessionalMemberships.CABE.CompletionState === ComponentCompletionState.Complete &&
      this.applicationService.model.ProfessionalMemberships.CIOB.CompletionState === ComponentCompletionState.Complete &&
      this.applicationService.model.ProfessionalMemberships.OTHER.CompletionState === ComponentCompletionState.Complete
    ) {
      return this.navigationService.navigateRelative(
        `professional-body-membership-summary`,
        this.activatedRoute,
        { queryParam: this.queryParam }

      );
    } else if (this.model!.IsProfessionBodyRelevantYesNo === 'yes') {
      return this.navigationService.navigateRelative(
        `professional-body-selection`,
        this.activatedRoute,
        { queryParam: this.queryParam }
      );
    }

    return this.navigationService.navigateRelative(
      ProfessionalActivityEmploymentTypeComponent.route,
      this.activatedRoute
    );
  }

}
