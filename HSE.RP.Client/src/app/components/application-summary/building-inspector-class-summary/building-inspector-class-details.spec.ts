import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicationSummaryBuildingInspectorClassDetailsComponent } from './building-inspector-class-details.component';

describe('ApplicationSummaryBuildingInspectorClassDetailsComponent', () => {
  let component: ApplicationSummaryBuildingInspectorClassDetailsComponent;
  let fixture: ComponentFixture<ApplicationSummaryBuildingInspectorClassDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApplicationSummaryBuildingInspectorClassDetailsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ApplicationSummaryBuildingInspectorClassDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
