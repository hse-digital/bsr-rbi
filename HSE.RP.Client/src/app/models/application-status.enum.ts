export enum ApplicationStatus {
    None = 0,
    EmailVerified = 1,
    PhoneVerified = 2,
    PersonalDetailsComplete = 4,
    BuildingInspectorClassComplete = 8,
    CompetencyComplete = 16,
    ProfessionalActivityComplete = 23,
    ApplicationSubmissionComplete = 64,
    PaymentInProgress = 128,
    PaymentComplete = 256,
  }