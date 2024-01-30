import { Params, Route } from "@angular/router";
import { ApplicantProfessionBodyMemberships, ApplicantProfessionBodyMembershipsHelper, ApplicantProfessionalBodyMembership } from "../models/applicant-professional-body-membership";
import { BuildingInspectorClass } from "../models/building-inspector-class.model";
import { BuildingInspectorClassType } from "../models/building-inspector-classtype.enum";
import { BuildingProfessionalModel } from "../models/building-professional.model";
import { ComponentCompletionState } from "../models/component-completion-state.enum";
import { ProfessionalBodyMembershipStep } from "../models/professional-body-membership-step.enum";
import { ApplicantEmploymentDetails } from "../models/applicant-employment-details";
import { EmploymentType } from "../models/employment-type.enum";

/**
 * Type for the route of a professional body membership.
 */
export type ProfessionalBodyMembershipRoute = { route: string, queryParams?: Params };

/**
 * Helper class for subjourneys.
 */
export class SubjourneyHelper {
    /**
     * Private constructor to prevent instantiation.
     */
    private constructor() {}

    /**
     * Determines the next route for a building inspector class.
     * @param model The building inspector class model.
     * @returns The next route as a string.
     */
    static getClassSelectionRoute(model: BuildingInspectorClass): string {
        if (model.CompletionState == ComponentCompletionState.Complete || model.CompletionState == ComponentCompletionState.NotStarted) {
            return "building-inspector-class-selection";
        }

        if (model.ClassType.Class == BuildingInspectorClassType.Class1 || model.ClassType.CompletionState != ComponentCompletionState.Complete) {
            return "building-inspector-class-selection";
        }

        if (model.Activities.CompletionState != ComponentCompletionState.Complete) {
            return "building-inspector-regulated-activities";
        }

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

        return "building-class-technical-manager";
    }

    /**
     * Determines the next route for an applicant's employment details.
     * @param model The applicant's employment details model.
     * @returns The next route as a string.
     */
    static getEmploymentRoute(model: ApplicantEmploymentDetails): string {
        if (model.CompletionState == ComponentCompletionState.Complete || model.CompletionState == ComponentCompletionState.NotStarted) {
            return "professional-activity-employment-type";
        }

        if (model.CompletionState == ComponentCompletionState.InProgress && model.EmploymentTypeSelection!.CompletionState != ComponentCompletionState.Complete) {
            return "professional-activity-employment-type";
        }

        if (model.CompletionState == ComponentCompletionState.InProgress && model.EmploymentTypeSelection!.CompletionState == ComponentCompletionState.Complete) {
            if (model.EmployerName?.CompletionState != ComponentCompletionState.Complete) {
                switch (model!.EmploymentTypeSelection!.EmploymentType) {
                    case EmploymentType.Other:
                        return "employment-other-name";
                    case EmploymentType.PublicSector:
                        return "employment-public-sector-body-name";
                    case EmploymentType.PrivateSector:
                        return "employment-private-sector-body-name";
                    case EmploymentType.Unemployed:
                        return "professional-membership-and-employment-summary";
                }
            }

            return "search-employment-org-address";
        }

        return "professional-activity-employment-type";
    }

    /**
     * Determines the next route for an applicant's professional body memberships.
     * @param model The applicant's professional body memberships model.
     * @returns The next route as a ProfessionalBodyMembershipRoute object.
     */
    static getProfessionalBodyMembershipRoute(model: ApplicantProfessionBodyMemberships): ProfessionalBodyMembershipRoute {
        if (model.CompletionState == ComponentCompletionState.Complete && model.ApplicantHasProfessionBodyMemberships.CompletionState == ComponentCompletionState.Complete) {
            if (model.ApplicantHasProfessionBodyMemberships.IsProfessionBodyRelevantYesNo == "no") {
                return { route: "professional-body-memberships" };
            }

            if (model.ApplicantHasProfessionBodyMemberships.IsProfessionBodyRelevantYesNo == "yes") {
                if (ApplicantProfessionBodyMembershipsHelper.AnyCompleted(model)) {
                    return { route: "professional-body-membership-summary" };
                } else {
                    return { route: "professional-body-memberships" };
                }
            }
        }

        if (model.CompletionState == ComponentCompletionState.InProgress && model.ApplicantHasProfessionBodyMemberships.CompletionState == ComponentCompletionState.Complete && model.ApplicantHasProfessionBodyMemberships.IsProfessionBodyRelevantYesNo == "yes") {
            const inProgress = ApplicantProfessionBodyMembershipsHelper.GetInProgress(model);
            if (inProgress != null) {
                const routes: { [key in ProfessionalBodyMembershipStep]: string } = {
                    [ProfessionalBodyMembershipStep.NotStarted]: "professional-membership-information",
                    [ProfessionalBodyMembershipStep.EnterDetails]: "professional-membership-information",
                    [ProfessionalBodyMembershipStep.ConfirmDetails]: "professional-individual-membership-details",
                    [ProfessionalBodyMembershipStep.Remove]: "professional-confirmation-membership-removal"
                };

                return { route: routes[inProgress.CurrentStep!], queryParams: { membershipCode: inProgress.MembershipBodyCode } };
            }

            if (ApplicantProfessionBodyMembershipsHelper.AnyCompleted(model)) {
                return { route: "professional-body-membership-summary" };
            } else {
                return { route: "professional-body-selection" };
            }
        }

        return { route: "professional-body-memberships" };
    }
}