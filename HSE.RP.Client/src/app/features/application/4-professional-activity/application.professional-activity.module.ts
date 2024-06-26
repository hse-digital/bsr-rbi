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
import { ProfessionalBodyMembershipSummaryComponent } from './professional-body-membership-summary/professional-body-membership-summary.component';
import { ProfessionalBodySelectionComponent } from './professional-body-selection/professional-body-selection.component';
import { ProfessionalMembershipInformationComponent } from './professional-membership-information/professional-membership-information.component';
import { ProfessionalIndividualMembershipDetailsComponent } from './professional-individual-membership-details/professional-individual-membership-details.component';
import { ProfessionalConfirmationMembershipRemovalComponent } from './professional-confirmation-membership-removal/professional-confirmation-membership-removal.component';
import { EmploymentPublicSectorBodyNameComponent } from './employment-public-sector-body-name/employment-public-sector-body-name.component';
import { EmploymentPrivateSectorBodyNameComponent } from './employment-private-sector-body-name/employment-private-sector-body-name.component';
import { SearchEmploymentOrganisationAddressComponent } from './search-employment-organisation-address/search-employment-organisation-address.component';
import { EmploymentOtherNameComponent } from './employment-other-name/employment-other-name.component';
import { ProfessionalMembershipAndEmploymentSummaryComponent } from './professional-membership-and-employment-summary/professional-membership-and-employment-summary.component';

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
    EmploymentPublicSectorBodyNameComponent.route,
    EmploymentPublicSectorBodyNameComponent,
    EmploymentPublicSectorBodyNameComponent.title
  ),
  HseRoute.protected(
    EmploymentPrivateSectorBodyNameComponent.route,
    EmploymentPrivateSectorBodyNameComponent,
    EmploymentPrivateSectorBodyNameComponent.title
  ),
  HseRoute.protected(
    SearchEmploymentOrganisationAddressComponent.route,
    SearchEmploymentOrganisationAddressComponent,
    SearchEmploymentOrganisationAddressComponent.title
  ),
  HseRoute.protected(
    EmploymentOtherNameComponent.route,
    EmploymentOtherNameComponent,
    EmploymentOtherNameComponent.title
  ),
  HseRoute.protected(
    ProfessionalMembershipAndEmploymentSummaryComponent.route,
    ProfessionalMembershipAndEmploymentSummaryComponent,
    ProfessionalMembershipAndEmploymentSummaryComponent.title
  ),
]);

@NgModule({
  declarations: [
    ProfessionalBodyMembershipsComponent,
    ProfessionalActivityEmploymentTypeComponent,
    ProfessionalBodyMembershipSummaryComponent,
    ProfessionalBodySelectionComponent,
    ProfessionalMembershipInformationComponent,
    ProfessionalIndividualMembershipDetailsComponent,
    ProfessionalConfirmationMembershipRemovalComponent,
    EmploymentPublicSectorBodyNameComponent,
    EmploymentPrivateSectorBodyNameComponent,
    EmploymentOtherNameComponent,
    SearchEmploymentOrganisationAddressComponent,
    ProfessionalMembershipAndEmploymentSummaryComponent,
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
