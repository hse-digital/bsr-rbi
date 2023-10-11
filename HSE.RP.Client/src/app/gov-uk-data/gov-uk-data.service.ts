import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';

@Injectable()
export class GovUKDataService {
  private readonly _url: string = '';

  constructor(private readonly httpClient: HttpClient) {}

  async getGovUKData(): Promise<void> {
    const uri = await this.getGovUKDataUri();

    const httpOptions: Object = {
      headers: new HttpHeaders({
        Accept: 'application/text',
        'Content-Type': 'application/text; charset=utf-8',
      }),
      responseType: 'text',
    };

    this.httpClient.get<any>(this._url, httpOptions).subscribe({
      next: (data: any) => {
        const blob = new Blob([data], { type: 'application/text' }); // application/csv

        var downloadURL = window.URL.createObjectURL(blob);
        var link = document.createElement('a');
        link.href = downloadURL;
        link.download = 'GovUKData.csv';
        link.click();
        window.history.back();
      },
      error: (err: any) => {
        console.error(err);
      },
    });
  }

  async getGovUKDataUri(): Promise<string> {
    return await firstValueFrom(this.httpClient.get<string>(`api/GetGovUKDataUri`));
  }
}
