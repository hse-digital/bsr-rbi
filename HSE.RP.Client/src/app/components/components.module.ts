import { HttpClient, HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { HseAngularModule } from "hse-angular";
import { CommonModule } from "@angular/common";

import { GovukRequiredDirective } from "./required.directive";
import { TaskListItemComponent } from "./task-list-item/task-list-item.component";

import { AddressComponent } from "./address/address.component";
import { ConfirmAddressComponent } from "./address/confirm-address.component";
import { FindAddressComponent } from "./address/find-address.component";
import { AddressService } from "src/app/services/address.service";
import { ManualAddressComponent } from "./address/manual-address.component";
import { NotFoundAddressComponent } from "./address/not-found-address.component";
import { SelectAddressComponent } from "./address/select-address.component";
import { TooManyAddressComponent } from "./address/too-many-address.component";
import { AddressDescriptionComponent } from './address-description.component';
import { ApplicationSummaryPersonalDetailsComponent } from './application-summary/personal-details-summary/personal-details.component';
import { ApplicationSummaryBuildingInspectorClassDetailsComponent } from "./application-summary/building-inspector-class-summary/building-inspector-class-details.component";
import { ApplicationSummaryCompetencyDetailsComponent } from "./application-summary/competency-summary/competency.component";
import { ApplicationSummaryProfessionalMembershipAndEmploymentDetailsComponent } from "./application-summary/professional-membership-and-employment-summary/professional-membership-and-employment-details.component";
import { GovukErrorLinkableComponent, GovukSummaryErrorLinkableComponent } from "./govuk-error-linkable/govuk-error-linkable.component";
import { GovukInputLinkableComponent } from "./govuk-input-linkable/govuk-input-linkable.component";
import { FormsModule } from "@angular/forms";
import { GovukFieldLinkableComponent } from "./govuk-field-linkable/govuk-field-linkable.component";



@NgModule({
  declarations: [
    GovukRequiredDirective,
    TaskListItemComponent,
    AddressComponent,
    ConfirmAddressComponent,
    FindAddressComponent,
    ManualAddressComponent,
    NotFoundAddressComponent,
    TooManyAddressComponent,
    SelectAddressComponent,
    AddressDescriptionComponent,
    ApplicationSummaryPersonalDetailsComponent,
    ApplicationSummaryBuildingInspectorClassDetailsComponent,
    ApplicationSummaryCompetencyDetailsComponent,
    ApplicationSummaryProfessionalMembershipAndEmploymentDetailsComponent,
    GovukSummaryErrorLinkableComponent,
    GovukErrorLinkableComponent,
    GovukInputLinkableComponent,
    GovukFieldLinkableComponent
  ],
  imports: [
    HseAngularModule,
    CommonModule,
    HttpClientModule,
    FormsModule
  ],
  exports: [
    GovukRequiredDirective,
    TaskListItemComponent,
    AddressComponent,
    AddressDescriptionComponent,
    ApplicationSummaryPersonalDetailsComponent,
    ApplicationSummaryBuildingInspectorClassDetailsComponent,
    ApplicationSummaryCompetencyDetailsComponent,
    ApplicationSummaryProfessionalMembershipAndEmploymentDetailsComponent,
    GovukSummaryErrorLinkableComponent,
    GovukErrorLinkableComponent,
    GovukInputLinkableComponent,
    GovukFieldLinkableComponent




  ],
  providers: [HttpClient, AddressService]
})
export class ComponentsModule {

}
