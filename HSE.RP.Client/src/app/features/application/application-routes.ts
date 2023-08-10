import { NavigationService } from 'src/app/services/navigation.service';
import { FieldValidations } from '../../helpers/validators/fieldvalidations';
import { Injectable } from '@angular/core';
import { BuildingProfessionalModel } from 'src/app/models/building-professional.model';
import { ApplicationPersonalDetailsModule } from './1-personal-details/application.personal-details.module';
import { BuildingInspectorClassModule } from './2-building-inspector-class/application.building-inspector-class.module';
import { CompetencyModule } from './3-competency/application.competency.module';
import { ProfessionalActivityModule } from './4-professional-activity/application.professional-activity.module';

export const PersonalDetailRoutes = {
  ADDRESS: 'applicant-address',
  ALT_EMAIL: 'applicant-alternative-email',
  ALT_PHONE: 'applicant-alternative-phone',
  DATE_OF_BIRTH: 'applicant-date-of-birth',
  NAME: 'applicant-name',
  NATIONAL_INS_NUMBER: 'applicant-national-insurance-number',
  PROOF_OF_ID: 'applicant-photo',
  SUMMARY: 'applicant-summary',
};

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
};

export const CompetencyRoutes = {
  INDEPENDENT_COMPETENCY_STATUS: 'independent-competency-status',
  COMPETENCY_ASSESSMENT_ORGANISATION: 'competency-assesesment-organisation',
  NO_COMPETENCY_ASSESSMENT: 'no-competency-assessment',
  COMPETENCY_ASSESSMENT_DATE: 'competency-assesesment-date',
  COMPETENCY_ASSESSMENT_CERTIFICATE_NUMBER:
    'competency-assessment-certificate-number',
  SUMMARY: 'competency-summary',
};

export const ProfessionalActivityRoutes = {
  PROFESSIONAL_BODY_MEMBERSHIPS: 'professional-body-memberships',
  PROFESSIONAL_BODY_SELECTION: 'professional-body-selection',
  PROFESSIONAL_BODY_SUMMARY: 'professional-body-summary',
  PROFESSIONAL_BODY_REMOVE: 'professional-body-remove',
  PROFESSIONAL_BODY_MODIFY: 'professional-body-modify',
  PROFESSIONAL_MEMBERSHIP_INFORMATION: 'professional-membership-information',
  PROFESSIONAL_ACTIVITY_SUMMARY: 'professional-activity-summary',
};

@Injectable({ providedIn: 'root' })
export class PersonalDetailRouter {
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
      useRoute += `/personal-details/${component}`;
      useRoute += `/${ApplicationPersonalDetailsModule.baseRoute}/${component}`;

    }

    return this.navigationService.navigate(useRoute);
  }
}

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
