import { Component } from '@angular/core';
import { CompetencyRoutes } from '../CompetencyRoutes';
import { environment } from 'src/environments/environment';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { PageComponent } from 'src/app/helpers/page.component';
import { ApplicationService } from 'src/app/services/application.service';
import { BuildingInspectorClassSelectionComponent } from '../../2-building-inspector-class/class-selection/building-inspector-class-selection.component';
import { BuildingInspectorRoutes } from '../../2-building-inspector-class/BuildingInspectorRoutes';
import { ApplicationSummaryComponent } from '../../5-application-submission/application-summary/application-summary.component';
import { StageCompletionState } from 'src/app/models/stage-completion-state.enum';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { Competency } from 'src/app/models/competency.model';

@Component({
  selector: 'hse-confirmation-ia-update',
  templateUrl: './confirmation-ia-update.component.html',
  styles: [],
})
export class ConfirmationIaUpdateComponent extends PageComponent<string> {
  BuildingInspectorRoutes = BuildingInspectorRoutes;

  public static route: string = CompetencyRoutes.CONFIRMATION_IA_UPDATE;
  static title: string =
    'Competency - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  errorMessage: string = '';
  selectedOption?: string = '';
  queryParam?: string = '';

  constructor(activatedRoute: ActivatedRoute) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
    this.activatedRoute.queryParams.subscribe((params) => {
      this.queryParam = params['queryParam'];
    });
  }
  override async onSave(
    applicationService: ApplicationService
  ): Promise<void> {}
  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return true;
  }
  override isValid(): boolean {
    this.hasErrors = false;
    this.errorMessage = '';

    if (this.selectedOption === '') {
      this.hasErrors = true;
      this.errorMessage = 'Tell us if you want to change your assessment status';
    }

    return !this.hasErrors;
  }
  override async navigateNext(): Promise<boolean> {
    const isValid = this.isValid();

    if (
      this.queryParam != null &&
      this.queryParam != undefined &&
      this.queryParam != ''
    ) {
      const queryParam = this.queryParam;

      if (isValid && this.selectedOption === 'yes') {
        return this.navigationService.navigateRelative(
          `../building-inspector-class/${BuildingInspectorClassSelectionComponent.route}`,
          this.activatedRoute,
          { resetIA: true, queryParam: queryParam}
        );
      } else if (isValid && this.selectedOption === 'no') {
        return this.navigationService.navigateRelative(
          `../application-submission/${ApplicationSummaryComponent.route}`,
          this.activatedRoute
        );
      }

      return true;
    } else {
      if (isValid && this.selectedOption === 'yes') {
        this.applicationService.model.StageStatus["Competency"] = StageCompletionState.Incomplete;
        this.applicationService.model.Competency! = new Competency();
        return this.navigationService.navigateRelative(
          `../building-inspector-class/${BuildingInspectorClassSelectionComponent.route}`,
          this.activatedRoute,
          { resetIA: true}
        );
      } else if (isValid && this.selectedOption === 'no') {
        return this.navigationService.navigateRelative(
          `${CompetencyRoutes.SUMMARY}`,
          this.activatedRoute
        );
      }

      return true;
    }
  }
}
