import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicantDateOfBirth, ApplicationService, ApplicationStatus } from '../../../../services/application.service';
import { ApplicantAddressComponent } from '../applicant-address/applicant-address.component';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { PersonalDetailRoutes } from '../PersonalDetailRoutes'

type DateInputControlDate = {
  day?: string;
  month?: string;
  year?: string;
} | undefined;

type DobValidationItem = {
  Text: string;
  Anchor: string;
};

@Component({
  selector: 'hse-applicant-date-of-birth',
  templateUrl: './applicant-date-of-birth.component.html',
})
export class ApplicantDateOfBirthComponent extends PageComponent<DateInputControlDate>
{

  public static route: string = PersonalDetailRoutes.DATE_OF_BIRTH;
  static title: string = "Personal details - Register as a building inspector - GOV.UK";
  production: boolean = environment.production;
  modelValid: boolean = false;
  validationErrors: DobValidationItem[] = [];

  override model?: { day?: string | undefined; month?: string | undefined; year?: string | undefined; } | undefined;


  constructor(activatedRoute: ActivatedRoute, applicationService: ApplicationService) {
    super(activatedRoute);
    this.updateOnSave = true;
  }

  override onInit(applicationService: ApplicationService): void {
    this.model = { day: "", month: "", year: "" };
    if (applicationService.model.PersonalDetails?.ApplicantDateOfBirth) {
      this.model = {
        day: applicationService.model.PersonalDetails.ApplicantDateOfBirth.Day,
        month: applicationService.model.PersonalDetails.ApplicantDateOfBirth.Month,
        year: applicationService.model.PersonalDetails.ApplicantDateOfBirth.Year,
      };
    }
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {

    this.applicationService.model.PersonalDetails!.ApplicantDateOfBirth = {
      Day: this.model!.day,
      Month: this.model!.month,
      Year: this.model!.year,
    }
  }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {

    return this.applicationService.model.ApplicationStatus >= ApplicationStatus.PhoneVerified && this.applicationService.model.id != null;
  }

  isDateNumber(dateNumber: string | undefined): boolean {
    return FieldValidations.IsNotNullOrWhitespace(dateNumber) && FieldValidations.IsWholeNumber(Number(dateNumber))
  }

  override isValid(): boolean {
    this.validationErrors = [];

    if (!this.isDateNumber(this.model?.day)) {
      this.validationErrors.push({ Text: "Your date of birth must include a day", Anchor: "dob-input-day" });
    }

    if (!this.isDateNumber(this.model?.month)) {
      this.validationErrors.push({ Text: "Your date of birth must include a month", Anchor: "dob-input-month" });
    }

    if (!this.isDateNumber(this.model?.year)) {
      this.validationErrors.push({ Text: "Your date of birth must include a year", Anchor: "dob-input-year" });
    }

    if (this.isDateNumber(this.model?.year) && Number(this.model?.year!) < 1000) {
      this.validationErrors.push({ Text: "Your date of birth must include all four numbers of the year, for example 1981, not just 81", Anchor: "dob-input-year" });
    }

    if (this.validationErrors.length == 0) {

      if (new Date() < this.getDateOfBirth()) {
        this.validationErrors.push({ Text: "Your date of birth must be in the past", Anchor: "dob-input-day" });
      }

      if (!this.isWorkingAge()) {
        this.validationErrors.push({ Text: "Your date of birth indicates you are not of working age", Anchor: "dob-input-day" });
      }
    }

    return this.validationErrors.length == 0;
  }

  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(ApplicantAddressComponent.route, this.activatedRoute);
  }

  getDateOfBirth(): Date {
    if (this.model!.day && this.model!.month && this.model!.year) {
      return new Date(Number(this.model!.year), Number(this.model!.month) - 1, Number(this.model!.day));
    }
    else {
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
    return "";
  }
}
