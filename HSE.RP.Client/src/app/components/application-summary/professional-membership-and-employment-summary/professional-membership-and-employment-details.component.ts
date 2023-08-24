import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { DateFormatHelper } from '../../../helpers/date-format-helper';
import { FieldValidations } from 'src/app/helpers/validators/fieldvalidations';
import { ProfessionalActivity } from 'src/app/models/professional-activity.model';
import { Competency } from 'src/app/models/competency.model';
import { ApplicantProfessionBodyMemberships } from 'src/app/models/applicant-professional-body-membership';
import { ProfessionalActivityHelper } from 'src/app/helpers/professional-activity-helper.component';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { EmploymentType } from 'src/app/models/employment-type.enum';

@Component({
  selector: 'hse-professional-membership-and-employment-details',
  templateUrl: './professional-membership-and-employment-details.component.html',
  styleUrls: ['./professional-membership-and-employment-details.scss']
})
export class ApplicationSummaryProfessionalMembershipAndEmploymentDetailsComponent implements OnInit{
  @Input() BuildingInspectorProfessionalActivityDetails?: ProfessionalActivity;
  @Input() BuildingInspectorProfessionalMembershipsDetails?: ApplicantProfessionBodyMemberships;
  @Input() BuildingInspectorCompetencyDetails?: Competency;
  @Output() onNavigateTo = new EventEmitter<string>();



  assessPlansCategories: string = "";
  assessPlansLink: string = "";

  inspectionCategories: string = "";
  inspectionLink: string = "";

  ngOnInit(): void {

  }

  public isCompetencyAssessmentStatusYes(): boolean {
    return this.BuildingInspectorCompetencyDetails?.CompetencyIndependentAssessmentStatus?.IAStatus === 'yes';
  }

  public getProfessionalBodyMemberships(): string[] {

    let professionalBodyMemberships: string[] = [];
    //if this.applicationService.model.ProfessionalMemberships members completion state is complete then add body name to array
    if (this.BuildingInspectorProfessionalMembershipsDetails!.CABE.CompletionState === ComponentCompletionState.Complete) {
      professionalBodyMemberships.push(ProfessionalActivityHelper.professionalBodyNames["CABE"]);
    }
    if (this.BuildingInspectorProfessionalMembershipsDetails!.CIOB.CompletionState === ComponentCompletionState.Complete) {
      professionalBodyMemberships.push(ProfessionalActivityHelper.professionalBodyNames["CIOB"]);
    }

    if (this.BuildingInspectorProfessionalMembershipsDetails!.RICS.CompletionState === ComponentCompletionState.Complete) {
      professionalBodyMemberships.push(ProfessionalActivityHelper.professionalBodyNames["RICS"]);
    }

    if (this.BuildingInspectorProfessionalMembershipsDetails!.OTHER.CompletionState === ComponentCompletionState.Complete) {
      professionalBodyMemberships.push(ProfessionalActivityHelper.professionalBodyNames["OTHER"]);
    }

    return professionalBodyMemberships;


  }

  public getEmploymentType(): EmploymentType {
    return this.BuildingInspectorProfessionalActivityDetails!.EmploymentDetails?.EmploymentTypeSelection?.EmploymentType ?? EmploymentType.None;
  }

  public getEmploymentTypeName(): string {
    return ProfessionalActivityHelper.employmentTypeNames[this.BuildingInspectorProfessionalActivityDetails!.EmploymentDetails!.EmploymentTypeSelection!.EmploymentType!]
  }

  public getEmployerName(): string {
    if(FieldValidations.IsNotNullOrWhitespace(this.BuildingInspectorProfessionalActivityDetails!.EmploymentDetails!.EmployerName?.FullName))
    {
      return this.BuildingInspectorProfessionalActivityDetails!.EmploymentDetails!.EmployerName!.FullName!
    }
    else{
      return "None"
    }
  }

  public getEmploymentTypeTitle(): string | null {
    if(this.BuildingInspectorProfessionalActivityDetails!.EmploymentDetails?.EmploymentTypeSelection?.EmploymentType == EmploymentType.PublicSector
      || this.BuildingInspectorProfessionalActivityDetails!.EmploymentDetails?.EmploymentTypeSelection?.EmploymentType == EmploymentType.PrivateSector)
      {
        return "Employer"
      }
      else if(this.BuildingInspectorProfessionalActivityDetails!.EmploymentDetails?.EmploymentTypeSelection?.EmploymentType == EmploymentType.Other)
      {
        return "Business name";
      }
      else
      {
        return null;
      }
  }

  public getEmploymentAddressTitle(): string | null {
    if(this.BuildingInspectorProfessionalActivityDetails!.EmploymentDetails?.EmploymentTypeSelection?.EmploymentType == EmploymentType.PublicSector
      || this.BuildingInspectorProfessionalActivityDetails!.EmploymentDetails?.EmploymentTypeSelection?.EmploymentType == EmploymentType.PrivateSector)
      {
        return "Employer address"
      }
      else if(this.BuildingInspectorProfessionalActivityDetails!.EmploymentDetails?.EmploymentTypeSelection?.EmploymentType == EmploymentType.Other)
      {
        return "Business address";
      }
      else
      {
        return null;
      }
  }

  public isUnemployed(): boolean {
    return this.BuildingInspectorProfessionalActivityDetails!.EmploymentDetails?.EmploymentTypeSelection?.EmploymentType == EmploymentType.Unemployed
  }

  public navigateTo(name: string) {
    return this.onNavigateTo.emit(name);
  }
}
