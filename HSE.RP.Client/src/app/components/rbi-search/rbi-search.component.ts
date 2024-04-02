import { Component, EventEmitter, Injector, Input, OnInit, Output } from '@angular/core';
import { NavigationService } from '../../services/navigation.service';
import { SearchService } from '../../services/search.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'hse-rbi-search',
  templateUrl: './rbi-search.component.html',
  styleUrls: ['./rbi-search.component.scss']
})
export class RBISearchComponent implements OnInit {
  @Input() country!: string;

  @Input() searchModel: { "inspector-name"?: string, "building-control-body"?: string } = { "inspector-name": '', "building-control-body": '' };

  searchModelHasErrors: boolean = false;
  searchModelBuildingInspectorErrorText: string = '';
  searchModelBuildingControllerErrorText: string = '';
  lastUpdated: string = '';

  searching = false;

  constructor(private searchService: SearchService,
    protected activatedRoute: ActivatedRoute,
    protected navigationService: NavigationService) {

  }

  async ngOnInit(): Promise<void> {
    //this.lastUpdated = await this.searchService.GetRegisterLastUpdated('BuildingInspector', this.country);
  }

  isSearchModelValid(): boolean {

    let name = this.searchModel['inspector-name'];
    let employer = this.searchModel['building-control-body'];

    this.searchModelHasErrors = false;

    if (!name && !employer) {
      this.searchModelBuildingInspectorErrorText = 'Enter an inspector name';
      this.searchModelBuildingControllerErrorText = 'Enter a building control body';
      this.searchModelHasErrors = true;
    }

    return !this.searchModelHasErrors;
  }

  getErrorDescription(showError: boolean, errorMessage: string): string | undefined {
    return this.searchModelHasErrors && showError ? errorMessage : undefined;
  }

  async searchBuildingInspectors() {
    this.searching = true;
    if (this.isSearchModelValid()) {
      this.startSearch();
    }
    else{
      this.searching = false;
    }
  }

  async startSearch() {
    this.navigationService.navigateAppend('results', this.activatedRoute,
      {
        "inspector-name": this.searchModel['inspector-name'],
        "building-control-body": this.searchModel['building-control-body'],
        "class-filter": []      
      });
      this.searching = false;
  }

  companies: string[] = [];
  async searchCompanies(company: string) {
    if (company?.length > 2) {
      var response = await this.searchService.SearchRBICompanyNames(
        company,
        this.country
      );
      this.companies = response.Companies;
    }
  }

  selectCompanyName(company: string) {
    this.searchModel['building-control-body'] = company;
  }


  inspectors: string[] = [];

  async searchInspectors(name: string) {
    if (name?.length > 2) {
      var response = await this.searchService.SearchRBIInspectorNames(
        name,
        this.country
      );
      this.inspectors = response.Inspectors;
    }
  }

  selectInspectorName(name: string) {
    this.searchModel['inspector-name'] = name;
  }


}
