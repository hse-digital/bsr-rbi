using System;
namespace HSE.RP.API.Enums
{

    [Flags]
    public enum ApplicationStatus
    {
        None = 0,
        EmailVerified = 1,
        PhoneVerified = 2,
        PersonalDetailsComplete = 4,
        BuildingInspectorClassComplete = 8,
        CompetencyComplete = 16,
        ProfessionalActivityComplete = 32,
        ApplicationSubmissionComplete = 64,
        PaymentInProgress = 128,
        PaymentComplete = 256,
    }
}

