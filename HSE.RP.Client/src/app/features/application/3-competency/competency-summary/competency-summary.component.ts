import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService } from '../../../../services/application.service';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { CompetencyRouter, CompetencyRoutes } from '../CompetencyRoutes';
import { DateFormatHelper } from 'src/app/helpers/date-format-helper';
import { DateBase } from 'src/app/models/date-base.model';

@Component({
  selector: 'hse-competency-summary',
  templateUrl: './competency-summary.component.html',
})
export class CompetencySummaryComponent extends PageComponent<string> {  
  public static route: string = CompetencyRoutes.COMPETENCY_SUMMARY;
  static title: string = "Competency - Register as a building inspector - GOV.UK";
  
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
