using HSE.RP.API.Enums;
using HSE.RP.API.Models;
using HSE.RP.Domain.Entities;
using BuildingInspectorClass = HSE.RP.API.Models.BuildingInspectorClass;

namespace HSE.RP.API.UnitTests.TestData
{
    public class ProfessionalMembershipTestConfigurations
    {
        public ApplicantProfessionalBodyMembership RICSApplicantProfessionalBodyMembership;
        public ApplicantProfessionalBodyMembership CABEApplicantProfessionalBodyMembership;
        public ApplicantProfessionalBodyMembership CIOBApplicantProfessionalBodyMembership;
        public ApplicantProfessionalBodyMembership OtherApplicantProfessionalBodyMembership;


        public BuildingInspectorProfessionalBodyMembership RICSBuildingProfessionApplication;
        public BuildingInspectorProfessionalBodyMembership CABEBuildingProfessionApplication;
        public BuildingInspectorProfessionalBodyMembership CIOBBuildingProfessionApplication;
        public BuildingInspectorProfessionalBodyMembership OtherBuildingProfessionApplication;


        public DynamicsBuildingInspectorProfessionalBodyMembership RICSDynamicsBuildingInspectorProfessionalBodyMembership;
        public DynamicsBuildingInspectorProfessionalBodyMembership CABEDynamicsBuildingInspectorProfessionalBodyMembership;
        public DynamicsBuildingInspectorProfessionalBodyMembership CIOBDynamicsBuildingInspectorProfessionalBodyMembership;
        public DynamicsBuildingInspectorProfessionalBodyMembership OtherDynamicsBuildingInspectorProfessionalBodyMembership;



