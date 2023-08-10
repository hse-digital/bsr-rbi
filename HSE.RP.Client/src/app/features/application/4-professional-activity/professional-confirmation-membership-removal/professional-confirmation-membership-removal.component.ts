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

@Component({
  selector: 'hse-professional-confirmation-membership-removal',
  templateUrl: './professional-confirmation-membership-removal.component.html',
  styles: [],
})
export class ProfessionalConfirmationMembershipRemovalComponent extends PageComponent<ApplicantProfessionBodyMemberships> {
  public static route: string =
    ProfessionalActivityRoutes.PROFESSIONAL_CONFIRMATION_MEMBERSHIP_REMOVAL;
  static title: string =
    'Professional activity - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  // override model?: string;
  selectedOption: string = '';
  errorMessage: string = '';
  membershipCode: string = '';

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService,
    private professionalActivityRouter: ProfessionalActivityRouter
  ) {
    super(activatedRoute);
    this.updateOnSave = true;
    // this.activatedRoute.params.subscribe((params) => {
    //   console.log('Params Removal', params);
    //   this.membershipCode = params['membershipCode'];
    //   console.log('Membership Code Removal:', this.membershipCode);
    // });
  }

  override onInit(applicationService: ApplicationService): void {
    this.activatedRoute.queryParams.subscribe(query => {
      this.membershipCode = query['membershipCode'];
    });

    console.log('Membership Code Removal:', this.membershipCode);
    this.applicationService = applicationService;
  }
  override async onSave(applicationService: ApplicationService): Promise<void> {
    const memberships = applicationService.model.ProfessionalMemberships;

    if (this.selectedOption === 'yes') {
      let result = ApplicantProfessionBodyMembershipsHelper.Reset('CABE');
      memberships.CABE = result;
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
  override async navigateNext(): Promise<boolean> {
    return true;
  }
}
