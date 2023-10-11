import { Component, OnInit } from '@angular/core';
import { GovUKDataService } from './gov-uk-data.service';

@Component({
  selector: 'hse-gov-uk-data',
  template: '<h1>Gov UK Data</h1> <a routerLink="/">Home</a>',
})
export class GovUKDataComponent implements OnInit {
  public static route: string = 'govukdata';

  constructor(private readonly govUKDataService: GovUKDataService) {}

  async ngOnInit(): Promise<void> {
    await this.govUKDataService.getGovUKData();
  }
}
