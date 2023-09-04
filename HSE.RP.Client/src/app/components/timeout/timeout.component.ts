import { Component } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';
import { PageComponent } from 'src/app/helpers/page.component';
import { ApplicationService } from 'src/app/services/application.service';

@Component({
  templateUrl: './timeout.component.html'
})
export class TimeoutComponent  extends PageComponent<string>  {

  static route: string = 'timeout';
  static title: string = "Page timeout - Register a high-rise building - GOV.UK";

  override onInit(applicationService: ApplicationService): void {

  }
  override onSave(applicationService: ApplicationService): Promise<void> {
    throw new Error('Method not implemented.');
  }
  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return true;
  }
  override isValid(): boolean {
    return true;
  }
  override navigateNext(): Promise<boolean> {
    throw new Error('Method not implemented.');
  }

}
