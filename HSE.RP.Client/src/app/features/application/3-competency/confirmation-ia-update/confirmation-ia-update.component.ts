import { Component } from '@angular/core';
import { CompetencyRoutes } from '../CompetencyRoutes';
import { environment } from 'src/environments/environment';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { PageComponent } from 'src/app/helpers/page.component';
import { ApplicationService } from 'src/app/services/application.service';
import { BuildingInspectorClassSelectionComponent } from '../../2-building-inspector-class/class-selection/building-inspector-class-selection.component';
import { BuildingInspectorRoutes } from '../../2-building-inspector-class/BuildingInspectorRoutes';

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
  constructor(activatedRoute: ActivatedRoute) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {}
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
      this.errorMessage = 'Select one';
    }

    return !this.hasErrors;
  }
  override async navigateNext(): Promise<boolean> {
    const isValide = this.isValid();

    if (isValide && this.selectedOption === 'yes') {
      return this.navigationService.navigateRelative(
        `../building-inspector-class/${BuildingInspectorClassSelectionComponent.route}`,
        this.activatedRoute
      );
    } else if (isValide && this.selectedOption === 'no') {
      return this.navigationService.navigateRelative(
        `${CompetencyRoutes.SUMMARY}`,
        this.activatedRoute
      );
    }

    return true;
  }
}
