import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { ApplicationService } from '../../../../services/application.service';
import { ProfessionalActivityEmploymentTypeComponent } from '../employment-type/professional-activity-employment-type.component';
import { ProfessionalBodiesMember } from 'src/app/models/professional-bodies-memeber.model';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { ProfessionalActivityRoutes } from '../ProfessionalActivityRoutes';

@Component({
  selector: 'hse-professional-body-memberships',
  templateUrl: './professional-body-memberships.component.html',
})
export class ProfessionalBodyMembershipsComponent extends PageComponent<ProfessionalBodiesMember> {
  // public static route: string = ProfessionalActivityRoutes.PROFESSIONAL_BODY_MEMBERSHIPS;

  public static route: string = 'professional-body-memberships';
  static title: string =
    'Professional activity - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  errorMessage: string = '';
  selectedOption: string = 'no';

  constructor(activatedRoute: ActivatedRoute) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
    this.updateOnSave = true;

    if (!applicationService.model.ProfessionalActivity) {
      applicationService.model.ProfessionalActivity = {};
    }

    if (
      applicationService.model.ProfessionalActivity?.ProfessionalBodiesMember ==
      null
    ) {
      applicationService.model.ProfessionalActivity!.ProfessionalBodiesMember =
        new ProfessionalBodiesMember();
      this.selectedOption = 'no';
    }

    this.selectedOption = applicationService.model.ProfessionalActivity
      ?.ProfessionalBodiesMember
      ? applicationService.model.ProfessionalActivity?.ProfessionalBodiesMember!
          .Membership!
      : 'no';
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    if (['no', 'yes'].includes(this.selectedOption)) {
      applicationService.model.ProfessionalActivity!.ProfessionalBodiesMember!.Membership =
        this.selectedOption;
    }

    if (this.model?.CompletionState !== ComponentCompletionState.InProgress) {
      applicationService.model.ProfessionalActivity!.ProfessionalBodiesMember!.CompletionState =
        ComponentCompletionState.Complete;
    }
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return true;
  }

  override isValid(): boolean {
    return true;
  }

  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(
      ProfessionalActivityEmploymentTypeComponent.route,
      this.activatedRoute
    );
  }
  DerivedIsComplete(value: boolean): void {
    this.applicationService.model.ProfessionalActivity!.ProfessionalBodiesMember!.CompletionState =
      value
        ? ComponentCompletionState.Complete
        : ComponentCompletionState.InProgress;
  }
}
