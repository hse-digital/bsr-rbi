import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PrivacyNoticeComponent } from './privacy-notice/privacy-notice.component';
import { RouterModule } from '@angular/router';
import { HseAngularModule } from 'hse-angular';
import { TermsConditionsComponent } from './terms-conditions/terms-conditions.component';
import { AccessibilityComponent } from './accessibility/accessibility.component';
import { CookiesComponent } from './cookies/cookies.component';
import { HseRoute, HseRoutes } from 'src/app/helpers/hse.route';
import { AccessibilityRegisterComponent } from './accessibility-register/accessibility-register.component';

const routes = new HseRoutes([
  HseRoute.unsafe(PrivacyNoticeComponent.route, PrivacyNoticeComponent, undefined, PrivacyNoticeComponent.title),
  HseRoute.unsafe(TermsConditionsComponent.route, TermsConditionsComponent, undefined, TermsConditionsComponent.title),
  HseRoute.unsafe(AccessibilityComponent.route, AccessibilityComponent, undefined, AccessibilityComponent.title),
  HseRoute.unsafe(AccessibilityRegisterComponent.route, AccessibilityRegisterComponent, undefined, AccessibilityRegisterComponent.title),
  HseRoute.unsafe(CookiesComponent.route, CookiesComponent, undefined, CookiesComponent.title),
]);

@NgModule({
  declarations: [
    PrivacyNoticeComponent,
    TermsConditionsComponent,
    AccessibilityComponent,
    AccessibilityRegisterComponent,
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
    { title: "Contact us", href: 'https://www.gov.uk/guidance/contact-the-building-safety-regulator', isNewTab: false},
    { title: "Accessibility", href: `/${HelpPagesModule.baseRoute}/${AccessibilityComponent.route}`,isNewTab: false },
    { title: "Cookies", href: `/${HelpPagesModule.baseRoute}/${CookiesComponent.route}`, isNewTab: false},
    { title: "Feedback", href: "https://forms.office.com/e/p1rYGz27V6" ,isNewTab: false},
    { title: "Privacy", href: `/${HelpPagesModule.baseRoute}/${PrivacyNoticeComponent.route}`, isNewTab: false }
  ];

  static registerFooterLinks = [
    { title: "Contact us", href: 'https://www.gov.uk/guidance/contact-the-building-safety-regulator', isNewTab: false},
    { title: "Accessibility", href: `/${HelpPagesModule.baseRoute}/${AccessibilityRegisterComponent.route}`,isNewTab: false }
  ];
}
