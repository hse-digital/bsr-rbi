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
  ApplicantProfessionalBodyMembership,
  ProfessionalBodies,
} from 'src/app/models/applicant-professional-body-membership';
import { ProfessionalBodySelectionComponent } from '../professional-body-selection/professional-body-selection.component';
import { ProfessionalBodyMembershipSummaryComponent } from '../professional-body-membership-summary/professional-body-membership-summary.component';
import { ProfessionalBodyMembershipsComponent } from '../professional-body-memberships/professional-body-memberships.component';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { ProfessionalBodyMembershipStep } from 'src/app/models/professional-body-membership-step.enum';

@Component({
  selector: 'hse-professional-confirmation-membership-removal',
  templateUrl: './professional-confirmation-membership-removal.component.html',
  styles: [],
})
export class ProfessionalConfirmationMembershipRemovalComponent extends PageComponent<ApplicantProfessionalBodyMembership> {
  public static route: string = 'professional-confirmation-membership-removal';
  static title: string =
    'Professional activity - Professional body removal - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  // override model?: string;
  selectedOption: string = '';
  errorMessage: string = '';
  membershipCode: string = '';
  PROFESSIONAL_BODY_ORG_NAME: string = '';
  queryParam?: string = '';

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
      this.membershipCode = params['membershipCode'];
      this.queryParam = params['queryParam'];
    });

    this.getProfessionalBodyOrgName(this.membershipCode);

    this.applicationService = applicationService;

    if (this.membershipCode === 'CABE') {
      this.model = applicationService.model.ProfessionalMemberships.CABE;
    } else if (this.membershipCode === 'RICS') {
      this.model = applicationService.model.ProfessionalMemberships.RICS;

    } else if (this.membershipCode === 'CIOB') {
      this.model = applicationService.model.ProfessionalMemberships.CIOB;
    } else if (this.membershipCode === 'OTHER') {
      this.model = applicationService.model.ProfessionalMemberships.OTHER;
    }

    this.model!.CurrentStep = ProfessionalBodyMembershipStep.Remove;
    this.model!.CompletionState = ComponentCompletionState.InProgress;





  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    const memberships = applicationService.model.ProfessionalMemberships;

    if (this.model?.CompletionState == ComponentCompletionState.Complete) {
      if (this.model.RemoveOptionSelected === 'yes') {
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
      else {
        this.model.RemoveOptionSelected = '';
        this.model.CurrentStep = ProfessionalBodyMembershipStep.ConfirmDetails;
      }
    }

    else {

      if (this.membershipCode === 'CABE') {
        memberships.CABE = this.model!;
      } else if (this.membershipCode === 'RICS') {
        memberships.RICS = this.model!;
      } else if (this.membershipCode === 'CIOB') {
        memberships.CIOB = this.model!;
      } else if (this.membershipCode === 'OTHER') {
        memberships.OTHER = this.model!;
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
    if (this.model?.RemoveOptionSelected === '') {
      this.hasErrors = true;
      this.errorMessage =
        'Select whether you want to delete a professional body membership or not';
    }

    return !this.hasErrors;
  }

  override navigateNext(): Promise<boolean> {
    const queryParam = this.queryParam;
    const memberships = this.applicationService.model.ProfessionalMemberships;
    if (
      memberships.RICS.MembershipNumber === '' &&
      memberships.CABE.MembershipNumber === '' &&
      memberships.CIOB.MembershipNumber === '' &&
      memberships.OTHER.CompletionState !== ComponentCompletionState.Complete
    ) {
      return this.navigationService.navigateRelative(
        ProfessionalBodyMembershipsComponent.route,
        this.activatedRoute,

        { queryParam }
      );
    } else {
      return this.navigationService.navigateRelative(
        ProfessionalBodyMembershipSummaryComponent.route,
        this.activatedRoute,
        { queryParam }
      );
    }
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
