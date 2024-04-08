import { Component, EventEmitter, Injector, Input, OnInit, Output, ViewEncapsulation, } from '@angular/core';
import { NavigationService } from '../../services/navigation.service';
import { RBISearchResponse, SearchService } from '../../services/search.service';
import { ActivatedRoute } from '@angular/router';
import { BuildingProfessionalModel } from '../../models/building-profession-application.model';
import { isEqual } from 'lodash';
import { TitleService } from '../../services/title.service';

@Component({
  selector: 'hse-rbi-results',
  templateUrl: './rbi-results.component.html',
  styleUrls: ['./rbi-results.component.scss'],
  encapsulation: ViewEncapsulation.None 
})
export class RBIResultsComponent implements OnInit {
  @Input() country!: string;
  @Input() searchModel: { "inspector-name": string, "building-control-body": string, "class-filter": string[] } = { "inspector-name": '', "building-control-body": '', "class-filter": [] };
  //event emitter for the search button

  classOneDisabled: boolean = false;
  classTwoDisabled: boolean = false;
  classThreeDisabled: boolean = false;
  classFourDisabled: boolean = false;

  searchResponse!: RBISearchResponse;
  displayResults!: BuildingProfessionalModel[];
  searchResultPageItems: BuildingProfessionalModel[] = [];

  searching = true;

  searchModelHasErrors: boolean = false;
  searchModelBuildingInspectorErrorText: string = '';
  searchModelBuildingControllerErrorText: string = '';

  pageSize = 20;
  currentPage = 1;
  page = 1;
  skip = 0;
  pages: number[] = [];

  constructor(private searchService: SearchService,
    protected activatedRoute: ActivatedRoute,
    protected navigationService: NavigationService) {

  }

  async ngOnInit(): Promise<void> {
    await this.startSearch();

  }

  async setClassFilterValidation() {

    if(this.searchModel["class-filter"]!.length == 0)
    {
      this.classOneDisabled = false;
      this.classTwoDisabled = false;
      this.classThreeDisabled = false;
      this.classFourDisabled = true;
    }
    if (this.searchModel["class-filter"]!.includes("Class 1 trainee building inspector")) {
      this.classOneDisabled = false;
      this.classTwoDisabled = true;
      this.classThreeDisabled = true;
      this.classFourDisabled = true;
    } else if (this.searchModel["class-filter"]!.includes("Class 2 building inspector")) {
      this.classOneDisabled = true;
      this.classTwoDisabled = false;
      this.classThreeDisabled = true;
      this.classFourDisabled = false;
    } else if (this.searchModel["class-filter"]!.includes("Class 3 specialist building inspector")) {
      this.classOneDisabled = true;
      this.classTwoDisabled = true;
      this.classThreeDisabled = false;
      this.classFourDisabled = false;
    } else if (this.searchModel["class-filter"]!.includes("Class 4 technical manager")) {
      this.classOneDisabled = true;
      this.classTwoDisabled = false;
      this.classThreeDisabled = false;
      this.classFourDisabled = false;
    } else {
      this.classOneDisabled = false;
      this.classTwoDisabled = false;
      this.classThreeDisabled = false;
      this.classFourDisabled = true;
    }

  }

  async filterByClass() {
    await this.setClassFilterValidation();
    if (this.searchModel["class-filter"]!.length > 0) {
      if (this.searchModel["class-filter"]!.length == 2) {
        this.displayResults = this.searchResponse.RBIApplications.filter(application => isEqual(application.classes, this.searchModel["class-filter"]));
      } else {
        this.displayResults = this.searchResponse.RBIApplications.filter(application => {
          return application.classes!.some(cls => this.searchModel["class-filter"]!.includes(cls));
        });
      }
    }
    else {
      this.displayResults = this.searchResponse.RBIApplications;
    }

    this.getApplicationPage();

    this.page = 1;
    this.currentPage = 1;

  }

  async startSearch() {
    this.searchResponse = await this.searchService.SearchRBIRegister(
      this.searchModel['inspector-name']!,
      this.searchModel['building-control-body']!,
      this.country
    );

    this.filterByClass();
    this.searching = false;

  }

  isSearchModelValid(): boolean {

    this.searchModelHasErrors = false;
    this.searchModelBuildingControllerErrorText = '';
    this.searchModelBuildingInspectorErrorText = '';

    let name = this.searchModel['inspector-name'];
    let employer = this.searchModel['building-control-body'];


    if (!name && !employer) {
      this.searchModelBuildingInspectorErrorText = 'Enter a building inspector name';
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

    if (!this.isSearchModelValid()) {
      this.searching = false;
      return;
    }

    this.startSearch();
    this.page = 1;
    this.currentPage = 1;

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

  range(start: number, end: number): number[] {
    return [...Array(end).keys()].map((el) => el + start);
  }

  stringifySearchModel(): string {
    return JSON.stringify(this.searchModel);
  }

  async pageNext() {
    const pagesCount = Math.ceil(this.searchResponse.Results / this.pageSize);
    const next = this.currentPage + 1;

    if (next <= pagesCount) {
      this.page = this.page + 1;
      await this.getApplicationPage(this.currentPage);
      this.currentPage = this.page;
      if (!this.pages.includes(next)) {
        this.pages.push(next);
      }
    }


  }



  async pagePrev() {
    if (this.currentPage === 1) return;
    this.page = this.page - 1;
    await this.getApplicationPage(this.currentPage - 2);
    this.currentPage = this.page;
  }

  async pageChange(page: number) {
    if (page === this.currentPage) return;
    this.page = page;
    await this.getApplicationPage(this.page - 1);
    this.currentPage = this.page

  }

  getApplicationPage(skip: number = 0, take: number = this.pageSize,) {
    const totalPages = Math.ceil(this.displayResults.length / take);
    this.pages = this.range(1, totalPages);
    this.searchResultPageItems = this.displayResults.slice(skip * take, (skip * take) + take);

  }

  getPages() {
    let pages = [this.currentPage];
    let i = 1;
    while (pages.length < 5) {
      if (this.currentPage - i > 0) {
        pages.unshift(this.currentPage - i);
      }
      if (this.currentPage + i <= this.pages.length) {
        pages.push(this.currentPage + i);
      }
      i++;
    }
    if (pages[0] !== 1) {
      pages.unshift(1);
    }
    if (pages[pages.length - 1] !== this.pages.length) {
      pages.push(this.pages.length);
    }
    return pages;
  }


  isActivePage(page: number): boolean {
    return this.currentPage === page;
  }

  isBoundaryPage(page: number): boolean {
    return page === 1 || page === this.pages.length;
  }

  isAdjacentPage(page: number): boolean {
    return page === this.currentPage + 1 || page === this.currentPage - 1;
  }

  isEarlyPage(page: number): boolean {
    return page <= 4 && this.currentPage <= 4;
  }

  isLatePage(page: number): boolean {
    return page >= this.pages.length - 3 && this.currentPage >= this.pages.length - 3;
  }


  getPageLabel(page: number): string | number {
    if (this.isActivePage(page) || this.isBoundaryPage(page) || this.isAdjacentPage(page) || this.isEarlyPage(page) || this.isLatePage(page)) {
      return page;
    } else {
      return '⋯';
    }
  }

}
