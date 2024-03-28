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
  headerTitleText = "";

  title: string = "HSE.RP.Client";
  constructor(private applicationService: ApplicationService,
    private router: Router, private idleTimerService: IdleTimerService, private activatedRoute: ActivatedRoute, private cookiesBannerService: CookiesBannerService, private navigationService: NavigationService) {
    this.initTimer();
    this.initCookiesBanner();
  }

  ngAfterViewChecked(): void {
    this.setHeaderLink();
    this.setHeaderText();

  }

  private doesUrlContains(...segment: string[]) {
    return segment.filter(x => window.location.href.indexOf(x) > -1).length > 0;
  }

  setHeaderLink() {
    if (this.doesUrlContains("/public-register/england")) {
      this.appHeaderLink = "/public-register/england";
    } else if (this.doesUrlContains("/public-register/wales")) {
      this.appHeaderLink = "/public-register/wales";
    }
    else {
      environment.headerLink
    }
  }


  setHeaderText() {
    if (this.doesUrlContains("/public-register")) {
      if (this.doesUrlContains("england")) {
        this.headerTitleText = "Find a registered building inspector in England";
      }
      else if (this.doesUrlContains("wales")) {
        this.headerTitleText = "Find a registered building inspector in Wales";
      }
      else {
        this.headerTitleText = ""
      }
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
    this.viewCookiesLink = `/${HelpPagesModule.baseRoute}/${CookiesComponent.route}`;
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
    await this.navigationService.navigate(`/${HelpPagesModule.baseRoute}/${CookiesComponent.route}`);
  }


}
