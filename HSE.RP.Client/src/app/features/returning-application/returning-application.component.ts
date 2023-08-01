import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot, Router } from '@angular/router';
import { ApplicationService } from 'src/app/services/application.service';
import { NavigationService } from 'src/app/services/navigation.service';
import { TitleService } from 'src/app/services/title.service';
import { PageComponent } from '../../helpers/page.component';
import { verify } from 'crypto';
import { BuildingInspectorClass } from 'src/app/models/building-inspector-class.model';
import { BuildingProfessionalModel } from 'src/app/models/building-professional.model';

export type VerificationData = {
  verificationEmail: string | undefined,
  verificationPhone: string | undefined
}

@Component({
  templateUrl: './returning-application.component.html'
})
export class ReturningApplicationComponent extends PageComponent<string>{
  DerivedIsComplete(value: boolean): void {

  }
  onInit(applicationService: ApplicationService): void {
    this.applicationService.model = new BuildingProfessionalModel();
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
  phoneNumber?: string
  applicationNumber?: string;
  verificationPhone?: string
  verificationEmail?: string
  verificationOption?: string

  canContinue(): boolean {
    return false;
  }

  showVerifyApplication(event: VerificationData) {
    this.verificationEmail = event.verificationEmail;
    this.verificationPhone = event.verificationPhone;
    this.step = "verify";
  }

  showResendStep() {
    this.step = 'resend';
  }

  showVerifyResend() {
    this.step = 'resendverify';
  }

  showChangeVerificationStep() {
    if(this.verificationOption == "email-option")
    {
      this.verificationOption = "phone-option";
    }
    else
    {
      this.verificationOption = "email-option";
    }
    this.step = 'enterdata';
  }


}
