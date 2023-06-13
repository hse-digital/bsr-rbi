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

const routes = new HseRoutes([
  HseRoute.protected(ApplicationTaskListComponent.route, ApplicationTaskListComponent, ApplicationTaskListComponent.title),
  HseRoute.forLoadChildren(ApplicationPersonalDetailsModule.baseRoute, () => import('./1-personal-details/application.personal-details.module').then(m => m.ApplicationPersonalDetailsModule)),
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
