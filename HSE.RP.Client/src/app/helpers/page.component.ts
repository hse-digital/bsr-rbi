import {
  Component,
  Injector,
  OnInit,
  QueryList,
  ViewChildren,
} from '@angular/core';
import { Observable } from 'rxjs';
import { GetInjector } from './injector.helper';
import { ApplicationService } from '../services/application.service';
import { GovukErrorSummaryComponent } from 'hse-angular';
import { TitleService } from '../services/title.service';
import { GovukRequiredDirective } from '../components/required.directive';
import { NavigationService } from '../services/navigation.service';
import { NotFoundComponent } from '../components/not-found/not-found.component';
import {
  ActivatedRoute,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { IComponentModel } from '../models/component. interface';
import { ComponentCompletionState } from '../models/component-completion-state.enum';
import { ApplicationStage } from '../models/application-stage.enum';

@Component({ template: '' })
export abstract class PageComponent<T> implements OnInit {
  // This implementation should really extend the generic to an IComponentModel as in the export statement below ,
  // but it's breaking too much at the moment to fix this up, just don't have the time.
  //export abstract class PageComponent<T extends IComponentModel> implements OnInit {
  model?: T;
  processing: boolean = false;
  hasErrors: boolean = false;
  updateOnSave: boolean = true;
  originalModelStringified: string = '';
  private injector: Injector = GetInjector();
  protected applicationService: ApplicationService =
    this.injector.get(ApplicationService);
  protected titleService: TitleService = this.injector.get(TitleService);
  protected navigationService: NavigationService =
    this.injector.get(NavigationService);

  @ViewChildren(GovukRequiredDirective)
  private requiredFields?: QueryList<GovukRequiredDirective>;
  private summaryError?: QueryList<GovukErrorSummaryComponent>;

  abstract onInit(applicationService: ApplicationService): void;
  abstract onSave(applicationService: ApplicationService): Promise<void>;
  abstract canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean;
  abstract isValid(): boolean;
  abstract navigateNext(): Promise<boolean>;

  constructor(protected activatedRoute: ActivatedRoute) {
    this.triggerScreenReaderNotification('');
  }

  ngOnInit(): void {
    this.onInit(this.applicationService);
    this.originalModelStringified = JSON.stringify(this.model);
    if (this.modelImplementsIComponent(this.model)) {
      var componentModel = this.model as IComponentModel;
      if (
        componentModel.CompletionState === ComponentCompletionState.NotStarted
      ) {
        componentModel.CompletionState = ComponentCompletionState.InProgress;
      }
    }
    this.updateApplicationStage();
  }

  /**
   * Updates the application stage based on the current route.
   * @returns void
   */
  updateApplicationStage() {
    var currentRoute = this.getCurrentRoute();
    var section = currentRoute.split('/')[3];
    var hasChanged = false;
    if (section) {
      switch (section) {
        case 'personal-details':
          if (
            this.applicationService.model.ApplicationStage ==
            ApplicationStage.PersonalDetails
          ) {
            hasChanged = false;
          } else {
            this.applicationService.model.ApplicationStage =
              ApplicationStage.PersonalDetails;
            hasChanged = true;
          }
          break;
        case 'building-inspector-class':
          if (
            this.applicationService.model.ApplicationStage ==
            ApplicationStage.BuildingInspectorClass
          ) {
            hasChanged = false;
          } else {
            this.applicationService.model.ApplicationStage =
              ApplicationStage.BuildingInspectorClass;
            hasChanged = true;
          }
          break;
        case 'competency':
          if (
            this.applicationService.model.ApplicationStage ==
            ApplicationStage.Competency
          ) {
            hasChanged = false;
          } else {
            this.applicationService.model.ApplicationStage =
              ApplicationStage.Competency;
            hasChanged = true;
          }
          break;
        case 'professional-activity':
          if (
            this.applicationService.model.ApplicationStage ==
            ApplicationStage.ProfessionalMembershipsAndEmployment
          ) {
            hasChanged = false;
          } else {
            this.applicationService.model.ApplicationStage =
              ApplicationStage.ProfessionalMembershipsAndEmployment;
            hasChanged = true;
          }
          break;
        case 'application-submission':
          console.log("application-submission");
          if (currentRoute.split('/')[4] === 'application-summary') {
            if (
              this.applicationService.model.ApplicationStage ==
              ApplicationStage.ApplicationSummary
            ) {
              hasChanged = false;
            } else {
              this.applicationService.model.ApplicationStage =
                ApplicationStage.ApplicationSummary;
                hasChanged = true;
            }
            break;
          } else if (currentRoute.split('/')[4] === 'payment') {
            if (currentRoute.split('/')[5] === 'declaration') {
              if (
                this.applicationService.model.ApplicationStage ==
                ApplicationStage.PayAndSubmit
              ) {
                hasChanged = false;
              } else {
                this.applicationService.model.ApplicationStage =
                  ApplicationStage.PayAndSubmit;
                hasChanged = true;
              }
              break;
            }
          }
          break;
        default:
          // Add code to handle unknown section
          break;
      }
      if(hasChanged){
        this.applicationService.syncApplicationStage();
      }
    }
  }

  modelImplementsIComponent(obj: any): obj is IComponentModel {
    return (
      this.model !== undefined &&
      this.model !== null &&
      'CompletionState' in this.model &&
      (obj.CompletionState === undefined ||
        Object.values(ComponentCompletionState).includes(obj.CompletionState))
    );
  }

  async saveAndContinue(): Promise<void> {
    this.processing = true;
    this.hasErrors = !this.isValid();
    if (!this.hasErrors) {
      this.triggerScreenReaderNotification();
      //------------------------------------------------------------------------------
      // If the model implements IComponentModel, set the completion state to complete
      //------------------------------------------------------------------------------
      if (this.modelImplementsIComponent(this.model)) {
        var componentModel = this.model as IComponentModel;
        componentModel.CompletionState = ComponentCompletionState.Complete;
      }
      this.onSave(this.applicationService);
      this.applicationService.updateLocalStorage();
      if (this.updateOnSave) {
        await this.saveAndUpdate();
      }

      let navigationSucceeded = await this.navigateNext();
      if (!navigationSucceeded) {
        await this.navigationService.navigate(NotFoundComponent.route);
      }
    } else {
      this.focusAndUpdateErrors();
    }

    this.processing = false;
  }

  async saveAndComeBack(): Promise<void> {
    this.processing = true;
    let canSave = this.requiredFieldsAreEmpty() || this.isValid();
    this.hasErrors = !canSave;
    //------------------------------------------------------------------------------
    // If the model implements IComponentModel, see if the model had been previously
    // completed. If the model has been modified, set the completion state to in progress,
    // otherwise leave it as completed.
    //------------------------------------------------------------------------------
    if (this.modelImplementsIComponent(this.model)) {
      var componentModel = this.model as IComponentModel;
      if (
        componentModel.CompletionState === ComponentCompletionState.Complete
      ) {
        if (this.originalModelStringified !== JSON.stringify(this.model)) {
          componentModel.CompletionState = ComponentCompletionState.InProgress;
        }
      }
    }

    if (!this.hasErrors) {
      this.triggerScreenReaderNotification();
      this.applicationService.updateLocalStorage();
      if (this.updateOnSave) {
        await this.saveAndUpdate();
      }
      await this.navigateBack();
    } else {
      this.focusAndUpdateErrors();
    }

    this.processing = false;
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    _: RouterStateSnapshot
  ):
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {
    if (!this.canAccess(this.applicationService, route)) {
      this.navigationService.navigate(NotFoundComponent.route);
      return false;
    }

    return true;
  }

  getErrorDescription(
    showError: boolean,
    errorMessage: string
  ): string | undefined {
    return this.hasErrors && showError ? errorMessage : undefined;
  }

  triggerScreenReaderNotification(message: string = 'Sending success') {
    var alertContainer = document!.getElementById('hiddenAlertContainer');
    if (alertContainer) {
      alertContainer.innerHTML = message;
    }
  }

  getCurrentRoute(): string {
    return this.navigationService.getCurrentRoute();
  }

  private requiredFieldsAreEmpty() {
    return (
      this.requiredFields?.filter((x) => {
        if (Array.isArray(x.govukRequired.model)) {
          return x.govukRequired.model.length == 0;
        }

        return !x.govukRequired.model;
      }).length == this.requiredFields?.length
    );
  }

  private navigateBack(): Promise<boolean> {
    return this.navigationService.navigate(
      `application/${this.applicationService.model.id}`
    );
  }

  private async saveAndUpdate(): Promise<void> {
    await this.onSave(this.applicationService);
    await this.applicationService.updateApplication();
  }

  protected focusAndUpdateErrors() {
    this.summaryError?.first?.focus();
    this.titleService.setTitleError();
  }
}
