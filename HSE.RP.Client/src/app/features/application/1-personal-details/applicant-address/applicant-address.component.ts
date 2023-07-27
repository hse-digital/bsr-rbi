import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../../services/application.service';
import { ApplicantAlternativeEmailComponent } from '../applicant-alternative-email/applicant-alternative-email.component';

import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { AddressSearchMode } from 'src/app/components/address/address.component';
import { NotFoundComponent } from 'src/app/components/not-found/not-found.component';
import { NavigationService } from 'src/app/services/navigation.service';
import { TitleService } from 'src/app/services/title.service';
import { ApplicantAlternativePhoneComponent } from '../applicant-alternative-phone/applicant-alternative-phone.component';
import { TaskStatus } from '../../task-list/task-list.component';
import {
  PersonalDetailRoutes,
  PersonalDetailRouter,
} from '../PersonalDetailRoutes';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { AddressModel } from 'src/app/models/address.model';

@Component({
  selector: 'hse-applicant-address',
  templateUrl: './applicant-address.component.html',
})
export class ApplicantAddressComponent extends PageComponent<AddressModel> {
  public static route: string = PersonalDetailRoutes.ADDRESS;
  static title: string =
    'Personal details - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  searchMode = AddressSearchMode.HomeAddress;

  @Input() addressName?: string;

  constructor(
    applicationService: ApplicationService,
    navigationService: NavigationService,
    activatedRoute: ActivatedRoute,
    titleService: TitleService,
    private personalDetailRouter: PersonalDetailRouter
  ) {
    super(activatedRoute);
    this.updateOnSave = true;
  }

  override onInit(applicationService: ApplicationService): void {
    this.model = applicationService.model.PersonalDetails?.ApplicantAddress;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    this.applicationService.model.PersonalDetails!.ApplicantAddress =
      this.model;
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return (
      this.applicationService.model.ApplicationStatus >=
        ApplicationStatus.PhoneVerified &&
      this.applicationService.model.id != null
    );
  }

  override DerivedIsComplete(value: boolean) {
    this.applicationService.model.PersonalDetails!.ApplicantAddress!.CompletionState =
      value
        ? ComponentCompletionState.Complete
        : ComponentCompletionState.InProgress;
  }

  override isValid(): boolean {
    return true;
  }
  async addressConfirmed(address: AddressModel) {
    this.applicationService.model.PersonalDetails!.ApplicantAddress = address;
    this.applicationService.model.PersonalDetails!.ApplicantAddress.CompletionState =
      ComponentCompletionState.Complete;

    await this.applicationService.updateApplication();

    this.navigationService.navigateRelative(
      ApplicantAlternativeEmailComponent.route,
      this.activatedRoute
    );
  }

  changeStep(event: any) {
    return;
  }

  override navigateNext(): Promise<boolean> {
    return this.personalDetailRouter.navigateTo(
      this.applicationService.model,
      PersonalDetailRoutes.ALT_PHONE
    );
  }
}
