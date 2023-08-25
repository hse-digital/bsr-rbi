import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicationSummaryProfessionalMembershipAndEmploymentDetailsComponent } from './professional-membership-and-employment-details.component';

describe('ApplicationSummaryProfessionalMembershipAndEmploymentDetailsComponent', () => {
  let component: ApplicationSummaryProfessionalMembershipAndEmploymentDetailsComponent;
  let fixture: ComponentFixture<ApplicationSummaryProfessionalMembershipAndEmploymentDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApplicationSummaryProfessionalMembershipAndEmploymentDetailsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ApplicationSummaryProfessionalMembershipAndEmploymentDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
