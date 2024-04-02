import { Component } from '@angular/core';

@Component({
  selector: 'hse-accessibility',
  templateUrl: './accessibility.component.html'
})
export class AccessibilityComponent {
  public static route: string = "accessibility-statement";
  static title: string = "Accessibility statement - Register as a building inspector - GOV.UK";

  constructor() { }
}
