import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { DateFormatHelper } from '../../helpers/date-format-helper';
import { PersonalDetails } from '../../models/personal-details.model';

@Component({
  selector: 'hse-applicant-summary-detail',
  templateUrl: './applicant-summary-detail.component.html',
  styleUrls: ['./applicant-summary-detail.component.scss']
})
export class ApplicantSummaryDetailComponent implements OnInit{
  @Input() PersonalDetails?: PersonalDetails;
  @Output() onNavigateTo = new EventEmitter<string>();

  ngOnInit(): void {
  }

  public GetFormattedDateofBirth(): string {
    return DateFormatHelper.LongMonthFormat(
      this.PersonalDetails?.ApplicantDateOfBirth?.Year,
      this.PersonalDetails?.ApplicantDateOfBirth?.Month,
      this.PersonalDetails?.ApplicantDateOfBirth?.Day
    );
  }

  public getAlternativePhone(): string {
    return this.PersonalDetails?.ApplicantAlternativePhone?.PhoneNumber || 'none';
  }

  public getAlternativeEmail(): string {
    return this.PersonalDetails?.ApplicantAlternativeEmail?.Email || 'none';
  }

  public navigateTo(name: string) {
    return this.onNavigateTo.emit(name);
  }
}
