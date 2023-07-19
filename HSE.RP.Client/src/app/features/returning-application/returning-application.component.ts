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
  DerivedIsComplete(value: boolean): void {

  }
  onInit(applicationService: ApplicationService): void {
      //throw new Error('Method not implemented.');
  }
  onSave(applicationService: ApplicationService): Promise<void> {
      throw new Error('Method not implemented.');
      }
  canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return true;
  }
  isValid(): boolean {
      return true;
  }
  navigateNext(): Promise<boolean> {
    return this.navigationService.navigate("application/task-list");;
  }
  static route: string = "returning-application";
  static title: string = "Continue a saved application - Register as a building inspector - GOV.UK";

  stepBack() {
    if(this.step == "verify")
    {
      this.step = "enterdata";
    }
    else if(this.step == "resend")
    {
      this.step = "verify";
    }
    else if(this.step == "resendverify")
    {
      this.step = "resend";
    }
    else if(this.step == "enterdata")
    {
      history.back();
    }
  }
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

  showVerifyResend() {
    this.step = 'resendverify';
  }


}
