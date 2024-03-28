import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HseRoute, HseRoutes } from '../../../services/hse.route';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ComponentsModule } from '../../../components/components.module';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { HseAngularModule } from 'hse-angular';
import { CookiesBannerService } from '../../../services/cookies-banner.service';
import { RBISearchEnglandComponent } from './rbi-search-england.component';
import { RBIResultsEnglandComponent } from './rbi-results-england.component';
import { RBIResultDetailsEnglandComponent } from './rbi-result-details-england.component';

const routes = new HseRoutes([
  HseRoute.unsafe(
    RBISearchEnglandComponent.route,
    RBISearchEnglandComponent,
    undefined,
    RBISearchEnglandComponent.title
  ),
  HseRoute.unsafe(
    RBIResultsEnglandComponent.route,
    RBIResultsEnglandComponent,
    undefined,
    RBIResultsEnglandComponent.title
  ),
  HseRoute.unsafe(
    RBIResultDetailsEnglandComponent.route,
    RBIResultDetailsEnglandComponent,
    undefined,
    RBIResultDetailsEnglandComponent.title
  ),
]);

@NgModule({
    declarations: [
        RBISearchEnglandComponent,
        RBIResultsEnglandComponent,
        RBIResultDetailsEnglandComponent
    ],
    providers: [
        HttpClient,
        CookiesBannerService,
        ...routes.getProviders(),
    ],
    imports: [
        RouterModule.forChild(routes.getRoutes()),
        ComponentsModule,
        HseAngularModule,
        CommonModule,
        FormsModule,
        HttpClientModule
        
    ]
})
export class RBIEnglandModule {
  static baseRoute: string = 'public-register-england';
}
