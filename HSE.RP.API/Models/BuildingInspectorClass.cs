namespace HSE.RP.API.Models
{
    public record BuildingInspectorClass
    {
        public ClassSelection ClassType { get; set; }
        public BuildingInspectorRegulatedActivies Activities { get; set; }
        public BuildingAssessingPlansCategoriesClass2 AssessingPlansClass2 { get; set; }
        public BuildingAssessingPlansCategoriesClass3 AssessingPlansClass3 { get; set; }
        public string ClassTechnicalManager { get; set; } = string.Empty;
        public BuildingInspectorCountryOfWork InspectorCountryOfWork { get; set; }
        public Class2InspectBuildingCategories Class2InspectBuildingCategories { get; set; }
        public Class3InspectBuildingCategories Class3InspectBuildingCategories { get; set; }
    }
}

