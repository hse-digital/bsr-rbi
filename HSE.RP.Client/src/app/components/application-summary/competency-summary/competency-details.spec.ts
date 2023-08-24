import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicationSummaryCompetencyDetailsComponent } from './competency.component';

describe('ApplicationSummaryCompetencyDetailsComponent', () => {
  let component: ApplicationSummaryCompetencyDetailsComponent;
  let fixture: ComponentFixture<ApplicationSummaryCompetencyDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApplicationSummaryCompetencyDetailsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ApplicationSummaryCompetencyDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
