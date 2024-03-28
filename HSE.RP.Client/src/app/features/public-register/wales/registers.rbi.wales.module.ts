import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HseRoute, HseRoutes } from '../../../services/hse.route';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ComponentsModule } from '../../../components/components.module';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { HseAngularModule } from 'hse-angular';
import { CookiesBannerService } from '../../../services/cookies-banner.service';
import { RBISearchWalesComponent } from './rbi-search-wales.component';
import { RBIResultsWalesComponent } from './rbi-results-wales.component';
import { RBIResultDetailsWalesComponent } from './rbi-result-details-wales.component';

const routes = new HseRoutes([
  HseRoute.unsafe(
    RBISearchWalesComponent.route,
    RBISearchWalesComponent,
    undefined,
    RBISearchWalesComponent.title
  ),
  HseRoute.unsafe(
    RBIResultsWalesComponent.route,
    RBIResultsWalesComponent,
    undefined,
    RBIResultsWalesComponent.title
  ),
  HseRoute.unsafe(
    RBIResultDetailsWalesComponent.route,
    RBIResultDetailsWalesComponent,
    undefined,
    RBIResultDetailsWalesComponent.title
  ),
]);

@NgModule({
  declarations: [
    RBISearchWalesComponent,
    RBIResultsWalesComponent,
    RBIResultDetailsWalesComponent
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
    CookiesBannerService,
    ...routes.getProviders(),
  ],
})
export class RBIWalesModule {
  static baseRoute: string = 'public-register-wales';
}
