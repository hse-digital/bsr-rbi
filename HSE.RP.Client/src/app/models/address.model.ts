import { ComponentCompletionState } from "./component-completion-state.enum";
import { IComponentModel } from "./component. interface";

export class AddressModel implements IComponentModel {
    IsManual: boolean = false;
    UPRN?: string;
    USRN?: string;
    Address?: string;
    AddressLineTwo?: string;
    BuildingName?: string;
    Number?: string;
    Street?: string;
    Town?: string;
    Country?: string;
    AdministrativeArea?: string;
    Postcode?: string;
    CompletionState?: ComponentCompletionState;
  }