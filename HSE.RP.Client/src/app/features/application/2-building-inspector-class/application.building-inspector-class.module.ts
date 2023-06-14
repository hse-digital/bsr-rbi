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
import { BuildingInspectorClassPlaceholderComponent } from './building-inspector-class-placeholder/building-inspector-class-placeholder.component';


const routes = new HseRoutes([
  HseRoute.protected(BuildingInspectorClassPlaceholderComponent.route, BuildingInspectorClassPlaceholderComponent, BuildingInspectorClassPlaceholderComponent.title),
]);

@NgModule({
  declarations: [
    BuildingInspectorClassPlaceholderComponent,

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
