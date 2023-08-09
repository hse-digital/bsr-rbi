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
import { ProfessionalActivityRoutes } from '../ProfessionalActivityRoutes';

@Component({
  selector: 'hse-professional-activity-summary',
  templateUrl: './professional-activity-summary.component.html',
})
export class ProfessionalActivitySummaryComponent extends PageComponent<ApplicantProfessionBodyMemberships> {

  static title: string = "Professional activity - Register as a building inspector - GOV.UK";
  public static route: string = ProfessionalActivityRoutes.PROFESSIONAL_BODY_SUMMARY;
  readonly ComponentCompletionState = ComponentCompletionState;
  readonly ApplicantProfessionBodyMembershipsHelper = ApplicantProfessionBodyMembershipsHelper;
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  selectedOption: string = 'no';
  override model?: ApplicantProfessionBodyMemberships;

  constructor(activatedRoute: ActivatedRoute, applicationService: ApplicationService) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {
    this.model = applicationService.model.ProfessionalMemberships;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    if (this.selectedOption === "no") {
      this.model!.CompletionState = ComponentCompletionState.Complete;
      await this.navigateTo(`application/${this.applicationService.model.id}`); // Back to the task list.
    }
    if (this.selectedOption === "yes") {
      await this.navigateTo('professional-body-memberships'); // Back to the task list.
    }
   }

  optionClicked(value: string) {
    this.selectedOption = value;
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
  public navigateToChange(membershipCode: string) {
    return this.navigationService.navigateRelative(`professional-body-change`, this.activatedRoute, { queryParams: { membershipCode: membershipCode } });
  }
  public navigateToRemove(membershipCode: string) {
    return this.navigationService.navigateRelative(`professional-body-remove`, this.activatedRoute, { queryParams: { membershipCode: membershipCode } });
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
