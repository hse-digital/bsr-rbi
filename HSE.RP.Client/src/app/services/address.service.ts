import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { IComponentModel } from '../models/component. interface';
import { ComponentCompletionState } from '../models/component-completion-state.enum';
import { AddressModel } from '../models/address.model';
import { AddressResponseModel } from '../models/address-response.model';

@Injectable({
  providedIn: 'root',
})
export class AddressService {
  constructor(private httpClient: HttpClient) {}

  async SearchBuildingByPostcode(
    postcode: string
  ): Promise<AddressResponseModel> {
    return await firstValueFrom(
      this.httpClient.get<AddressResponseModel>(
        `api/SearchBuildingByPostcode/${postcode}?includeAllUKAddresses=1`
      )
    );
  }

  async SearchPostalAddressByPostcode(
    postcode: string
  ): Promise<AddressResponseModel> {
    return await firstValueFrom(
      this.httpClient.get<AddressResponseModel>(
        `api/SearchPostalAddressByPostcode/${postcode}?includeAllUKAddresses=1`
      )
    );
  }

  async SearchAddress(query: string): Promise<AddressResponseModel> {
    return await firstValueFrom(
      this.httpClient.get<AddressResponseModel>(`api/SearchAddress/${query}?includeAllUKAddresses=1`)
    );
  }

  async SearchAllAddressByPostcode(query: string): Promise<AddressResponseModel> {
    return await firstValueFrom(
      this.httpClient.get<AddressResponseModel>(`api/SearchAllAddressByPostcode/${query}?includeAllUKAddresses=1`)
    );
  }
}
