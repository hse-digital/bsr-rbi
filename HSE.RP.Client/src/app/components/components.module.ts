import { HttpClient, HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { HseAngularModule } from "hse-angular";
import { CommonModule } from "@angular/common";

import { GovukRequiredDirective } from "./required.directive";

@NgModule({
  declarations: [
    GovukRequiredDirective,
  ],
  imports: [
    HseAngularModule,
    CommonModule,
    HttpClientModule
  ],
  exports: [
    GovukRequiredDirective,
  ],
  providers: [HttpClient]
})
export class ComponentsModule {

}
