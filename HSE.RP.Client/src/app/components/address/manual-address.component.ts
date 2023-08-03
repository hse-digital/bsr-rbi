import { Component, EventEmitter, Input, Output, QueryList, ViewChildren } from '@angular/core';
import { ApplicationService } from 'src/app/services/application.service';
import { AddressSearchMode } from './address.component';
import { GovukErrorSummaryComponent } from 'hse-angular';
import { TitleService } from 'src/app/services/title.service';
import { ComponentCompletionState } from 'src/app/models/component-completion-state.enum';
import { AddressModel } from 'src/app/models/address.model';

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

  hasErrors = false;
  errors = {
    lineOneHasErrors: false,
    townOrCityHasErrors: false,
    postcode: { hasErrors: false, errorText: '' },
  }

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

  private isModelValid() {
    this.errors.lineOneHasErrors = !this.model.Address;
    this.errors.townOrCityHasErrors = !this.model.Town;
    this.isPostcodeValid();

    this.hasErrors = this.errors.lineOneHasErrors || this.errors.townOrCityHasErrors || this.errors.postcode.hasErrors || this.errors.postcode.hasErrors;

    return !this.hasErrors;
  }

  private isPostcodeValid(): boolean {
    let postcode = this.model.Postcode?.replace(' ', '');
    this.errors.postcode.hasErrors = true;
    if (!postcode) {
      this.errors.postcode.errorText = 'Enter a postcode';
    } else if (postcode.length < 5 || postcode.length > 7) {
      this.errors.postcode.errorText = "Enter a real postcode, like 'EC3A 8BF'.";
    } else {
      this.errors.postcode.hasErrors = false;
    }

    return !this.errors.postcode.hasErrors;
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
