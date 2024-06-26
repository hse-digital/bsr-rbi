import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../../services/application.service';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import {
  PersonalDetailRoutes,
  PersonalDetailRouter,
} from '../PersonalDetailRoutes';
import { ApplicantName } from 'src/app/models/applicant-name.model';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { ApplicationSummaryComponent } from '../../5-application-submission/application-summary/application-summary.component';

@Component({
  selector: 'hse-applicant-name',
  templateUrl: './applicant-name.component.html',
})
export class ApplicantNameComponent extends PageComponent<ApplicantName> {
  public static route: string = PersonalDetailRoutes.NAME;
  static title: string =
    'Your name - Apply for building control approval for a higher-risk building - GOV.UK';
  production: boolean = environment.production;
  FirstNameValid: boolean = false;
  LastNameValid: boolean = false;
  queryParam?: string = '';

  override model: ApplicantName = new ApplicantName();

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

    if (!applicationService.model.PersonalDetails) {
      applicationService.model.PersonalDetails = {};
    }
    if (applicationService.model.PersonalDetails?.ApplicantName == null) {
      applicationService.model.PersonalDetails!.ApplicantName =
        new ApplicantName();
    }
    this.model.FirstName =
      applicationService.model.PersonalDetails?.ApplicantName?.FirstName ?? '';
    this.model.LastName =
      applicationService.model.PersonalDetails?.ApplicantName?.LastName ?? '';
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    applicationService.model.PersonalDetails!.ApplicantName! = this.model;
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return true;
  }

  override isValid(): boolean {
    this.FirstNameValid = FieldValidations.IsNotNullOrWhitespace(
      this.model.FirstName
    );
    this.LastNameValid = FieldValidations.IsNotNullOrWhitespace(
      this.model.LastName
    );

    if (!this.FirstNameValid && !this.LastNameValid) {
    }

    return this.FirstNameValid && this.LastNameValid;
  }

  override navigateNext(): Promise<boolean> {
    if (this.queryParam === 'personal-details-change') {
      return this.personalDetailRouter.navigateTo(
        this.applicationService.model,
        PersonalDetailRoutes.SUMMARY
      );
    } else if (this.queryParam === 'application-summary') {
      return this.navigationService.navigateRelative(`../application-submission/${ApplicationSummaryComponent.route}`, this.activatedRoute);
    } {
      return this.personalDetailRouter.navigateTo(
        this.applicationService.model,
        PersonalDetailRoutes.TASK_LIST
      );
    }
  }
}
