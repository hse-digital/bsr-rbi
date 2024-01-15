import { BuildingInspectorClass } from "../models/building-inspector-class.model";
import { BuildingInspectorClassType } from "../models/building-inspector-classtype.enum";
import { BuildingProfessionalModel } from "../models/building-professional.model";
import { ComponentCompletionState } from "../models/component-completion-state.enum";

export class SubjourneyHelper {


    private constructor() {
    }

    static getClassSelectionRoute(model: BuildingInspectorClass): string {

        if (model.ClassType.CompletionState == ComponentCompletionState.Complete || model.ClassType.CompletionState == ComponentCompletionState.NotStarted) {
            return "building-inspector-class-selection";
        }
        else {
            //If class 1 return class selection screen
            if (model.ClassType.Class == BuildingInspectorClassType.Class1) {
                return "building-inspector-class-selection";
            }
            else{
                if(model.Activities.CompletionState != ComponentCompletionState.Complete){
                    return "building-inspector-regulated-activities";
                }
                
                else{

                    if(model.ClassType.Class == BuildingInspectorClassType.Class2){
                    }
                    
                    if(model.ClassType.Class == BuildingInspectorClassType.Class3)
                    {

                    }

                }
            }





        }

            return "";
        }
    }