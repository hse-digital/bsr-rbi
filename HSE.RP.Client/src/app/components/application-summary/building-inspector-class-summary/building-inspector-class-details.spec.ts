import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicationSummaryPersonalDetailsComponent } from './personal-details.component';

describe('ApplicationSummaryDetailComponent', () => {
  let component: ApplicationSummaryPersonalDetailsComponent;
  let fixture: ComponentFixture<ApplicationSummaryPersonalDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApplicationSummaryPersonalDetailsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ApplicationSummaryPersonalDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
