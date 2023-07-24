
import { BuildingAssessingPlansCategoriesClass3 } from "./buidling-assessing-plans-categories-class3.model";
import { BuildingAssessingPlansCategoriesClass2 } from "./building-assessing-plans-categories-class2.model";
import { BuildingInspectorClassType } from "./building-inspector-classtype.enum";
import { BuildingInspectorCountryOfWork } from "./building-inspector-country-of-work.model";
import { BuildingInspectorRegulatedActivies } from "./building-inspector-regulated-activies.model";
import { ClassSelection } from "./class-selection.model";
import { Class2InspectBuildingCategories } from "./class2-inspect-building-categories.model";
import { Class3InspectBuildingCategories } from "./class3-inspect-building-categories.model";

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
    AssessingPlansClass2: BuildingAssessingPlansCategoriesClass2 = {
      CategoryA: false,
      CategoryB: false,
      CategoryC: false,
      CategoryD: false,
      CategoryE: false,
      CategoryF: false,
    };
    AssessingPlansClass3: BuildingAssessingPlansCategoriesClass3 = {
      CategoryA: false,
      CategoryB: false,
      CategoryC: false,
      CategoryD: false,
      CategoryE: false,
      CategoryF: false,
      CategoryG: false,
      CategoryH: false,
    }
    Class2InspectBuildingCategories: Class2InspectBuildingCategories = new Class2InspectBuildingCategories();
    Class3InspectBuildingCategories: Class3InspectBuildingCategories = new Class3InspectBuildingCategories();

    ClassTechnicalManager?: string;
      InspectorCountryOfWork?: BuildingInspectorCountryOfWork = { England: false, Wales: false };
      Class3BuildingPlanCategories: Class3InspectBuildingCategories = new Class3InspectBuildingCategories();
  }