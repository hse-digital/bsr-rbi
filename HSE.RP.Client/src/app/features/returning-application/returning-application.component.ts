import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot, Router } from '@angular/router';
import { ApplicationService } from 'src/app/services/application.service';
import { NavigationService } from 'src/app/services/navigation.service';
import { TitleService } from 'src/app/services/title.service';
import { PageComponent } from '../../helpers/page.component';

@Component({
  templateUrl: './returning-application.component.html'
})
export class ReturningApplicationComponent extends PageComponent<string>{
  onInit(applicationService: ApplicationService): void {
      throw new Error('Method not implemented.');
  }
  onSave(applicationService: ApplicationService): Promise<void> {
      throw new Error('Method not implemented.');
  }
  canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
      throw new Error('Method not implemented.');
  }
  isValid(): boolean {
      throw new Error('Method not implemented.');
  }
  navigateNext(): Promise<boolean> {
      throw new Error('Method not implemented.');
  }
  static route: string = "returning-application";
  static title: string = "Continue a saved application - Register a high-rise building - GOV.UK";

  step = "enterdata";
  emailAddress?: string;
  applicationNumber?: string;

  canContinue(): boolean {
    return false;
  }

  showVerifyApplication() {
    this.step = 'verify';
  }

  showResendStep() {
    this.step = 'resend';
  }
}
