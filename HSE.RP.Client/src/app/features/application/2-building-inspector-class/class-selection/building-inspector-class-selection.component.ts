import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { ApplicationService } from '../../../../services/application.service';
import { BuildingInspectorCountryComponent } from '../country/building-inspector-country.component';
import { BuildingInspectorRegulatedActivitiesComponent } from '../regulated-activities/building-inspector-regulated-activities.component';
import { BuildingInspectorRoutes } from '../BuildingInspectorRoutes';
import { ClassSelection } from 'src/app/models/class-selection.model';
import { BuildingInspectorClassType } from 'src/app/models/building-inspector-classtype.enum';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { BuildingAssessingPlansCategoriesClass3 } from 'src/app/models/buidling-assessing-plans-categories-class3.model';
import { BuildingAssessingPlansCategoriesClass2 } from 'src/app/models/building-assessing-plans-categories-class2.model';
import { Class2InspectBuildingCategories } from 'src/app/models/class2-inspect-building-categories.model';
import { Class3InspectBuildingCategories } from 'src/app/models/class3-inspect-building-categories.model';
import { Competency } from 'src/app/models/competency.model';
import { StageCompletionState } from 'src/app/models/stage-completion-state.enum';
import { ApplicationSummaryComponent } from '../../5-application-submission/application-summary/application-summary.component';

@Component({
  selector: 'hse-building-inspector-class-selection',
  templateUrl: './building-inspector-class-selection.component.html',
})
export class BuildingInspectorClassSelectionComponent extends PageComponent<ClassSelection> {
  public static route: string = BuildingInspectorRoutes.CLASS_SELECTION;
  static title: string =
    'Building inspector class - Class selection - Register as a building inspector - GOV.UK';

  production: boolean = environment.production;

