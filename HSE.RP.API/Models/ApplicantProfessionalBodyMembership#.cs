using HSE.RP.API.Enums;

namespace HSE.RP.API.Models;

public class ApplicantProfessionalBodyMembership
{
    public string MembershipBodyCode { get; set; }
    public string MembershipNumber { get; set; }
    public string MembershipLevel { get; set; }
    public int MembershipYear { get; set; }
    public ComponentCompletionState CompletionState { get; set; }

    public ProfessionalBodyMembershipStep CurrentStep { get; set; }

    public string RemoveOptionSelected { get; set; }
}

public class ApplicantProfessionBodyMemberships
{
    public ApplicantHasProfessionBodyMemberships ApplicantHasProfessionBodyMemberships { get; set; }
    public ApplicantProfessionalBodyMembership RICS { get; set; }
    public ApplicantProfessionalBodyMembership CABE { get; set; }
    public ApplicantProfessionalBodyMembership CIOB { get; set; }
    public ApplicantProfessionalBodyMembership OTHER { get; set; }
    public ComponentCompletionState CompletionState { get; set; }

}

public class ApplicantHasProfessionBodyMemberships
{
    public string IsProfessionBodyRelevantYesNo { get; set; }
    public ComponentCompletionState CompletionState { get; set; }
}