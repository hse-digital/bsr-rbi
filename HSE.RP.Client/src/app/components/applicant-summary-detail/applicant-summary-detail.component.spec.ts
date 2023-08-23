import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicantSummaryDetailComponent } from './applicant-summary-detail.component';

describe('ApplicantSummaryDetailComponent', () => {
  let component: ApplicantSummaryDetailComponent;
  let fixture: ComponentFixture<ApplicantSummaryDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApplicantSummaryDetailComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ApplicantSummaryDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
