﻿using HSE.RP.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE.RP.API.Models;

public class ApplicantProfessionalBodyMembership 
{
    public string MembershipBodyCode { get; set; }
    public string MembershipNumber { get; set; }
    public string MembershipLevel { get; set; }
    public int MembershipYear { get; set; }
    public ComponentCompletionState CompletionState;
}

public class ApplicantProfessionBodyMemberships
{
    public string IsProfessionBodyRelevantYesNo { get; set; }
    public ApplicantProfessionalBodyMembership RICS { get; set; }
    public ApplicantProfessionalBodyMembership CABE { get; set; }
    public ApplicantProfessionalBodyMembership CIOB { get; set; }
    public ApplicantProfessionalBodyMembership OTHER{ get; set; }
}