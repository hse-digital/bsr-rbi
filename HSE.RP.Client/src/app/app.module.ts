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
import { ApplicantEmailComponent } from './features/new-application/applicant-email/applicant-email.component';
import { ApplicantEmailVerifyComponent } from './features/new-application/applicant-email/applicant-email-verify.component';
import { ApplicantGenerateNewSecurityCodeComponent } from './features/new-application/applicant-email/applicant-email-generate-new-security-code';

const routes = new HseRoutes([
  HseRoute.protected(SampleComponent.route, SampleComponent, SampleComponent.title),
  HseRoute.unsafe(HomeComponent.route, HomeComponent, undefined, HomeComponent.title),
  HseRoute.unsafe(ApplicationSelectorComponent.route, ApplicationSelectorComponent, undefined, ApplicationSelectorComponent.title),
  HseRoute.forLoadChildren(HelpPagesModule.baseRoute, () => import('./components/footer/help-pages.module').then(m => m.HelpPagesModule)),
  HseRoute.forLoadChildren(ApplicationModule.baseRoute, () => import('./features/application/application.module').then(m => m.ApplicationModule)),
  HseRoute.unsafe(NotFoundComponent.route, NotFoundComponent, undefined, NotFoundComponent.title),
  HseRoute.protected(ReturningApplicationComponent.route, ReturningApplicationComponent, ReturningApplicationComponent.title),
  HseRoute.protected(ApplicantEmailComponent.route, ApplicantEmailComponent, ApplicantEmailComponent.title),
  HseRoute.protected(ApplicantEmailVerifyComponent.route, ApplicantEmailVerifyComponent, ApplicantEmailVerifyComponent.title),
  HseRoute.protected(ApplicantGenerateNewSecurityCodeComponent.route, ApplicantGenerateNewSecurityCodeComponent, ApplicantGenerateNewSecurityCodeComponent.title),
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
    ApplicantEmailComponent,
    ApplicantEmailVerifyComponent,
    ReturningApplicationEnterDataComponent,
    ReturningApplicationResendCodeComponent,
    ReturningApplicationVerifyComponent,
    ApplicantGenerateNewSecurityCodeComponent,
    SampleComponent,
  ],
  imports: [
    RouterModule.forRoot(routes.getRoutes(), { initialNavigation: 'enabledBlocking', scrollPositionRestoration: 'enabled' }),
    BrowserModule.withServerTransition({ appId: 'serverApp' }),
    FormsModule,
    ComponentsModule,
    CommonModule,
    HseAngularModule,
    HttpClientModule,
    HelpPagesModule
  ],
  providers: [HttpClient, ApplicationService, CookiesBannerService, ...routes.getProviders()],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(injector: Injector) {
    GetInjector(injector);
  }
}
