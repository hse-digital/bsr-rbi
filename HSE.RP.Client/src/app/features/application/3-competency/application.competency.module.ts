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
import { CompetencyCertificateCodeComponent } from './certificate-code/competency-certificate-code.component';
import { CompetencyAssessmentOrganisationComponent } from './assesesment-organisation/competency-assesesment-organisation.component';
import { CompetencyAssessmentDateComponent } from './assesesment-date/competency-assesesment-date.component';
import { CompetencySummaryComponent } from './competency-summary/competency-summary.component';
import { NoCompetencyAssessmentComponent } from './no-competency-assessment/no-competency-assessment.component';


const routes = new HseRoutes([
  HseRoute.protected(CompetencyIndependentStatusComponent.route, CompetencyIndependentStatusComponent, CompetencyIndependentStatusComponent.title),
  HseRoute.protected(CompetencyCertificateCodeComponent.route, CompetencyCertificateCodeComponent, CompetencyCertificateCodeComponent.title),
  HseRoute.protected(CompetencyAssessmentOrganisationComponent.route, CompetencyAssessmentOrganisationComponent, CompetencyAssessmentOrganisationComponent.title),
  HseRoute.protected(CompetencyAssessmentDateComponent.route, CompetencyAssessmentDateComponent, CompetencyAssessmentDateComponent.title),
  HseRoute.protected(NoCompetencyAssessmentComponent.route, NoCompetencyAssessmentComponent, NoCompetencyAssessmentComponent.title),
  HseRoute.protected(CompetencySummaryComponent.route, CompetencySummaryComponent, CompetencySummaryComponent.title),
]);

@NgModule({
  declarations: [
CompetencyIndependentStatusComponent,
CompetencyCertificateCodeComponent,
CompetencyAssessmentOrganisationComponent,
CompetencyAssessmentDateComponent,
CompetencySummaryComponent,
NoCompetencyAssessmentComponent
  ],
  imports: [
    RouterModule.forChild(routes.getRoutes()),
    ComponentsModule,
    HseAngularModule,
    CommonModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [HttpClient, ApplicationService, CookiesBannerService, ...routes.getProviders()]
})
export class CompetencyModule {
  static baseRoute: string = 'competency';
}
