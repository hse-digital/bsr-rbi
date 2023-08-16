import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../../services/application.service';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { ProfessionalActivityEmploymentDetailsComponent } from '../employment-details/professional-activity-employment-details.component';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { EmploymentType } from 'src/app/models/employment-type';

@Component({
  selector: 'hse-professional-activity-employment-type',
  templateUrl: './professional-activity-employment-type.component.html',
})
export class ProfessionalActivityEmploymentTypeComponent extends PageComponent<EmploymentType> {
  DerivedIsComplete(value: boolean): void {

  }

  public static route: string = "professional-activity-employment-type";
  static title: string = "Professional activity - Register as a building inspector - GOV.UK";
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  // override model?: EmploymentType;
  selectedOptionError: boolean = false;
  errorMessage: string = "";

  constructor(activatedRoute: ActivatedRoute, applicationService: ApplicationService) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {
    this.model = applicationService.model.ProfessionalActivity.EmploymentType
    console.log(this.model)
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    console.log(this.model)
    applicationService.model.ProfessionalActivity.EmploymentType = this.model;
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
    return this.navigationService.navigateRelative(ProfessionalActivityEmploymentDetailsComponent.route, this.activatedRoute);
  }

}
