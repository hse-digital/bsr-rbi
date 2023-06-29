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
import { PayAndSubmitPlaceholderComponent } from './pay-and-submit-placeholder/pay-and-submit-placeholder.component';
import { PayAndSubmitComponent } from './pay-and-submit-application/pay-and-submit.component';


const routes = new HseRoutes([
  HseRoute.protected(PayAndSubmitPlaceholderComponent.route, PayAndSubmitPlaceholderComponent, PayAndSubmitPlaceholderComponent.title),
  HseRoute.protected(PayAndSubmitComponent.route, PayAndSubmitComponent, PayAndSubmitComponent.title),
]);

@NgModule({
  declarations: [
  PayAndSubmitPlaceholderComponent,
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
export class PayAndSubmitModule {
  static baseRoute: string = 'pay-and-submit';
}
