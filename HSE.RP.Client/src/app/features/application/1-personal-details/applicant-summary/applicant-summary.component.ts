import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../../services/application.service';
import { ApplicantAddressComponent } from '../applicant-address/applicant-address.component';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { NavigationService } from 'src/app/services/navigation.service';
import {
  PersonalDetailRoutes,
  PersonalDetailRouter,
} from '../PersonalDetailRoutes';
import { DateFormatHelper } from 'src/app/helpers/date-format-helper';
import { BuildingProfessionalModel } from 'src/app/models/building-professional.model';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { StageCompletionState } from 'src/app/models/stage-completion-state.enum';
import { AddressModel } from 'src/app/models/address.model';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { NextStage } from 'src/app/helpers/next-section.helper';

@Component({
  selector: 'hse-applicant-summary',
  templateUrl: './applicant-summary.component.html',
})
export class ApplicantSummaryComponent extends PageComponent<string> {
  DerivedIsComplete(value: boolean): void { }
  PersonalDetailRoutes = PersonalDetailRoutes;

  public static route: string = PersonalDetailRoutes.SUMMARY;
  static title: string =
    'Personal details - Summary - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  private personalDetailRouter: PersonalDetailRouter;
  override model?: string;
  override processing = false;

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService,
    personalDetailRouter: PersonalDetailRouter
  ) {
    super(activatedRoute);
    this.personalDetailRouter = personalDetailRouter;
    this.updateOnSave = true;
  }

  override onInit(applicationService: ApplicationService): void {
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    this.applicationService.model.StageStatus['PersonalDetails'] =
      StageCompletionState.Complete;
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    try {
      return this.applicationService.model.PersonalDetails!.ApplicantName!
        .CompletionState === ComponentCompletionState.Complete &&
        this.applicationService.model.PersonalDetails!.ApplicantAddress!
          .CompletionState === ComponentCompletionState.Complete &&
        this.applicationService.model.PersonalDetails!
          .ApplicantAlternativeEmail!.CompletionState ===
        ComponentCompletionState.Complete &&
        this.applicationService.model.PersonalDetails!
          .ApplicantAlternativePhone!.CompletionState ===
        ComponentCompletionState.Complete &&
        this.applicationService.model.PersonalDetails!.ApplicantDateOfBirth!
          .CompletionState === ComponentCompletionState.Complete &&
        this.applicationService.model.PersonalDetails!
          .ApplicantNationalInsuranceNumber!.CompletionState ===
        ComponentCompletionState.Complete &&
        this.applicationService.model.PersonalDetails!.ApplicantPhone!
          .CompletionState === ComponentCompletionState.Complete &&
        this.applicationService.model.PersonalDetails!.ApplicantEmail!
          .CompletionState === ComponentCompletionState.Complete;
    } catch (e) {
      return false;

    }

    //return (FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.firstName) || FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.lastName));
  }

  override isValid(): boolean {
    return true;

  }

  override navigateNext(): Promise<boolean> {

    return this.navigationService.navigateRelative(
      `../${ApplicationTaskListComponent.route}`,
      this.activatedRoute, undefined, NextStage.getNextStage(this.applicationService.model)
    );


  }

  public navigateToNamedRoute(namedRoute: string) {
    return this.navigateTo(PersonalDetailRoutes.MapNameToRoute(namedRoute));
  }

  public navigateTo(route: string) {
    const queryParam = 'personal-details-change';
    return this.navigationService.navigateRelative(
      `${route}`,
      this.activatedRoute,
      { queryParam }
    );
  }

  public GetFormattedDateofBirth(): string {
    return DateFormatHelper.LongMonthFormat(
      this.applicationService.model.PersonalDetails?.ApplicantDateOfBirth?.Year,
      this.applicationService.model.PersonalDetails?.ApplicantDateOfBirth
        ?.Month,
      this.applicationService.model.PersonalDetails?.ApplicantDateOfBirth?.Day
    );
  }

  public getAlternativePhone(): string {
    return (
      this.applicationService.model.PersonalDetails?.ApplicantAlternativePhone
        ?.PhoneNumber || 'none'
    );
  }

  public getAlternativeEmail(): string {
    return (
      this.applicationService.model.PersonalDetails?.ApplicantAlternativeEmail
        ?.Email || 'none'
    );
  }

  async SyncAndContinue() {
    if (!this.processing) {
      this.processing = true;
      await this.applicationService.syncPersonalDetails();
      this.applicationService.model.StageStatus['PersonalDetails'] =
        StageCompletionState.Complete;
      this.saveAndContinue();
    }
  }
}
