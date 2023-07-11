import { HttpClient, HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { HseAngularModule } from "hse-angular";
import { CommonModule } from "@angular/common";

import { GovukRequiredDirective } from "./required.directive";
import { TaskListItemComponent } from "./task-list-item/task-list-item.component";

@NgModule({
  declarations: [
    GovukRequiredDirective,
    TaskListItemComponent
  ],
  imports: [
    HseAngularModule,
    CommonModule,
    HttpClientModule,
  ],
  exports: [
    GovukRequiredDirective,
    TaskListItemComponent
  ],
  providers: [HttpClient]
})
export class ComponentsModule {

}
