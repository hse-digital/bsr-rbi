import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Injector, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HseAngularModule } from 'hse-angular';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { SampleComponent } from './features/sample.component';
import { HseRoute, HseRoutes } from './helpers/hse.route';
import { TimeoutComponent } from './components/timeout/timeout.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { TimeoutModalComponent } from './components/timeout/timeout.modal';
import { RouterModule } from '@angular/router';
import { HelpPagesModule } from './components/footer/help-pages.module';
import { ApplicationService } from './services/application.service';
import { CommonModule } from '@angular/common';
import { ComponentsModule } from './components/components.module';
import { GetInjector } from './helpers/injector.helper';
import { CookiesBannerService } from './services/cookies-banner.service';
import { HomeComponent } from './features/home/home.component';
import { ApplicationSelectorComponent } from './features/application-selector/application-selector.component';
import { ApplicationModule } from './features/application/application.module';
import { ReturningApplicationComponent } from './features/returning-application/returning-application.component';
import { ReturningApplicationEnterDataComponent } from './features/returning-application/enterdata.component';
import { ReturningApplicationResendCodeComponent } from './features/returning-application/resend.component';
import { ReturningApplicationVerifyComponent } from './features/returning-application/verify.component';
import { NewApplicationModule } from './features/new-application/new-application.module';
import { ApplicationSubmissionComponent } from './features/returning-application/application-submission/application-submission.component';
import { A11yModule } from '@angular/cdk/a11y';
import { GovUKDataModule } from './gov-uk-data/gov-uk-data.module';

const routes = new HseRoutes([
  HseRoute.protected(SampleComponent.route, SampleComponent, SampleComponent.title),
  HseRoute.unsafe(HomeComponent.route, HomeComponent, undefined, HomeComponent.title),
  HseRoute.unsafe(ApplicationSelectorComponent.route, ApplicationSelectorComponent, undefined, ApplicationSelectorComponent.title),
  HseRoute.forLoadChildren(HelpPagesModule.baseRoute, () => import('./components/footer/help-pages.module').then(m => m.HelpPagesModule)),
  HseRoute.forLoadChildren(ApplicationModule.baseRoute, () => import('./features/application/application.module').then(m => m.ApplicationModule)),
  HseRoute.forLoadChildren(NewApplicationModule.baseRoute, () => import('./features/new-application/new-application.module').then(m => m.NewApplicationModule)),
  HseRoute.unsafe(NotFoundComponent.route, NotFoundComponent, undefined, NotFoundComponent.title),
  HseRoute.protected(ReturningApplicationComponent.route, ReturningApplicationComponent, ReturningApplicationComponent.title),
  HseRoute.protected(ApplicationSubmissionComponent.route, ApplicationSubmissionComponent, ApplicationSubmissionComponent.title),
  HseRoute.protected(TimeoutComponent.route, TimeoutComponent, TimeoutComponent.title),
  //HseRoute.unsafe('**', undefined, NotFoundComponent.route)
]);

@NgModule({
  declarations: [
    AppComponent,
    TimeoutComponent,
    TimeoutModalComponent,
    NotFoundComponent,
    HomeComponent,
    ApplicationSelectorComponent,
    ReturningApplicationComponent,
    ReturningApplicationEnterDataComponent,
    ReturningApplicationResendCodeComponent,
    ReturningApplicationVerifyComponent,
    TimeoutComponent,
    SampleComponent,
    ApplicationSubmissionComponent,
  ],
  imports: [
    RouterModule.forRoot(routes.getRoutes(), { initialNavigation: 'enabledBlocking', scrollPositionRestoration: 'enabled' }),
    BrowserModule.withServerTransition({ appId: 'serverApp' }),
    FormsModule,
    ComponentsModule,
    CommonModule,
    HseAngularModule,
    HttpClientModule,
    HelpPagesModule,
    A11yModule,
    GovUKDataModule
  ],
  providers: [HttpClient, ApplicationService, CookiesBannerService, ...routes.getProviders()],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(injector: Injector) {
    GetInjector(injector);
  }
}
