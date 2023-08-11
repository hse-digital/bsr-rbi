import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { PageComponent } from 'src/app/helpers/page.component';
import { ApplicationService } from 'src/app/services/application.service';
import { environment } from 'src/environments/environment';
import {
  ProfessionalActivityRouter,
  ProfessionalActivityRoutes,
} from '../ProfessionalActivityRoutes';

@Component({
  selector: 'hse-professional-individual-membership-details',
  templateUrl: './professional-individual-membership-details.component.html',
  styles: [],
})
export class ProfessionalIndividualMembershipDetailsComponent extends PageComponent<string> {
  ProfessionalActivityRoutes = ProfessionalActivityRoutes;
  public static route: string =
    ProfessionalActivityRoutes.PROFESSIONAL_INDIVIDUAL_MEMEBERSHIP_DETAILS;
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

  private getValidMembershipField(
    memberships: any,
    field: string
  ): string | number {
    const membershipOrder = ['RICS', 'CABE', 'CIOB', 'OTHER'];

    for (const membership of membershipOrder) {
      if (
        memberships[membership][field] !== '' &&
        memberships[membership][field] !== -1
      ) {
        return memberships[membership][field];
      }
    }

    return 'None';
  }

  public getMembershipNumber(): number | string {
    const memberships: any =
      this.applicationService.model.ProfessionalMemberships;
    return this.getValidMembershipField(memberships, 'MembershipNumber');
  }

  public getMembershipLevel(): number | string {
    const memberships: any =
      this.applicationService.model.ProfessionalMemberships;
    return this.getValidMembershipField(memberships, 'MembershipLevel');
  }

  public getMembershipYear(): number | string {
    const memberships: any =
      this.applicationService.model.ProfessionalMemberships;
    return this.getValidMembershipField(memberships, 'MembershipYear');
  }

  private getProfessionalBodyOrgName(membershipCode: string): string {
    const professionalBodyOrgNames: { [code: string]: string } = {
      RICS: 'Royal Institution of Chartered Surveyors (RICS)',
      CABE: 'Chartered Association of Building Engineers (CABE)',
      CIOB: 'Chartered Institute of Building (CIOB)',
      OTHER: 'OTHER',
    };

    this.PROFESSIONAL_BODY_ORG_NAME =
      professionalBodyOrgNames[membershipCode] || '';
    return this.PROFESSIONAL_BODY_ORG_NAME;
  }
}
