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
import { CompetencyIndependentStatusComponent } from './independent-competency-status/independent-competency-status.component';
import { CompetencyAssessmentOrganisationComponent } from './assesesment-organisation/competency-assesesment-organisation.component';
import { CompetencyAssessmentDateComponent } from './assesesment-date/competency-assesesment-date.component';
import { CompetencySummaryComponent } from './competency-summary/competency-summary.component';
import { NoCompetencyAssessmentComponent } from './no-competency-assessment/no-competency-assessment.component';
import { CompetencyAssessmentCertificateNumberComponent } from './assessment-certificate-number/competency-assessment-certificate-number.component';
import { ConfirmationIaUpdateComponent } from './confirmation-ia-update/confirmation-ia-update.component';
import { BuildingInspectorClassModule } from '../2-building-inspector-class/application.building-inspector-class.module';

const routes = new HseRoutes([
  HseRoute.protected(
    CompetencyIndependentStatusComponent.route,
    CompetencyIndependentStatusComponent,
    CompetencyIndependentStatusComponent.title
  ),
  HseRoute.protected(
    CompetencyAssessmentOrganisationComponent.route,
    CompetencyAssessmentOrganisationComponent,
    CompetencyAssessmentOrganisationComponent.title
  ),
  HseRoute.protected(
    CompetencyAssessmentCertificateNumberComponent.route,
    CompetencyAssessmentCertificateNumberComponent,
    CompetencyAssessmentCertificateNumberComponent.title
  ),
  HseRoute.protected(
    CompetencyAssessmentDateComponent.route,
    CompetencyAssessmentDateComponent,
    CompetencyAssessmentDateComponent.title
  ),
  HseRoute.protected(
    NoCompetencyAssessmentComponent.route,
    NoCompetencyAssessmentComponent,
    NoCompetencyAssessmentComponent.title
  ),
  HseRoute.protected(
    CompetencySummaryComponent.route,
    CompetencySummaryComponent,
    CompetencySummaryComponent.title
  ),
  HseRoute.protected(
    ConfirmationIaUpdateComponent.route,
    ConfirmationIaUpdateComponent,
    ConfirmationIaUpdateComponent.title
  ),
]);

@NgModule({
  declarations: [
    CompetencyIndependentStatusComponent,
    CompetencyAssessmentOrganisationComponent,
    CompetencyAssessmentCertificateNumberComponent,
    CompetencyAssessmentDateComponent,
    CompetencySummaryComponent,
    NoCompetencyAssessmentComponent,
    ConfirmationIaUpdateComponent,
  ],
  imports: [
    RouterModule.forChild(routes.getRoutes()),
    ComponentsModule,
    HseAngularModule,
    CommonModule,
    FormsModule,
    HttpClientModule,
    BuildingInspectorClassModule,
  ],
  providers: [
    HttpClient,
    ApplicationService,
    CookiesBannerService,
    ...routes.getProviders(),
  ],
})
export class CompetencyModule {
  static baseRoute: string = 'competency';
}
