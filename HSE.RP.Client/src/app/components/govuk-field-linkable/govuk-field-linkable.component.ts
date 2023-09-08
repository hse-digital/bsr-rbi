import { Component, Input } from '@angular/core';

@Component({
  selector: 'govuk-field-linkable',
  templateUrl: './govuk-field-linkable.component.html'
})
export class GovukFieldLinkableComponent {
  @Input() public id?: string;
  @Input() public class?: string;

  @Input() public label!: string;
  @Input() public labelClass?: string;
  @Input() public labelFor?: string;
  @Input() public isPageHeading?: boolean = false;

  @Input() public hint?: string;
  @Input() public hintClass?: string;

  @Input() public errorText?: string;
  @Input() public errorClass?: string;

  @Input() linkText?: string;
  @Input() linkUri?: string;

}
