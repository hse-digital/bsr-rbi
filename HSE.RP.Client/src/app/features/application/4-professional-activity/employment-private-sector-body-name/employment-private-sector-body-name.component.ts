import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../../services/application.service';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { ProfessionalBodyMembershipSummaryComponent } from '../professional-body-membership-summary/professional-body-membership-summary.component';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { promises } from 'fs';
import { ApplicantEmploymentDetails } from 'src/app/models/applicant-employment-details';
import { EmployerName } from 'src/app/models/employment-employer-name';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { CompaniesService } from 'src/app/services/companies.service';
import { SearchEmploymentOrganisationAddressComponent } from '../search-employment-organisation-address/search-employment-organisation-address.component';
import { EmploymentType } from 'src/app/models/employment-type.enum';

@Component({
  selector: 'hse-employment-private-sector-body-name',
  templateUrl: './employment-private-sector-body-name.component.html',
})
export class EmploymentPrivateSectorBodyNameComponent extends PageComponent<EmployerName> {
  DerivedIsComplete(value: boolean): void {}

  public static route: string = 'employment-private-sector-body-name';
  static title: string =
    'Employment - Private sector body name - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  privateSectorBodyNameHasErrors = true;
  queryParam?: string = '';

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService,
    private companiesService: CompaniesService
  ) {
    super(activatedRoute);
    this.updateOnSave = true;
  }

  override async onInit(applicationService: ApplicationService): Promise<void> {
    this.activatedRoute.queryParams.subscribe((params) => {
      this.queryParam = params['queryParam'];
    });

    if (
      !this.applicationService.model.ProfessionalActivity.EmploymentDetails
        ?.EmployerName
    ) {
      this.applicationService.model.ProfessionalActivity.EmploymentDetails!.EmployerName =
        new EmployerName();
    }

    this.model =
      this.applicationService.model.ProfessionalActivity.EmploymentDetails!.EmployerName;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {

    if(this.model?.CompletionState != ComponentCompletionState.Complete){
      this.applicationService.model.ProfessionalActivity.EmploymentDetails!.CompletionState = ComponentCompletionState.InProgress;
    }

    this.applicationService.model.ProfessionalActivity.EmploymentDetails!.EmployerName =
      this.model;

  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return (
      this.applicationService.model.ProfessionalActivity.EmploymentDetails!
        .EmploymentTypeSelection?.CompletionState ==
        ComponentCompletionState.Complete &&
      this.applicationService.model.ProfessionalActivity.EmploymentDetails!
        .EmploymentTypeSelection?.EmploymentType == EmploymentType.PrivateSector
    );
  }

  override isValid(): boolean {
    this.privateSectorBodyNameHasErrors =
      !FieldValidations.IsNotNullOrWhitespace(this.model?.FullName);

    return !this.privateSectorBodyNameHasErrors;
  }

  override navigateNext(): Promise<boolean> {
    if (
      this.queryParam != null &&
      this.queryParam != undefined &&
      this.queryParam != ''
    ) {
      const queryParam = this.queryParam;
      if (this.queryParam == 'application-summary') {
        return this.navigationService.navigateRelative(
          SearchEmploymentOrganisationAddressComponent.route,
          this.activatedRoute,
          { queryParam: this.queryParam }
        ); //update to address
      }
    }
    return this.navigationService.navigateRelative(
      SearchEmploymentOrganisationAddressComponent.route,
      this.activatedRoute
    ); //update to address
  }

  companies: string[] = [];
  async searchCompanies(company: string) {
    if (company?.length > 2) {
      var response = await this.companiesService.SearchCompany(
        company,
        'other'
      );
      this.companies = response.Companies.map((x) => x.Name);
    }
  }

  selectCompanyName(company: string) {
    this.model!.FullName = company;
  }
}
