import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { PageComponent } from '../../../helpers/page.component';
import { ApplicationService } from '../../../services/application.service';

@Component({
  selector: 'hse-application-info',
  templateUrl: './application-info.component.html',
})
export class ApplicationInfoComponent {
  public static route: string = "application-info";
  static title: string = "Your application - Apply for building control approval for a higher-risk building - GOV.UK";
  production: boolean = environment.production;
  constructor(private router: Router) {
  }

  continue() {
    this.router.navigate(['application/applicant-name'])
  }
}
