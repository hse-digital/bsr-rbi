import { Component } from '@angular/core';
import { PageComponent } from 'src/app/helpers/page.component';
import { ApplicantProfessionBodyMemberships } from 'src/app/models/applicant-professional-body-membership';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { ApplicationService } from 'src/app/services/application.service';
import { environment } from 'src/environments/environment';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { ProfessionalIndividualMembershipDetailsComponent } from '../professional-individual-membership-details/professional-individual-membership-details.component';
import { ProfessionalMembershipInformationComponent } from '../professional-membership-information/professional-membership-information.component';
import { ProfessionalBodyMembershipSummaryComponent } from '../professional-body-membership-summary/professional-body-membership-summary.component';

@Component({
  selector: 'hse-professional-body-selection',
  templateUrl: './professional-body-selection.component.html',
  styles: [],
})
export class ProfessionalBodySelectionComponent extends PageComponent<ApplicantProfessionBodyMemberships> {
  public static route: string = 'professional-body-selection';
  static title: string =
    'Professional activity - Register as a building inspector - GOV.UK';
  readonly ComponentCompletionState = ComponentCompletionState;
  production: boolean = environment.production;
  modelValid: boolean = false;
  errorMessage: string = '';
  selectedOption: string = '';
  existingSelection: string = '';
  existingStatus?: ComponentCompletionState =
    ComponentCompletionState.NotStarted;
  queryParam?: string = '';

  constructor(activatedRoute: ActivatedRoute) {
    super(activatedRoute);
  }
  override onInit(applicationService: ApplicationService): void {
    this.updateOnSave = true;
    this.activatedRoute.queryParams.subscribe((params) => {
      this.queryParam = params['queryParam'];
    });
    if (applicationService.model.ProfessionalMemberships == null) {
      applicationService.model.ProfessionalMemberships =
        new ApplicantProfessionBodyMemberships();
      this.selectedOption = '';
    } else {
      this.model = applicationService.model.ProfessionalMemberships;
    }

    const memberships = applicationService.model.ProfessionalMemberships;
    this.model = memberships;

    if (
      memberships.RICS.CompletionState === ComponentCompletionState.Complete
    ) {
      this.existingSelection = memberships.RICS.MembershipBodyCode;
      this.existingStatus = memberships.RICS.CompletionState;
    } else if (
      memberships.CABE.CompletionState === ComponentCompletionState.Complete
    ) {
      this.existingSelection = memberships.CABE.MembershipBodyCode;
      this.existingStatus = memberships.CABE.CompletionState;
    } else if (
      memberships.CIOB.CompletionState === ComponentCompletionState.Complete
    ) {
      this.existingSelection = memberships.CIOB.MembershipBodyCode;
      this.existingStatus = memberships.CIOB.CompletionState;
    } else if (
      memberships.OTHER.CompletionState === ComponentCompletionState.Complete
    ) {
      this.existingSelection = memberships.OTHER.MembershipBodyCode;
      this.existingStatus = memberships.OTHER.CompletionState;
    }

    if (
      this.existingSelection === '' &&
      this.existingStatus !== ComponentCompletionState.Complete
    ) {
      this.selectedOption = '';
    } else if (
      this.existingSelection !== '' &&
      this.existingStatus === ComponentCompletionState.Complete
    ) {
      this.selectedOption = '';
    } else if (
      this.existingSelection === '' &&
      this.existingStatus === ComponentCompletionState.Complete
    ) {
      this.selectedOption = '';
    } else {
      this.selectedOption =
        memberships.RICS.MembershipBodyCode ||
        memberships.CABE.MembershipBodyCode ||
        memberships.CIOB.MembershipBodyCode ||
        memberships.OTHER.MembershipBodyCode;
    }
  }
  override async onSave(applicationService: ApplicationService): Promise<void> {
    const memberships = applicationService.model.ProfessionalMemberships;
    const selectedOption = this.selectedOption;

    if (['RICS', 'CABE', 'CIOB', 'OTHER'].includes(selectedOption)) {
      memberships.RICS.MembershipBodyCode =
        selectedOption === 'RICS' ? selectedOption : '';
      memberships.CABE.MembershipBodyCode =
        selectedOption === 'CABE' ? selectedOption : '';
      memberships.CIOB.MembershipBodyCode =
        selectedOption === 'CIOB' ? selectedOption : '';
      memberships.OTHER.MembershipBodyCode =
        selectedOption === 'OTHER' ? selectedOption : '';
      if (selectedOption === 'OTHER') {
        memberships.OTHER.CompletionState = ComponentCompletionState.Complete;
      }
    }
  }
  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return (
      this.applicationService.model.ProfessionalMemberships
        .IsProfessionBodyRelevantYesNo === 'yes'
    );
  }

  override isValid(): boolean {
    this.hasErrors = false;
    this.errorMessage = '';
    if (this.selectedOption === '') {
      this.hasErrors = true;
      this.errorMessage =
        'Select a professional body you are a member of. If yours is not listed, select "other"';
    }

    return !this.hasErrors;
  }

  override async navigateNext(): Promise<boolean> {
    const membershipCode = this.selectedOption;
    if (['RICS', 'CABE', 'CIOB'].includes(this.selectedOption)) {
      return this.navigationService.navigateRelative(
        ProfessionalMembershipInformationComponent.route,
        this.activatedRoute,
        {membershipCode: membershipCode, queryParam: this.queryParam }
      );
    }
    return this.navigationService.navigateRelative(
      ProfessionalBodyMembershipSummaryComponent.route,
      this.activatedRoute
    ); // Back to the task list.
  }

  public navigateTo(route: string) {
    return this.navigationService.navigateRelative(
      `${route}`,
      this.activatedRoute
    );
  }

  public display(): boolean {
    const memberships = this.applicationService.model.ProfessionalMemberships;

    if (
      memberships.RICS.CompletionState === ComponentCompletionState.Complete &&
      memberships.CABE.CompletionState === ComponentCompletionState.Complete &&
      memberships.CIOB.CompletionState === ComponentCompletionState.Complete
    ) {
      return false;
    } else {
      return true;
    }
  }

}
