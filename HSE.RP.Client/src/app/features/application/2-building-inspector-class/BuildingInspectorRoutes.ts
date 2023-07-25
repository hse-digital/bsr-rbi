import { NavigationService } from 'src/app/services/navigation.service';
import { FieldValidations } from '../../../helpers/validators/fieldvalidations';
import { Injectable } from '@angular/core';
import { BuildingInspectorClassModule } from './application.building-inspector-class.module';
import { BuildingProfessionalModel } from 'src/app/models/building-professional.model';
export const BuildingInspectorRoutes = {
  CLASS_SELECTION: 'building-inspector-class-selection',
  REGULATED_ACTIVITIES: 'building-inspector-regulated-activities',
  CLASS2_ACCESSING_PLANS_CATEGORIES:
    'building-class2-assessing-plans-categories',
  CLASS3_ACCESSING_PLANS_CATEGORIES:
    'building-class3-assessing-plans-categories',
  CLASS2_INSPECT_BUILDING_CATEGORIES:
    'building-class2-inspect-building-categories',
  CLASS3_INSPECT_BUILDING_CATEGORIES:
    'building-class3-inspect-building-categories',
  CLASS_TECHNICAL_MANAGER: 'building-class-technical-manager',
  INSPECTOR_COUNTRY: 'building-inspector-country',
  SUMMARY: 'building-inspector-summary',
  TASK_LIST: '',
};
/// <summary>
/// This class is used to navigate to the different components of
/// the personal details section or back to the task list.
/// </summary>
@Injectable({ providedIn: 'root' })
export class BuildingInspectorRouter {
  constructor(private navigationService: NavigationService) {}
  public navigateTo(
    model: BuildingProfessionalModel,
    component: string
  ): Promise<boolean> {
    // This initial will bring the user to the task list.
    const taskListRoute: string = `application/${model.id}`;
    var useRoute = taskListRoute;
    // If a component is specified then we will navigate to that component.
    if (FieldValidations.IsNotNullOrWhitespace(component)) {
      useRoute += `/${BuildingInspectorClassModule.baseRoute}/${component}`;
    }

    return this.navigationService.navigate(useRoute);
  }
}
