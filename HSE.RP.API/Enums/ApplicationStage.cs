namespace HSE.RP.API.Enums
{

    [Flags]
    public enum ApplicationStage
    {
        NotStarted = 0,
        PersonalDetails = 1,
        BuildingInspectorClass = 2,
        Competency = 3,
        ProfessionalMembershipsAndEmployment = 4,
        ApplicationSummary = 5,
        PayAndSubmit = 6,
        ApplicationSubmitted = 7
    }
}

