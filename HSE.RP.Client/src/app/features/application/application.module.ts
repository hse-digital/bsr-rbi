import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HseRoute, HseRoutes } from '../../helpers/hse.route';
import { RouterModule } from '@angular/router';
import { HseAngularModule } from "hse-angular";
import { FormsModule } from '@angular/forms';
import { ComponentsModule } from '../../components/components.module';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { ApplicationService } from '../../services/application.service';
import { CookiesBannerService } from '../../services/cookies-banner.service';
import { ApplicationTaskListComponent } from './task-list/task-list.component';
import { ApplicationPersonalDetailsModule } from './1-personal-details/application.personal-details.module';
import { BuildingInspectorClassModule } from './2-building-inspector-class/application.building-inspector-class.module';
import { CompetencyModule } from './3-competency/application.competency.module';
import { ProfessionalActivityModule } from './4-professional-activity/application.professional-activity.module';
import { ApplicationOverviewModule } from './5-application-overview/application.application-overview.module';
import { PayAndSubmitModule } from './6-pay-and-submit/application.pay-and-submit.module';

const routes = new HseRoutes([
  HseRoute.protected(ApplicationTaskListComponent.route, ApplicationTaskListComponent, ApplicationTaskListComponent.title),
  HseRoute.forLoadChildren(ApplicationPersonalDetailsModule.baseRoute, () => import('./1-personal-details/application.personal-details.module').then(m => m.ApplicationPersonalDetailsModule)),
  HseRoute.forLoadChildren(BuildingInspectorClassModule.baseRoute, () => import('./2-building-inspector-class/application.building-inspector-class.module').then(m => m.BuildingInspectorClassModule)),
  HseRoute.forLoadChildren(CompetencyModule.baseRoute, () => import('./3-competency/application.competency.module').then(m => m.CompetencyModule)),
  HseRoute.forLoadChildren(ProfessionalActivityModule.baseRoute, () => import('./4-professional-activity/application.professional-activity.module').then(m => m.ProfessionalActivityModule)),
  HseRoute.forLoadChildren(ApplicationOverviewModule.baseRoute, () => import('./5-application-overview/application.application-overview.module').then(m => m.ApplicationOverviewModule)),
  HseRoute.forLoadChildren(PayAndSubmitModule.baseRoute, () => import('./6-pay-and-submit/application.pay-and-submit.module').then(m => m.PayAndSubmitModule)),
]);

@NgModule({
  declarations: [
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
