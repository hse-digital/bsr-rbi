import { Component, EventEmitter, Injector, Input, OnInit, Output, } from '@angular/core';
import { NavigationService } from '../../services/navigation.service';
import { RBISearchResponse, SearchService } from '../../services/search.service';
import { ActivatedRoute } from '@angular/router';
import { Activity, BuildingProfessionalModel } from '../../models/building-profession-application.model';

@Component({
  selector: 'hse-rbi-details',
  templateUrl: './rbi-details.component.html',
  styleUrls: ['./rbi-details.component.scss']
})
export class RBIDetailsComponent implements OnInit {

  @Input() Id?: string = '';
  @Input() searchModel: { "inspector-name"?: string, "building-control-body"?: string, "class-filter"?: string[] } = { "inspector-name": '', "building-control-body": '', "class-filter": [] };

  rbiApplication!: BuildingProfessionalModel | undefined;

  loading = true;


  constructor(private searchService: SearchService,
    protected activatedRoute: ActivatedRoute,
    protected navigationService: NavigationService) {

  }

  async ngOnInit(): Promise<void> {
    this.activatedRoute.queryParams.subscribe(async params => {
      this.Id = params['Id']
    });

    if (this.Id) {
      this.rbiApplication = await this.searchService.GetRBIDetails(this.Id);
    }
    else {
      this.rbiApplication = undefined;
    }
    this.loading = false;
  }

  //check if rbiapplcation contains a specific activity name
  hasActivity(activity: string): boolean {
    return this.rbiApplication?.Activities?.some(act => act.ActivityName === activity) ?? false;
  }

  getActivity(activity: string): Activity | undefined {

    //Return activity by name
    return this.rbiApplication?.Activities?.find(act => act.ActivityName === activity);

  }

  //stringify the class filter
  stringifyClassFilter(): string {
    return JSON.stringify(this.searchModel["class-filter"]);
  }

  mapClassDisplayName(classString: string): string {

    //Switch expression to map class string to display name
    switch (classString) {
      case "Class 1 trainee building inspector":
        return "Class 1 - must work under the supervision of a registered building inspector with the appropriate scope of registration to supervise the allocated tasks";
      case "Class 2 building inspector":
        return "Class 2 - Building Inspector";
      case "Class 3 specialist building inspector":
        return "Class 3  - Specialist Building Inspector";
      case "Class 4 technical manager":
        return "Class 4 - Technical Manager";
      default:
        return "";
    }
  }
}
