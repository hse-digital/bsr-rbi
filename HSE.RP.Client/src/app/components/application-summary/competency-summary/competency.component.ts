import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { DateFormatHelper } from '../../../helpers/date-format-helper';
import { PersonalDetails } from '../../../models/personal-details.model';
import { Competency } from 'src/app/models/competency.model';

@Component({
  selector: 'hse-competency-details',
  templateUrl: './competency-details.component.html',
  styleUrls: ['./competency-details.scss']
})
export class ApplicationSummaryCompetencyDetailsComponent implements OnInit{
  @Input() BuildingInspectorCompetencyDetails?: Competency;
  @Output() onNavigateTo = new EventEmitter<string>();



  assessPlansCategories: string = "";
  assessPlansLink: string = "";

  inspectionCategories: string = "";
  inspectionLink: string = "";

  ngOnInit(): void {

  }

  public getIndependentAssessmentStatus(): string {
    return this.BuildingInspectorCompetencyDetails
      ?.CompetencyIndependentAssessmentStatus?.IAStatus === 'yes'
      ? 'Yes'
      : 'None';
  }

  public isCompetencyAssessmentStatusYes(): boolean {
    return this.BuildingInspectorCompetencyDetails?.CompetencyIndependentAssessmentStatus?.IAStatus === 'yes';
  }

  public getCompetencyAssessmentOrg(): string {
    return this.BuildingInspectorCompetencyDetails
      ?.CompetencyAssessmentOrganisation?.ComAssessmentOrganisation === 'BSCF'
      ? 'Building Safety Competence Foundation (BSCF)'
      : 'Chartered Association of Building Engineers (CABE)';
  }

  public getCompetencyAssessmentCertificateNo(): string {
    return (
      this.BuildingInspectorCompetencyDetails
        ?.CompetencyAssessmentCertificateNumber?.CertificateNumber || 'None'
    );
  }

  public GetFormattedDateOfAssessment(): string {
    return DateFormatHelper.LongMonthFormat(
      this.BuildingInspectorCompetencyDetails?.CompetencyDateOfAssessment
        ?.Year,
      this.BuildingInspectorCompetencyDetails?.CompetencyDateOfAssessment
        ?.Month,
      this.BuildingInspectorCompetencyDetails?.CompetencyDateOfAssessment?.Day
    );
  }


  public navigateTo(name: string) {
    return this.onNavigateTo.emit(name);
  }
}
