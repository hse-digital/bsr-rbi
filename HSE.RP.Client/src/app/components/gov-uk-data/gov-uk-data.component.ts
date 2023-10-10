import { Component, OnInit } from '@angular/core';
import { GovukDataService } from './gov-uk-data.service';

@Component({
  selector: 'hse-gov-uk-data',
  template: '<h1>Gov UK Data</h1> <a routerLink="/">Home</a>',
})
export class GovukDataComponent implements OnInit {
  public static route: string = 'govukdata';

  constructor(private readonly govUkDataService: GovukDataService) {}

  async ngOnInit(): Promise<void> {
    await this.govUkDataService.getGovUkData();
  }
}
