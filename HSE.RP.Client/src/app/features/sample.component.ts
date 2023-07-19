import { ActivatedRouteSnapshot } from "@angular/router";
import { PageComponent } from "../helpers/page.component";
import { ApplicationService } from "../services/application.service";
import { FieldValidations } from "../helpers/validators/fieldvalidations";
import { Component } from "@angular/core";

@Component({
  templateUrl: './sample.component.html'
})
export class SampleComponent extends PageComponent<string> {
  DerivedIsComplete(value: boolean): void {

  }
  static route: string = 'sample';
  static title: string = 'page title';

  override onInit(applicationService: ApplicationService): void {
    // use this to reinitialize your page model based on your application data
    // for example

   // this.model = applicationService.model.property1;
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    // this is called when the page is sucessfully validate and before data is sent to the API
    // use this to set your application model data based on this page model
    // for example

    //applicationService.model.property1 = this.model;
  }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    // this tells routing if this page can be accessed or not, works as CanActivate
    return false;
  }

  modelValid: boolean = false;
  override isValid(): boolean {
    // this validates your model, add any logic required here
    // for example
    this.modelValid = FieldValidations.IsNotNullOrWhitespace(this.model);
    return this.modelValid;
  }

  override navigateNext(): Promise<boolean> {
    // this is used to navigate the user to the next page on the flow
    // use the built in navigation service to send the user to the proper route
    // for example

    return this.navigationService.navigate('');

    // alternatively if no next page is required just return
    // return Promise.resolve(true);
  }
}
