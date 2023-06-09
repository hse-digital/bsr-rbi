import { Component } from "@angular/core";

@Component({
  templateUrl: './not-found.component.html'
})
export class NotFoundComponent {
  public static route: string = "not-found";
  static title: string = "Not found - GOV.UK";

  returnToApplicationLink = "/";
}