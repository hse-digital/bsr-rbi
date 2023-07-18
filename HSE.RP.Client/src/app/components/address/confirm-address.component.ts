import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AddressModel } from 'src/app/services/address.service';
import { ApplicationService } from 'src/app/services/application.service';
import { NavigationService } from 'src/app/services/navigation.service';

@Component({
  selector: 'confirm-address',
  templateUrl: './confirm-address.component.html'
})
export class ConfirmAddressComponent {

  @Input() address!: AddressModel;
  @Input() addressName?: string;
  @Input() selfAddress = false;
  @Output() onAddressConfirmed = new EventEmitter<boolean | undefined>();
  @Output() onSearchAgain = new EventEmitter();
  @Output() onEnterManualAddress = new EventEmitter();

  constructor(public applicationService: ApplicationService, public navigationService: NavigationService) {
  }

  confirm(): void {
    this.onAddressConfirmed.emit();
  }

  getTitle(): string {
    return "Confirm your home address";
  }
}