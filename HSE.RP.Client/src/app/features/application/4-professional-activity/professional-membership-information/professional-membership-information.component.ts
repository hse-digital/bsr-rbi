import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ApplicantProfessionalBodyMembership } from 'src/app/models/applicant-professional-body-membership';
import { PageComponent } from 'src/app/helpers/page.component';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { ApplicationService } from 'src/app/services/application.service';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';

interface IPageValidationItem {
  Text: string;
  Anchor: string;
}

enum MembershipType {
  RICS = 'RICS',
  CABE = 'CABE',
  CIOB = 'CIOB',
  OTHER = 'OTHER',
}

const ERROR_MESSAGES = {
  INPUT_MEMBERSHIPNUMBER_REQUIRED: 'Enter your membership number',
  INPUT_MEMBERSHIPLEVEL_REQUIRED: 'Enter your current membership level or type',
  INPUT_MEMBERSHIP_REQUIRED: 'Enter your membership number',
  YEAR_ACHIVED_REQUIRED:
    'Enter the year in which you obtained your current membership level or type',
  YEAR_REQUIRED: 'Your date of membership include a year',
  YEAR_FORMAT: 'Your date of membership must be today or a date in the past',
  YEAR_IN_PRESENT_OR_PAST:
    'Your date of membership must be today or a date in the past',
};

@Component({
  selector: 'hse-professional-membership-information',
  templateUrl: './professional-membership-information.component.html',
  styles: [],
})
export class ProfessionalMembershipInformationComponent extends PageComponent<ApplicantProfessionalBodyMembership> {
  public static route: string =
'professional-membership-information';
  static title: string =
    'Professional activity - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  validationErrors: IPageValidationItem[] = [];
  override model: ApplicantProfessionalBodyMembership =
    new ApplicantProfessionalBodyMembership();

  constructor(activatedRoute: ActivatedRoute) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
    this.updateOnSave = true;
    this.initializeModel(applicationService);
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    const memberships = this.applicationService.model.ProfessionalMemberships;
    const model = this.model;

    const resetMembership = () => {
      for (const type of Object.values(MembershipType)) {
        memberships[type].MembershipNumber = '';
        memberships[type].MembershipLevel = '';
        memberships[type].MembershipYear = -1;
      }
    };

    for (const type of Object.values(MembershipType)) {
      if (memberships[type].MembershipBodyCode !== '') {
        resetMembership();
        memberships[type].MembershipNumber = model.MembershipNumber;
        memberships[type].MembershipLevel = model.MembershipLevel;
        memberships[type].MembershipYear = Number(model.MembershipYear);
        break;
      }
    }

  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return true;
  }

  override isValid(): boolean {
    this.validationErrors = [];

    if (!this.model.MembershipNumber) {
      this.validationErrors.push({
        Text: ERROR_MESSAGES.INPUT_MEMBERSHIPNUMBER_REQUIRED,
        Anchor: 'mem-input-num',
      });
    }

    if (!this.model.MembershipLevel) {
      this.validationErrors.push({
        Text: ERROR_MESSAGES.INPUT_MEMBERSHIPLEVEL_REQUIRED,
        Anchor: 'mem-input-level',
      });
    }

    if (!this.model.MembershipYear) {
      this.validationErrors.push({
        Text: ERROR_MESSAGES.YEAR_ACHIVED_REQUIRED,
        Anchor: 'mem-input-year',
      });
    } else {
      const membershipYear = Number(this.model.MembershipYear);
      if (isNaN(membershipYear) || membershipYear < 1000) {
        this.validationErrors.push({
          Text: ERROR_MESSAGES.YEAR_FORMAT,
          Anchor: 'mem-input-year',
        });
      } else {
        const currentDate = new Date().getFullYear();
        if (membershipYear > currentDate) {
          this.validationErrors.push({
            Text: ERROR_MESSAGES.YEAR_IN_PRESENT_OR_PAST,
            Anchor: 'mem-input-year',
          });
        }
      }
    }

    return this.validationErrors.length === 0;
  }

  getDateOfMembership(): number {
    return this.model.MembershipYear;
  }

  override async navigateNext(): Promise<boolean> {
    return true;
  }

  isDateNumber(dateNumber: string | undefined): boolean {
    return (
      FieldValidations.IsNotNullOrWhitespace(dateNumber) &&
      FieldValidations.IsWholeNumber(Number(dateNumber))
    );
  }

  checkAnchorExistence(anchor: string): boolean {
    return this.validationErrors.some((r) => r.Anchor === anchor);
  }

  getMembershipErrorDescription(): string {
    const indexNumber = this.validationErrors.findIndex(
      (x) => x.Anchor === 'mem-input-num'
    );

    if (indexNumber >= 0) {
      return this.validationErrors[indexNumber].Text;
    }

    return '';
  }

  getLevelErrorDescription(): string {
    const indexNumber = this.validationErrors.findIndex(
      (x) => x.Anchor === 'mem-input-level'
    );

    if (indexNumber >= 0) {
      return this.validationErrors[indexNumber].Text;
    }

    return '';
  }

  getYearErrorDescription(): string {
    const indexNumber = this.validationErrors.findIndex(
      (x) => x.Anchor === 'mem-input-year'
    );

    if (indexNumber >= 0) {
      return this.validationErrors[indexNumber].Text;
    }

    return '';
  }

  private initializeModel(applicationService: ApplicationService): void {
    const memberships = applicationService.model.ProfessionalMemberships;

    const { RICS, CABE, CIOB, OTHER } = memberships;

    this.model.MembershipNumber =
      RICS.MembershipNumber ||
      CABE.MembershipNumber ||
      CIOB.MembershipNumber ||
      OTHER.MembershipNumber;

    this.model.MembershipLevel =
      RICS.MembershipLevel ||
      CABE.MembershipLevel ||
      CIOB.MembershipLevel ||
      OTHER.MembershipLevel;

    const membershipYears = [
      RICS.MembershipYear,
      CABE.MembershipYear,
      CIOB.MembershipYear,
      OTHER.MembershipYear,
    ];

    this.model.MembershipYear =
      membershipYears.find((year) => year !== -1) || -1;
  }
}
