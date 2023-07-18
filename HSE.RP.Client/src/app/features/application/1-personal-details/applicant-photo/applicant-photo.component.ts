import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../../services/application.service';
import { ApplicantAddressComponent } from '../applicant-address/applicant-address.component';
import { PersonalDetailRoutes, PersonalDetailRouter } from '../PersonalDetailRoutes'

@Component({
  selector: 'hse-applicant-photo',
  templateUrl: './applicant-photo.component.html',
})
export class ApplicantPhotoComponent extends PageComponent<string> {

  DerivedIsComplete(value: boolean): void {
       
  }

  public static route: string = PersonalDetailRoutes.PROOF_OF_ID;
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
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {
    //this.model = applicationService.model.personalDetails?.applicantPhoto?.toString() ?? '';
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    //
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
    return this.personalDetailRouter.navigateTo(this.applicationService.model, PersonalDetailRoutes.ADDRESS)

  }

}
