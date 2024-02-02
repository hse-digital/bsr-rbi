import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../../services/application.service';
import { ApplicantAddressComponent } from '../applicant-address/applicant-address.component';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import {
  PersonalDetailRoutes,
  PersonalDetailRouter,
} from '../PersonalDetailRoutes';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { IComponentModel } from '../../../../models/component. interface';
import { ApplicantDateOfBirth } from '../../../../models/applicant-date-of-birth.model';
import { ApplicationSummaryComponent } from '../../5-application-submission/application-summary/application-summary.component';

class DateInputControlDate implements IComponentModel {
  constructor(private containedModel: ApplicantDateOfBirth) {}
  get day(): string | undefined {
    return this.containedModel.Day;
  }
  set day(value: string | undefined) {
    this.containedModel.Day = value;
  }
  get month(): string | undefined {
    return this.containedModel.Month;
  }
  set month(value: string | undefined) {
    this.containedModel.Month = value;
  }
  get year(): string | undefined {
    return this.containedModel.Year;
  }
  set year(value: string | undefined) {
    this.containedModel.Year = value;
  }
  get CompletionState(): ComponentCompletionState | undefined {
    return this.containedModel.CompletionState;
  }
  set CompletionState(value: ComponentCompletionState | undefined) {
    this.containedModel.CompletionState = value;
  }
}

type DobValidationItem = {
  Text: string;
  Anchor: string;
};

@Component({
  selector: 'hse-applicant-date-of-birth',
  templateUrl: './applicant-date-of-birth.component.html',
})
export class ApplicantDateOfBirthComponent extends PageComponent<DateInputControlDate> {
  public static route: string = PersonalDetailRoutes.DATE_OF_BIRTH;
  static title: string =
    'Personal details - Date of birth - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  queryParam?: string = '';
  validationErrors: DobValidationItem[] = [];

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

    if (!applicationService.model.PersonalDetails?.ApplicantDateOfBirth) {
      applicationService.model.PersonalDetails!.ApplicantDateOfBirth =
        new ApplicantDateOfBirth();
    }
    if (applicationService.model.PersonalDetails?.ApplicantDateOfBirth)
      this.model = new DateInputControlDate(
        applicationService.model.PersonalDetails?.ApplicantDateOfBirth
      );
  }

  override async onSave(
    applicationService: ApplicationService
  ): Promise<void> {}

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return this.applicationService.model.id != null;
  }

  isDateNumber(dateNumber: string | undefined): boolean {
    return (
      FieldValidations.IsNotNullOrWhitespace(dateNumber) &&
      FieldValidations.IsWholeNumber(Number(dateNumber))
    );
  }

  override isValid(): boolean {
    this.validationErrors = [];
    if (
      (this.model?.day ?? '') == '' &&
      (this.model?.month ?? '') == '' &&
      (this.model?.year ?? '') == ''
    ) {
      this.validationErrors.push({
        Text: 'Enter your date of birth',
        Anchor: 'dob-input-day',
      });
    } else {
      if (
        !this.isDateNumber(this.model?.day) ||
        !this.isDayValid(Number(this.model?.day))
      ) {
        this.validationErrors.push({
          Text: 'Your date of birth must include a day',
          Anchor: 'dob-input-day',
        });
      }

      if (
        !this.isDateNumber(this.model?.month) ||
        !this.isMonthValid(Number(this.model?.month))
      ) {
        this.validationErrors.push({
          Text: 'Your date of birth must include a month',
          Anchor: 'dob-input-month',
        });
      }

      if (!this.isDateNumber(this.model?.year)) {
        this.validationErrors.push({
          Text: 'Your date of birth must include a year',
          Anchor: 'dob-input-year',
        });
      }

      if (
        this.isDateNumber(this.model?.year) &&
        Number(this.model?.year!) < 1000
      ) {
        this.validationErrors.push({
          Text: 'Your date of birth must include all four numbers of the year, for example 1981, not just 81',
          Anchor: 'dob-input-year',
        });
      }

      if (
        this.isDateNumber(this.model?.year) &&
        Number(this.model?.year!) < 1900
      ) {
        this.validationErrors.push({
          Text: 'Please check the year you were born',
          Anchor: 'dob-input-year',
        });
      }

      if (this.validationErrors.length == 0) {
        if (new Date() < this.getDateOfBirth()) {
          this.validationErrors.push({
            Text: 'Your date of birth must be in the past',
            Anchor: 'dob-input-day',
          });
        }

        if (!this.isWorkingAge()) {
          this.validationErrors.push({
            Text: 'Your date of birth indicates you are not of working age',
            Anchor: 'dob-input-day',
          });
        }
      }
    }

    return this.validationErrors.length == 0;
  }

  override navigateNext(): Promise<boolean> {
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
      PersonalDetailRoutes.ADDRESS
    );
  }

  getDateOfBirth(): Date {
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

  isWorkingAge(): boolean {
    let sixteenYearsAgo = new Date();
    let dob = this.getDateOfBirth();
    sixteenYearsAgo.setFullYear(sixteenYearsAgo.getFullYear() - 16);
    return dob <= sixteenYearsAgo;
  }

  getFirstErrorDescription(): string {
    if (this.validationErrors.length > 0) {
      return this.validationErrors[0].Text;
    }
    return '';
  }

  isDayValid(day: number) {
    return day <= 31 && day >= 1;
  }

  isMonthValid(month: number) {
    return month <= 12 && month >= 1;
  }
}
