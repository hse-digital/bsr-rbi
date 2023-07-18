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
import { ApplicantAlternativeEmailComponent } from './applicant-alternative-email/applicant-alternative-email.component';
import { ApplicantAddressComponent } from './applicant-address/applicant-address.component';
import { ApplicantSummaryComponent } from './applicant-summary/applicant-summary.component';
import { ApplicantAlternativePhoneComponent } from './applicant-alternative-phone/applicant-alternative-phone.component';
import { ApplicantNationalInsuranceNumberComponent } from './applicant-national-insurance-number/applicant-national-insurance-number.component';
import { ApplicantDateOfBirthComponent } from './applicant-date-of-birth/applicant-date-of-birth.component';
import { ApplicantNameComponent } from './applicant-name/applicant-name.component';

const routes = new HseRoutes([
  HseRoute.protected(ApplicantAlternativePhoneComponent.route, ApplicantAlternativePhoneComponent, ApplicantAlternativePhoneComponent.title),
  HseRoute.protected(ApplicantAlternativeEmailComponent.route, ApplicantAlternativeEmailComponent, ApplicantAlternativeEmailComponent.title),
  HseRoute.protected(ApplicantAddressComponent.route, ApplicantAddressComponent, ApplicantAddressComponent.title),
  HseRoute.protected(ApplicantSummaryComponent.route, ApplicantSummaryComponent, ApplicantSummaryComponent.title),
  HseRoute.protected(ApplicantNationalInsuranceNumberComponent.route, ApplicantNationalInsuranceNumberComponent, ApplicantNationalInsuranceNumberComponent.title),
  HseRoute.protected(ApplicantDateOfBirthComponent.route, ApplicantDateOfBirthComponent, ApplicantDateOfBirthComponent.title),
  HseRoute.protected(ApplicantNameComponent.route, ApplicantNameComponent, ApplicantNameComponent.title),
]);

@NgModule({
  declarations: [
    ApplicantAlternativeEmailComponent,
    ApplicantAlternativePhoneComponent,
    ApplicantAddressComponent,
    ApplicantSummaryComponent,
    ApplicantNationalInsuranceNumberComponent,
    ApplicantDateOfBirthComponent,
    ApplicantNameComponent
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
export class ApplicationPersonalDetailsModule {
  static baseRoute: string = 'personal-details';
}
