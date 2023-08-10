import { Component } from '@angular/core';
import { PageComponent } from 'src/app/helpers/page.component';
import { ApplicantProfessionBodyMemberships } from 'src/app/models/applicant-professional-body-membership';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { ApplicationService } from 'src/app/services/application.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'hse-professional-body-selection',
  templateUrl: './professional-body-selection.component.html',
  styles: [],
})
export class ProfessionalBodySelectionComponent extends PageComponent<ApplicantProfessionBodyMemberships> {
  public static route: string =
    "professional-body-selection";
  static title: string =
    'Professional activity - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  errorMessage: string = '';
  selectedOption: string = 'OTHER';

  constructor(activatedRoute: ActivatedRoute) {
    super(activatedRoute);
  }
  override onInit(applicationService: ApplicationService): void {
    this.updateOnSave = true;

    if (!applicationService.model.ProfessionalMemberships) {
      applicationService.model.ProfessionalMemberships =
        new ApplicantProfessionBodyMemberships();
    }

    if (applicationService.model.ProfessionalMemberships == null) {
      applicationService.model.ProfessionalMemberships =
        new ApplicantProfessionBodyMemberships();
      this.selectedOption = 'OTHER';
    }

    const memberships = applicationService.model.ProfessionalMemberships;
    this.selectedOption =
      memberships.RICS.MembershipBodyCode ||
      memberships.CABE.MembershipBodyCode ||
      memberships.CIOB.MembershipBodyCode ||
      memberships.OTHER.MembershipBodyCode;
  }
  override async onSave(applicationService: ApplicationService): Promise<void> {
    const memberships = applicationService.model.ProfessionalMemberships;
    const selectedOption = this.selectedOption;

    if (['RICS', 'CABE', 'CIOB', 'OTHER'].includes(selectedOption)) {
      memberships.RICS.MembershipBodyCode =
        selectedOption === 'RICS' ? selectedOption : '';
      memberships.CABE.MembershipBodyCode =
        selectedOption === 'CABE' ? selectedOption : '';
      memberships.CIOB.MembershipBodyCode =
        selectedOption === 'CIOB' ? selectedOption : '';
      memberships.OTHER.MembershipBodyCode =
        selectedOption === 'OTHER' ? selectedOption : '';
    }
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
}
