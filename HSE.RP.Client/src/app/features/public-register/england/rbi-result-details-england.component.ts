import { ActivatedRoute, ActivatedRouteSnapshot } from "@angular/router";
import { PageComponent } from "../../../helpers/page.component";
import { ApplicationService } from "../../../services/application.service";
import { FieldValidations } from "../../../helpers/validators/fieldvalidations";
import { Component, EventEmitter, Output } from "@angular/core";
import { SearchService } from "../../../services/search.service";
import { NavigationService } from "../../../services/navigation.service";

@Component({
  selector: 'hse-rbi-result-details-england',
  templateUrl: './rbi-result-details-england.component.html'
})
export class RBIResultDetailsEnglandComponent  {

  static route: string = 'detail';
  static title: string = 'Register of building inspectors - Details';
  searchModel: { "inspector-name"?: string, "building-control-body": string, "class-filter": string[] } = { "inspector-name": '', "building-control-body": '', "class-filter": []};

  Id: string | undefined;
  
  constructor(private searchService: SearchService,
    protected activatedRoute: ActivatedRoute,
    protected navigationService: NavigationService) {
  }

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(async params => {
      this.Id= params['Id']
      if (params['searchModel'] == null) {
        this.searchModel = { "inspector-name": '', "building-control-body": '', "class-filter": []};
      }
      else {
        this.searchModel = JSON.parse(params['searchModel'])
      }

    });

  }
  
  
}
