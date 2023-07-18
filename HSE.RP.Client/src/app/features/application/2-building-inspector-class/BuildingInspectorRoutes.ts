import { BuildingProfessionalModel } from '../../../services/application.service';
import { NavigationService } from 'src/app/services/navigation.service';
import { FieldValidations } from '../../../helpers/validators/fieldvalidations';
import { Injectable } from '@angular/core';

export const BuildingInspectorRoutes = {
  CLASS_SELECTION: 'building-inspector-class-selection',
  REGULATED_ACTIVITIES: 'building-inspector-regulated-activities',
  SUMMARY: 'building-inspector-summary',
  TASK_LIST: ''
}
/// <summary>
/// This class is used to navigate to the different components of
/// the personal details section or back to the task list.
/// </summary>
@Injectable({ providedIn: 'root' })
export class BuildingInspectorRouter {
  constructor(private navigationService: NavigationService) {
  }
  public navigateTo(model: BuildingProfessionalModel, component: string): Promise<boolean> {
    // This initial will bring the user to the task list.
    const taskListRoute: string = `application/${model.id}`;
    var useRoute = taskListRoute;
    // If a component is specified then we will navigate to that component.
    if (FieldValidations.IsNotNullOrWhitespace(component)) {
      useRoute += `/building-inspector/${component}`;
    }

    return this.navigationService.navigate(useRoute);
  };
}
