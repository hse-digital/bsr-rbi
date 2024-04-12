import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { PhoneNumberValidator } from '../../../../helpers/validators/phone-number-validator';
import { ApplicationService } from '../../../../services/application.service';
import { ApplicantNationalInsuranceNumberComponent } from '../applicant-national-insurance-number/applicant-national-insurance-number.component';
import {
  PersonalDetailRoutes,
  PersonalDetailRouter,
} from '../PersonalDetailRoutes';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { ApplicantPhone } from '../../../../models/applicant-phone-model';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationSummaryComponent } from '../../5-application-submission/application-summary/application-summary.component';

@Component({
  selector: 'hse-applicant-alternative-phone',
  templateUrl: './applicant-alternative-phone.component.html',
})
export class ApplicantAlternativePhoneComponent extends PageComponent<ApplicantPhone> {
  public static route: string = PersonalDetailRoutes.ALT_PHONE;
  static title: string =
    'Personal details - Alternative phone number - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  phoneNumberHasErrors = false;
  selectedOption: string = '';
  selectedOptionError: boolean = false;
  errorMessage: string = '';
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

    if (!applicationService.model.PersonalDetails?.ApplicantAlternativePhone) {
      applicationService.model.PersonalDetails!.ApplicantAlternativePhone =
        new ApplicantPhone();
    }
    this.model =
      applicationService.model.PersonalDetails?.ApplicantAlternativePhone;

    if (this.model?.PhoneNumber === '') {
      this.selectedOption = 'no';
    } else if (this.model?.PhoneNumber) {
      this.selectedOption = 'yes';
    }
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    this.applicationService.model.PersonalDetails!.ApplicantAlternativePhone =
      this.model;
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return this.applicationService.model.id != null;
  }

  override isValid(): boolean {
    // if no option is selected, skip phone validation and save number as empty string
    if (this.selectedOption === '') {
      this.selectedOptionError = true;
      this.errorMessage =
        'Select yes if you want to provide an alternative telephone number';
      return false;
    }
    if (this.selectedOption === 'no') {
      if (this.model) {
        this.model.PhoneNumber = '';
      }
      this.modelValid = true;
      return this.modelValid;
    }
    this.phoneNumberHasErrors = !PhoneNumberValidator.isValid(
      this.model?.PhoneNumber ?? ''
    );
    this.modelValid = !this.phoneNumberHasErrors;
    if (this.phoneNumberHasErrors) {
      if (this.isNullOrWhitespace(this.model?.PhoneNumber)) {
        this.errorMessage = 'Enter your alternative telephone number';
      } else {
        this.errorMessage = 'Enter a valid mobile telephone number';
      }
    }

    return !this.phoneNumberHasErrors;
  }

  isNullOrWhitespace(input: string | null | undefined): boolean {
    return !input || input.trim().length === 0;
  }

  override navigateNext(): Promise<boolean> {
    if (this.queryParam === 'personal-details-change') {
      return this.personalDetailRouter.navigateTo(
        this.applicationService.model,
        PersonalDetailRoutes.SUMMARY
      );
    } else if (this.queryParam === 'application-summary') {
      return this.navigationService.navigateRelative(`../application-submission/${ApplicationSummaryComponent.route}`, this.activatedRoute);
    }
    else {
      return this.personalDetailRouter.navigateTo(
        this.applicationService.model,
        PersonalDetailRoutes.NATIONAL_INS_NUMBER
      );
    }
  }
}
