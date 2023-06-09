import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { PageComponent } from '../../../helpers/page.component';
import { FieldValidations } from '../../../helpers/validators/fieldvalidations';
import { ApplicationService, BuildingControlModel } from '../../../services/application.service';

@Component({
  templateUrl: './application-name.component.html'
})
export class ApplicationNameComponent extends PageComponent<string> {

  public static route: string = "application-name";
  static title: string = "Choose a name for your application - Apply for building control approval for a higher-risk building - GOV.UK";
  production: boolean = environment.production;
  modelValid: boolean = false;
  override model?: string;

  constructor(activatedRoute: ActivatedRoute, applicationService: ApplicationService) {
    super(activatedRoute);
    this.updateOnSave = false;
  }


  override onInit(applicationService: ApplicationService): void {
   this.model = applicationService.model.ApplicationName;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    applicationService.model.ApplicationName = this.model;
  }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    let AppType = applicationService.model.ApplicationType;
    
    return FieldValidations.IsNotNullOrWhitespace(AppType?.toString())
  }

  override isValid(): boolean {
    this.modelValid = FieldValidations.IsNotNullOrWhitespace(this.model)
    return this.modelValid;
  }

  override navigateNext(): Promise<boolean> {
    return this.navigationService.navigate('application/application-info');
  }
}
