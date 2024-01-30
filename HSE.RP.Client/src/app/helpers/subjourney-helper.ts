import { Params, Route } from "@angular/router";
import { ApplicantProfessionBodyMemberships, ApplicantProfessionBodyMembershipsHelper, ApplicantProfessionalBodyMembership } from "../models/applicant-professional-body-membership";
import { BuildingInspectorClass } from "../models/building-inspector-class.model";
import { BuildingInspectorClassType } from "../models/building-inspector-classtype.enum";
import { BuildingProfessionalModel } from "../models/building-professional.model";
import { ComponentCompletionState } from "../models/component-completion-state.enum";
import { ProfessionalBodyMembershipStep } from "../models/professional-body-membership-step.enum";
import { ApplicantEmploymentDetails } from "../models/applicant-employment-details";
import { EmploymentType } from "../models/employment-type.enum";


export type ProfessionalBodyMembershipRoute = { route: string, queryParams?: Params };

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
            else {

                //If activities not selected return activities screen
                if (model.Activities.CompletionState != ComponentCompletionState.Complete) {
                    return "building-inspector-regulated-activities";
                }

                //Else determine next screen based on class type
                else {
                    if (model.ClassType.Class == BuildingInspectorClassType.Class2) {
                        if (model.Activities.AssessingPlans == true && model.AssessingPlansClass2.CompletionState != ComponentCompletionState.Complete) {
                            return "building-class2-assessing-plans-categories";
                        }
                        if (model.Activities.Inspection == true && model.Class2InspectBuildingCategories.CompletionState != ComponentCompletionState.Complete) {
                            return "building-class2-inspect-building-categories";
                        }
                    }

                    if (model.ClassType.Class == BuildingInspectorClassType.Class3) {
                        if (model.Activities.AssessingPlans == true && model.AssessingPlansClass3.CompletionState != ComponentCompletionState.Complete) {
                            return "building-class3-assessing-plans-categories";
                        }
                        if (model.Activities.Inspection == true && model.Class3InspectBuildingCategories.CompletionState != ComponentCompletionState.Complete) {
                            return "building-class3-inspect-building-categories";
                        }
                    }

                }
                //If class 2 or 3 and all activities complete return technical manager screen

                return "building-class-technical-manager";
            }


        }

    }

    static getEmploymentRoute(model: ApplicantEmploymentDetails): string {

        if (model.CompletionState == ComponentCompletionState.Complete || model.CompletionState == ComponentCompletionState.NotStarted) {
            return "professional-activity-employment-type";
        }

        if (model.CompletionState == ComponentCompletionState.InProgress && model.EmploymentTypeSelection!.CompletionState != ComponentCompletionState.Complete) {
            return "professional-activity-employment-type";
        }

        if (model.CompletionState == ComponentCompletionState.InProgress && model.EmploymentTypeSelection!.CompletionState == ComponentCompletionState.Complete) {

            if (model.EmployerName?.CompletionState != ComponentCompletionState.Complete) {

                if (model!.EmploymentTypeSelection!.EmploymentType == EmploymentType.Other) {
                    return "employment-other-name"
                }

                if (model!.EmploymentTypeSelection!.EmploymentType == EmploymentType.PublicSector) {
                    return "employment-public-sector-body-name"
                }

                if (model!.EmploymentTypeSelection!.EmploymentType == EmploymentType.PrivateSector) {
                    return "employment-private-sector-body-name"
                }

                if (model!.EmploymentTypeSelection!.EmploymentType == EmploymentType.Unemployed) {
                    return "professional-membership-and-employment-summary"
                }
            }
            
            if(model.EmployerAddress?.CompletionState != ComponentCompletionState.Complete)
            {
                return "search-employment-org-address"
            }

            return "professional-membership-and-employment-summary"

        }

        return "professional-activity-employment-type";
    }

    //Change return type to dictionary of two string

    static getProfessionalBodyMembershipRoute(model: ApplicantProfessionBodyMemberships): ProfessionalBodyMembershipRoute {




        //If no professional body memberships return start screen
        if (model.CompletionState == ComponentCompletionState.Complete && model.ApplicantHasProfessionBodyMemberships.CompletionState == ComponentCompletionState.Complete && model.ApplicantHasProfessionBodyMemberships.IsProfessionBodyRelevantYesNo == "no") {

            return { route: "professional-body-memberships" };
        }

        if (model.CompletionState == ComponentCompletionState.Complete && model.ApplicantHasProfessionBodyMemberships.CompletionState == ComponentCompletionState.Complete && model.ApplicantHasProfessionBodyMemberships.IsProfessionBodyRelevantYesNo == "yes") {

            //If any body selected return the professional body summary screen
            if (ApplicantProfessionBodyMembershipsHelper.AnyCompleted(model)) {
                return { route: "professional-body-membership-summary" };
            }
            else {
                return { route: "professional-body-memberships" };
            }
        }


        //If yes professional body memberships return professional-body-selection
        if (model.CompletionState == ComponentCompletionState.InProgress && model.ApplicantHasProfessionBodyMemberships.CompletionState == ComponentCompletionState.Complete && model.ApplicantHasProfessionBodyMemberships.IsProfessionBodyRelevantYesNo == "yes") {
            //If any body is currently in progress return that body details screen
            var inProgress = ApplicantProfessionBodyMembershipsHelper.GetInProgress(model);
            if (inProgress != null) {

                if (inProgress.CurrentStep == ProfessionalBodyMembershipStep.EnterDetails) {
                    return { route: "professional-membership-information", queryParams: { membershipCode: inProgress.MembershipBodyCode } };
                }

                if (inProgress.CurrentStep == ProfessionalBodyMembershipStep.ConfirmDetails) {
                    return { route: "professional-individual-membership-details", queryParams: { membershipCode: inProgress.MembershipBodyCode } };
                }

                if (inProgress.CurrentStep == ProfessionalBodyMembershipStep.Remove && inProgress.CompletionState == ComponentCompletionState.InProgress) {
                    return { route: "professional-confirmation-membership-removal", queryParams: { membershipCode: inProgress.MembershipBodyCode } };
                }

            }

            //If any body selected return the professional body summary screen
            if (ApplicantProfessionBodyMembershipsHelper.AnyCompleted(model)) {
                return { route: "professional-body-membership-summary" };
            }
            else if (!ApplicantProfessionBodyMembershipsHelper.AnyCompleted(model)) {
                return { route: "professional-body-selection" };
            }
        }


        return { route: "professional-body-memberships" };
    }
}