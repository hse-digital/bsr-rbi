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
import { ProfessionalActivityPlaceholderComponent } from './professional-activity-placeholder/professional-activity-placeholder.component';


const routes = new HseRoutes([
  HseRoute.protected(ProfessionalActivityPlaceholderComponent.route, ProfessionalActivityPlaceholderComponent, ProfessionalActivityPlaceholderComponent.title),
]);

@NgModule({
  declarations: [
ProfessionalActivityPlaceholderComponent
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
export class ProfessionalActivityModule {
  static baseRoute: string = 'professional-activity';
}
