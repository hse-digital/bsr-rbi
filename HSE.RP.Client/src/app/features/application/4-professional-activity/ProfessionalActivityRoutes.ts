import { NavigationService } from 'src/app/services/navigation.service';
import { FieldValidations } from '../../../helpers/validators/fieldvalidations';
import { Injectable } from '@angular/core';
import { BuildingProfessionalModel } from 'src/app/models/building-professional.model';
import { ProfessionalActivityModule } from './application.professional-activity.module';

export const ProfessionalActivityRoutes = {
  PROFESSIONAL_BODY_MEMBERSHIPS: 'professional-body-memberships',
  PROFESSIONAL_BODY_SELECTION: 'professional-body-selection',
  PROFESSIONAL_MEMBERSHIP_INFORMATION: 'professional-membership-information',
  PROFESSIONAL_INDIVIDUAL_MEMEBERSHIP_DETAILS:
    'professional-individual-membership-details',
  PROFESSIONAL_CONFIRMATION_MEMBERSHIP_REMOVAL:
    'professional-confirmation-membership-removal',
  SEARCH_EMPLOYEMENT_ORG_ADDRESS: 'search-empoloyment-org-address',
};

@Injectable({ providedIn: 'root' })
export class ProfessionalActivityRouter {
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
      useRoute += `/${ProfessionalActivityModule.baseRoute}/${component}`;
    }

    return this.navigationService.navigate(useRoute);
  }
}