  BuildingInspectorClassType = BuildingInspectorClassType;
  selectedOptionError: boolean = false;
  errorMessage: string = '';
  originalOption?: BuildingInspectorClassType =
    this.applicationService.model.InspectorClass?.ClassType.Class;
  queryParam?: string = '';
  resetIA?: boolean = false;

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService
  ) {
    super(activatedRoute);
    this.updateOnSave = true;
  }

  override onInit(applicationService: ApplicationService): void {
    this.activatedRoute.queryParams.subscribe((params) => {
      this.queryParam = params['queryParam'];
      this.resetIA = params['resetIA'] == 'true' ? true : false;
    });

    if (this.resetIA === true) {
      this.model = {
        Class: BuildingInspectorClassType.Class1,
        CompletionState: ComponentCompletionState.InProgress,
      };
    } else {
      if (
        applicationService.model.InspectorClass?.ClassType.Class ===
        BuildingInspectorClassType.ClassNone
      ) {
        applicationService.model.InspectorClass!.ClassType = {
          Class: BuildingInspectorClassType.ClassNone,
          CompletionState: ComponentCompletionState.InProgress,
        };
      }
      this.model = applicationService.model.InspectorClass?.ClassType;
    }
  }

  override async saveAndComeBack(): Promise<void> {
    this.processing = true;

    const STATUS =
      this.applicationService.model.InspectorClass?.ClassType.CompletionState;

    if (this.model?.Class === this.originalOption && STATUS === 2) {
      this.applicationService.model.InspectorClass!.ClassType.CompletionState =
        ComponentCompletionState.Complete;
    } else {
      this.applicationService.model.InspectorClass!.ClassType.CompletionState =
        ComponentCompletionState.InProgress;
    }

    this.applicationService.model.InspectorClass!.ClassType.Class =
      this.model?.Class;

    if (!this.hasErrors) {
      this.triggerScreenReaderNotification();
      this.applicationService.updateLocalStorage();
      await this.applicationService.updateApplication();
    } else {
      this.focusAndUpdateErrors();
    }

    this.processing = false;

    const taskListRoute: string = `application/${this.applicationService.model.id}`;
    this.navigationService.navigate(taskListRoute);
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    if (this.resetIA === true) {
      if (this.model?.Class === BuildingInspectorClassType.Class1) {
        this.applicationService.model.InspectorClass!.ClassType!.Class =
          this.originalOption;
      } else {
        this.applicationService.model.InspectorClass!.ClassType!.Class =
          this.model?.Class;
      }
    } else {
      this.applicationService.model.InspectorClass!.ClassType!.Class =
        this.model?.Class;
      // reset state if the user changes their input
      if (this.model!.Class !== this.originalOption) {
        // reset all other info to false
        this.applicationService.model.InspectorClass!.Activities = {
          AssessingPlans: false,
          Inspection: false,
          CompletionState: ComponentCompletionState.NotStarted,
        };

        this.applicationService.model.InspectorClass!.AssessingPlansClass2 =
          new BuildingAssessingPlansCategoriesClass2();
        this.applicationService.model.InspectorClass!.AssessingPlansClass3 =
          new BuildingAssessingPlansCategoriesClass3();
        this.applicationService.model.InspectorClass!.Class2InspectBuildingCategories =
          new Class2InspectBuildingCategories();
        this.applicationService.model.InspectorClass!.Class3InspectBuildingCategories =
          new Class3InspectBuildingCategories();

        //cannot be class 4 if you select class 1
        if (this.model?.Class === BuildingInspectorClassType.Class1) {
          this.applicationService.model.InspectorClass!.ClassTechnicalManager =
            'no';
          this.applicationService.model.InspectorClass!.ClassType.CompletionState =
            ComponentCompletionState.Complete;

          this.applicationService.model.Competency = new Competency();
          this.applicationService.model.StageStatus!['Competency'] =
            StageCompletionState.Complete;
        } else {
          this.applicationService.model.InspectorClass!.ClassType.CompletionState =
            ComponentCompletionState.InProgress;
        }
      }
    }
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    // if (this.applicationService.model.ApplicationStatus === ApplicationStatus.PersonalDetailsComplete) {
    //   return true;
    // }

    // return false;
    return true;
  }

  override isValid(): boolean {
    if (!this.model?.Class) {
      this.selectedOptionError = true;
      this.errorMessage =
        'Select a class of building inspector you are applying for';
      return false;
    }

    return true;
  }

  override navigateNext(): Promise<boolean> {
    if (
      this.queryParam != null &&
      this.queryParam != undefined &&
      this.queryParam != ''
    ) {
      const queryParam = this.queryParam;
      if (this.queryParam == 'application-summary') {


        if (this.model?.Class === BuildingInspectorClassType.Class1) {
          return this.navigationService.navigateRelative(
            `../application-submission/${ApplicationSummaryComponent.route}`,
            this.activatedRoute
          );
        }
        else if(this.originalOption === this.model?.Class) {
          if(this.resetIA===false)
          {
          return this.navigationService.navigateRelative(
            `../application-submission/${ApplicationSummaryComponent.route}`,
            this.activatedRoute
          );
          }
          else{
            return this.navigationService.navigateRelative(
              BuildingInspectorRegulatedActivitiesComponent.route,
              this.activatedRoute,
              { resetIA: true, queryParam: queryParam }
            );
          }
        }
        else {
          if(this.originalOption == BuildingInspectorClassType.Class1 && (this.model?.Class == BuildingInspectorClassType.Class2 || this.model?.Class == BuildingInspectorClassType.Class3)) {
            return this.navigationService.navigateRelative(
              BuildingInspectorRegulatedActivitiesComponent.route,
              this.activatedRoute,
              { resetIA: true, queryParam: queryParam }
            );
          }
          else{
          return this.navigationService.navigateRelative(
            BuildingInspectorRegulatedActivitiesComponent.route,
            this.activatedRoute,
            { resetIA: this.resetIA, queryParam: queryParam }
          );
          }
        }
      }

    }
    if (this.model?.Class === BuildingInspectorClassType.Class1) {
      return this.navigationService.navigateRelative(
        BuildingInspectorCountryComponent.route,
        this.activatedRoute
      );
    } else {
      return this.navigationService.navigateRelative(
        BuildingInspectorRegulatedActivitiesComponent.route,
        this.activatedRoute
      );
    }
  }

  DerivedIsComplete(value: boolean): void {
    // this.applicationService.model.BuildingInspectorClass!.ClassSelection!.CompletionState = value ? ComponentCompletionState.Complete : ComponentCompletionState.InProgress;
  }
}
