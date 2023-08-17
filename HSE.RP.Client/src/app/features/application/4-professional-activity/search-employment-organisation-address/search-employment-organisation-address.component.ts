import { Component, EventEmitter, Output } from '@angular/core';
import { ProfessionalActivityRoutes } from '../ProfessionalActivityRoutes';
import { PageComponent } from 'src/app/helpers/page.component';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { ApplicationService } from 'src/app/services/application.service';
import { environment } from 'src/environments/environment';
import { AddressModel } from 'src/app/models/address.model';
import { AddressSearchMode } from 'src/app/components/address/address.component';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { ApplicantAlternativeEmailComponent } from '../../1-personal-details/applicant-alternative-email/applicant-alternative-email.component';

@Component({
  selector: 'hse-search-employment-organisation-address',
  templateUrl: './search-employment-organisation-address.component.html',
  styles: [],
})
export class SearchEmploymentOrganisationAddressComponent extends PageComponent<AddressModel> {
  public static route: string =
    ProfessionalActivityRoutes.SEARCH_EMPLOYEMENT_ORG_ADDRESS;
  static title: string =
    'Professional activity - Register as a building inspector - GOV.UK';

  addressBodyText = 'This address must be in England or Wales.';
  production: boolean = environment.production;
  modelValid: boolean = false;
  searchMode = AddressSearchMode.HomeAddress;
  title = 'Organisation Address';
  // step = 'find';
  // private history: string[] = [];

  // @Output() onChangeStep = new EventEmitter();

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService
  ) {
    super(activatedRoute);
    this.updateOnSave = true;
  }

  override onInit(applicationService: ApplicationService): void {}
  override async onSave(
    applicationService: ApplicationService
  ): Promise<void> {}
  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return true;
  }
  override isValid(): boolean {
    return true;
  }
  override async navigateNext(): Promise<boolean> {
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

  getAddressName() {
    return 'Find the address of [organisation name]';
  }

  // enterManualAddress() {
  //   this.changeStepTo('manual');
  // }

  // private changeStepTo(step: string) {
  //   this.history.push(this.step);
  //   this.step = step;
  //   this.resetFocus();
  //   this.onChangeStep.emit(this.step);
  // }

  // resetFocus() {
  //   const mainHeader = document.querySelector('#gouvk-header-service-name');
  //   if (mainHeader) {
  //     (mainHeader as HTMLElement).focus();
  //     (mainHeader as HTMLElement).blur();
  //   }
  // }

}
