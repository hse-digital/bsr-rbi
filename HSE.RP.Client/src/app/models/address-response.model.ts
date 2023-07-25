import { AddressModel } from "./address.model";

export class AddressResponseModel {
    Offset!: number;
    MaxResults!: number;
    TotalResults!: number;
    Results: AddressModel[] = [];
  }