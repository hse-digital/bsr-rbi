import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../../services/application.service';
import { ApplicantAlternativeEmailComponent } from '../applicant-alternative-email/applicant-alternative-email.component';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { ApplicantAlternativePhoneComponent } from '../applicant-alternative-phone/applicant-alternative-phone.component';
import { PersonalDetailRoutes, PersonalDetailRouter } from '../PersonalDetailRoutes'

@Component({
  selector: 'hse-applicant-address',
  templateUrl: './applicant-address.component.html',
})
export class ApplicantAddressComponent extends PageComponent<string> {

  public static route: string = PersonalDetailRoutes.ADDRESS;
  static title: string = "Personal details - Register as a building inspector - GOV.UK";
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  override model?: string;

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService,
    private personalDetailRouter: PersonalDetailRouter) {
    super(activatedRoute);
    this.updateOnSave = true;
  }

  override onInit(applicationService: ApplicationService): void {
    //this.model = applicationService.model.personalDetails?.applicantPhoto?.toString() ?? '';
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    //throw new Error('Method not implemented.');
   }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return true;
    //return (FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.firstName) || FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.lastName));

  }


  override isValid(): boolean {
    return true;
/*     this.phoneNumberHasErrors = !PhoneNumberValidator.isValid(this.model?.toString() ?? '');
    return !this.phoneNumberHasErrors; */

  }

  override navigateNext(): Promise<boolean> {
    return this.personalDetailRouter.navigateTo(this.applicationService.model, PersonalDetailRoutes.ALT_PHONE);
  }

}
