import { Component, Output, EventEmitter, Input, ViewChildren, QueryList } from '@angular/core';
import { ApplicationService } from 'src/app/services/application.service';
import { AddressService } from 'src/app/services/address.service';
import { AddressSearchMode } from './address.component';
import { GovukErrorSummaryComponent } from 'hse-angular';
import { TitleService } from 'src/app/services/title.service';
import { AddressResponseModel } from 'src/app/models/address-response.model';
import { FieldValidations } from 'src/app/helpers/validators/fieldvalidations';

@Component({
  selector: 'find-address',
  templateUrl: './find-address.component.html'
})
export class FindAddressComponent {

  @Input() searchMode: AddressSearchMode = AddressSearchMode.HomeAddress;
  @Input() searchModel!: { postcode?: string, addressLineOne?: string };
  @Input() addressName!: string;
  @Input() addressBodyText?: string;
  @Input() selfAddress = false;
  @Input() showOptionalAddressLineOne = false;
  @Input() isBusinessAddressSearch: string = "false";

  @Output() public onSearchPerformed = new EventEmitter<AddressResponseModel>();

  postcodeHasErrors: boolean = false;
  postcodeErrorText: string = '';

  loading = false;

  @ViewChildren("summaryError") summaryError?: QueryList<GovukErrorSummaryComponent>;

  constructor(public applicationService: ApplicationService, private addressService: AddressService, private titleService: TitleService) { }

  async findAddress() {
    if (this.isPostcodeValid()) {
      this.loading = true;
      let addressResponse = await this.searchAddress();

      if(FieldValidations.IsNotNullOrWhitespace(this.searchModel.addressLineOne)){
        addressResponse.Results = this.filterAddresses(addressResponse);
      }

      for (let address of addressResponse.Results) {
        address.IsManual = false;
      }
      this.onSearchPerformed.emit(addressResponse);
    } else {
      this.summaryError?.first?.focus();
      this.titleService.setTitleError();
    }
  }

  filterAddresses(addresses: AddressResponseModel) {
    return addresses.Results.filter(x => !!x.Address && x.Address?.toLowerCase().indexOf(this.searchModel.addressLineOne!.toLowerCase()) > -1);
  }

  isPostcodeValid(): boolean {

    let postcode = this.searchModel.postcode?.replace(/[^\w\s]/gi, '')?.trim()?.replace(/\s+/g, '');


    
    this.postcodeHasErrors = true;
    if (!postcode) {
      this.postcodeErrorText = 'Enter postcode';
    } else if (!FieldValidations.PostcodeValidator(postcode)) {
      this.postcodeErrorText = "Enter a full UK postcode";
    }
    else{
      this.postcodeHasErrors = false;

    }

    return !this.postcodeHasErrors;
  }

  getErrorDescription(showError: boolean, errorMessage: string): string | undefined {
    return this.postcodeHasErrors && showError ? errorMessage : undefined;
  }

  private searchAddress(): Promise<AddressResponseModel> {
    return this.addressService.SearchAllAddressByPostcode(this.searchModel.postcode!.replace(/[^\w\s]/gi, '')?.trim()?.replace(/\s+/g, ''));
  }

  warningMessage(): string {
    return "Your home address will not be published unless it is also your business address.";
  }


}
