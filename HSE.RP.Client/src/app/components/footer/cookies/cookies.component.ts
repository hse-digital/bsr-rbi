import { AfterViewInit, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Cookies, CookiesBannerService } from 'src/app/services/cookies-banner.service';

@Component({
  selector: 'hse-cookies',
  templateUrl: './cookies.component.html'
})
export class CookiesComponent implements OnInit {
  public static route: string = "cookies";
  static title: string = "Cookies - Register a high-rise building - GOV.UK";

  cookieModel?: string;
  errorMessage?: string;

  constructor(public cookiesBannerService: CookiesBannerService) { }

  async ngOnInit() {
    this.cookieModel = !this.cookiesBannerService.cookiesModel.showCookies
      ? String(this.cookiesBannerService.cookiesModel.cookiesAccepted)
      : undefined;
  }

  saveCookieSettings() {
    if (this.cookieModel === "true"){
      this.cookiesBannerService.acceptCookies(false);
      this.cookiesBannerService.refreshPage();
      this.cookiesBannerService.removeConfirmationBanner();
    } else if (this.cookieModel === "false") {
      this.cookiesBannerService.rejectCookies(false);
      this.cookiesBannerService.refreshPage();
      this.cookiesBannerService.removeConfirmationBanner();
    }

    if (this.cookieModel === undefined) { this.errorMessage = "Select yes if you accept analytics cookies"; }
  }
}
