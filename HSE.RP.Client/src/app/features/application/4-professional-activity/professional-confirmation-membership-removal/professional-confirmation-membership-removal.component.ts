import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { PageComponent } from 'src/app/helpers/page.component';
import { ApplicationService } from 'src/app/services/application.service';
import {
  ProfessionalActivityRouter,
  ProfessionalActivityRoutes,
} from '../ProfessionalActivityRoutes';
import { environment } from 'src/environments/environment';
import {
  ApplicantProfessionBodyMemberships,
  ApplicantProfessionBodyMembershipsHelper,
  ProfessionalBodies,
} from 'src/app/models/applicant-professional-body-membership';
import { ProfessionalBodySelectionComponent } from '../professional-body-selection/professional-body-selection.component';
import { ProfessionalBodyMembershipSummaryComponent } from '../professional-body-membership-summary/professional-body-membership-summary.component';

@Component({
  selector: 'hse-professional-confirmation-membership-removal',
  templateUrl: './professional-confirmation-membership-removal.component.html',
  styles: [],
})
export class ProfessionalConfirmationMembershipRemovalComponent extends PageComponent<ApplicantProfessionBodyMemberships> {
  public static route: string = "professional-confirmation-membership-removal";
  static title: string =
    'Professional activity - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  // override model?: string;
  selectedOption: string = '';
  errorMessage: string = '';
  membershipCode: string = '';
  PROFESSIONAL_BODY_ORG_NAME: string = '';

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService,
    private professionalActivityRouter: ProfessionalActivityRouter
  ) {
    super(activatedRoute);
    this.updateOnSave = true;
  }

  override onInit(applicationService: ApplicationService): void {
    this.activatedRoute.queryParams.subscribe((params) => {
      this.membershipCode = params['queryParams'];
    });

    this.getProfessionalBodyOrgName(this.membershipCode)

    this.applicationService = applicationService;
  }
  override async onSave(applicationService: ApplicationService): Promise<void> {
    const memberships = applicationService.model.ProfessionalMemberships;

    if (this.selectedOption === 'yes') {
      let result = ApplicantProfessionBodyMembershipsHelper.Reset(
        this.membershipCode
      );
      if (this.membershipCode === 'CABE') {
        memberships.CABE = result;
      } else if (this.membershipCode === 'RICS') {
        memberships.RICS = result;
      } else if (this.membershipCode === 'CIOB') {
        memberships.CIOB = result;
      } else if (this.membershipCode === 'OTHER') {
        memberships.OTHER = result;
      }
      applicationService.model.ProfessionalMemberships = memberships;
    }
  }
  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return true;
  }
  override isValid(): boolean {
    this.hasErrors = false;
    this.errorMessage = '';
    if (this.selectedOption === '') {
      this.hasErrors = true;
      this.errorMessage = 'Select one option';
    }

    return !this.hasErrors;
  }

  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(
      ProfessionalBodyMembershipSummaryComponent.route,
      this.activatedRoute
    );
  }

  private getProfessionalBodyOrgName(membershipCode: string): void {
    const professionalBodyOrgNames: { [code: string]: string } = {
      RICS: 'Royal Institution of Chartered Surveyors (RICS)',
      CABE: 'Chartered Association of Building Engineers (CABE)',
      CIOB: 'Chartered Institute of Building (CIOB)',
      OTHER: 'OTHER',
    };

    this.PROFESSIONAL_BODY_ORG_NAME =
      professionalBodyOrgNames[membershipCode] || '';
    this.PROFESSIONAL_BODY_ORG_NAME;
  }
}
