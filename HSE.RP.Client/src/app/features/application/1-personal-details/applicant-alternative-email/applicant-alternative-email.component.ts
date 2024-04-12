import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { EmailValidator } from '../../../../helpers/validators/email-validator';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import {
  PersonalDetailRoutes,
  PersonalDetailRouter,
} from '../PersonalDetailRoutes';
import { ApplicationService } from '../../../../services/application.service';
import { ApplicantAlternativePhoneComponent } from '../applicant-alternative-phone/applicant-alternative-phone.component';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { ApplicantEmail } from '../../../../models/applicant-email.model';
import { ApplicationSummaryComponent } from '../../5-application-submission/application-summary/application-summary.component';

@Component({
  selector: 'hse-applicant-alternative-email',
  templateUrl: './applicant-alternative-email.component.html',
})
export class ApplicantAlternativeEmailComponent extends PageComponent<ApplicantEmail> {
  public static route: string = PersonalDetailRoutes.ALT_EMAIL;
  static title: string =
    'Personal details - Alternative email - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  emailHasErrors: boolean = false;
  emailErrorMessage: string = '';
  modelValid: boolean = false;
  selectedOption: string = '';
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

    if (!applicationService.model.PersonalDetails?.ApplicantAlternativeEmail) {
      applicationService.model.PersonalDetails!.ApplicantAlternativeEmail = {
        Email: '',
        CompletionState: ComponentCompletionState.InProgress,
      };
    }
    this.model =
      applicationService.model.PersonalDetails?.ApplicantAlternativeEmail;
    if (this.model?.Email === '') {
      this.selectedOption = 'no';
    } else if (this.model?.Email) {
      this.selectedOption = 'yes';
    }
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    this.applicationService.model.PersonalDetails!.ApplicantAlternativeEmail =
      this.model;
    if (this.selectedOption === 'no') {
      this.applicationService.model.PersonalDetails!.ApplicantAlternativeEmail!.Email =
        '';
    }
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return this.applicationService.model.id != null;
  }

  override isValid(): boolean {
    this.emailHasErrors = false;
    if (this.selectedOption === '') {
      this.emailHasErrors = true;
      this.emailErrorMessage =
        'Select yes if you want to provide an alternative email address';
    } else if (this.selectedOption === 'no') {
      return !this.emailHasErrors;
    } else if (this.model?.Email == '') {
      this.emailErrorMessage = 'The alternative email address cannot be blank';
      this.emailHasErrors = true;
    } else {
      this.emailHasErrors = !EmailValidator.isValid(this.model!.Email ?? '');
      this.emailErrorMessage = 'Enter an email address in the correct format, like name@example.com';
    }
    return !this.emailHasErrors;
  }

  navigateNext(): Promise<boolean> {
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
      PersonalDetailRoutes.ALT_PHONE
    );
  }
}
