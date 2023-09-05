import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../../services/application.service';
import { ApplicantAddressComponent } from '../applicant-address/applicant-address.component';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { ApplicantSummaryComponent } from '../applicant-summary/applicant-summary.component';
import { NationalInsuranceNumberValidator } from '../../../../helpers/validators/national-insurance-number-validator';
import {
  PersonalDetailRoutes,
  PersonalDetailRouter,
} from '../PersonalDetailRoutes';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { ApplicantNationalInsuranceNumber } from '../../../../models/applicant-national-insurance-number.model';
import { ApplicationSummaryComponent } from '../../5-application-submission/application-summary/application-summary.component';

@Component({
  selector: 'hse-applicant-national-insurance-number',
  templateUrl: './applicant-national-insurance-number.component.html',
  styleUrls: ['./applicant-national-insurance-number.component.scss'],
  encapsulation: ViewEncapsulation.None

})
export class ApplicantNationalInsuranceNumberComponent extends PageComponent<ApplicantNationalInsuranceNumber> {
  public static route: string = PersonalDetailRoutes.NATIONAL_INS_NUMBER;
  static title: string =
    'Personal details - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  nsiHasErrors: boolean = false;
  nsiIsNullOrWhiteSpace: boolean = false;
  nsiIsInvalidFormat: boolean = false;
  queryParam?: string = '';

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService,
    private personalDetailRouter: PersonalDetailRouter
  ) {
    super(activatedRoute);
    this.updateOnSave = true;
  }

  override onInit(applicationService: ApplicationService): void {
    this.activatedRoute.queryParams.subscribe((params) => {
      this.queryParam = params['queryParam'];
    });
    if (
      !applicationService.model.PersonalDetails
        ?.ApplicantNationalInsuranceNumber
    ) {
      applicationService.model.PersonalDetails!.ApplicantNationalInsuranceNumber =
        new ApplicantNationalInsuranceNumber();
    }
    this.model =
      applicationService.model.PersonalDetails!.ApplicantNationalInsuranceNumber;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    this.applicationService.model.PersonalDetails!.ApplicantNationalInsuranceNumber =
      this.model;
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return this.applicationService.model.id != null;
  }

  getErrorMessage(): string {
    if (this.nsiIsNullOrWhiteSpace) {
      return 'Enter your National Insurance number';
    }
    if (this.nsiIsInvalidFormat) {
      return 'Enter a National Insurance number in the correct format';
    }
    return '';
  }

  override isValid(): boolean {
    this.nsiIsNullOrWhiteSpace = !FieldValidations.IsNotNullOrWhitespace(
      this.model?.NationalInsuranceNumber
    );
    this.nsiIsInvalidFormat = !NationalInsuranceNumberValidator.isValid(
      this.model?.NationalInsuranceNumber ?? ''
    );
    this.nsiHasErrors = this.nsiIsNullOrWhiteSpace || this.nsiIsInvalidFormat;
    return !this.nsiHasErrors;
  }

  override navigateNext(): Promise<boolean> {
    try {
      if (
        this.applicationService.model.PersonalDetails!.ApplicantName!
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
          .CompletionState === ComponentCompletionState.Complete
      ) {
        if (this.queryParam === 'personal-details-change') {
          return this.personalDetailRouter.navigateTo(
            this.applicationService.model,
            PersonalDetailRoutes.SUMMARY
          );
        } else if (this.queryParam === 'application-summary') {
          return this.navigationService.navigateRelative(
            `../application-submission/${ApplicationSummaryComponent.route}`,
            this.activatedRoute
          );
        }
        return this.personalDetailRouter.navigateTo(
          this.applicationService.model,
          PersonalDetailRoutes.SUMMARY
        );
      } else {
        return this.navigationService.navigate(
          `application/${this.applicationService.model.id}`
        );
      }
    } catch (e) {
      return this.navigationService.navigate(
        `application/${this.applicationService.model.id}`
      );
    }
  }
}
