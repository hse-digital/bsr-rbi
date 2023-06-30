import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HseRoute, HseRoutes } from '../../helpers/hse.route';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ComponentsModule } from '../../components/components.module';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { ApplicationService } from '../../services/application.service';
import { HseAngularModule } from 'hse-angular';
import { CookiesBannerService } from '../../services/cookies-banner.service';
import { ApplicantEmailComponent } from './applicant-email/applicant-email.component';
import { NewApplicantNameComponent } from './new-applicant-name/new-applicant-name.component';
import { ApplicantEmailVerifyComponent } from './applicant-email/applicant-email-verify.component';
import { ApplicantGenerateNewSecurityCodeComponent } from './applicant-email/applicant-email-generate-new-security-code';
import { ApplicantPhoneComponent } from './applicant-phone/applicant-phone.component';
import { ApplicantPhoneVerifyComponent } from './applicant-phone/applicant-phone-verify.component';


const routes = new HseRoutes([
  HseRoute.protected(ApplicantPhoneComponent.route, ApplicantPhoneComponent, ApplicantPhoneComponent.title),
  HseRoute.protected(NewApplicantNameComponent.route, NewApplicantNameComponent, NewApplicantNameComponent.title),
  HseRoute.protected(ApplicantEmailComponent.route, ApplicantEmailComponent, ApplicantEmailComponent.title),
  HseRoute.protected(ApplicantEmailVerifyComponent.route, ApplicantEmailVerifyComponent, ApplicantEmailVerifyComponent.title),
  HseRoute.protected(ApplicantGenerateNewSecurityCodeComponent.route, ApplicantGenerateNewSecurityCodeComponent, ApplicantGenerateNewSecurityCodeComponent.title),
  HseRoute.protected(ApplicantPhoneVerifyComponent.route, ApplicantPhoneVerifyComponent, ApplicantPhoneVerifyComponent.title),
]);

@NgModule({
  declarations: [
    ApplicantEmailComponent,
    NewApplicantNameComponent,
    ApplicantEmailVerifyComponent,
    ApplicantGenerateNewSecurityCodeComponent,
    ApplicantPhoneComponent,
    ApplicantPhoneVerifyComponent
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
export class NewApplicationModule {
  static baseRoute: string = 'new-application';
}


