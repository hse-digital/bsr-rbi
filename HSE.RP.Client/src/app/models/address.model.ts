import { ComponentCompletionState } from "./component-completion-state.enum";
import { IComponentModel } from "./component. interface";

export class AddressModel implements IComponentModel {
    Address?: string;
    AddressLineTwo?: string;
    BuildingName?: string;
    Number?: string;
    Street?: string;
    Town?: string;
    Country?: string;
    AdministrativeArea?: string;
    Postcode?: string;
    ClassificationCode?: string;
    CustodianCode?: string;
    CustodianDescription?: string;
    IsManual: boolean = false;
    UPRN?: string;
    USRN?: string;
    CompletionState?: ComponentCompletionState = ComponentCompletionState.NotStarted;
  }
