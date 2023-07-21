import { BuidlingInspectorAssessingPlansClass3 } from "./buidling-inspector-assessing-plans-class3.model";
import { BuildingAssessingPlansCategories } from "./building-assessing-plans-categories.model";
import { BuildingInspectorClassType } from "./building-inspector-classtype.enum";
import { BuildingInspectorCountryOfWork } from "./building-inspector-country-of-work.model";
import { BuildingInspectorRegulatedActivies } from "./building-inspector-regulated-activies.model";
import { ClassSelection } from "./class-selection.model";
import { Class3BuildingAssessingPlansCategories } from "./class3-building-assessing-plan-scategories.model";
import { ComponentCompletionState } from "./component-completion-state.enum";

export class BuildingInspectorClass {
    ClassType: ClassSelection = {
      Class: BuildingInspectorClassType.ClassNone,
      CompletionState: ComponentCompletionState.NotStarted,
    };
    Activities: BuildingInspectorRegulatedActivies = {
      AssessingPlans: false,
      Inspection: false,
      CompletionState: ComponentCompletionState.NotStarted,
    };
    BuildingPlanCategories?: BuildingAssessingPlansCategories = {
      CategoryA: false,
      CategoryB: false,
      CategoryC: false,
      CategoryD: false,
      CategoryE: false,
      CategoryF: false,
    };
    AssessingPlansClass3: BuidlingInspectorAssessingPlansClass3 = {
      CategoryA: false,
      CategoryB: false,
      CategoryC: false,
      CategoryD: false,
      CategoryE: false,
      CategoryF: false,
      CategoryG: false,
      CategoryH: false,
    }
    ClassTechnicalManager?: string;
      InspectorCountryOfWork?: BuildingInspectorCountryOfWork = { England: false, Wales: false };
      Class3BuildingPlanCategories: Class3BuildingAssessingPlansCategories = new Class3BuildingAssessingPlansCategories();
  }