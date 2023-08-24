import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { DateFormatHelper } from '../../../helpers/date-format-helper';
import { PersonalDetails } from '../../../models/personal-details.model';
import { BuildingInspectorClass } from 'src/app/models/building-inspector-class.model';
import { BuildingInspectorClassType } from 'src/app/models/building-inspector-classtype.enum';

@Component({
  selector: 'hse-building-inspector-class-details',
  templateUrl: './building-inspector-class-details.component.html',
  styleUrls: ['./building-inspector-class-details.scss']
})
export class ApplicationSummaryBuildingInspectorClassDetailsComponent implements OnInit{
  @Input() BuildingInspectorClassDetails?: BuildingInspectorClass;
  @Output() onNavigateTo = new EventEmitter<string>();

  BuildingInspectorClassType = BuildingInspectorClassType;
  //BuildingInspectorRoutes = BuildingInspectorRoutes;

  assessPlansCategories: string = "";
  assessPlansLink: string = "";

  inspectionCategories: string = "";
  inspectionLink: string = "";

  ngOnInit(): void {
    if (this.BuildingInspectorClassDetails!.ClassType.Class === BuildingInspectorClassType.Class2)
    {
      this.assessPlansLink = 'building-class2-assessing-plans-categories';
      this.inspectionLink = 'building-class2-inspect-building-categories';
      this.inspectionCategories = this.categoriesToString(this.BuildingInspectorClassDetails!.Class2InspectBuildingCategories)
      this.assessPlansCategories = this.categoriesToString(this.BuildingInspectorClassDetails!.AssessingPlansClass2)
    }

    if (this.BuildingInspectorClassDetails!.ClassType.Class === BuildingInspectorClassType.Class3)
    {
      this.assessPlansLink = 'building-class3-assessing-plans-categories';
      this.inspectionLink = 'building-class3-inspect-building-categories';

      this.inspectionCategories = this.categoriesToString(this.BuildingInspectorClassDetails!.Class3InspectBuildingCategories)
      this.assessPlansCategories = this.categoriesToString(this.BuildingInspectorClassDetails!.AssessingPlansClass3)
    }
  }

  categoriesToString(categories: any): string {
    // loop through keys in object
    // if keys value is true, add the last character of the key name to string with a comma
    // when the loop is complete, remove the trailing comma and space
    let newString = "";

    Object.keys(categories).forEach(key => {
      if (categories[key] === true) {
        newString = newString + key.slice(-1) + ", ";
      }
    });

    return newString.slice(0, -2);
  }

  public navigateTo(name: string) {
    return this.onNavigateTo.emit(name);
  }
}
