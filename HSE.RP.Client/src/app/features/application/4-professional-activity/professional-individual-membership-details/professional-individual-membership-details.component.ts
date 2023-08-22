import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { PageComponent } from 'src/app/helpers/page.component';
import { ApplicationService } from 'src/app/services/application.service';
import { environment } from 'src/environments/environment';
import {
  ProfessionalActivityRouter,
  ProfessionalActivityRoutes,
} from '../ProfessionalActivityRoutes';
import { ApplicantProfessionBodyMembershipsHelper } from 'src/app/models/applicant-professional-body-membership';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { ProfessionalMembershipInformationComponent } from '../professional-membership-information/professional-membership-information.component';

@Component({
  selector: 'hse-professional-individual-membership-details',
  templateUrl: './professional-individual-membership-details.component.html',
  styles: [],
})
export class ProfessionalIndividualMembershipDetailsComponent extends PageComponent<string> {
  ProfessionalActivityRoutes = ProfessionalActivityRoutes;
  public static route: string =
    "professional-individual-membership-details";
  static title: string =
    'Professional activity - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  override model?: string;
  membershipCode: string = '';
  PROFESSIONAL_BODY_ORG_NAME: string = '';

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService,
    private professionalActivityRouter: ProfessionalActivityRouter
  ) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {
    this.activatedRoute.queryParams.subscribe((params) => {
      this.membershipCode = params['queryParams'];
      this.getProfessionalBodyOrgName(this.membershipCode);
    });
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    if (this.membershipCode === 'CABE') {
      applicationService.model.ProfessionalMemberships.CABE.CompletionState =
        ComponentCompletionState.Complete;
        applicationService.model.ProfessionalMemberships.CABE.MembershipNumber = this.getMembershipNumber() ?? '';
        applicationService.model.ProfessionalMemberships.CABE.MembershipLevel = this.getMembershipLevel() ?? '';
        applicationService.model.ProfessionalMemberships.CABE.MembershipYear = Number(this.getMembershipYear());
    } else if (this.membershipCode === 'RICS') {
      applicationService.model.ProfessionalMemberships.RICS.CompletionState =
        ComponentCompletionState.Complete;
        applicationService.model.ProfessionalMemberships.RICS.MembershipNumber = this.getMembershipNumber() ?? '';
        applicationService.model.ProfessionalMemberships.RICS.MembershipLevel = this.getMembershipLevel() ?? '';
        applicationService.model.ProfessionalMemberships.RICS.MembershipYear = Number(this.getMembershipYear());
    } else if (this.membershipCode === 'CIOB') {
      applicationService.model.ProfessionalMemberships.CIOB.CompletionState =
        ComponentCompletionState.Complete;
        applicationService.model.ProfessionalMemberships.CIOB.MembershipNumber = this.getMembershipNumber() ?? '';
        applicationService.model.ProfessionalMemberships.CIOB.MembershipLevel = this.getMembershipLevel() ?? '';
        applicationService.model.ProfessionalMemberships.CIOB.MembershipYear = Number(this.getMembershipYear());
    } else if (this.membershipCode === 'OTHER') {
      applicationService.model.ProfessionalMemberships.OTHER.CompletionState =
        ComponentCompletionState.Complete;
        applicationService.model.ProfessionalMemberships.OTHER.MembershipNumber = this.getMembershipNumber() ?? '';
        applicationService.model.ProfessionalMemberships.OTHER.MembershipLevel = this.getMembershipLevel() ?? '';
        applicationService.model.ProfessionalMemberships.OTHER.MembershipYear = Number(this.getMembershipYear());
    }
  }
  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return this.applicationService.model.ProfessionalMemberships.IsProfessionBodyRelevantYesNo === 'yes';
  }
  override isValid(): boolean {
    return true;
  }
  override async navigateNext(): Promise<boolean> {
    return this.professionalActivityRouter.navigateTo(
      this.applicationService.model,
      'professional-body-membership-summary'
    );
  }

  public navigateTo(route: string) {
    return this.navigationService.navigateRelative(
      `${route}`,
      this.activatedRoute
    );
  }


  public getMembershipNumber(): string | undefined{
      if (this.membershipCode === 'CABE') {
        return this.applicationService.model.ProfessionalMemberships.CABE.MembershipNumber;
      } else if (this.membershipCode === 'RICS') {
        return this.applicationService.model.ProfessionalMemberships.RICS.MembershipNumber;

      } else if (this.membershipCode === 'CIOB') {
        return this.applicationService.model.ProfessionalMemberships.CIOB.MembershipNumber;

      } else if (this.membershipCode === 'OTHER') {
        return this.applicationService.model.ProfessionalMemberships.OTHER.MembershipNumber;
      }
      else {
        return undefined;
      }
  }

  public getMembershipLevel(): string | undefined{
    if (this.membershipCode === 'CABE') {
      return this.applicationService.model.ProfessionalMemberships.CABE.MembershipLevel;
    } else if (this.membershipCode === 'RICS') {
      return this.applicationService.model.ProfessionalMemberships.RICS.MembershipLevel;

    } else if (this.membershipCode === 'CIOB') {
      return this.applicationService.model.ProfessionalMemberships.CIOB.MembershipLevel;

    } else if (this.membershipCode === 'OTHER') {
      return this.applicationService.model.ProfessionalMemberships.OTHER.MembershipLevel;
    }
    else {
      return undefined;
    }
  }

  public getMembershipYear(): number | undefined {
    if (this.membershipCode === 'CABE') {
      return this.applicationService.model.ProfessionalMemberships.CABE.MembershipYear;
    } else if (this.membershipCode === 'RICS') {
      return this.applicationService.model.ProfessionalMemberships.RICS.MembershipYear;

    } else if (this.membershipCode === 'CIOB') {
      return this.applicationService.model.ProfessionalMemberships.CIOB.MembershipYear;

    } else if (this.membershipCode === 'OTHER') {
      return this.applicationService.model.ProfessionalMemberships.OTHER.MembershipYear;
    }
    else {
      return undefined;
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

  public changeMembershipDetails() {
    const queryParams = this.membershipCode;
    return this.navigationService.navigateRelative(
      ProfessionalMembershipInformationComponent.route,
      this.activatedRoute,
      { queryParams }
    ); // Back to the task list.
    
  }

}
