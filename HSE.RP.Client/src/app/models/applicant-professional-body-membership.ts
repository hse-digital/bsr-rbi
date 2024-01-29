import { Component } from "@angular/core";
import { ComponentCompletionState } from "./component-completion-state.enum";
import { IComponentModel } from "./component. interface";
import { ProfessionalBodyMembershipStep } from "./professional-body-membership-step.enum";

/// Defines a model for an individudal membership
export class ApplicantProfessionalBodyMembership implements IComponentModel {
  MembershipBodyCode: string = '';
  MembershipNumber: string = '';
  MembershipLevel: string = '';
  MembershipYear?: number;
  CompletionState?: ComponentCompletionState = ComponentCompletionState.NotStarted;
  CurrentStep?: ProfessionalBodyMembershipStep = ProfessionalBodyMembershipStep.NotStarted;
  RemoveOptionSelected?: string = '';
}


/// Known set of memberships an applicant can possibly have.
/// NOTE: Review the CompletionState of an membership to know if an applicant has entered that
///       membership. A CompletionState of Completed, show the member has selected and fully entered
///       a membership for that organisation.

export class ApplicantProfessionBodyMemberships implements IComponentModel {

  CompletionState?: ComponentCompletionState = ComponentCompletionState.NotStarted;
  ApplicantHasProfessionBodyMemberships: ApplicantHasProfessionBodyMemberships = new ApplicantHasProfessionBodyMemberships();
  RICS: ApplicantProfessionalBodyMembership = ApplicantProfessionBodyMembershipsHelper.Reset(ProfessionalBodies.RICS.BodyCode);
  CABE: ApplicantProfessionalBodyMembership = ApplicantProfessionBodyMembershipsHelper.Reset(ProfessionalBodies.CABE.BodyCode);
  CIOB: ApplicantProfessionalBodyMembership = ApplicantProfessionBodyMembershipsHelper.Reset(ProfessionalBodies.CIOB.BodyCode);
  OTHER: ApplicantProfessionalBodyMembership = ApplicantProfessionBodyMembershipsHelper.Reset(ProfessionalBodies.OTHER.BodyCode);
  

}

export class ApplicantHasProfessionBodyMemberships implements IComponentModel {

  CompletionState?: ComponentCompletionState = ComponentCompletionState.NotStarted;
  IsProfessionBodyRelevantYesNo: string = '';

}

export class ApplicantProfessionBodyMembershipsHelper {

  static GetInProgress(instance: ApplicantProfessionBodyMemberships): ApplicantProfessionalBodyMembership | null {

      //Check if any memberships have been entered
      if (instance.RICS.CompletionState == ComponentCompletionState.InProgress) {
        return instance.RICS;
      }
      if (instance.CABE.CompletionState == ComponentCompletionState.InProgress) {
        return instance.CABE;
      }
      if (instance.CIOB.CompletionState == ComponentCompletionState.InProgress) {
        return instance.CIOB;
      }
      if (instance.OTHER.CompletionState == ComponentCompletionState.InProgress) {
        return instance.OTHER;
      }
      return null;
    }

  static AnyCompleted(instance: ApplicantProfessionBodyMemberships): boolean {
    return (instance.RICS.CompletionState === ComponentCompletionState.Complete) ||
      (instance.CABE.CompletionState === ComponentCompletionState.Complete) ||
      (instance.CIOB.CompletionState === ComponentCompletionState.Complete) ||
      (instance.OTHER.CompletionState === ComponentCompletionState.Complete);
  }

  static AllCompleted(instance: ApplicantProfessionBodyMemberships): boolean {
    var a = instance.RICS.CompletionState === ComponentCompletionState.Complete;
    var b = instance.CABE.CompletionState === ComponentCompletionState.Complete;
    var c = instance.CIOB.CompletionState === ComponentCompletionState.Complete;
    var d = instance.OTHER.CompletionState === ComponentCompletionState.Complete;
    return a && b && c && d;
  }


  // Resets the professional body passed in, as if it were deleted.
  static Reset(bodyCode: string): ApplicantProfessionalBodyMembership {
    var memberShip = new ApplicantProfessionalBodyMembership();
    memberShip.MembershipBodyCode = bodyCode;
    memberShip.CompletionState = ComponentCompletionState.NotStarted;
    memberShip.CurrentStep = ProfessionalBodyMembershipStep.NotStarted;
    memberShip.RemoveOptionSelected = '';
    switch (bodyCode) {
      case ProfessionalBodies.RICS.BodyCode:
      case ProfessionalBodies.CABE.BodyCode:
      case ProfessionalBodies.CIOB.BodyCode:
      case ProfessionalBodies.OTHER.BodyCode:
        break;
      default:
        {
          throw new Error(`Unknown professional body code of ${bodyCode} passed into the reset method.`);
        }
    }
    return memberShip;
  }
}


export class ApplicantProfessionalBodyOrganisation {
  constructor(code: string, description: string, requestMembershipDetail: boolean) {
    this.BodyCode = code;
    this.BodyDescription = description;
    this.BodyRequestMembershipDetail = requestMembershipDetail;
  }
  BodyCode: string;
  BodyDescription: string;
  BodyRequestMembershipDetail: boolean;
}

// Professional bodies and applicant can select for entry.
export  class ProfessionalBodies {
  static RICS: ApplicantProfessionalBodyOrganisation = new ApplicantProfessionalBodyOrganisation("RICS", "Royal Institution of Chartered Surveyors (RISC)", true);
  static CABE: ApplicantProfessionalBodyOrganisation = new ApplicantProfessionalBodyOrganisation("CABE", "Chartered Association of Building Engineers (CABE)", true);
  static CIOB: ApplicantProfessionalBodyOrganisation = new ApplicantProfessionalBodyOrganisation("CIOB", "Chartered Institute of Building (CIOB)", true);
  static OTHER: ApplicantProfessionalBodyOrganisation = new ApplicantProfessionalBodyOrganisation("OTHER", "Other", false);

}
