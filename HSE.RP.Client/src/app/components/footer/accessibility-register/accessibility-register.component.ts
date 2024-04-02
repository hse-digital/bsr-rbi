import { Component } from '@angular/core';

@Component({
  selector: 'hse-accessibility',
  templateUrl: './accessibility-register.component.html'
})
export class AccessibilityRegisterComponent {
  public static route: string = "accessibility-statement-register";
  static title: string = "Accessibility statement - Find a registered building inspector - GOV.UK";

  constructor() { }
}
