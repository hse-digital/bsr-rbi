import { Component, EventEmitter, Output } from '@angular/core';
import { PageComponent } from 'src/app/helpers/page.component';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { ApplicationService } from 'src/app/services/application.service';
import { environment } from 'src/environments/environment';
import { AddressModel } from 'src/app/models/address.model';
import { AddressSearchMode } from 'src/app/components/address/address.component';

@Component({
  selector: 'hse-search-employment-organisation-address',
  templateUrl: './search-employment-organisation-address.component.html',
  styles: [],
})
export class SearchEmploymentOrganisationAddressComponent extends PageComponent<AddressModel> {
  public static route: string = 'search-empoloyment-org-address';
  static title: string =
    'Professional activity - Register as a building inspector - GOV.UK';

  addressBodyText = 'This address must be in England or Wales.';
  production: boolean = environment.production;
  modelValid: boolean = false;
  searchMode = AddressSearchMode.HomeAddress;
  orgTitle? = '';
  orgFullName?: string;

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService
  ) {
    super(activatedRoute);
    this.updateOnSave = true;
  }

  override onInit(applicationService: ApplicationService): void {
    this.orgFullName =
      this.applicationService.model.ProfessionalActivity.EmploymentDetails?.EmployerName?.FullName;
  }
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

  addressConfirmed(address: AddressModel) {
    return this.navigationService.navigate(
      `application/${this.applicationService.model.id}`
    );
  }

  changeStep(event: any) {
    return;
  }

  getAddressName() {
    const employerFullName =
      this.applicationService.model.ProfessionalActivity.EmploymentDetails
        ?.EmployerName?.FullName;

    if (employerFullName) {
      this.orgTitle = `Select the address of ${employerFullName}`;
      return `Find the address of ${employerFullName}`;
    } else {
      this.orgTitle = 'Select your business address';
      return 'Find the address of your business';
    }
  }
}
