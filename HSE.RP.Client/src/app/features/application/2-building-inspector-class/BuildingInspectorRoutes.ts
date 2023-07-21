import { BuildingProfessionalModel } from '../../../services/application.service';
import { NavigationService } from 'src/app/services/navigation.service';
import { FieldValidations } from '../../../helpers/validators/fieldvalidations';
import { Injectable } from '@angular/core';
import { BuildingInspectorClassModule } from './application.building-inspector-class.module';
export const BuildingInspectorRoutes = {
  CLASS_SELECTION: 'building-inspector-class-selection',
  REGULATED_ACTIVITIES: 'building-inspector-regulated-activities',
  PLANS_CATEGORIES: 'building-assessing-plans-categories',
  CLASS_TECHNICAL_MANAGER: 'building-class-technical-manager',
  INSPECTOR_COUNTRY: 'building-inspector-country',
  SUMMARY: 'building-inspector-summary',
  TASK_LIST: '',
  ASSESSING_PLANS_CLASS_3: 'building-inspector-assessing-plans-class3'
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
      useRoute += `/${BuildingInspectorClassModule.baseRoute}/${component}`;
    }

    return this.navigationService.navigate(useRoute);
  };
}
