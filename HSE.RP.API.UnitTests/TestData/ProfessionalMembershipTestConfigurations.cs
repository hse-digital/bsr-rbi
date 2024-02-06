using HSE.RP.API.Enums;
using HSE.RP.API.Models;
using HSE.RP.Domain.Entities;
using BuildingInspectorClass = HSE.RP.API.Models.BuildingInspectorClass;

namespace HSE.RP.API.UnitTests.TestData
{
    public class ProfessionalMembershipTestConfigurations
    {
        public BuildingInspectorClass Class1;
        public BuildingInspectorClass Class2;
        public BuildingInspectorClass Class3;
        public BuildingInspectorClass Class4;

        public BuildingInspectorRegistrationClass Class1Registration;
        public BuildingInspectorRegistrationClass Class2Registration;
        public BuildingInspectorRegistrationClass Class3Registration;
        public BuildingInspectorRegistrationClass Class4Registration;

        public DynamicsBuildingInspectorRegistrationClass Class1DynamicsRegistration;
        public DynamicsBuildingInspectorRegistrationClass Class2DynamicsRegistration;
        public DynamicsBuildingInspectorRegistrationClass Class3DynamicsRegistration;
        public DynamicsBuildingInspectorRegistrationClass Class4DynamicsRegistration;

        public BuildingInspectorRegistrationCountry BICountryEngland;
        public BuildingInspectorRegistrationCountry BICountryWales;

        public DynamicsBuildingInspectorRegistrationCountry BIRegistrationCountryEngland;
        public DynamicsBuildingInspectorRegistrationCountry BIRegistrationCountryWales;

        public BuildingInspectorRegistrationActivity BIActivityInspectBuildings2A;
        public BuildingInspectorRegistrationActivity BIActivityInspectBuildings2B;
        public BuildingInspectorRegistrationActivity BIActivityInspectBuildings2C;
        public BuildingInspectorRegistrationActivity BIActivityInspectBuildings2D;
        public BuildingInspectorRegistrationActivity BIActivityInspectBuildings2E;
        public BuildingInspectorRegistrationActivity BIActivityInspectBuildings2F;

        public BuildingInspectorRegistrationActivity BIActivityInspectBuildings3A;
        public BuildingInspectorRegistrationActivity BIActivityInspectBuildings3B;
        public BuildingInspectorRegistrationActivity BIActivityInspectBuildings3C;
        public BuildingInspectorRegistrationActivity BIActivityInspectBuildings3D;
        public BuildingInspectorRegistrationActivity BIActivityInspectBuildings3E;
        public BuildingInspectorRegistrationActivity BIActivityInspectBuildings3F;
        public BuildingInspectorRegistrationActivity BIActivityInspectBuildings3G;
        public BuildingInspectorRegistrationActivity BIActivityInspectBuildings3H;

        public BuildingInspectorRegistrationActivity BIActivityAssessingPlans2A;
        public BuildingInspectorRegistrationActivity BIActivityAssessingPlans2B;
        public BuildingInspectorRegistrationActivity BIActivityAssessingPlans2C;
        public BuildingInspectorRegistrationActivity BIActivityAssessingPlans2D;
        public BuildingInspectorRegistrationActivity BIActivityAssessingPlans2E;
        public BuildingInspectorRegistrationActivity BIActivityAssessingPlans2F;

        public BuildingInspectorRegistrationActivity BIActivityAssessingPlans3A;
        public BuildingInspectorRegistrationActivity BIActivityAssessingPlans3B;
        public BuildingInspectorRegistrationActivity BIActivityAssessingPlans3C;
        public BuildingInspectorRegistrationActivity BIActivityAssessingPlans3D;
        public BuildingInspectorRegistrationActivity BIActivityAssessingPlans3E;
        public BuildingInspectorRegistrationActivity BIActivityAssessingPlans3F;
        public BuildingInspectorRegistrationActivity BIActivityAssessingPlans3G;
        public BuildingInspectorRegistrationActivity BIActivityAssessingPlans3H;

        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityInspectBuildings2A;
        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityInspectBuildings2B;
        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityInspectBuildings2C;
        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityInspectBuildings2D;
        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityInspectBuildings2E;
        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityInspectBuildings2F;

        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityInspectBuildings3A;
        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityInspectBuildings3B;
        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityInspectBuildings3C;
        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityInspectBuildings3D;
        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityInspectBuildings3E;
        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityInspectBuildings3F;
        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityInspectBuildings3G;
        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityInspectBuildings3H;

        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityAssessingPlans2A;
        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityAssessingPlans2B;
        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityAssessingPlans2C;
        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityAssessingPlans2D;
        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityAssessingPlans2E;
        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityAssessingPlans2F;

        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityAssessingPlans3A;
        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityAssessingPlans3B;
        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityAssessingPlans3C;
        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityAssessingPlans3D;
        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityAssessingPlans3E;
        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityAssessingPlans3F;
        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityAssessingPlans3G;
        public DynamicsBuildingInspectorRegistrationActivity DynamicsBIActivityAssessingPlans3H;



