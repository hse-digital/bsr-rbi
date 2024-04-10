import { AfterViewChecked, AfterViewInit, Component, ViewChild } from '@angular/core';
import { GovukCookieBannerComponent } from 'hse-angular';
import { CookiesBannerModel, CookiesBannerService } from './services/cookies-banner.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CookiesComponent } from './components/footer/cookies/cookies.component';
import { HelpPagesModule } from './components/footer/help-pages.module';
import { ApplicationService } from './services/application.service';
import { IdleTimerService } from './services/idle-timer.service';
import { NavigationService } from './services/navigation.service';
import { environment } from 'src/environments/environment';
import { CookiesRegisterComponent } from './components/footer/cookies/cookies-register.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements AfterViewInit, AfterViewChecked {

  showTimeoutDialog = false;
  footerLinks = HelpPagesModule.footerLinks;
  viewCookiesLink!: string;

  appHeaderLink = environment.headerLink;
  govukLogoLink = environment.govukLogoLink;
  feedbackLink = "";
  headerTitleText = "";
  cookieTitleText = "";

  title: string = "HSE.RP.Client";
  constructor(private applicationService: ApplicationService,
    private router: Router, private idleTimerService: IdleTimerService, private activatedRoute: ActivatedRoute, private cookiesBannerService: CookiesBannerService, private navigationService: NavigationService) {
    this.initTimer();
    this.initCookiesBanner();
  }

  ngAfterViewChecked(): void {
    this.setHeaderLink();
    this.setHeaderText();
    this.setFooterLinks();
    this.setFeedbackLink();
    this.setCookieTitleText();
  }

  private doesUrlContains(...segment: string[]) {
    return segment.filter(x => window.location.href.indexOf(x) > -1).length > 0;
  }

  setFeedbackLink() {
    if (this.doesUrlContains("/public-register-england")) {
      this.feedbackLink = "https://forms.office.com/e/bYnhFX6gCU";
    } else if (this.doesUrlContains("/public-register-wales")) {
      this.feedbackLink = "https://forms.office.com/e/bYnhFX6gCU";
    }
    else {
      this.feedbackLink = "https://forms.office.com/e/p1rYGz27V6"
    }
  }

  isPublicRegisterPage() {
    if (this.doesUrlContains("/public-register-england")) {
      return true;
    } else if (this.doesUrlContains("/public-register-wales")) {
      return true;
    }
    else if (this.doesUrlContains("/help/cookies-register")) {
      return true;
    }

    else {
      return false;
    }
  }

  setHeaderLink() {
    if (this.doesUrlContains("/public-register-england")) {
      this.appHeaderLink = "/public-register-england";
    } else if (this.doesUrlContains("/public-register-wales")) {
      this.appHeaderLink = "/public-register-wales";
    }
    else if (this.doesUrlContains("/help/cookies-register")) {
      //if user was on public-register-england then go back to public-register-england without query param
      if (window.location.href.indexOf("/public-register-england") > -1) {
        this.appHeaderLink = "/public-register-england";
      }
      //if user was on public-register-wales then go back to public-register-wales without query param
      else if (window.location.href.indexOf("/public-register-wales") > -1) {
        this.appHeaderLink = "/public-register-wales";
      }

    }
    else {
      this.appHeaderLink = environment.headerLink
    }
  }

  setCookieTitleText() {
    if (this.doesUrlContains("/public-register-england")) {
      this.cookieTitleText = "Find a registered building inspector in England";
    } else if (this.doesUrlContains("/public-register-wales")) {
      this.cookieTitleText = "Find a registered building inspector in Wales";
    }
    else {
      this.cookieTitleText = "Register as a building inspector"
    }
  }

  setFooterLinks() {
    if (this.doesUrlContains("/public-register-england")) {
      this.footerLinks = HelpPagesModule.registerFooterLinks;
    } else if (this.doesUrlContains("/public-register-wales")) {
      this.footerLinks = HelpPagesModule.registerFooterLinks;

    } else if (this.doesUrlContains("/cookies-register")) {
      this.footerLinks = HelpPagesModule.registerFooterLinks;
    }

    else {
      this.footerLinks = HelpPagesModule.footerLinks
    }
  }


  setHeaderText() {
    if (this.doesUrlContains("/public-register-england")) {
      if (this.doesUrlContains("results", "details")) {
        this.headerTitleText = "Find a registered building inspector in England";
      }
      else {
        this.headerTitleText = ""
      }
    }
    else if (this.doesUrlContains("/public-register-wales")) {
      if (this.doesUrlContains("results", "details")) {
        this.headerTitleText = "Find a registered building inspector in Wales";
      }
      else {
        this.headerTitleText = ""
      }
    }
    else if (this.doesUrlContains("/help/cookies-register")) {
      this.headerTitleText = "Find a registered building inspector"
    }
    else {
      this.headerTitleText = "Register as a building inspector"
    }
  }


  async timeoutSaveAndComeBack() {
    await this.applicationService.updateApplication();
    this.showTimeoutDialog = false;
    this.router.navigate(['']);
  }

  timeoutContinue() {
    this.showTimeoutDialog = false;
    this.initTimer();
  }

  timeout() {
    this.showTimeoutDialog = false;
    this.applicationService.clearApplication();
    this.router.navigate(['/timeout']);
  }

  initTimer() {
    this.idleTimerService.initTimer(13 * 60, () => {
      if (typeof window !== 'undefined' && (this.doesUrlContains("/application/", "/new-application/", "/returning-application"))) {
        this.showTimeoutDialog = true;
      } else {
        this.initTimer();
      }
    });
  }

  @ViewChild(GovukCookieBannerComponent) cookieBanner?: GovukCookieBannerComponent;
  cookieBannerModel!: CookiesBannerModel;

  ngAfterViewInit(): void {
    this.cookieBanner?.onHideCookieBannerConfirmation.subscribe(() => {
      this.cookiesBannerService.removeConfirmationBanner();
      this.cookieBannerModel = this.cookiesBannerService.getShowCookiesStatus();
    });
  }

  initCookiesBanner() {
    this.cookieBannerModel = this.cookiesBannerService.getShowCookiesStatus();
    if (this.isPublicRegisterPage()) {
      this.viewCookiesLink = `/${HelpPagesModule.baseRoute}/${CookiesRegisterComponent.route}`;
    }
    else {
      this.viewCookiesLink = `/${HelpPagesModule.baseRoute}/${CookiesComponent.route}`;
    }
  }

  cookiesAccepted() {
    this.cookiesBannerService.acceptCookies(true);
    this.cookieBannerModel = this.cookiesBannerService.getShowCookiesStatus();
    this.cookiesBannerService.refreshPage();
  }

  cookiesRejected() {
    this.cookiesBannerService.rejectCookies(true);
    this.cookieBannerModel = this.cookiesBannerService.getShowCookiesStatus();
  }

  async cookiesChanged() {
    if (this.isPublicRegisterPage()) {
      await this.navigationService.navigate(`/${HelpPagesModule.baseRoute}/${CookiesRegisterComponent.route}`);
    }
    else {
      await this.navigationService.navigate(`/${HelpPagesModule.baseRoute}/${CookiesComponent.route}`);
    }
  }


}