        public ProfessionalMembershipTestConfigurations()
        {
            RICSApplicantProfessionalBodyMembership = new ApplicantProfessionalBodyMembership
            {
                MembershipBodyCode = "RICS",
                MembershipNumber = "1111",
                MembershipLevel = "Member",
                MembershipYear = 2024,
                CompletionState = ComponentCompletionState.Complete,
                RemoveOptionSelected = ""
            };
            CABEApplicantProfessionalBodyMembership = new ApplicantProfessionalBodyMembership
            {
                MembershipBodyCode = "CABE",
                MembershipNumber = "2222",
                MembershipLevel = "Member",
                MembershipYear = 2023,
                CompletionState = ComponentCompletionState.Complete,
                RemoveOptionSelected = ""
            };
            CIOBApplicantProfessionalBodyMembership = new ApplicantProfessionalBodyMembership
            {
                MembershipBodyCode = "CIOB",
                MembershipNumber = "3333",
                MembershipLevel = "Member",
                MembershipYear = 2022,
                CompletionState = ComponentCompletionState.Complete,
                RemoveOptionSelected = ""
            };
            OtherApplicantProfessionalBodyMembership = new ApplicantProfessionalBodyMembership
            {
                MembershipBodyCode = "OTHER",
                MembershipNumber = "",
                MembershipLevel = "",
                MembershipYear = 0,
                CompletionState = ComponentCompletionState.Complete
            };

            RICSBuildingProfessionApplication = new BuildingInspectorProfessionalBodyMembership
            {
                Name = "Membership: Royal Institution of Chartered Surveyors (RICS)",
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                ProfessionalBodyId = BuildingInspectorProfessionalBodyIds.Ids["RICS"],
                MembershipNumber = "1111",
                CurrentMembershipLevelOrType = "Member",
                YearId = "50c9aa5a-ef3c-ee11-bdf4-0022481b56d1",
                StatusCode = 1,
                StateCode = 0
            };
            CABEBuildingProfessionApplication = new BuildingInspectorProfessionalBodyMembership
            {
                Name = "Membership: Royal Institution of Chartered Surveyors (CABE)",
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                ProfessionalBodyId = BuildingInspectorProfessionalBodyIds.Ids["CABE"],
                MembershipNumber = "2222",
                CurrentMembershipLevelOrType = "Member",
                YearId = "50c9aa5a-ef3c-ee11-bdf4-0022481b56d2",
                StatusCode = 1,
                StateCode = 0
            };
            CIOBBuildingProfessionApplication = new BuildingInspectorProfessionalBodyMembership
            {
                Name = "Membership: Royal Institution of Chartered Surveyors (CIOB)",
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                ProfessionalBodyId = BuildingInspectorProfessionalBodyIds.Ids["CIOB"],
                MembershipNumber = "3333",
                CurrentMembershipLevelOrType = "Member",
                YearId = "50c9aa5a-ef3c-ee11-bdf4-0022481b56d3",
                StatusCode = 1,
                StateCode = 0
            };
            OtherBuildingProfessionApplication = new BuildingInspectorProfessionalBodyMembership
            {
                Name = "Membership: Royal Institution of Chartered Surveyors (Other)",
                BuildingProfessionApplicationId = "21ca463a-8988-ee11-be36-0022481b5210",
                BuildingInspectorId = "123456789",
                ProfessionalBodyId = BuildingInspectorProfessionalBodyIds.Ids["OTHER"],
                MembershipNumber = null,
                CurrentMembershipLevelOrType = "Member",
                YearId = null,
                StatusCode = 1,
                StateCode = 0
            };

            RICSDynamicsBuildingInspectorProfessionalBodyMembership = new DynamicsBuildingInspectorProfessionalBodyMembership
            {
                bsr_name = "Membership: Royal Institution of Chartered Surveyors (RICS)",
                bsr_biprofessionalmembershipid = "759289a6-b7be-ee11-9079-0022481b5210",
                buidingProfessionApplicationReferenceId = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                contactRefId = "123456789",
                _bsr_buildinginspectorid_value = "123456789",
                professionalBodyRefId = BuildingInspectorProfessionalBodyIds.Ids["RICS"],
                _bsr_professionalbodyid_value = BuildingInspectorProfessionalBodyIds.Ids["RICS"],
                bsr_membershipnumber = "1111",
                bsr_currentmembershiplevelortype = "Member",
                yearRefId = "1",
                _bsr_yearid_value = "50c9aa5a-ef3c-ee11-bdf4-0022481b56d1",
                statuscode = 1,
                statecode = 0
            };
            CABEDynamicsBuildingInspectorProfessionalBodyMembership = new DynamicsBuildingInspectorProfessionalBodyMembership
            {
                bsr_name = "Membership: Royal Institution of Chartered Surveyors (CABE)",
                bsr_biprofessionalmembershipid = "84062dd4-797d-ee11-8179-0022481b56d1",
                buidingProfessionApplicationReferenceId = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                contactRefId = "123456789",
                _bsr_buildinginspectorid_value = "123456789",
                professionalBodyRefId = BuildingInspectorProfessionalBodyIds.Ids["CABE"],
                _bsr_professionalbodyid_value = BuildingInspectorProfessionalBodyIds.Ids["CABE"],
                bsr_membershipnumber = "2222",
                bsr_currentmembershiplevelortype = "Member",
                yearRefId = "2",
                _bsr_yearid_value = "50c9aa5a-ef3c-ee11-bdf4-0022481b56d2",
                statuscode = 1,
                statecode = 0
            };
            CIOBDynamicsBuildingInspectorProfessionalBodyMembership = new DynamicsBuildingInspectorProfessionalBodyMembership
            {
                bsr_name = "Membership: Royal Institution of Chartered Surveyors (CIOB)",
                bsr_biprofessionalmembershipid = "d1072ae5-ddc0-ee11-9079-0022481b5210",
                buidingProfessionApplicationReferenceId = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                contactRefId = "123456789",
                _bsr_buildinginspectorid_value = "123456789",
                professionalBodyRefId = BuildingInspectorProfessionalBodyIds.Ids["CIOB"],
                _bsr_professionalbodyid_value = BuildingInspectorProfessionalBodyIds.Ids["CIOB"],
                bsr_membershipnumber = "3333",
                bsr_currentmembershiplevelortype = "Member",
                yearRefId = "3",
                _bsr_yearid_value = "50c9aa5a-ef3c-ee11-bdf4-0022481b56d3",
                statuscode = 1,
                statecode = 0
            };
            OtherDynamicsBuildingInspectorProfessionalBodyMembership = new DynamicsBuildingInspectorProfessionalBodyMembership
            {
                bsr_name = "Membership: Royal Institution of Chartered Surveyors (Other)",
                bsr_biprofessionalmembershipid = "0aeacf13-4fc0-ee11-9079-0022481b5210",
                buidingProfessionApplicationReferenceId = "21ca463a-8988-ee11-be36-0022481b5210",
                _bsr_biapplicationid_value = "21ca463a-8988-ee11-be36-0022481b5210",
                contactRefId = "123456789",
                _bsr_buildinginspectorid_value = "123456789",
                professionalBodyRefId = BuildingInspectorProfessionalBodyIds.Ids["OTHER"],
                _bsr_professionalbodyid_value = BuildingInspectorProfessionalBodyIds.Ids["OTHER"],
                bsr_membershipnumber = null,
                bsr_currentmembershiplevelortype = "Member",
                yearRefId = null,
                _bsr_yearid_value = null,
                statuscode = 1,
                statecode = 0
            };
        }

        public static string GetYear(int year)
        {
            return year switch
            {
                2024 => "50c9aa5a-ef3c-ee11-bdf4-0022481b56d1",
                2023 => "50c9aa5a-ef3c-ee11-bdf4-0022481b56d2",
                2022 => "50c9aa5a-ef3c-ee11-bdf4-0022481b56d3",
                _ => null
            };
        }
    }
}
