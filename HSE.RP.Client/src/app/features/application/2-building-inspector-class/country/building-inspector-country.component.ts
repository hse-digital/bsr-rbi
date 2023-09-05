import { Component, EventEmitter, Output, QueryList, ViewChild } from '@angular/core';
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
import { ApplicationSummaryComponent } from '../../5-application-submission/application-summary/application-summary.component';
import { GovukRequiredDirective } from 'src/app/components/required.directive';
import { IComponentModel } from 'src/app/models/component. interface';
import { GovukCheckboxComponent, GovukCheckboxGroupComponent } from 'hse-angular';

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
  //override model?: BuildingInspectorCountryOfWork;
  public selections: string[] = [];
  queryParam?: string = '';
  resetIA? : boolean = false;
  errorAnchorId?: string;


  @ViewChild(GovukCheckboxComponent) checkboxGroup?: GovukCheckboxComponent;


  @Output() onClicked = new EventEmitter();
  @Output() onKeyupEnter = new EventEmitter();

  constructor(activatedRoute: ActivatedRoute) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
    this.updateOnSave = true;
    this.activatedRoute.queryParams.subscribe((params) => {
      this.queryParam = params['queryParam'];
      this.resetIA = params['resetIA'] == 'true' ? true : false;

    });

    if (!applicationService.model.InspectorClass?.InspectorCountryOfWork) {
      applicationService.model.InspectorClass!.InspectorCountryOfWork =
        new BuildingInspectorCountryOfWork();
    } else {
      this.model =
        applicationService.model.InspectorClass?.InspectorCountryOfWork;
    }



    const demandModel = this.DemandModel();
    const countryKeys = ['England', 'Wales'];

    this.selections.push(
      ...countryKeys.filter((key) => demandModel[key] === true)
    );

    this.originalModelStringified = JSON.stringify(this.model);

  }

  override async onSave(applicationService: ApplicationService): Promise<void> {

    const demandModel = this.DemandModel();
    demandModel.England = false;
    demandModel.Wales = false;

    this.selections.forEach((value: keyof typeof demandModel) => {
      demandModel[value] = true;
    });

    this.model!.England = demandModel.England;
    this.model!.Wales = demandModel.Wales;

    // if (this.selections.length > 0) {
    //   this.model!.CompletionState = ComponentCompletionState.Complete;
    // } else {
    //   this.model!.CompletionState = ComponentCompletionState.InProgress;
    // }


    applicationService.model.InspectorClass!.InspectorCountryOfWork = this.model;

  }

  override async saveAndComeBack(): Promise<void> {

    this.processing = true;
    let canSave = this.selections.length == 0 || this.isValid();
    this.hasErrors = !canSave;
    this.model!.Wales = this.selections.includes('Wales');
    this.model!.England = this.selections.includes('England');
    //------------------------------------------------------------------------------
    // If the model implements IComponentModel, see if the model had been previously
    // completed. If the model has been modified, set the completion state to in progress,
    // otherwise leave it as completed.
    //------------------------------------------------------------------------------
    if (this.modelImplementsIComponent(this.model)) {
      var componentModel = this.model as IComponentModel;
      if (componentModel.CompletionState === ComponentCompletionState.Complete) {
        if(this.originalModelStringified !== JSON.stringify(this.model)) {
          console.log('model changed');
          componentModel.CompletionState = ComponentCompletionState.InProgress;
        }
      }
    }

    if (!this.hasErrors) {
      this.triggerScreenReaderNotification();
      this.applicationService.updateLocalStorage();
      if (this.updateOnSave) {
        await this.onSave(this.applicationService);
        await this.applicationService.updateApplication();
          }
          this.navigationService.navigate(
            `application/${this.applicationService.model.id}`
          );    } else {
      this.focusAndUpdateErrors();
    }
    this.processing = false;
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
      this.errorAnchorId = `england-${this.checkboxGroup?.innerId}`;

    }
    return this.selections.length > 0;
  }

  override navigateNext(): Promise<boolean> {
    if (
      this.queryParam != null &&
      this.queryParam != undefined &&
      this.queryParam != ''
    ) {
      const queryParam = this.queryParam;

      if(this.queryParam == "application-summary" && this.resetIA == true)
      {
        return this.navigationService.navigateRelative(
          BuildingInspectorSummaryComponent.route,
          this.activatedRoute,
          { resetIA: this.resetIA, queryParam: queryParam }
        );
      }

      if(this.queryParam == "application-summary")
      {
        return this.navigationService.navigateRelative(
          `../application-submission/${ApplicationSummaryComponent.route}`,
          this.activatedRoute
        );
      }

    }
    return this.navigationService.navigateRelative(
      BuildingInspectorSummaryComponent.route,
      this.activatedRoute,
      { resetIA: this.resetIA}
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


