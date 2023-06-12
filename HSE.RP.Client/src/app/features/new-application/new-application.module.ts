import { CommonModule } from "@angular/common";
import { HttpClient, HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { HseAngularModule } from "hse-angular";
import { ApplicationService } from "src/app/services/application.service";
import { HseRoute, HseRoutes } from "src/app/services/hse.route";
import { RegistrationEmailComponent } from "./registration-email/registration-email.component";

const routes = new HseRoutes([
  HseRoute.protected(RegistrationEmailComponent.route, RegistrationEmailComponent, RegistrationEmailComponent.title),
]);

@NgModule({
  declarations: [
    RegistrationEmailComponent
  ],
  imports: [
    RouterModule.forChild(routes.getRoutes()),
    HseAngularModule,
    CommonModule,
    HttpClientModule
  ],
  providers: [HttpClient, ApplicationService, ...routes.getProviders()]
})
export class NewApplicationModule {
  static baseRoute: string = 'new-application';
}
