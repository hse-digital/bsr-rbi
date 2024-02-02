import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { PageComponent } from 'src/app/helpers/page.component';
import { ApplicationService } from 'src/app/services/application.service';
import {
  BuildingInspectorRouter,
  BuildingInspectorRoutes,
} from '../BuildingInspectorRoutes';
import { environment } from 'src/environments/environment';
import { BuildingInspectorSummaryComponent } from '../building-inspector-summary/building-inspector-summary.component';
import { IComponentModel } from '../../../../models/component. interface';
import { ComponentCompletionState } from '../../../../models/component-completion-state.enum';
import { ApplicationSummaryComponent } from '../../5-application-submission/application-summary/application-summary.component';
import { Competency } from 'src/app/models/competency.model';
import { StageCompletionState } from 'src/app/models/stage-completion-state.enum';

class YesNoModel implements IComponentModel {
  YesNo: string = '';
  CompletionState: ComponentCompletionState = ComponentCompletionState.NotStarted;
}


@Component({
  selector: 'hse-building-class-technical-manager',
  templateUrl: './building-class-technical-manager.component.html',
  styles: [],
})
export class BuildingClassTechnicalManagerComponent extends PageComponent<YesNoModel> {
  public static route: string = BuildingInspectorRoutes.CLASS_TECHNICAL_MANAGER;
  public id: string = BuildingInspectorSummaryComponent.route;
  static title =
    'Building inspector class - Register as class 4 - Register as a building inspector - GOV.UK';
  production = environment.production;
  errorMessage: string = '';
  modelValid: boolean = false;
  queryParam?: string = '';
  resetIA?: boolean = false;


  constructor(
    activatedRoute: ActivatedRoute,
    private buildingInspectorRouter: BuildingInspectorRouter
  ) {
    super(activatedRoute);
  }

  override onInit(applicationService: ApplicationService): void {
    this.activatedRoute.queryParams.subscribe((params) => {
      this.queryParam = params['queryParam'];
      this.resetIA = params['resetIA'] == 'true' ? true : false;

    });

    this.updateOnSave = true;
    this.model = new YesNoModel();

    if(applicationService.model.InspectorClass?.ClassTechnicalManager) {
      this.model!.YesNo = applicationService.model.InspectorClass?.ClassTechnicalManager;
    }

    this.applicationService = applicationService;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    /**
     * Below block of code condition: "if (['no', 'yes'].includes(this.selectedOption))"
     * is equal to condition: "if (this.selectedOption === 'no' || this.selectedOption === 'yes')""
     **/

    if (this.model && ['no', 'yes'].includes(this.model?.YesNo)) {
      applicationService.model.InspectorClass!.ClassTechnicalManager = this.model?.YesNo;
    }


    applicationService.model.InspectorClass!.CompletionState = ComponentCompletionState.Complete

  }

  override async saveAndComeBack(): Promise<void> {


      this.applicationService.model.InspectorClass!.ClassTechnicalManager = this.model?.YesNo;
      const taskListRoute: string = `application/${this.applicationService.model.id}`;
      this.navigationService.navigate(taskListRoute);
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return true;
  }

  override isValid(): boolean {
    this.hasErrors = false;
    this.errorMessage = '';

    if (this.model!.YesNo === '') {
      this.hasErrors = true;
      this.errorMessage = 'Select one option';
    }

    return !this.hasErrors;
  }

  override async navigateNext(): Promise<boolean> {


    if (this.queryParam === 'inspector-class-change') {
      return this.navigationService.navigateRelative(
        BuildingInspectorSummaryComponent.route,
        this.activatedRoute
      );
    }


    else if(this.queryParam == "application-summary" && this.resetIA == true)
    {
      this.applicationService.model.StageStatus['Competency'] = StageCompletionState.Incomplete;
      this.applicationService.model.Competency = new Competency();
      return this.navigationService.navigate(
        `application/${this.applicationService.model.id}`
      );
    }

    else if (this.queryParam == 'application-summary') {
      return this.navigationService.navigateRelative(
        `../application-submission/${ApplicationSummaryComponent.route}`,
        this.activatedRoute
      );
    }

    else{
    return this.buildingInspectorRouter.navigateTo(
      this.applicationService.model,
      BuildingInspectorRoutes.INSPECTOR_COUNTRY
    );
    }
  }


}
