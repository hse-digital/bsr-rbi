import { BuildingInspectorClass } from "../models/building-inspector-class.model";
import { BuildingInspectorClassType } from "../models/building-inspector-classtype.enum";
import { BuildingProfessionalModel } from "../models/building-professional.model";
import { ComponentCompletionState } from "../models/component-completion-state.enum";

export class SubjourneyHelper {


    private constructor() {
    }

    static getClassSelectionRoute(model: BuildingInspectorClass): string {

        if (model.CompletionState == ComponentCompletionState.Complete || model.CompletionState == ComponentCompletionState.NotStarted) {
            return "building-inspector-class-selection";
        }
        else {

            //If class 1 return class selection screen
            if (model.ClassType.Class == BuildingInspectorClassType.Class1 || model.ClassType.CompletionState != ComponentCompletionState.Complete) {
                return "building-inspector-class-selection";
            }

            //If class 2 or 3 determine next screen
            else{

                //If activities not selected return activities screen
                if(model.Activities.CompletionState != ComponentCompletionState.Complete){
                    return "building-inspector-regulated-activities";
                }

                //Else determine next screen based on class type
                else{
                    if(model.ClassType.Class == BuildingInspectorClassType.Class2){
                        if(model.Activities.AssessingPlans == true && model.AssessingPlansClass2.CompletionState != ComponentCompletionState.Complete){
                            return "building-class2-assessing-plans-categories";
                        }
                        if(model.Activities.Inspection == true && model.Class2InspectBuildingCategories.CompletionState != ComponentCompletionState.Complete)
                        {
                            return "building-class2-inspect-building-categories";
                        }
                    }
                    
                    if(model.ClassType.Class == BuildingInspectorClassType.Class3)
                    {
                        if(model.Activities.AssessingPlans == true && model.AssessingPlansClass3.CompletionState != ComponentCompletionState.Complete){
                            return "building-class3-assessing-plans-categories";
                        }
                        if(model.Activities.Inspection == true && model.Class3InspectBuildingCategories.CompletionState != ComponentCompletionState.Complete)
                        {
                            return "building-class3-inspect-building-categories";
                        }
                    }

                }
                 //If class 2 or 3 and all activities complete return technical manager screen

                return "building-class-technical-manager";
            }

            
        }

    }
}