import { Component } from '@angular/core';

@Component({
  templateUrl: './home.component.html',
  styles: ['a:link.info { color: black; }',
           'a:visited.info { color: black; }',
           'a:hover.info { text-decoration-thickness: max(3px, .1875rem, .12em); -webkit-text-decoration-skip-ink: none; text-decoration-skip-ink: none; -webkit-text-decoration-skip: none; }'],
})
export class HomeComponent {
  public static route: string = "";
  static title: string = "Register as a building inspector";

  constructor() { }

}
