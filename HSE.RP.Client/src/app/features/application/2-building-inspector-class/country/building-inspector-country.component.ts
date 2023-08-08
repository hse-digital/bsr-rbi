import { Component, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { ApplicationService } from '../../../../services/application.service';
import { BuildingInspectorSummaryComponent } from '../building-inspector-summary/building-inspector-summary.component';
import { BuildingClassTechnicalManagerComponent } from '../class-technical-manager/building-class-technical-manager.component';
import { BuildingInspectorRoutes } from '../BuildingInspectorRoutes';
import { BuildingInspectorCountryOfWork } from 'src/app/models/building-inspector-country-of-work.model';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';

@Component({
  selector: 'hse-building-inspector-country',
  templateUrl: './building-inspector-country.component.html',
})
export class BuildingInspectorCountryComponent extends PageComponent<BuildingInspectorCountryOfWork> {
  static route: string = BuildingInspectorRoutes.INSPECTOR_COUNTRY;
  public id: string = BuildingClassTechnicalManagerComponent.route;
  static title: string =
    'Building inspector class - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  public hint = 'Select all that apply';
  public errorText = '';
  selectedOptionError: boolean = false;
  override model?: BuildingInspectorCountryOfWork;
  public selections: string[] = [];

  @Output() onClicked = new EventEmitter();
  @Output() onKeyupEnter = new EventEmitter();

  constructor(activatedRoute: ActivatedRoute) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
    this.updateOnSave = true;

    if (!applicationService.model.InspectorClass?.InspectorCountryOfWork) {
      applicationService.model.InspectorClass!.InspectorCountryOfWork =
        new BuildingInspectorCountryOfWork();
    } else {
      this.model =
        applicationService.model.InspectorClass?.InspectorCountryOfWork;
      this.model!.CompletionState = ComponentCompletionState.InProgress;
    }

    const demandModel = this.DemandModel();
    const countryKeys = ['England', 'Wales'];

    this.selections.push(
      ...countryKeys.filter((key) => demandModel[key] === true)
    );

    // this.model!.CompletionState = ComponentCompletionState.InProgress;
    this.applicationService = applicationService;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    const demandModel = this.DemandModel();
    demandModel.England = false;
    demandModel.Wales = false;

    this.selections.forEach((value: keyof typeof demandModel) => {
      demandModel[value] = true;
    });

    if (this.selections.length > 0) {
      this.model!.CompletionState = ComponentCompletionState.Complete;
      applicationService.model.ApplicationStatus =
        ApplicationStatus.BuildingInspectorClassComplete;
    } else {
      this.model!.CompletionState = ComponentCompletionState.InProgress;
    }

    applicationService.model.InspectorClass!.InspectorCountryOfWork =
      demandModel;
    // applicationService.model.InspectorClass!.InspectorCountryOfWork.CompletionState =
    //   ComponentCompletionState.Complete;
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return true;
  }

  override isValid(): boolean {
    if (this.selections.length == 0) {
      this.selectedOptionError = true;
      this.errorText = 'Select a country you will be working in';
    }
    return this.selections.length > 0;
  }

  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(
      BuildingInspectorSummaryComponent.route,
      this.activatedRoute
    );
  }

  DerivedIsComplete(value: boolean): void {}

  public DemandModel(): BuildingInspectorCountryOfWork {
    if (this.model === undefined || this.model === null) {
      throw new Error('Model is undefined');
    }
    return this.model;
  }

  optionClicked() {}
}
