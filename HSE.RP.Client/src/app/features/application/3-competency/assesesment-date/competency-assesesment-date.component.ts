import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../../services/application.service';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { CompetencySummaryComponent } from '../competency-summary/competency-summary.component';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { CompetencyRoutes } from '../CompetencyRoutes';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { DateInputControlDate } from 'src/app/models/date-input-control-date.model';

type DoaValidationItem = {
  Text: string;
  Anchor: string;
};

const ERROR_MESSAGES = {
  DATE_ASSESSMENT_REQUIRED: 'Enter your date of assessment',
  DAY_REQUIRED: 'Your date of assessment must include a day',
  MONTH_REQUIRED: 'Your date of assessment must include a month',
  YEAR_REQUIRED: 'Your date of assessment must include a year',
  YEAR_FORMAT:
    'Your date of assessment must include all four numbers of the year, for example 1981, not just 81.',
  DATE_IN_PRESENT_OR_PAST:
    'Your date of assessment must be today or a date in the past',
};

@Component({
  selector: 'hse-competency-assesesment-date',
  templateUrl: './competency-assesesment-date.component.html',
})
export class CompetencyAssessmentDateComponent extends PageComponent<DateInputControlDate> {
  public static route: string = CompetencyRoutes.COMPETENCY_ASSESSMENT_DATE;
  static title: string =
    'Competency - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  errorMessage: string = '';
  validationErrors: DoaValidationItem[] = [];

  constructor(activatedRoute: ActivatedRoute) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
    this.updateOnSave = true;
    if (!applicationService.model.Competency?.CompetencyDateOfAssessment) {
      applicationService.model.Competency!.CompetencyDateOfAssessment = {
        Day: '',
        Month: '',
        Year: '',
        CompletionState: ComponentCompletionState.InProgress,
      };
    }

    this.model = {
      day: applicationService.model.Competency!.CompetencyDateOfAssessment.Day,
      month:
        applicationService.model.Competency!.CompetencyDateOfAssessment.Month,
      year: applicationService.model.Competency!.CompetencyDateOfAssessment
        .Year,
    };
  }

  DerivedIsComplete(value: boolean): void {
    this.applicationService.model.Competency!.CompetencyDateOfAssessment!.CompletionState =
      value
        ? ComponentCompletionState.Complete
        : ComponentCompletionState.InProgress;
  }

  override async onSave(): Promise<void> {
    this.applicationService.model.Competency!.CompetencyDateOfAssessment!.Day =
      this.model?.day;
    this.applicationService.model.Competency!.CompetencyDateOfAssessment!.Month =
      this.model?.month;
    this.applicationService.model.Competency!.CompetencyDateOfAssessment!.Year =
      this.model?.year;
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return true;
    //return (FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.firstName) || FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.lastName));
  }

  isDateNumber(dateNumber: string | undefined): boolean {
    return (
      FieldValidations.IsNotNullOrWhitespace(dateNumber) &&
      FieldValidations.IsWholeNumber(Number(dateNumber))
    );
  }

  override isValid(): boolean {
    this.validationErrors = [];
    if (!this.model?.day && !this.model?.month && !this.model?.year) {
      this.validationErrors.push({
        Text: ERROR_MESSAGES.DATE_ASSESSMENT_REQUIRED,
        Anchor: 'doa-input-day',
      });
      return false;
    }

    if (!this.isDateNumber(this.model?.day)) {
      this.validationErrors.push({
        Text: ERROR_MESSAGES.DAY_REQUIRED,
        Anchor: 'doa-input-day',
      });
    }

    if (!this.isDateNumber(this.model?.month)) {
      this.validationErrors.push({
        Text: ERROR_MESSAGES.MONTH_REQUIRED,
        Anchor: 'doa-input-month',
      });
    }

    if (!this.isDateNumber(this.model?.year)) {
      this.validationErrors.push({
        Text: ERROR_MESSAGES.YEAR_REQUIRED,
        Anchor: 'doa-input-year',
      });
    } else if (Number(this.model?.year!) < 1000) {
      this.validationErrors.push({
        Text: ERROR_MESSAGES.YEAR_FORMAT,
        Anchor: 'doa-input-year',
      });
    }

    const currentDate = new Date();
    const selectedDate = this.getDateOfAssessment();
    if (selectedDate > currentDate) {
      this.validationErrors.push({
        Text: ERROR_MESSAGES.DATE_IN_PRESENT_OR_PAST,
        Anchor: 'doa-input-day',
      });
    }

    return this.validationErrors.length === 0;
  }

  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(
      CompetencySummaryComponent.route,
      this.activatedRoute
    );
  }

  getDateOfAssessment(): Date {
    if (this.model!.day && this.model!.month && this.model!.year) {
      return new Date(
        Number(this.model!.year),
        Number(this.model!.month) - 1,
        Number(this.model!.day)
      );
    } else {
      return new Date();
    }
  }

  getFirstErrorDescription(): string {
    if (this.validationErrors.length > 0) {
      return this.validationErrors[0].Text;
    }
    return '';
  }
}
