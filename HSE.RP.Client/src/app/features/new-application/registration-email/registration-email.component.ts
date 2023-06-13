import { Component, QueryList, ViewChildren } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';
import { ApplicationService, BuildingInspectorModel } from 'src/app/services/application.service';
import { NavigationService } from 'src/app/services/navigation.service';
import { EmailValidator } from 'src/app/helpers/validators/email-validator';
import { GovukErrorSummaryComponent } from 'hse-angular';
import { TitleService } from 'src/app/services/title.service';
import { FieldValidations } from 'src/app/helpers/validators/fieldvalidations';
import { PageComponent } from '../../../helpers/page.component';
import { environment } from '../../../../environments/environment';
import { ApplicationTaskListComponent } from '../../application/task-list/task-list.component';
import { relative } from 'path';

@Component({
  templateUrl: './registration-email.component.html',
})
export class RegistrationEmailComponent extends PageComponent<BuildingInspectorModel> {

  static route: string = "registration-email";
  static title: string = "Personal details - Register a high-rise building - GOV.UK";
  production: boolean = environment.production;
  emailHasErrors: boolean = false;
  modelValid: boolean = false;

  constructor(activatedRoute: ActivatedRoute, applicationService: ApplicationService) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {
    this.model = applicationService.model;
  }

  override isValid(): boolean {
    this.emailHasErrors = !EmailValidator.isValid(this.model?.applicationEmail ?? '');
    return !this.emailHasErrors;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    applicationService.model.applicationEmail = this.model?.applicationEmail;


    //await applicationService.sendVerificationEmail(this.model?.Email!);
  }
  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return true;
  }

  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigate("application/task-list");;
  }
}
