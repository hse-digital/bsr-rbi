import { BuildingProfessionalModel } from "../models/building-professional.model";
import { StageCompletionState } from "../models/stage-completion-state.enum";

export class NextStage {


    private constructor() {
    }

    static getNextStage(model: BuildingProfessionalModel): string {



        if (model.StageStatus["EmailVerification"] == StageCompletionState.Incomplete) {
            return "email-verification";
        }
        if (model.StageStatus["PhoneVerification"] == StageCompletionState.Incomplete) {
            return "phone-verification";
        }
        if (model.StageStatus["PersonalDetails"] == StageCompletionState.Incomplete) {
            return "personal-details";
        }
        if (model.StageStatus["BuildingInspectorClass"] == StageCompletionState.Incomplete) {
            return "building-inspector-class";
        }
        if (model.StageStatus["Competency"] == StageCompletionState.Incomplete) {
            return "competency";
        }
        if (model.StageStatus["ProfessionalActivity"] == StageCompletionState.Incomplete) {
            return "professional-memberships-and-employment";
        }
        if (model.StageStatus["ApplicationConfirmed"] == StageCompletionState.Incomplete) {
            return "application-summary";
        }
        if (model.StageStatus["Declaration"] == StageCompletionState.Incomplete) {
            return "application-summary";
        }
        if (model.StageStatus["Payment"] == StageCompletionState.Incomplete) {
            return "application-summary";
        }
        else {
            return "main-content";
        }

    }
    
}
  