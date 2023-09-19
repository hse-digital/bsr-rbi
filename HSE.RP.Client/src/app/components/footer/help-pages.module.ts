import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PrivacyNoticeComponent } from './privacy-notice/privacy-notice.component';
import { RouterModule } from '@angular/router';
import { HseAngularModule } from 'hse-angular';
import { TermsConditionsComponent } from './terms-conditions/terms-conditions.component';
import { AccessibilityComponent } from './accessibility/accessibility.component';
import { CookiesComponent } from './cookies/cookies.component';
import { HseRoute, HseRoutes } from 'src/app/helpers/hse.route';

const routes = new HseRoutes([
  HseRoute.unsafe(PrivacyNoticeComponent.route, PrivacyNoticeComponent, undefined, PrivacyNoticeComponent.title),
  HseRoute.unsafe(TermsConditionsComponent.route, TermsConditionsComponent, undefined, TermsConditionsComponent.title),
  HseRoute.unsafe(AccessibilityComponent.route, AccessibilityComponent, undefined, AccessibilityComponent.title),
  HseRoute.unsafe(CookiesComponent.route, CookiesComponent, undefined, CookiesComponent.title),
]);

@NgModule({
  declarations: [
    PrivacyNoticeComponent,
    TermsConditionsComponent,
    AccessibilityComponent,
    CookiesComponent,
  ],
  imports: [
    RouterModule.forChild(routes.getRoutes()),
    CommonModule,
    HseAngularModule,
  ],
  providers: [...routes.getProviders()]
})
export class HelpPagesModule {
  static baseRoute: string = 'help';

  static footerLinks = [
    { title: "Contact", href: 'https://contact-the-building-safety-regulator.hse.gov.uk/'},
    { title: "Accessibility", href: `/${HelpPagesModule.baseRoute}/${AccessibilityComponent.route}` },
    { title: "Cookies", href: `/${HelpPagesModule.baseRoute}/${CookiesComponent.route}` },
    { title: "Feedback", href: "https://forms.office.com/e/p1rYGz27V6" },
    { title: "Privacy", href: `/${HelpPagesModule.baseRoute}/${PrivacyNoticeComponent.route}` },
    { title: "Terms and conditions", href: `/${HelpPagesModule.baseRoute}/${TermsConditionsComponent.route}` },
  ];
}
