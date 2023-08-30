import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ApplicantProfessionalBodyMembership } from 'src/app/models/applicant-professional-body-membership';
import { PageComponent } from 'src/app/helpers/page.component';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { ApplicationService } from 'src/app/services/application.service';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ProfessionalIndividualMembershipDetailsComponent } from '../professional-individual-membership-details/professional-individual-membership-details.component';

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
    'Enter the year you obtained your current membership level or type',
  YEAR_REQUIRED: 'Your date of membership include a year',
  YEAR_FORMAT: 'Your date of membership must be today or a date in the past',
  YEAR_IN_PRESENT_OR_PAST:
    'Your date of membership must be today or a date in the past',
  YEAR_VALIDATION_ERROR: 'Enter the year in full, for example 1984',
};

@Component({
  selector: 'hse-professional-membership-information',
  templateUrl: './professional-membership-information.component.html',
  styles: [],
})
export class ProfessionalMembershipInformationComponent extends PageComponent<ApplicantProfessionalBodyMembership> {
  public static route: string = 'professional-membership-information';
  static title: string =
    'Professional activity - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  validationErrors: IPageValidationItem[] = [];
  singleValidationErrors: IPageValidationItem[] = [];
  isSingleMessage: boolean = false;
  override model: ApplicantProfessionalBodyMembership =
    new ApplicantProfessionalBodyMembership();
  membershipCode: string = '';
  PROFESSIONAL_BODY_ORG_NAME: string = '';
  queryParam?: string = '';

  constructor(activatedRoute: ActivatedRoute) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
    this.updateOnSave = true;

    this.activatedRoute.queryParams.subscribe((params) => {
      this.membershipCode = params['membershipCode'];
      this.queryParam = params['queryParam'];
      this.getProfessionalBodyOrgName(this.membershipCode);
    });

    this.initializeModel(applicationService);
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    if (this.membershipCode === 'CABE') {
      applicationService.model.ProfessionalMemberships.CABE.MembershipNumber =
        this.model.MembershipNumber;
      applicationService.model.ProfessionalMemberships.CABE.MembershipLevel =
        this.model.MembershipLevel;
      applicationService.model.ProfessionalMemberships.CABE.MembershipYear =
        Number(this.model.MembershipYear);
    } else if (this.membershipCode === 'RICS') {
      applicationService.model.ProfessionalMemberships.RICS.MembershipNumber =
        this.model.MembershipNumber;
      applicationService.model.ProfessionalMemberships.RICS.MembershipLevel =
        this.model.MembershipLevel;
      applicationService.model.ProfessionalMemberships.RICS.MembershipYear =
        Number(this.model.MembershipYear);
    } else if (this.membershipCode === 'CIOB') {
      applicationService.model.ProfessionalMemberships.CIOB.MembershipNumber =
        this.model.MembershipNumber;
      applicationService.model.ProfessionalMemberships.CIOB.MembershipLevel =
        this.model.MembershipLevel;
      applicationService.model.ProfessionalMemberships.CIOB.MembershipYear =
        Number(this.model.MembershipYear);
    } else if (this.membershipCode === 'OTHER') {
      applicationService.model.ProfessionalMemberships.OTHER.MembershipNumber =
        this.model.MembershipNumber;
      applicationService.model.ProfessionalMemberships.OTHER.MembershipLevel =
        this.model.MembershipLevel;
      applicationService.model.ProfessionalMemberships.OTHER.MembershipYear =
        Number(this.model.MembershipYear);
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
    this.singleValidationErrors = [];
    this.isSingleMessage = false;

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
          Text: ERROR_MESSAGES.YEAR_VALIDATION_ERROR,
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

    if (this.validationErrors.length > 2) {
      this.singleValidationErrors.push({
        Text: `Enter your ${this.PROFESSIONAL_BODY_ORG_NAME} membership details`,
        Anchor: '',
      });
      this.isSingleMessage = true;
    }

    return this.validationErrors.length === 0;
  }

  getDateOfMembership(): number | undefined {
    return this.model.MembershipYear;
  }

