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
import { ApplicationSummaryComponent } from './application-summary/application-summary.component';
import { ApplicationAdditionalInformationComponent } from './additional-information/application-information.component';
import { PaymentModule } from './payment/payment.module';


const routes = new HseRoutes([
  HseRoute.protected(ApplicationSummaryComponent.route, ApplicationSummaryComponent, ApplicationSummaryComponent.title),
  HseRoute.protected(ApplicationAdditionalInformationComponent.route, ApplicationAdditionalInformationComponent, ApplicationAdditionalInformationComponent.title),
  HseRoute.forLoadChildren(PaymentModule.baseRoute, () => import('./payment/payment.module').then(m => m.PaymentModule)),

]);

@NgModule({
  declarations: [
  ApplicationSummaryComponent,
  ApplicationAdditionalInformationComponent,
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
