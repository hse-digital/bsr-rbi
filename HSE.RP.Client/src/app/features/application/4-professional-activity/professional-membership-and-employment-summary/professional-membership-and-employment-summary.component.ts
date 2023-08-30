import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { ApplicationService } from '../../../../services/application.service';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { DateFormatHelper } from 'src/app/helpers/date-format-helper';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { ProfessionalActivity } from 'src/app/models/professional-activity.model';
import { ProfessionalActivityHelper } from 'src/app/helpers/professional-activity-helper.component';
import { EmploymentType } from 'src/app/models/employment-type.enum';
import { FieldValidations } from 'src/app/helpers/validators/fieldvalidations';
import { StageCompletionState } from 'src/app/models/stage-completion-state.enum';

@Component({
  selector: 'hse-professional-membership-and-employment-summary',
  templateUrl: './professional-membership-and-employment-summary.component.html',
})
export class ProfessionalMembershipAndEmploymentSummaryComponent extends PageComponent<string> {

  public static route: string = "professional-membership-and-employment-summary";
  static title: string =
    'Professional memberships and employment summary - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;


  constructor(
    activatedRoute: ActivatedRoute,
  ) {
    super(activatedRoute);
    this.updateOnSave = true;
  }

  override onInit(applicationService: ApplicationService): void {
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    applicationService.model.ProfessionalActivity.CompletionState = ComponentCompletionState.Complete;
    applicationService.model.ProfessionalActivity.EmploymentDetails!.CompletionState = ComponentCompletionState.Complete;
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return true;
    // return this.applicationService.model.ProfessionalMemberships.CompletionState==ComponentCompletionState.Complete
    // && this.applicationService.model.ProfessionalActivity.EmploymentDetails?.EmployerAddress?.CompletionState==ComponentCompletionState.Complete
    //return (FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.firstName) || FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.lastName));
  }

  override isValid(): boolean {
    return true;
    /*     this.phoneNumberHasErrors = !PhoneNumberValidator.isValid(this.model?.toString() ?? '');
    return !this.phoneNumberHasErrors; */
  }

  async SyncAndContinue() {
    await this.applicationService.syncEmploymentDetails();
    this.applicationService.model.StageStatus['ProfessionalActivity'] = StageCompletionState.Complete;
    this.saveAndContinue();
  }

  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(
      `../${ApplicationTaskListComponent.route}`,
      this.activatedRoute
    );
  }

  public navigateTo(route: string, queryParam?: string) {

    if (queryParam === undefined) {
      return this.navigationService.navigateRelative(
        `${route}`,
        this.activatedRoute
      );
    } else {
      return this.navigationService.navigateRelative(
        `${route}`,
        this.activatedRoute,
        { queryParam }
      );
    }
  }

  public isCompetencyAssessmentStatusYes(): boolean {
    return this.applicationService.model.Competency?.CompetencyIndependentAssessmentStatus?.IAStatus === 'yes';
  }

  public getProfessionalBodyMemberships(): string[] {

    let professionalBodyMemberships: string[] = [];
    //if this.applicationService.model.ProfessionalMemberships members completion state is complete then add body name to array
    if (this.applicationService.model.ProfessionalMemberships.CABE.CompletionState === ComponentCompletionState.Complete) {
      professionalBodyMemberships.push(ProfessionalActivityHelper.professionalBodyNames["CABE"]);
    }
    if (this.applicationService.model.ProfessionalMemberships.CIOB.CompletionState === ComponentCompletionState.Complete) {
      professionalBodyMemberships.push(ProfessionalActivityHelper.professionalBodyNames["CIOB"]);
    }

    if (this.applicationService.model.ProfessionalMemberships.RICS.CompletionState === ComponentCompletionState.Complete) {
      professionalBodyMemberships.push(ProfessionalActivityHelper.professionalBodyNames["RICS"]);
    }

    if (this.applicationService.model.ProfessionalMemberships.OTHER.CompletionState === ComponentCompletionState.Complete) {
      professionalBodyMemberships.push(ProfessionalActivityHelper.professionalBodyNames["OTHER"]);
    }

    return professionalBodyMemberships;


  }

  public getEmploymentType(): EmploymentType {
    return this.applicationService.model.ProfessionalActivity.EmploymentDetails?.EmploymentTypeSelection?.EmploymentType ?? EmploymentType.None;
  }

  public getEmploymentTypeName(): string {
    return ProfessionalActivityHelper.employmentTypeNames[this.applicationService.model.ProfessionalActivity.EmploymentDetails!.EmploymentTypeSelection!.EmploymentType!]
  }

  public getEmployerName(): string {
    if(FieldValidations.IsNotNullOrWhitespace(this.applicationService.model.ProfessionalActivity.EmploymentDetails!.EmployerName?.FullName))
    {
      return this.applicationService.model.ProfessionalActivity.EmploymentDetails!.EmployerName!.FullName!
    }
    else{
      return "None"
    }
  }

  public getEmploymentTypeTitle(): string | null {
    if(this.applicationService.model.ProfessionalActivity.EmploymentDetails?.EmploymentTypeSelection?.EmploymentType == EmploymentType.PublicSector
      || this.applicationService.model.ProfessionalActivity.EmploymentDetails?.EmploymentTypeSelection?.EmploymentType == EmploymentType.PrivateSector)
      {
        return "Employer"
      }
      else if(this.applicationService.model.ProfessionalActivity.EmploymentDetails?.EmploymentTypeSelection?.EmploymentType == EmploymentType.Other)
      {
        return "Business name";
      }
      else
      {
        return null;
      }
  }

  public getEmploymentAddressTitle(): string | null {
    if(this.applicationService.model.ProfessionalActivity.EmploymentDetails?.EmploymentTypeSelection?.EmploymentType == EmploymentType.PublicSector
      || this.applicationService.model.ProfessionalActivity.EmploymentDetails?.EmploymentTypeSelection?.EmploymentType == EmploymentType.PrivateSector)
      {
        return "Employer address"
      }
      else if(this.applicationService.model.ProfessionalActivity.EmploymentDetails?.EmploymentTypeSelection?.EmploymentType == EmploymentType.Other)
      {
        return "Business address";
      }
      else
      {
        return null;
      }
  }

  public isUnemployed(): boolean {
    return this.applicationService.model.ProfessionalActivity.EmploymentDetails?.EmploymentTypeSelection?.EmploymentType == EmploymentType.Unemployed
  }

}
