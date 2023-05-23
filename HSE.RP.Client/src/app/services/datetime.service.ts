import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class DateTimeService {

  constructor(private httpClinet: HttpClient) { }

  async GetTimeDate(): Promise<string> {
    return await firstValueFrom(this.httpClinet.get<string>('api/DisplayDateTime'));
  }
}
