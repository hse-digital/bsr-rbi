import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { DateTimeService } from './services/datetime.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  providers: [DateTimeService]
})
export class AppComponent {
  title = 'HSE Regulating Professions Client';
  dateTime = '';

  constructor(private dateTimeService: DateTimeService ) {   
    this.initDateTime();
  }

  async initDateTime() {
    this.dateTime = await this.dateTimeService.GetTimeDate();

  }
}
