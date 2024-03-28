import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HseRoute, HseRoutes } from '../../helpers/hse.route';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ComponentsModule } from '../../components/components.module';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { ApplicationService } from '../../services/application.service';
import { HseAngularModule } from 'hse-angular';
import { CookiesBannerService } from '../../services/cookies-banner.service';
import { RBIWalesModule } from './wales/registers.rbi.wales.module';
import { RBIEnglandModule } from './england/registers.rbi.england.module';


const routes = new HseRoutes([
  HseRoute.forLoadChildren(RBIWalesModule.baseRoute, () => import('./wales/registers.rbi.wales.module').then(m => m.RBIWalesModule)),
  HseRoute.forLoadChildren(RBIEnglandModule.baseRoute, () => import('./england/registers.rbi.england.module').then(m => m.RBIEnglandModule)),
  ]);

@NgModule({
  declarations: [

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
    ApplicationService,
    CookiesBannerService,
    ...routes.getProviders(),
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class RegistersModule {
  static baseRoute: string = '';
}
