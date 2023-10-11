import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
// import { BlockBlobClient } from '@azure/storage-blob';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class GovUKDataService {
  constructor(private readonly httpClient: HttpClient) {}

  async getGovUKData(): Promise<void> {
    const uri = await this.getGovUKDataSASUri();

    // try {
    //   const blockBlobClient = new BlockBlobClient(url);
    //   const result = await blockBlobClient.downloadToFile('GovUKData.csv');
    // } catch (err) {
    //   console.error(err);
    // }

    const httpOptions: Object = {
      headers: new HttpHeaders({
        Accept: 'application/text',
        'Content-Type': 'application/text; charset=utf-8',
      }),
      responseType: 'text',
    };

    this.httpClient.get<any>(uri, httpOptions).subscribe({
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

  async getGovUKDataSASUri(): Promise<string> {
    return await firstValueFrom(
      this.httpClient.get<string>(`api/GetGovUKDataSASUri`)
    );
  }
}
