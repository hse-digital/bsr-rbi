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

@Component({
  selector: 'hse-employment-private-sector-body-name',
  templateUrl: './employment-private-sector-body-name.component.html',
})
export class EmploymentPrivateSectorBodyNameComponent extends PageComponent<EmployerName> {
  DerivedIsComplete(value: boolean): void {

  }

  public static route: string = "employment-private-sector-body-name";
  static title: string = "Employment - Register as a building inspector - GOV.UK";
  production: boolean = environment.production;
  modelValid: boolean = false;
  privateSectorBodyNameHasErrors = false;

  constructor(activatedRoute: ActivatedRoute, applicationService: ApplicationService, private companiesService: CompaniesService) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override async onInit(applicationService: ApplicationService): Promise<void> {


    if(!this.applicationService.model.EmploymentDetails)
    {
      this.applicationService.model.EmploymentDetails = new ApplicantEmploymentDetails()
    }

      this.model = this.applicationService.model.EmploymentDetails.EmployerName;


  }

  override async onSave(applicationService: ApplicationService): Promise<void> {

    this.model!.CompletionState = ComponentCompletionState.Complete;
    this.applicationService.model.EmploymentDetails.EmployerName=this.model;

   }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return true;
    //return (FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.firstName) || FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.lastName));

  }


  override isValid(): boolean {

    this.privateSectorBodyNameHasErrors = !FieldValidations.IsNotNullOrWhitespace(this.model?.EmployerName)

    return !this.privateSectorBodyNameHasErrors;
  }

  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(ProfessionalBodyMembershipSummaryComponent.route, this.activatedRoute); //update to address
  }

  companies: string[] = [];
  async searchCompanies(company: string) {
    if (company?.length > 2) {
      var response = await this.companiesService.SearchCompany(company, "other");
      this.companies = response.Companies.map(x => x.Name);
    }
  }

  selectCompanyName(company: string) {
    this.model!.EmployerName = company;
  }


}
