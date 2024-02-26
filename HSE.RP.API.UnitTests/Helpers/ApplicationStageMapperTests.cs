using HSE.RP.API.Enums;
using HSE.RP.API.Mappers;
using HSE.RP.Domain.Entities;
using Xunit;

namespace HSE.RP.API.UnitTests.Helpers
{
    public class ApplicationStageMapperTests
    {
        private readonly IApplicationStageMapper _applicationStageMapper;

        public ApplicationStageMapperTests()
        {
            _applicationStageMapper = new ApplicationStageMapper();
        }

        [Fact]
        public void ToBuildingApplicationStage_ShouldReturnPersonalDetails_WhenApplicationStageIsPersonalDetails()
        {
            // Arrange
            var applicationStage = ApplicationStage.PersonalDetails;

            // Act
            var buildingApplicationStage = _applicationStageMapper.ToBuildingApplicationStage(applicationStage);

            // Assert
            Assert.Equal(BuildingProfessionApplicationStage.PersonalDetails, buildingApplicationStage);
        }

        [Fact]
        public void ToBuildingApplicationStage_ShouldReturnBuildingInspectorClass_WhenApplicationStageIsBuildingInspectorClass()
        {
            // Arrange
            var applicationStage = ApplicationStage.BuildingInspectorClass;

            // Act
            var buildingApplicationStage = _applicationStageMapper.ToBuildingApplicationStage(applicationStage);

            // Assert
            Assert.Equal(BuildingProfessionApplicationStage.BuildingInspectorClass, buildingApplicationStage);
        }

        [Fact]
        public void ToBuildingApplicationStage_ShouldReturnCompetency_WhenApplicationStageIsCompetency()
        {
            // Arrange
            var applicationStage = ApplicationStage.Competency;

            // Act
            var buildingApplicationStage = _applicationStageMapper.ToBuildingApplicationStage(applicationStage);

            // Assert
            Assert.Equal(BuildingProfessionApplicationStage.Competency, buildingApplicationStage);
        }

        [Fact]
        public void ToBuildingApplicationStage_ShouldReturnProfessionalMembershipsAndEmployment_WhenApplicationStageIsProfessionalMembershipsAndEmployment()
        {
            // Arrange
            var applicationStage = ApplicationStage.ProfessionalMembershipsAndEmployment;

            // Act
            var buildingApplicationStage = _applicationStageMapper.ToBuildingApplicationStage(applicationStage);

            // Assert
            Assert.Equal(BuildingProfessionApplicationStage.ProfessionalMembershipsAndEmployment, buildingApplicationStage);
        }

        [Fact]
        public void ToBuildingApplicationStage_ShouldReturnApplicationSummary_WhenApplicationStageIsApplicationSummary()
        {
            // Arrange
            var applicationStage = ApplicationStage.ApplicationSummary;

            // Act
            var buildingApplicationStage = _applicationStageMapper.ToBuildingApplicationStage(applicationStage);

            // Assert
            Assert.Equal(BuildingProfessionApplicationStage.ApplicationSummary, buildingApplicationStage);
        }

        [Fact]
        public void ToBuildingApplicationStage_ShouldReturnPayAndSubmit_WhenApplicationStageIsPayAndSubmit()
        {
            // Arrange
            var applicationStage = ApplicationStage.PayAndSubmit;

            // Act
            var buildingApplicationStage = _applicationStageMapper.ToBuildingApplicationStage(applicationStage);

            // Assert
            Assert.Equal(BuildingProfessionApplicationStage.PayAndSubmit, buildingApplicationStage);
        }

        [Fact]
        public void ToBuildingApplicationStage_ShouldReturnApplicationSubmitted_WhenApplicationStageIsApplicationSubmitted()
        {
            // Arrange
            var applicationStage = ApplicationStage.ApplicationSubmitted;

            // Act
            var buildingApplicationStage = _applicationStageMapper.ToBuildingApplicationStage(applicationStage);

            // Assert
            Assert.Equal(BuildingProfessionApplicationStage.ApplicationSubmitted, buildingApplicationStage);
        }
    }
}
