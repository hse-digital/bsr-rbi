import { Component, EventEmitter, Input, Output, QueryList, ViewChildren } from '@angular/core';
import { ApplicationService } from 'src/app/services/application.service';
import { AddressSearchMode } from './address.component';
import { GovukErrorSummaryComponent } from 'hse-angular';
import { TitleService } from 'src/app/services/title.service';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { AddressModel } from 'src/app/models/address.model';
import { FieldValidations } from 'src/app/helpers/validators/fieldvalidations';

@Component({
  selector: 'manual-address',
  templateUrl: './manual-address.component.html'
})
export class ManualAddressComponent {

  @Input() searchMode: AddressSearchMode = AddressSearchMode.HomeAddress;
  @Output() onSearchAgain = new EventEmitter();
  @Output() onAddressEntered = new EventEmitter<AddressModel>();
  @Input() addressName?: string;
  @Input() selfAddress = false;

  postcodeHasErrors = false;
  postcodeErrorMessage?: string;
  hasErrors = false;
  errorMessage?: string;

  model: AddressModel = { IsManual: true, CompletionState: ComponentCompletionState.Complete }

  @ViewChildren("summaryError") summaryError?: QueryList<GovukErrorSummaryComponent>;

  constructor(public applicationService: ApplicationService, private titleService: TitleService) { }

  confirmAddress() {
    if (this.isModelValid()) {
      this.model.IsManual = true;
      this.onAddressEntered.emit(this.model);
    } else {
      this.summaryError?.first?.focus();
      this.titleService.setTitleError();
    }
  }

  addressOneIsValid?: boolean;
  townOrCityIsValid?: boolean;
  postcodeIsValid?: boolean;

  private isModelValid() {
    this.addressOneIsValid = this.isAddressOneValid();
    this.townOrCityIsValid = this.isTownOrCityValid();
    this.postcodeIsValid = this.isPostcodeValid();
    
    this.hasErrors = !this.addressOneIsValid || !this.townOrCityIsValid || !this.postcodeIsValid;

    return !this.hasErrors;
  }

  private isAddressOneValid() {
    return FieldValidations.IsNotNullOrWhitespace(this.model.Address);
  }

  private isTownOrCityValid() {
    return FieldValidations.IsNotNullOrWhitespace(this.model.Town);
  }

  private isPostcodeValid(): boolean {
    let postcode = this.model.Postcode?.replace(' ', '');
    let error = true;
    if (!postcode) {
      this.postcodeErrorMessage = 'Enter a postcode';
    } else if (postcode.length < 5 || postcode.length > 7) {
      this.postcodeErrorMessage = "Enter a real postcode, like 'EC3A 8BF'.";
    } else {
      error = false;
    }
    this.postcodeHasErrors = error;
    return !error;
  }

  get anchorId() {
    if(!this.addressOneIsValid) {
      return "input-address-line-one";
    } else if (!this.townOrCityIsValid) {
      return "input-town-or-city";
    } else if (!FieldValidations.IsNotNullOrWhitespace(this.model.Postcode)) {
      return "input-postcode";
    }
    return "";
  }

  getErrorDescription(showError: boolean, errorMessage: string): string | undefined {
    return this.hasErrors && showError ? errorMessage : undefined;
  }

  addressTypeDescription() {
    return 'This address must be in England or Wales.';
  }

  getTitle() {
    return 'Enter home address manually';
  }

  warningMessage(): string {
    return "Your home address will not be published unless it is also your business address.";
  }

}
