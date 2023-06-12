import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HseRoute, HseRoutes } from '../../helpers/hse.route';
import { ApplicationNameComponent } from './application-name/application-name.component';
import { ApplicationInfoComponent } from './application-info/application-info.component';
import { ApplicantPhoneComponent } from './applicant-phone/applicant-phone.component';
import { ApplicantNameComponent } from './applicant-name/applicant-name.component';
import { ApplicantEmailComponent } from './applicant-email/applicant-email.component';
import { RouterModule } from '@angular/router';

import { FormsModule } from '@angular/forms';
import { ComponentsModule } from '../../components/components.module';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { ApplicationService } from '../../services/application.service';
import { HseAngularModule } from 'hse-angular';
import { CookiesBannerService } from '../../services/cookies-banner.service';
import { ApplicationTaskListComponent } from './task-list/task-list.component';

const routes = new HseRoutes([
  HseRoute.protected(ApplicationNameComponent.route, ApplicationNameComponent, ApplicationNameComponent.title),
  HseRoute.unsafe(ApplicationInfoComponent.route, ApplicationInfoComponent, undefined, ApplicationInfoComponent.title),
  HseRoute.protected(ApplicantPhoneComponent.route, ApplicantPhoneComponent, ApplicantPhoneComponent.title),
  HseRoute.protected(ApplicantNameComponent.route, ApplicantNameComponent, ApplicantNameComponent.title),
  HseRoute.protected(ApplicantEmailComponent.route, ApplicantEmailComponent, ApplicantEmailComponent.title),
  HseRoute.protected(ApplicationTaskListComponent.route, ApplicationTaskListComponent, ApplicationTaskListComponent.title),

]);

@NgModule({
  declarations: [
    ApplicationInfoComponent,
    ApplicantPhoneComponent,
    ApplicantNameComponent,
    ApplicantEmailComponent,
    ApplicationNameComponent,
    ApplicationTaskListComponent,

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
export class ApplicationModule {
  static baseRoute: string = 'application';

}
