import { Injectable } from '@angular/core';
import { FieldValidations } from 'src/app/helpers/validators/fieldvalidations';
import { BuildingProfessionalModel } from 'src/app/models/building-professional.model';
import { NavigationService } from 'src/app/services/navigation.service';
import { CompetencyModule } from './application.competency.module';

export const CompetencyRoutes = {
  INDEPENDENT_COMPETENCY_STATUS: 'independent-competency-status',
  COMPETENCY_ASSESSMENT_ORGANISATION: 'competency-assesesment-organisation',
  NO_COMPETENCY_ASSESSMENT: 'no-competency-assessment',
  COMPETENCY_ASSESSMENT_DATE: 'competency-assesesment-date',
  COMPETENCY_ASSESSMENT_CERTIFICATE_NUMBER:
    'competency-assessment-certificate-number',
  CONFIRMATION_IA_UPDATE: 'confirmation-ia-update',
  SUMMARY: 'competency-summary',
  TASK_LIST: '',
};

@Injectable({ providedIn: 'root' })
export class CompetencyRouter {
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
      useRoute += `/${CompetencyModule.baseRoute}/${component}`;
    }
    return this.navigationService.navigate(useRoute);
  }
}
