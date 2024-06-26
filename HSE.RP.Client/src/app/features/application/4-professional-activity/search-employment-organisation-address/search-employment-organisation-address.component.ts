import { Component, EventEmitter, Output } from '@angular/core';
import { PageComponent } from 'src/app/helpers/page.component';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { ApplicationService } from 'src/app/services/application.service';
import { environment } from 'src/environments/environment';
import { AddressModel } from 'src/app/models/address.model';
import { AddressSearchMode } from 'src/app/components/address/address.component';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { EmploymentType } from 'src/app/models/employment-type.enum';
import { ApplicationSummaryComponent } from '../../5-application-submission/application-summary/application-summary.component';

@Component({
  selector: 'hse-search-employment-organisation-address',
  templateUrl: './search-employment-organisation-address.component.html',
  styles: [],
})
export class SearchEmploymentOrganisationAddressComponent extends PageComponent<AddressModel> {
  public static route: string = 'search-employment-org-address';
  static title: string =
    'Professional activity - Employment address search - Register as a building inspector - GOV.UK';

  addressBodyText = 'This address must be in England or Wales.';
  production: boolean = environment.production;
  modelValid: boolean = false;
  searchMode = AddressSearchMode.HomeAddress;
  orgTitle? = '';
  orgFullName?: string;
  queryParam?: string = '';
  step?: string = '';
  address?: AddressModel;

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService
  ) {
    super(activatedRoute);
    this.updateOnSave = true;
  }

  override onInit(applicationService: ApplicationService): void {
    this.activatedRoute.queryParams.subscribe((params) => {
      this.queryParam = params['queryParam'];
    });

    this.orgFullName =
      this.applicationService.model.ProfessionalActivity.EmploymentDetails?.EmployerName?.FullName;

      if((this.queryParam == 'application-summary' || this.queryParam == 'professional-membership-and-employment-summary') && this.applicationService.model.ProfessionalActivity.EmploymentDetails?.EmployerAddress?.CompletionState == ComponentCompletionState.Complete )
      {
        this.step = 'confirm';
        this.address = this.applicationService.model.ProfessionalActivity.EmploymentDetails?.EmployerAddress;
      }

  }

  override async onSave(
    applicationService: ApplicationService
  ): Promise<void> {

  }
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
    this.applicationService.model.ProfessionalActivity.EmploymentDetails!.EmployerAddress =
      address;
      this.applicationService.model.ProfessionalActivity.EmploymentDetails!.CompletionState = ComponentCompletionState.Complete;
    this.applicationService.model.ProfessionalActivity.EmploymentDetails!.EmployerAddress.CompletionState =
      ComponentCompletionState.Complete;

    await this.applicationService.updateApplication();

    if (
      this.queryParam != null &&
      this.queryParam != undefined &&
      this.queryParam != ''
    ) {
      const queryParam = this.queryParam;
      if (this.queryParam == 'application-summary') {
        return this.navigationService.navigateRelative(
          `../application-submission/${ApplicationSummaryComponent.route}`,

          this.activatedRoute,
          { queryParam: this.queryParam }
        ); //update to address
      }
    }

    return this.navigationService.navigateRelative(
      `professional-membership-and-employment-summary`,
      this.activatedRoute
    );
  }

  changeStep(event: any) {
    return;
  }

  getAddressName() {
    const employerFullName =
      this.applicationService.model.ProfessionalActivity.EmploymentDetails
        ?.EmployerName?.FullName;

    if (
      this.applicationService.model.ProfessionalActivity.EmploymentDetails
        ?.EmploymentTypeSelection?.EmploymentType == EmploymentType.Other &&
      this.applicationService.model.ProfessionalActivity.EmploymentDetails
        ?.EmployerName?.OtherBusinessSelection == 'no'
    ) {
      return `Your business address`;
    }
    if (employerFullName) {
      this.orgTitle = `Select the address of ${employerFullName}`;
      return `Find the address of ${employerFullName}`;
    } else {
      this.orgTitle = 'Select your business address';
      return 'Find the address of your business';
    }
  }
}
