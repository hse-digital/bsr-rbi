import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ApplicationService, ApplicationType } from 'src/app/services/application.service';
import { TitleService } from 'src/app/services/title.service';
import { environment } from 'src/environments/environment';

@Component({
  templateUrl: './application-selector.component.html'
})
export class ApplicationSelectorComponent {

  constructor(private applicationService: ApplicationService,private router: Router, private titleService: TitleService) { }
  static route: string =  "select";
  static title: string = "What does this work involve? - Apply for building control approval for a higher-risk building - GOV.UK";

  selection?: string;
  continueLink?: string ='application/application-name';
  showError: boolean = false;
  production: boolean = environment.production;

  continue() {
    this.showError = !this.selection;
    if (!this.showError) {
      this.applicationService.newApplication();
      if (this.applicationService.model)
        this.applicationService.model.ApplicationType = this.selection == "0" ? ApplicationType.NewHRB : this.selection == "1" ? ApplicationType.ExistingHRB : ApplicationType.ExisitngBuildingToHRB

      this.applicationService.updateApplication();
      this.router.navigate([this.continueLink]);
    } else {
      this.titleService.setTitleError();
    }
  }
}
