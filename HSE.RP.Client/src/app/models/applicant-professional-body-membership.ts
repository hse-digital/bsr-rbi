import { Component } from "@angular/core";
import { ComponentCompletionState } from "./component-completion-state.enum";
import { IComponentModel } from "./component. interface";

/// Defines a model for an individudal membership
export class ApplicantProfessionalBodyMembership implements IComponentModel {
  MembershipBodyCode: string = '';
  MembershipNumber: string = '';
  MembershipLevel: string = '';
  MembershipYear: number = -1;
  CompletionState?: ComponentCompletionState = ComponentCompletionState.NotStarted;
}

/// Known set of memberships an applicant can possibly have.
/// NOTE: Review the CompletionState of an membership to know if an applicant has entered that
///       membership. A CompletionState of Completed, show the member has selected and fully entered
///       a membership for that organisation.

export class ApplicantProfessionBodyMemberships {
  constructor() {
    this.Reset(ProfessionalBodies.RICS.BodyCode);
    this.Reset(ProfessionalBodies.CABE.BodyCode);
    this.Reset(ProfessionalBodies.CIOB.BodyCode);
    this.Reset(ProfessionalBodies.OTHER.BodyCode);
  }
  IsProfessionBodyRelevantYesNo: string = '';
  RICS: ApplicantProfessionalBodyMembership = new ApplicantProfessionalBodyMembership();
  CABE: ApplicantProfessionalBodyMembership = new ApplicantProfessionalBodyMembership();
  CIOB: ApplicantProfessionalBodyMembership = new ApplicantProfessionalBodyMembership();
  OTHER: ApplicantProfessionalBodyMembership = new ApplicantProfessionalBodyMembership();
  // Resets the professional body passed in, as if it were deleted.
  Reset(bodyCode: string): void
  {
    switch (bodyCode) {
      case ProfessionalBodies.RICS.BodyCode:
        {
          this.RICS = new ApplicantProfessionalBodyMembership();
          this.RICS.MembershipBodyCode = ProfessionalBodies.RICS.BodyCode;
          break;
        }
      case ProfessionalBodies.CABE.BodyCode:
        {
          this.CABE = new ApplicantProfessionalBodyMembership();
          this.CABE.MembershipBodyCode = ProfessionalBodies.CABE.BodyCode;
          break;
        }
      case ProfessionalBodies.CIOB.BodyCode:
        {
          this.CIOB = new ApplicantProfessionalBodyMembership();
          this.CIOB.MembershipBodyCode = ProfessionalBodies.CIOB.BodyCode;
          break;
        }
      case ProfessionalBodies.OTHER.BodyCode:
        {
          this.OTHER = new ApplicantProfessionalBodyMembership();
          this.OTHER.MembershipBodyCode = ProfessionalBodies.OTHER.BodyCode;
          break;
        }
      default:
        {
          throw new Error(`Unknown professional body code of ${bodyCode} passed into the reset method.`);
        }
    }
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