  override async navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(
      ProfessionalIndividualMembershipDetailsComponent.route,
      this.activatedRoute,
      {membershipCode: this.membershipCode, queryParam: this.queryParam }
    ); // Back to the task list.
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

    if (this.membershipCode == 'RICS') {
      this.model.MembershipNumber = RICS.MembershipNumber ?? '';
      this.model.MembershipLevel = RICS.MembershipLevel ?? '';
      this.model.MembershipYear = RICS.MembershipYear ?? undefined;
    } else if (this.membershipCode == 'CABE') {
      this.model.MembershipNumber = CABE.MembershipNumber ?? '';
      this.model.MembershipLevel = CABE.MembershipLevel ?? '';
      this.model.MembershipYear = CABE.MembershipYear ?? undefined;
    } else if (this.membershipCode == 'CIOB') {
      this.model.MembershipNumber = CIOB.MembershipNumber ?? '';
      this.model.MembershipLevel = CIOB.MembershipLevel ?? '';
      this.model.MembershipYear = CIOB.MembershipYear ?? undefined;
    } else if (this.membershipCode == 'OTHER') {
      this.model.MembershipNumber = OTHER.MembershipNumber ?? '';
      this.model.MembershipLevel = OTHER.MembershipLevel ?? '';
      this.model.MembershipYear = OTHER.MembershipYear ?? undefined;
    } else {
      this.model.MembershipNumber = '';
      this.model.MembershipLevel = '';
      this.model.MembershipYear = undefined;
    }
  }

  private getProfessionalBodyOrgName(membershipCode: string): void {
    const professionalBodyOrgNames: { [code: string]: string } = {
      RICS: 'Royal Institution of Chartered Surveyors (RICS)',
      CABE: 'Chartered Association of Building Engineers (CABE)',
      CIOB: 'Chartered Institute of Building (CIOB)',
      OTHER: 'OTHER',
    };

    this.PROFESSIONAL_BODY_ORG_NAME =
      professionalBodyOrgNames[membershipCode] || '';
    this.PROFESSIONAL_BODY_ORG_NAME;
  }

  public getMembershipNumber(): string | undefined {
    if (this.membershipCode === 'CABE') {
      return this.applicationService.model.ProfessionalMemberships.CABE
        .MembershipNumber;
    } else if (this.membershipCode === 'RICS') {
      return this.applicationService.model.ProfessionalMemberships.RICS
        .MembershipNumber;
    } else if (this.membershipCode === 'CIOB') {
      return this.applicationService.model.ProfessionalMemberships.CIOB
        .MembershipNumber;
    } else if (this.membershipCode === 'OTHER') {
      return this.applicationService.model.ProfessionalMemberships.OTHER
        .MembershipNumber;
    } else {
      return undefined;
    }
  }

  public getMembershipLevel(): string | undefined {
    if (this.membershipCode === 'CABE') {
      return this.applicationService.model.ProfessionalMemberships.CABE
        .MembershipLevel;
    } else if (this.membershipCode === 'RICS') {
      return this.applicationService.model.ProfessionalMemberships.RICS
        .MembershipLevel;
    } else if (this.membershipCode === 'CIOB') {
      return this.applicationService.model.ProfessionalMemberships.CIOB
        .MembershipLevel;
    } else if (this.membershipCode === 'OTHER') {
      return this.applicationService.model.ProfessionalMemberships.OTHER
        .MembershipLevel;
    } else {
      return undefined;
    }
  }

  public getMembershipYear(): number | undefined {
    if (this.membershipCode === 'CABE') {
      return this.applicationService.model.ProfessionalMemberships.CABE
        .MembershipYear;
    } else if (this.membershipCode === 'RICS') {
      return this.applicationService.model.ProfessionalMemberships.RICS
        .MembershipYear;
    } else if (this.membershipCode === 'CIOB') {
      return this.applicationService.model.ProfessionalMemberships.CIOB
        .MembershipYear;
    } else if (this.membershipCode === 'OTHER') {
      return this.applicationService.model.ProfessionalMemberships.OTHER
        .MembershipYear;
    } else {
      return undefined;
    }
  }
}
