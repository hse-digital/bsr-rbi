import { Component } from '@angular/core';
import { CompetencyRouter, CompetencyRoutes } from '../CompetencyRoutes';
import { PageComponent } from 'src/app/helpers/page.component';
import { environment } from 'src/environments/environment';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { ApplicationService } from 'src/app/services/application.service';
import { DateBase } from 'src/app/models/date-base.model';
import { DateFormatHelper } from 'src/app/helpers/date-format-helper';
import { FieldValidations } from 'src/app/helpers/validators/fieldvalidations';

@Component({
  selector: 'hse-independent-assessment-check-answers',
  templateUrl: './independent-assessment-check-answers.component.html'
})
export class IndependentAssessmentCheckAnswersComponent extends PageComponent<string> {  
  public static route: string = CompetencyRoutes.INDEPENDENT_ASSESSMENT_SUMMARY;
  static title: string = "Summary of your independent assessment - Register as a building inspector - GOV.UK";
  
  competencyRoutes = CompetencyRoutes;
  production: boolean = environment.production;
  modelValid: boolean = false;
  
  private competencyRouter: CompetencyRouter;
  override model?: string;
  override processing = false;

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService,
    competencyRouter: CompetencyRouter) {
    super(activatedRoute);
    this.competencyRouter = competencyRouter;
    this.updateOnSave = true;
  }

  override onInit(applicationService: ApplicationService): void { }

  override async onSave(applicationService: ApplicationService): Promise<void> { }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return true;
  }

  override isValid(): boolean {
    return true;
  }

  override navigateNext(): Promise<boolean> {
    return this.competencyRouter.navigateTo(this.applicationService.model, this.competencyRoutes.TASK_LIST);
  }

  public navigateTo(route: string) {
    return this.navigationService.navigateRelative(`${route}`, this.activatedRoute);
  }

  public GetFormattedDate(date: DateBase): string {
    return this.IsDateValid(date) ? DateFormatHelper.LongMonthFormat(date?.Year, date?.Month, date?.Day) : "";
  }

  private IsDateValid(date: DateBase) {
    return FieldValidations.IsNotNullOrWhitespace(date.Day) 
      && FieldValidations.IsNotNullOrWhitespace(date.Month)
      && FieldValidations.IsNotNullOrWhitespace(date.Year);
  }

}
