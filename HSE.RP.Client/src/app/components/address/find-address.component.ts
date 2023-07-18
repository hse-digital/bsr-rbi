import { Component, Output, EventEmitter, Input, ViewChildren, QueryList } from '@angular/core';
import { ApplicationService } from 'src/app/services/application.service';
import { AddressResponseModel, AddressService } from 'src/app/services/address.service';
import { AddressSearchMode } from './address.component';
import { GovukErrorSummaryComponent } from 'hse-angular';
import { TitleService } from 'src/app/services/title.service';

@Component({
  selector: 'find-address',
  templateUrl: './find-address.component.html'
})
export class FindAddressComponent {

  @Input() searchMode: AddressSearchMode = AddressSearchMode.HomeAddress;
  @Input() searchModel!: { postcode?: string };
  @Input() addressName!: string;
  @Input() selfAddress = false;
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

      this.onSearchPerformed.emit(addressResponse);
    } else {
      this.summaryError?.first?.focus();
      this.titleService.setTitleError();
    }
  }

  isPostcodeValid(): boolean {
    let postcode = this.searchModel.postcode?.replace(' ', '');
    this.postcodeHasErrors = true;
    if (!postcode) {
      this.postcodeErrorText = 'Enter a postcode';
    } else if (postcode.length < 5 || postcode.length > 7) {
      this.postcodeErrorText = "Enter a real postcode, like 'EC3A 8BF'.";
    } else {
      this.postcodeHasErrors = false;
    }

    return !this.postcodeHasErrors;
  }

  getErrorDescription(showError: boolean, errorMessage: string): string | undefined {
    return this.postcodeHasErrors && showError ? errorMessage : undefined;
  }

  private searchAddress(): Promise<AddressResponseModel> {
    return this.addressService.SearchPostalAddressByPostcode(this.searchModel.postcode!);
  }

  warningMessage(): string {
    return "Your home address will not be published";
  }

 
}
