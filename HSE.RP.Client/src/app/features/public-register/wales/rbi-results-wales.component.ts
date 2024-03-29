import { ActivatedRoute, ActivatedRouteSnapshot } from "@angular/router";
import { PageComponent } from "../../../helpers/page.component";
import { ApplicationService } from "../../../services/application.service";
import { FieldValidations } from "../../../helpers/validators/fieldvalidations";
import { Component, OnInit } from "@angular/core";
import { RBISearchResponse, SearchService } from "../../../services/search.service";
import { NavigationService } from "../../../services/navigation.service";

@Component({
  selector: 'hse-rbi-results-wales',
  templateUrl: './rbi-results-wales.component.html'
})
export class RBIResultsWalesComponent implements OnInit {


  static route: string = 'results';
  static title: string = 'Register of building inspectors - Results';
  searchModel: { "inspector-name": string, "building-control-body": string, "class-filter" : string[] } = { "inspector-name": '', "building-control-body": '', "class-filter": []};
  searchResponse!: RBISearchResponse;


  constructor(private searchService: SearchService,
    protected activatedRoute: ActivatedRoute,
    protected navigationService: NavigationService) {
  }

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(async params => {
      this.searchModel['inspector-name'] = params['inspector-name']
      this.searchModel['building-control-body'] = params['building-control-body']
      if (params['class-filter'] == null) {
        this.searchModel["class-filter"] = [];
      }
      else {
        this.searchModel["class-filter"]= JSON.parse(params['class-filter'])
      }
    });
  }
}