        public ProfessionalMembershipTestConfigurations()
        {
            Class1 = new BuildingInspectorClass
            {
                ClassType = new ClassSelection
                {
                    Class = BuildingInspectorClassType.Class1,
                    CompletionState = ComponentCompletionState.Complete
                },
                Activities = new BuildingInspectorRegulatedActivies
                {
                    AssessingPlans = false,
                    Inspection = false,
                    CompletionState = ComponentCompletionState.NotStarted
                },
                AssessingPlansClass2 = new BuildingAssessingPlansCategoriesClass2
                {
                    CategoryA = false,
                    CategoryB = false,
                    CategoryC = false,
                    CategoryD = false,
                    CategoryE = false,
                    CategoryF = false,
                    CompletionState = ComponentCompletionState.NotStarted
                },
                AssessingPlansClass3 = new BuildingAssessingPlansCategoriesClass3
                {
                    CategoryG = false,
                    CategoryH = false,
                    CategoryA = false,
                    CategoryB = false,
                    CategoryC = false,
                    CategoryD = false,
                    CategoryE = false,
                    CategoryF = false,
                    CompletionState = ComponentCompletionState.NotStarted
                },
                ClassTechnicalManager = "no",
                InspectorCountryOfWork = new BuildingInspectorCountryOfWork
                {
                    England = true,
                    Wales = true,
                    CompletionState = ComponentCompletionState.Complete
                },
                Class2InspectBuildingCategories = new Class2InspectBuildingCategories
                {
                    CategoryA = false,
                    CategoryB = false,
                    CategoryC = false,
                    CategoryD = false,
                    CategoryE = false,
                    CategoryF = false,
                    CompletionState = ComponentCompletionState.NotStarted
                },
                Class3InspectBuildingCategories = new Class3InspectBuildingCategories
                {
                    CategoryG = false,
                    CategoryH = false,
                    CategoryA = false,
                    CategoryB = false,
                    CategoryC = false,
                    CategoryD = false,
                    CategoryE = false,
                    CategoryF = false,
                    CompletionState = ComponentCompletionState.NotStarted
                }
            };
            Class2 = new BuildingInspectorClass
            {
                ClassType = new ClassSelection
                {
                    Class = BuildingInspectorClassType.Class2,
                    CompletionState = ComponentCompletionState.Complete
                },
                Activities = new BuildingInspectorRegulatedActivies
                {
                    AssessingPlans = true,
                    Inspection = true,
                    CompletionState = ComponentCompletionState.Complete
                },
                AssessingPlansClass2 = new BuildingAssessingPlansCategoriesClass2
                {
                    CategoryA = true,
                    CategoryB = true,
                    CategoryC = true,
                    CategoryD = true,
                    CategoryE = true,
                    CategoryF = true,
                    CompletionState = ComponentCompletionState.Complete
                },
                AssessingPlansClass3 = new BuildingAssessingPlansCategoriesClass3
                {
                    CategoryG = false,
                    CategoryH = false,
                    CategoryA = false,
                    CategoryB = false,
                    CategoryC = false,
                    CategoryD = false,
                    CategoryE = false,
                    CategoryF = false,
                    CompletionState = ComponentCompletionState.NotStarted
                },
                ClassTechnicalManager = "no",
                InspectorCountryOfWork = new BuildingInspectorCountryOfWork
                {
                    England = true,
                    Wales = true,
                    CompletionState = ComponentCompletionState.Complete
                },
                Class2InspectBuildingCategories = new Class2InspectBuildingCategories
                {
                    CategoryA = true,
                    CategoryB = true,
                    CategoryC = true,
                    CategoryD = true,
                    CategoryE = true,
                    CategoryF = true,
                    CompletionState = ComponentCompletionState.Complete
                },
                Class3InspectBuildingCategories = new Class3InspectBuildingCategories
                {
                    CategoryG = false,
                    CategoryH = false,
                    CategoryA = false,
                    CategoryB = false,
                    CategoryC = false,
                    CategoryD = false,
                    CategoryE = false,
                    CategoryF = false,
                    CompletionState = ComponentCompletionState.NotStarted
                }
            };
            Class3 = new BuildingInspectorClass
            {
                ClassType = new ClassSelection
                {
                    Class = BuildingInspectorClassType.Class3,
                    CompletionState = ComponentCompletionState.Complete
                },
                Activities = new BuildingInspectorRegulatedActivies
                {
                    AssessingPlans = true,
                    Inspection = true,
                    CompletionState = ComponentCompletionState.Complete
                },
                AssessingPlansClass2 = new BuildingAssessingPlansCategoriesClass2
                {
                    CategoryA = false,
                    CategoryB = false,
                    CategoryC = false,
                    CategoryD = false,
                    CategoryE = false,
                    CategoryF = false,
                    CompletionState = ComponentCompletionState.NotStarted
                },
                AssessingPlansClass3 = new BuildingAssessingPlansCategoriesClass3
                {
                    CategoryG = true,
                    CategoryH = true,
                    CategoryA = true,
                    CategoryB = true,
                    CategoryC = true,
                    CategoryD = true,
                    CategoryE = true,
                    CategoryF = true,
                    CompletionState = ComponentCompletionState.Complete
                },
                ClassTechnicalManager = "no",
                InspectorCountryOfWork = new BuildingInspectorCountryOfWork
                {
                    England = true,
                    Wales = true,
                    CompletionState = ComponentCompletionState.Complete
                },
                Class2InspectBuildingCategories = new Class2InspectBuildingCategories
                {
                    CategoryA = false,
                    CategoryB = false,
                    CategoryC = false,
                    CategoryD = false,
                    CategoryE = false,
                    CategoryF = false,
                    CompletionState = ComponentCompletionState.NotStarted
                },
                Class3InspectBuildingCategories = new Class3InspectBuildingCategories
                {
                    CategoryG = true,
                    CategoryH = true,
                    CategoryA = true,
                    CategoryB = true,
                    CategoryC = true,
                    CategoryD = true,
                    CategoryE = true,
                    CategoryF = true,
                    CompletionState = ComponentCompletionState.Complete
                }
            };
            Class4 = Class3 with { ClassTechnicalManager = "yes" };

            Class1Registration = new BuildingInspectorRegistrationClass
            {
                Id = "dd31ab1b-b671-ee11-8179-0022481b5210",
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                ApplicantId = "123456789",
                ClassId = BuildingInspectorClassNames.Ids[1],
                Name = "Class 1 trainee building inspector",
                StatusCode = (int)BuildingInspectorRegistrationClassStatus.Registered,
                StateCode = 0
            };
            Class2Registration = new BuildingInspectorRegistrationClass
            {
                Id = "63e2f453-a4be-ee11-9079-0022481b5210",
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                ApplicantId = "123456789",
                ClassId = BuildingInspectorClassNames.Ids[1],
                Name = "Class 1 trainee building inspector",
                StatusCode = (int)BuildingInspectorRegistrationClassStatus.Applied,
                StateCode = 0
            };
            Class3Registration = new BuildingInspectorRegistrationClass
            {
                Id = "679e2bbb-4ec0-ee11-9079-0022481b5210",
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                ApplicantId = "123456789",
                ClassId = BuildingInspectorClassNames.Ids[3],
                Name = "Class 3 specialist building inspector",
                StatusCode = (int)BuildingInspectorRegistrationClassStatus.Applied,
                StateCode = 0
            };
            Class4Registration = new BuildingInspectorRegistrationClass
            {
                Id = "779a75c3-ba7f-ee11-8179-0022481b5210",
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                ApplicantId = "123456789",
                ClassId = BuildingInspectorClassNames.Ids[4],
                Name = "Class 4 technical manager",
                StatusCode = (int)BuildingInspectorRegistrationClassStatus.Applied,
                StateCode = 0
            };

            Class1DynamicsRegistration = new DynamicsBuildingInspectorRegistrationClass
            {
                bsr_biregclassid = "dd31ab1b-b671-ee11-8179-0022481b5210",
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_biclassid_value = BuildingInspectorClassNames.Ids[2],
                _bsr_buildinginspectorid_value = "123456789",
                bsr_name = "Class 1 trainee building inspector",
                statuscode = (int)BuildingInspectorRegistrationClassStatus.Registered,
                statecode = 0
            };
            Class2DynamicsRegistration = new DynamicsBuildingInspectorRegistrationClass
            {
                bsr_biregclassid = "63e2f453-a4be-ee11-9079-0022481b5210",
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_biclassid_value = BuildingInspectorClassNames.Ids[2],
                _bsr_buildinginspectorid_value = "123456789",
                bsr_name = "Class 2 building inspector",
                statuscode = (int)BuildingInspectorRegistrationClassStatus.Applied,
                statecode = 0
            };
            Class3DynamicsRegistration = new DynamicsBuildingInspectorRegistrationClass
            {
                bsr_biregclassid = "679e2bbb-4ec0-ee11-9079-0022481b5210",
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_biclassid_value = BuildingInspectorClassNames.Ids[3],
                _bsr_buildinginspectorid_value = "123456789",
                bsr_name = "Class 3 specialist building inspector",
                statuscode = (int)BuildingInspectorRegistrationClassStatus.Applied,
                statecode = 0
            };
            Class4DynamicsRegistration = new DynamicsBuildingInspectorRegistrationClass
            {
                bsr_biregclassid = "779a75c3-ba7f-ee11-8179-0022481b5210",
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_biclassid_value = BuildingInspectorClassNames.Ids[4],
                _bsr_buildinginspectorid_value = "123456789",
                bsr_name = "Class 4 technical manager",
                statuscode = (int)BuildingInspectorRegistrationClassStatus.Applied,
                statecode = 0
            };

            BICountryEngland = new BuildingInspectorRegistrationCountry
            {
                Id = "cb97db0d-a771-ee11-8179-0022481b5210",
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                CountryID = BuildingInspectorCountryNames.Ids["England"],
                BuildingInspectorId = "123456789",
                Name = "England",
                StatusCode = 1,
                StateCode = 0
            };
            BICountryWales = new BuildingInspectorRegistrationCountry
            {
                Id = "e631ab1b-b671-ee11-8179-0022481b5210",
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                CountryID = BuildingInspectorCountryNames.Ids["Wales"],
                BuildingInspectorId = "123456789",
                Name = "Wales",
                StatusCode = 1,
                StateCode = 0
            };

            BIRegistrationCountryEngland = new DynamicsBuildingInspectorRegistrationCountry
            {
                bsr_biregcountryid = "cb97db0d-a771-ee11-8179-0022481b5210",
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_countryid_value = BuildingInspectorCountryNames.Ids["England"],
                _bsr_buildinginspectorid_value = "123456789",
                bsr_name = "England",
                statuscode = 1,
                statecode = 0
            };
            BIRegistrationCountryWales = new DynamicsBuildingInspectorRegistrationCountry
            {
                bsr_biregcountryid = "e631ab1b-b671-ee11-8179-0022481b5210",
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_countryid_value = BuildingInspectorCountryNames.Ids["Wales"],
                _bsr_buildinginspectorid_value = "123456789",
                bsr_name = "Wales",
                statuscode = 1,
                statecode = 0
            };

            BIActivityInspectBuildings2A = new BuildingInspectorRegistrationActivity
            {
                Id = "ad0872cc-8b5c-ee11-8def-0022481b5210",
                ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryAClass2"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityInspectBuildings2B = new BuildingInspectorRegistrationActivity
            {
                Id = "b10872cc-8b5c-ee11-8def-0022481b5210",
                ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryBClass2"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityInspectBuildings2C = new BuildingInspectorRegistrationActivity
            {
                Id = "b50872cc-8b5c-ee11-8def-0022481b5210",
                ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryCClass2"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityInspectBuildings2D = new BuildingInspectorRegistrationActivity
            {
                Id = "b90872cc-8b5c-ee11-8def-0022481b5210",
                ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryDClass2"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityInspectBuildings2E = new BuildingInspectorRegistrationActivity
            {
                Id = "bd0872cc-8b5c-ee11-8def-0022481b5210",
                ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryEClass2"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityInspectBuildings2F = new BuildingInspectorRegistrationActivity
            {
                Id = "c10872cc-8b5c-ee11-8def-0022481b5210",
                ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryFClass2"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityInspectBuildings3A = new BuildingInspectorRegistrationActivity
            {
                Id = "c50872cc-8b5c-ee11-8def-0022481b5210",
                ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryAClass3"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityInspectBuildings3B = new BuildingInspectorRegistrationActivity
            {
                Id = "c90872cc-8b5c-ee11-8def-0022481b5210",
                ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryBClass3"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityInspectBuildings3C = new BuildingInspectorRegistrationActivity
            {
                Id = "cd0872cc-8b5c-ee11-8def-0022481b5210",
                ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryCClass3"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityInspectBuildings3D = new BuildingInspectorRegistrationActivity
            {
                Id = "d10872cc-8b5c-ee11-8def-0022481b5210",
                ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryDClass3"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityInspectBuildings3E = new BuildingInspectorRegistrationActivity
            {
                Id = "d50872cc-8b5c-ee11-8def-0022481b5210",
                ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryEClass3"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityInspectBuildings3F = new BuildingInspectorRegistrationActivity
            {
                Id = "d90872cc-8b5c-ee11-8def-0022481b5210",
                ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryFClass3"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityInspectBuildings3G = new BuildingInspectorRegistrationActivity
            {
                Id = "dd0872cc-8b5c-ee11-8def-0022481b5210",
                ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryGClass3"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityInspectBuildings3H = new BuildingInspectorRegistrationActivity
            {
                Id = "e10872cc-8b5c-ee11-8def-0022481b5210",
                ActivityId = BuildingInspectorActivityNames.Ids["Inspect"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryHClass3"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };

            BIActivityAssessingPlans2A = new BuildingInspectorRegistrationActivity
            {
                Id = "107db744-98ca-ed11-a7c6-000d3a0cd97b",
                ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryAClass2"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityAssessingPlans2B = new BuildingInspectorRegistrationActivity
            {
                Id = "7a4dbbea-2dc1-ee11-9079-0022481b5210",
                ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryBClass2"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityAssessingPlans2C = new BuildingInspectorRegistrationActivity
            {
                Id = "212c95fe-a9c1-ee11-9079-0022481b5210",
                ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryCClass2"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityAssessingPlans2D = new BuildingInspectorRegistrationActivity
            {
                Id = "5723c81c-8b24-ee11-9965-0022481b5210",
                ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryDClass2"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityAssessingPlans2E = new BuildingInspectorRegistrationActivity
            {
                Id = "1bb269e7-31bc-ee11-9078-0022481b56d1",
                ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryEClass2"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityAssessingPlans2F = new BuildingInspectorRegistrationActivity
            {
                Id = "bcd3ae04-aac1-ee11-9079-0022481b5210",
                ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryFClass2"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityAssessingPlans3A = new BuildingInspectorRegistrationActivity
            {
                Id = "107db744-98ca-ed11-a7c6-000d3a0cd97a",
                ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryAClass3"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityAssessingPlans3B = new BuildingInspectorRegistrationActivity
            {
                Id = "107db744-98ca-ez11-a7c6-000d3a0cd97a",
                ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryBClass3"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityAssessingPlans3C = new BuildingInspectorRegistrationActivity
            {
                Id = "107db744-97ca-ez11-a7c6-000d3a0cd97a",
                ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryCClass3"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityAssessingPlans3D = new BuildingInspectorRegistrationActivity
            {
                Id = "107db744-97ca-ez11-a7c6-000d3a0cd97a",
                ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryDClass3"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b9210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityAssessingPlans3E = new BuildingInspectorRegistrationActivity
            {
                Id = "107db744-97ca-ez11-a7z6-000d3a0cd97a",
                ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryEClass3"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b9210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityAssessingPlans3F = new BuildingInspectorRegistrationActivity
            {
                Id = "107db744-97ca-ez11-a7c6-100d3a0cd97a",
                ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryFClass3"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b9210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityAssessingPlans3G = new BuildingInspectorRegistrationActivity
            {
                Id = "107db744-97ca-ez11-a7c6-020d3a0cd97a",
                ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryGClass3"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b9210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };
            BIActivityAssessingPlans3H = new BuildingInspectorRegistrationActivity
            {
                Id = "107db744-97ca-ez11-a7c6-001d3a0cd97a",
                ActivityId = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                BuildingCategoryId = BuildingInspectorBuildingCategoryNames.Ids["CategoryHClass3"],
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b9210",
                BuildingInspectorId = "123456789",
                StatusCode = 1,
                StateCode = 0
            };

            DynamicsBIActivityInspectBuildings2A = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "ad0872cc-8b5c-ee11-8def-0022481b5210",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["Inspect"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryAClass2"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityInspectBuildings2B = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "b10872cc-8b5c-ee11-8def-0022481b5210",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["Inspect"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryBClass2"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityInspectBuildings2C = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "b50872cc-8b5c-ee11-8def-0022481b5210",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["Inspect"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryCClass2"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityInspectBuildings2D = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "b90872cc-8b5c-ee11-8def-0022481b5210",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["Inspect"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryDClass2"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityInspectBuildings2E = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "bd0872cc-8b5c-ee11-8def-0022481b5210",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["Inspect"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryEClass2"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityInspectBuildings2F = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "c10872cc-8b5c-ee11-8def-0022481b5210",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["Inspect"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryFClass2"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityInspectBuildings3A = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "c50872cc-8b5c-ee11-8def-0022481b5210",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["Inspect"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryAClass3"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityInspectBuildings3B = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "c90872cc-8b5c-ee11-8def-0022481b5210",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["Inspect"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryBClass3"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityInspectBuildings3C = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "cd0872cc-8b5c-ee11-8def-0022481b5210",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["Inspect"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryCClass3"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityInspectBuildings3D = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "d10872cc-8b5c-ee11-8def-0022481b5210",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["Inspect"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryDClass3"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityInspectBuildings3E = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "d50872cc-8b5c-ee11-8def-0022481b5210",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["Inspect"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryEClass3"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityInspectBuildings3F = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "d90872cc-8b5c-ee11-8def-0022481b5210",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["Inspect"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryFClass3"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityInspectBuildings3G = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "dd0872cc-8b5c-ee11-8def-0022481b5210",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["Inspect"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryGClass3"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityInspectBuildings3H = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "e10872cc-8b5c-ee11-8def-0022481b5210",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["Inspect"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryHClass3"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };

            DynamicsBIActivityAssessingPlans2A = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "107db744-98ca-ed11-a7c6-000d3a0cd97b",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryAClass2"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityAssessingPlans2B = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "7a4dbbea-2dc1-ee11-9079-0022481b5210",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryBClass2"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityAssessingPlans2C = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "212c95fe-a9c1-ee11-9079-0022481b5210",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryCClass2"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityAssessingPlans2D = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "5723c81c-8b24-ee11-9965-0022481b5210",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryDClass2"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityAssessingPlans2E = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "1bb269e7-31bc-ee11-9078-0022481b56d1",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryEClass2"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityAssessingPlans2F = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "bcd3ae04-aac1-ee11-9079-0022481b5210",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryFClass2"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityAssessingPlans3A = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "107db744-98ca-ed11-a7c6-000d3a0cd97a",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryAClass3"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityAssessingPlans3B = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "107db744-98ca-ez11-a7c6-000d3a0cd97a",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryBClass3"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityAssessingPlans3C = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "107db744-97ca-ez11-a7c6-000d3a0cd97a",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryCClass3"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityAssessingPlans3D = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "107db744-97ca-ez11-a7c6-000d3a0cd97a",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryDClass3"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b9210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityAssessingPlans3E = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "107db744-97ca-ez11-a7z6-000d3a0cd97a",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryEClass3"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b9210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityAssessingPlans3F = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "107db744-97ca-ez11-a7c6-100d3a0cd97a",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryFClass3"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b9210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityAssessingPlans3G = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "107db744-97ca-ez11-a7c6-020d3a0cd97a",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryGClass3"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b9210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };
            DynamicsBIActivityAssessingPlans3H = new DynamicsBuildingInspectorRegistrationActivity
            {
                bsr_biregactivityId = "107db744-97ca-ez11-a7c6-001d3a0cd97a",
                _bsr_biactivityid_value = BuildingInspectorActivityNames.Ids["AssessingPlans"],
                _bsr_bibuildingcategoryid_value = BuildingInspectorBuildingCategoryNames.Ids["CategoryHClass3"],
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b9210",
                _bsr_buildinginspectorid_value = "123456789",
                statuscode = 1,
                statecode = 0
            };




        }
    }
}
