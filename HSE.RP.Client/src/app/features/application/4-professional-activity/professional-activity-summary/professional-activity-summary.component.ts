import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../../services/application.service';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { ApplicantProfessionBodyMemberships, ApplicantProfessionBodyMembershipsHelper } from '../../../../models/applicant-professional-body-membership';
import { ComponentCompletionState } from '../../../../models/component-completion-state.enum';

@Component({
  selector: 'hse-professional-activity-summary',
  templateUrl: './professional-activity-summary.component.html',
})
export class ProfessionalActivitySummaryComponent extends PageComponent<ApplicantProfessionBodyMemberships> {

  public static route: string = "professional-activity-summary";
  static title: string = "Professional activity - Register as a building inspector - GOV.UK";
  readonly ComponentCompletionState = ComponentCompletionState;
  readonly ApplicantProfessionBodyMembershipsHelper = ApplicantProfessionBodyMembershipsHelper;
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  override model?: ApplicantProfessionBodyMemberships;

  constructor(activatedRoute: ActivatedRoute, applicationService: ApplicationService) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {
    this.model = applicationService.model.ProfessionalMemberships;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    applicationService.model.ApplicationStatus = ApplicationStatus.CompetencyComplete;
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
    return this.navigationService.navigateRelative(`../${ApplicationTaskListComponent.route}`, this.activatedRoute);
  }
  public navigateTo(route: string) {
    return this.navigationService.navigateRelative(`${route}`, this.activatedRoute);
  }
  public emptyActionText(): string {
    return "";
  }
  public changeActionText(): string {
    return "change";
  }
  public removeActionText(): string {
    return "remove";
  }

}
