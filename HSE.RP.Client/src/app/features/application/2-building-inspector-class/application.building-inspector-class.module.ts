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
import { BuildingInspectorClassSelectionComponent } from './class-selection/building-inspector-class-selection.component';
import { BuildingInspectorCountryComponent } from './country/building-inspector-country.component';
import { BuildingInspectorSummaryComponent } from './building-inspector-summary/building-inspector-summary.component';
import { BuildingAssessingPlansCategoriesComponent } from './assessing-plans/building-assessing-plans-categories.component';
import { BuildingInspectorRegulatedActivitiesComponent } from './regulated-activities/building-inspector-regulated-activities.component';
import { BuildingClassTechnicalManagerComponent } from './class-technical-manager/building-class-technical-manager.component';


const routes = new HseRoutes([
  HseRoute.protected(BuildingInspectorClassSelectionComponent.route, BuildingInspectorClassSelectionComponent, BuildingInspectorClassSelectionComponent.title),
  HseRoute.protected(BuildingInspectorCountryComponent.route, BuildingInspectorCountryComponent, BuildingInspectorCountryComponent.title),
  HseRoute.protected(BuildingInspectorSummaryComponent.route, BuildingInspectorSummaryComponent, BuildingInspectorSummaryComponent.title),
  HseRoute.protected(BuildingInspectorRegulatedActivitiesComponent.route, BuildingInspectorRegulatedActivitiesComponent, BuildingInspectorRegulatedActivitiesComponent.title),
  HseRoute.protected(BuildingAssessingPlansCategoriesComponent.route, BuildingAssessingPlansCategoriesComponent, BuildingAssessingPlansCategoriesComponent.title),
  HseRoute.protected(BuildingClassTechnicalManagerComponent.route, BuildingClassTechnicalManagerComponent, BuildingClassTechnicalManagerComponent.title),
]);

@NgModule({
  declarations: [
    BuildingInspectorClassSelectionComponent,
    BuildingInspectorCountryComponent,
    BuildingInspectorSummaryComponent,
    BuildingInspectorRegulatedActivitiesComponent,
    BuildingAssessingPlansCategoriesComponent,
    BuildingClassTechnicalManagerComponent
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
export class BuildingInspectorClassModule {
  static baseRoute: string = 'building-inspector-class';
}
