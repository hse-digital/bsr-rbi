import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../../services/application.service';
import { BuildingInspectorClassType } from 'src/app/models/building-inspector-classtype.enum';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import {
  BuildingInspectorRoutes,
  BuildingInspectorRouter,
} from '../BuildingInspectorRoutes';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { StageCompletionState } from 'src/app/models/stage-completion-state.enum';
import { Competency } from 'src/app/models/competency.model';
import { NextStage } from 'src/app/helpers/next-section.helper';

@Component({
  selector: 'hse-building-inspector-summary',
  templateUrl: './building-inspector-summary.component.html'
})
export class BuildingInspectorSummaryComponent extends PageComponent<string> {
  override async onSave(applicationService: ApplicationService): Promise<void> {
    this.applicationService.model.StageStatus['BuildingInspectorClass'] =
      StageCompletionState.Complete;
  }

  DerivedIsComplete(value: boolean): void { }

  BuildingInspectorRoutes = BuildingInspectorRoutes;
  BuildingInspectorClassType = BuildingInspectorClassType;

  public static route: string = BuildingInspectorRoutes.SUMMARY;
  static title: string =
    'Building inspector class - Summary - Register as a building inspector - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  override model?: string;
  queryParam?: string = '';
  resetIA?: boolean = false;
  assessPlansCategories: string = '';
  assessPlansLink: string = '';

  inspectionCategories: string = '';
  inspectionLink: string = '';

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService,
    private buildingInspectorRouter: BuildingInspectorRouter
  ) {
    super(activatedRoute);
    this.updateOnSave = false;
  }

  override onInit(applicationService: ApplicationService): void {
    //this.model = applicationService.model.personalDetails?.applicantPhoto?.toString() ?? '';
    this.activatedRoute.queryParams.subscribe((params) => {
      this.queryParam = params['queryParam'];
      this.resetIA = params['resetIA'] == 'true' ? true : false;
    });
    if (
      applicationService.model.InspectorClass?.ClassType.Class ===
      BuildingInspectorClassType.Class2
    ) {
      this.assessPlansLink =
        BuildingInspectorRoutes.CLASS2_ACCESSING_PLANS_CATEGORIES;
      this.inspectionLink =
        BuildingInspectorRoutes.CLASS2_INSPECT_BUILDING_CATEGORIES;
      this.inspectionCategories = this.categoriesToString(
        applicationService.model.InspectorClass?.Class2InspectBuildingCategories
      );
      this.assessPlansCategories = this.categoriesToString(
        applicationService.model.InspectorClass?.AssessingPlansClass2
      );
    }

    if (
      applicationService.model.InspectorClass?.ClassType.Class ===
      BuildingInspectorClassType.Class3
    ) {
      this.assessPlansLink =
        BuildingInspectorRoutes.CLASS3_ACCESSING_PLANS_CATEGORIES;
      this.inspectionLink =
        BuildingInspectorRoutes.CLASS3_INSPECT_BUILDING_CATEGORIES;

      this.inspectionCategories = this.categoriesToString(
        applicationService.model.InspectorClass?.Class3InspectBuildingCategories
      );
      this.assessPlansCategories = this.categoriesToString(
        applicationService.model.InspectorClass?.AssessingPlansClass3
      );
    }
  }

  async SyncAndContinue() {
    if (!this.processing) {
      this.processing = true;
      if (this.applicationService.model.InspectorClass?.ClassType.Class === BuildingInspectorClassType.Class1) {
        this.applicationService.model.InspectorClass!.ClassTechnicalManager = 'no';
        this.applicationService.model.Competency = new Competency();
        this.applicationService.model.InspectorClass.CompletionState = ComponentCompletionState.Complete;
        this.applicationService.model.StageStatus!['Competency'] = StageCompletionState.Complete;
      }
      this.applicationService.model.InspectorClass!.CompletionState = ComponentCompletionState.Complete;
      await this.applicationService.syncBuildingInspectorClass();
      this.applicationService.model.StageStatus['BuildingInspectorClass'] = StageCompletionState.Complete;
      this.saveAndContinue();
    }
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {



    return true;
    //! check if previous section (country) is comeplete
    //return (this.applicationService.model.InspectorClass?.InspectorCountryOfWork?.CompletionState === ComponentCompletionState.Complete);
  }

  override isValid(): boolean {
    return true;
  }

  override navigateNext(): Promise<boolean> {


    return this.navigationService.navigateRelative(
      `../${ApplicationTaskListComponent.route}`,
      this.activatedRoute, undefined, NextStage.getNextStage(this.applicationService.model)
    );

  }

  public navigateTo(route: string) {
    const queryParam = 'inspector-class-change';
    return this.navigationService.navigateRelative(
      `${route}`,
      this.activatedRoute,
      { queryParam }
    );
  }

  categoriesToString(categories: any): string {
    // loop through keys in object
    // if keys value is true, add the last character of the key name to string with a comma
    // when the loop is complete, remove the trailing comma and space
    let newString = '';

    Object.keys(categories).forEach((key) => {
      if (categories[key] === true) {
        newString = newString + key.slice(-1) + ', ';
      }
    });

    return newString.slice(0, -2);
  }

  public isCompetencyAssessmentStatusYes(): boolean {
    return this.applicationService.model!.Competency!.CompetencyIndependentAssessmentStatus?.IAStatus === 'yes' || this.applicationService.model!.Competency!.CompetencyIndependentAssessmentStatus?.IAStatus === '' || this.resetIA === true;
  }
}
