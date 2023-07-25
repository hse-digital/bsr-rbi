import { NavigationService } from 'src/app/services/navigation.service';
import { FieldValidations } from '../../../helpers/validators/fieldvalidations';
import { Injectable } from '@angular/core';
import { BuildingProfessionalModel } from 'src/app/models/building-professional.model';

export const PersonalDetailRoutes = {
  ADDRESS: 'applicant-address',
  ALT_EMAIL: 'applicant-alternative-email',
  ALT_PHONE: 'applicant-alternative-phone',
  DATE_OF_BIRTH: 'applicant-date-of-birth',
  NAME: 'applicant-name',
  NATIONAL_INS_NUMBER: 'applicant-national-insurance-number',
  PROOF_OF_ID: 'applicant-photo',
  SUMMARY: 'applicant-summary',
  TASK_LIST: ''
}
/// <summary>
/// This class is used to navigate to the different components of
/// the personal details section or back to the task list.
/// </summary>
@Injectable({ providedIn: 'root' })
export class PersonalDetailRouter {
  constructor(private navigationService: NavigationService) {
  }
  public navigateTo(model: BuildingProfessionalModel, component: string): Promise<boolean>  {
    // This initial will bring the user to the task list.
    const taskListRoute: string = `application/${model.id}`;
    var useRoute = taskListRoute;
    // If a component is specified then we will navigate to that component.
    if (FieldValidations.IsNotNullOrWhitespace(component)) {
      useRoute += `/personal-details/${component}`;
    }

    return this.navigationService.navigate(useRoute);
  };
}
