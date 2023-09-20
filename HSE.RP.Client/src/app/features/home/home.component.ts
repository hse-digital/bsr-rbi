import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  templateUrl: './home.component.html',
  styles: ['a:link.info { color: black; }',
           'a:visited.info { color: black; }',
           'a:hover.info { text-decoration-thickness: max(3px, .1875rem, .12em); -webkit-text-decoration-skip-ink: none; text-decoration-skip-ink: none; -webkit-text-decoration-skip: none; }'],
})
export class HomeComponent {
  static route: string = environment.production ? "home" : "";
  static title: string = "Register as a building inspector";

  constructor() { }

}
