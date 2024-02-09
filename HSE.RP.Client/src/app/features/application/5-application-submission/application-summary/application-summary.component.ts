import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { ApplicationService } from '../../../../services/application.service';
import { DateFormatHelper } from 'src/app/helpers/date-format-helper';
import { BuildingProfessionalModel } from 'src/app/models/building-professional.model';
import { ApplicationStatus } from 'src/app/models/application-status.enum';
import { StageCompletionState } from 'src/app/models/stage-completion-state.enum';
import { AddressModel } from 'src/app/models/address.model';
import {
  PersonalDetailRouter,
  PersonalDetailRoutes,
} from '../../1-personal-details/PersonalDetailRoutes';
import { ApplicationSubmissionModule } from '../application.application-submission.module';
import { BuildingInspectorRoutes } from '../../application-routes';
import { BuildingInspectorClassType } from '../../../../models/building-inspector-classtype.enum';
import { PaymentDeclarationComponent } from '../payment/payment-declaration/payment-declaration.component';
import html2canvas from 'html2canvas';
import { jsPDF } from 'jspdf';

@Component({
  selector: 'hse-application-summary',
  templateUrl: './application-summary.component.html',
})
export class ApplicationSummaryComponent extends PageComponent<string> {
  DerivedIsComplete(value: boolean): void {}
  PersonalDetailRoutes = PersonalDetailRoutes;

  BuildingInspectorRoutes = BuildingInspectorRoutes;
  BuildingInspectorClassType = BuildingInspectorClassType;

  public static route: string = 'application-summary';
  static title: string =
    'Register as a building inspector - Summary of your application - GOV.UK';
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  private personalDetailRouter: PersonalDetailRouter;
  override model?: string;
  override processing = false;

  // assessPlansCategories: string = "";
  // assessPlansLink: string = "";

  // inspectionCategories: string = "";
  // inspectionLink: string = "";

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService,
    personalDetailRouter: PersonalDetailRouter
  ) {
    super(activatedRoute);
    this.personalDetailRouter = personalDetailRouter;
    this.updateOnSave = true;
  }

  override onInit(applicationService: ApplicationService): void {}

  override async onSave(applicationService: ApplicationService): Promise<void> {
    // this.applicationService.model.StageStatus['PersonalDetails'] = StageCompletionState.Complete;
    // this.applicationService.model.ApplicationStatus = ApplicationStatus.PersonalDetailsComplete;
  }

  override canAccess(
    applicationService: ApplicationService,
    routeSnapshot: ActivatedRouteSnapshot
  ): boolean {
    return (
      this.applicationService.model.StageStatus!['EmailVerification'] ===
        StageCompletionState.Complete &&
      this.applicationService.model.StageStatus!['PhoneVerification'] ===
        StageCompletionState.Complete &&
      this.applicationService.model.StageStatus!['PersonalDetails'] ===
        StageCompletionState.Complete &&
      this.applicationService.model.StageStatus!['BuildingInspectorClass'] ===
        StageCompletionState.Complete &&
      this.applicationService.model.StageStatus!['Competency'] ===
        StageCompletionState.Complete &&
      this.applicationService.model.StageStatus!['ProfessionalActivity'] ===
        StageCompletionState.Complete
    );
  }

  override isValid(): boolean {
    return true;
  }

  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigateRelative(
      'payment/' + PaymentDeclarationComponent.route,
      this.activatedRoute
    );
  }

  public navigateToPersonalDetailsRoute(namedRoute: string) {
    return this.navigateTo(
      'personal-details/' + PersonalDetailRoutes.MapNameToRoute(namedRoute)
    );
  }

  public navigateToBuildingInspectorClassDetailsRoute(route: string) {
    return this.navigateTo('building-inspector-class/' + route);
  }

  public navigateToBuildingInspectorCompetencyDetailsRoute(route: string) {
    return this.navigateTo('competency/' + route);
  }

  public navigateToBuildingInspectorProfessionalActivityDetailsRoute(
    route: string
  ) {
    return this.navigateTo('professional-activity/' + route);
  }



  public navigateTo(route: string) {
    const queryParam = 'application-summary';
    return this.navigationService.navigateRelative(
      `../${route}`,
      this.activatedRoute,
      { queryParam }
    );
  }

  async SyncAndContinue() {
     await this.applicationService.syncFullApplication();
    this.applicationService.model.StageStatus['ApplicationConfirmed'] =
      StageCompletionState.Complete;
    this.saveAndContinue();
  }

  public getIndependentAssessmentStatus(): string {
    return this.applicationService.model.Competency
      ?.CompetencyIndependentAssessmentStatus?.IAStatus === 'yes'
      ? 'Yes'
      : 'No';
  }

  public isCompetencyAssessmentStatusYes(): boolean {
    return (
      this.applicationService.model.Competency?.CompetencyIndependentAssessmentStatus?.IAStatus === 'yes'
    );
  }

  public saveAsPDF() {
    const doc = new jsPDF();
    let htmlCotent = document.getElementById('pdfTApplicationSummary');

    this.generatePDF(htmlCotent!);
  }

  generatePDF(htmlCotent: HTMLElement) {
    html2canvas(htmlCotent!).then((canvas) => {
      let imgWidth = 290;
      let imgHeight = (canvas.height * imgWidth) / canvas.width;
      const contentDataURL = canvas.toDataURL('image/png');
      const pdf = new jsPDF({
        orientation: 'p',
        unit: 'mm',
        format: [600, 1000],
      });

      var position = 10;
      pdf.addImage(contentDataURL, 'PNG', 20, position, imgWidth, imgHeight);
      pdf.save('ApplicationSummary.pdf');
    });

  }

  printApplicationSummary() {
    window.print();
  }
}
