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
import { ApplicationOverviewPlaceholderComponent } from './application-overview-placeholder/application-overview-placeholder.component';
import { ApplicationSummaryComponent } from './application-summary/application-summary.component';
import { PayAndSubmitComponent } from './pay-and-submit-application/pay-and-submit-placeholder.component';


const routes = new HseRoutes([
  HseRoute.protected(ApplicationOverviewPlaceholderComponent.route, ApplicationOverviewPlaceholderComponent, ApplicationOverviewPlaceholderComponent.title),
  HseRoute.protected(ApplicationSummaryComponent.route, ApplicationSummaryComponent, ApplicationSummaryComponent.title),
  HseRoute.protected(PayAndSubmitComponent.route, PayAndSubmitComponent, PayAndSubmitComponent.title),
]);

@NgModule({
  declarations: [
  ApplicationOverviewPlaceholderComponent,
  ApplicationSummaryComponent,
  PayAndSubmitComponent
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
export class ApplicationSubmissionModule {
  static baseRoute: string = 'application-submission';
}
