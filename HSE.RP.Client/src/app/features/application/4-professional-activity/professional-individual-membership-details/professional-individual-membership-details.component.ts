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

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService,
    private professionalActivityRouter: ProfessionalActivityRouter
  ) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {}
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
      "professional-activity-summary"
    );
  }

  public navigateTo(route: string) {
    return this.navigationService.navigateRelative(
      `${route}`,
      this.activatedRoute
    );
  }

  public getMembershipNumber(): string {
    const memberships: any =
      this.applicationService.model.ProfessionalMemberships;

    const membershipOrder = ['RICS', 'CABE', 'CIOB', 'OTHER'];

    for (const membership of membershipOrder) {
      if (memberships[membership].MembershipNumber !== '') {
        return memberships[membership].MembershipNumber;
      }
    }

    return 'None';
  }

  public getMembershipLevel(): string {
    const memberships: any =
      this.applicationService.model.ProfessionalMemberships;

    const membershipOrder = ['RICS', 'CABE', 'CIOB', 'OTHER'];

    for (const membership of membershipOrder) {
      if (memberships[membership].MembershipLevel !== '') {
        return memberships[membership].MembershipLevel;
      }
    }

    return 'None';
  }

  public getMembershipYear(): string {
    const memberships: any =
      this.applicationService.model.ProfessionalMemberships;

    const membershipOrder = ['RICS', 'CABE', 'CIOB', 'OTHER'];

    for (const membership of membershipOrder) {
      if (memberships[membership].MembershipYear !== -1) {
        return memberships[membership].MembershipYear;
      }
    }

    return 'None';
  }
}
