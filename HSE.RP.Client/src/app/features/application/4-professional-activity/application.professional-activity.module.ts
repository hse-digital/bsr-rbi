import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HseRoute, HseRoutes } from '../../../helpers/hse.route';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ComponentsModule } from '../../../components/components.module';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { ApplicationService } from '../../../services/application.service';
import { HseAngularModule } from 'hse-angular';
import { CookiesBannerService } from '../../../services/cookies-banner.service';
import { ProfessionalBodyMembershipsComponent } from './professional-body-memberships/professional-body-memberships.component';
import { ProfessionalActivityEmploymentTypeComponent } from './employment-type/professional-activity-employment-type.component';
import { ProfessionalActivityEmploymentDetailsComponent } from './employment-details/professional-activity-employment-details.component';
import { ProfessionalBodyMembershipSummaryComponent } from './professional-body-membership-summary/professional-body-membership-summary.component';
import { ProfessionalBodySelectionComponent } from './professional-body-selection/professional-body-selection.component';
import { ProfessionalMembershipInformationComponent } from './professional-membership-information/professional-membership-information.component';
import { ProfessionalIndividualMembershipDetailsComponent } from './professional-individual-membership-details/professional-individual-membership-details.component';
import { ProfessionalConfirmationMembershipRemovalComponent } from './professional-confirmation-membership-removal/professional-confirmation-membership-removal.component';
import { SearchEmploymentOrganisationAddressComponent } from './search-employment-organisation-address/search-employment-organisation-address.component';

const routes = new HseRoutes([
  HseRoute.protected(
    ProfessionalBodyMembershipsComponent.route,
    ProfessionalBodyMembershipsComponent,
    ProfessionalBodyMembershipsComponent.title
  ),
  HseRoute.protected(
    ProfessionalActivityEmploymentTypeComponent.route,
    ProfessionalActivityEmploymentTypeComponent,
    ProfessionalActivityEmploymentTypeComponent.title
  ),
  HseRoute.protected(
    ProfessionalActivityEmploymentDetailsComponent.route,
    ProfessionalActivityEmploymentDetailsComponent,
    ProfessionalActivityEmploymentDetailsComponent.title
  ),
  HseRoute.protected(
    ProfessionalBodyMembershipSummaryComponent.route,
    ProfessionalBodyMembershipSummaryComponent,
    ProfessionalBodyMembershipSummaryComponent.title
  ),
  HseRoute.protected(
    ProfessionalBodySelectionComponent.route,
    ProfessionalBodySelectionComponent,
    ProfessionalBodySelectionComponent.title
  ),
  HseRoute.protected(
    ProfessionalBodyMembershipsComponent.route,
    ProfessionalBodyMembershipsComponent,
    ProfessionalBodyMembershipsComponent.title
  ),
  HseRoute.protected(
    ProfessionalIndividualMembershipDetailsComponent.route,
    ProfessionalIndividualMembershipDetailsComponent,
    ProfessionalIndividualMembershipDetailsComponent.title
  ),
  HseRoute.protected(
    ProfessionalConfirmationMembershipRemovalComponent.route,
    ProfessionalConfirmationMembershipRemovalComponent,
    ProfessionalConfirmationMembershipRemovalComponent.title
  ),
  HseRoute.protected(
    ProfessionalMembershipInformationComponent.route,
    ProfessionalMembershipInformationComponent,
    ProfessionalMembershipInformationComponent.title
  ),
  HseRoute.protected(
    SearchEmploymentOrganisationAddressComponent.route,
    SearchEmploymentOrganisationAddressComponent,
    SearchEmploymentOrganisationAddressComponent.title
  ),
]);

@NgModule({
  declarations: [
    ProfessionalBodyMembershipsComponent,
    ProfessionalActivityEmploymentTypeComponent,
    ProfessionalActivityEmploymentDetailsComponent,
    ProfessionalBodyMembershipSummaryComponent,
    ProfessionalBodySelectionComponent,
    ProfessionalMembershipInformationComponent,
    ProfessionalIndividualMembershipDetailsComponent,
    ProfessionalConfirmationMembershipRemovalComponent,
    SearchEmploymentOrganisationAddressComponent,
  ],
  imports: [
    RouterModule.forChild(routes.getRoutes()),
    ComponentsModule,
    HseAngularModule,
    CommonModule,
    FormsModule,
    HttpClientModule,
  ],
  providers: [
    HttpClient,
    ApplicationService,
    CookiesBannerService,
    ...routes.getProviders(),
  ],
})
export class ProfessionalActivityModule {
  static baseRoute: string = 'professional-activity';
}
