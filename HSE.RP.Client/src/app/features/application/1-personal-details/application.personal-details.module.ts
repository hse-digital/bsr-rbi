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
import { ApplicantPhotoComponent } from './applicant-photo/applicant-photo.component';
import { ApplicantPhoneComponent } from './applicant-phone/applicant-phone.component';
import { ApplicantNameComponent } from './applicant-name/applicant-name.component';
import { ApplicantAlternativeEmailComponent } from './applicant-alternative-email/applicant-alternative-email.component';
import { ApplicantAddressComponent } from './applicant-address/applicant-address.component';
import { ApplicantProofOfIdentityComponent } from './applicant-proof-of-identity/applicant-proof-of-identity.component';
import { ApplicantSummaryComponent } from './applicant-summary/applicant-summary.component';
import { ApplicantAlternativePhoneComponent } from './applicant-alternative-phone/applicant-alternative-phone.component';
import { PersonalDetailsPlaceholderComponent } from './personal-details-placeholder/personal-details-placeholder.component';


const routes = new HseRoutes([
  HseRoute.protected(ApplicantPhoneComponent.route, ApplicantPhoneComponent, ApplicantPhoneComponent.title),
  HseRoute.protected(ApplicantAlternativePhoneComponent.route, ApplicantAlternativePhoneComponent, ApplicantAlternativePhoneComponent.title),
  HseRoute.protected(ApplicantNameComponent.route, ApplicantNameComponent, ApplicantNameComponent.title),
  HseRoute.protected(ApplicantAlternativeEmailComponent.route, ApplicantAlternativeEmailComponent, ApplicantAlternativeEmailComponent.title),
  HseRoute.protected(ApplicantPhotoComponent.route, ApplicantPhotoComponent, ApplicantPhotoComponent.title),
  HseRoute.protected(ApplicantAddressComponent.route, ApplicantAddressComponent, ApplicantAddressComponent.title),
  HseRoute.protected(ApplicantProofOfIdentityComponent.route, ApplicantProofOfIdentityComponent, ApplicantProofOfIdentityComponent.title),
  HseRoute.protected(ApplicantSummaryComponent.route, ApplicantSummaryComponent, ApplicantSummaryComponent.title),
  HseRoute.protected(PersonalDetailsPlaceholderComponent.route, PersonalDetailsPlaceholderComponent, PersonalDetailsPlaceholderComponent.title),
]);

@NgModule({
  declarations: [
    ApplicantAlternativeEmailComponent,
    ApplicantNameComponent,
    ApplicantPhotoComponent,
    ApplicantPhoneComponent,
    ApplicantAlternativePhoneComponent,
    ApplicantAddressComponent,
    ApplicantProofOfIdentityComponent,
    ApplicantAlternativeEmailComponent,
    ApplicantSummaryComponent,
    PersonalDetailsPlaceholderComponent,
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
