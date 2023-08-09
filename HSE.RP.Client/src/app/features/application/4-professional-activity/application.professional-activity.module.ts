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
import { ProfessionalActivitySummaryComponent } from './professional-activity-summary/professional-activity-summary.component';
import { ProfessionalBodySelectionComponent } from './professional-body-selection/professional-body-selection.component';
import { ProfessionalMembershipInformationComponent } from './professional-membership-information/professional-membership-information.component';
import { ProfessionalIndividualMembershipDetailsComponent } from './professional-individual-membership-details/professional-individual-membership-details.component';

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
    ProfessionalActivitySummaryComponent.route,
    ProfessionalActivitySummaryComponent,
    ProfessionalActivitySummaryComponent.title
  ),
  HseRoute.protected(
    ProfessionalBodySelectionComponent.route,
    ProfessionalBodySelectionComponent,
    ProfessionalBodySelectionComponent.title
  ),
  HseRoute.protected(
    ProfessionalMembershipInformationComponent.route,
    ProfessionalMembershipInformationComponent,
    ProfessionalMembershipInformationComponent.title
  ),
  HseRoute.protected(
    ProfessionalIndividualMembershipDetailsComponent.route,
    ProfessionalIndividualMembershipDetailsComponent,
    ProfessionalIndividualMembershipDetailsComponent.title
  ),
]);

@NgModule({
  declarations: [
    ProfessionalBodyMembershipsComponent,
    ProfessionalActivityEmploymentTypeComponent,
    ProfessionalActivityEmploymentDetailsComponent,
    ProfessionalActivitySummaryComponent,
    ProfessionalBodySelectionComponent,
    ProfessionalMembershipInformationComponent,
    ProfessionalIndividualMembershipDetailsComponent,
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
