using HSE.RP.API.Mappers;
using Xunit;

namespace HSE.RP.API.UnitTests.Mappers
{
    public class CategoryMapperTests
    {
        private readonly ICategoryMapper _categoryMapper;

        public CategoryMapperTests()
        {
            _categoryMapper = new CategoryMapper();
        }

        [Fact]
        public void MapCategoryDescription_ShouldReturnResidentialDwellingHouses_WhenCategoryIsA()
        {
            // Arrange
            string category = "A";
            string expectedDescription = "Residential dwelling houses (single household) less than 7.5 metres in height";

            // Act
            string actualDescription = _categoryMapper.MapCategoryDescription(category);

            // Assert
            Assert.Equal(expectedDescription, actualDescription);
        }

        [Fact]
        public void MapCategoryDescription_ShouldReturnResidentialFlatsAndDwellingHousesLessThan11m_WhenCategoryIsB()
        {
            // Arrange
            string category = "B";
            string expectedDescription = "Residential flats and dwelling houses, less than 11 metres in height";

            // Act
            string actualDescription = _categoryMapper.MapCategoryDescription(category);

            // Assert
            Assert.Equal(expectedDescription, actualDescription);
        }

        [Fact]
        public void MapCategoryDescription_ShouldReturnResidentialFlatsAndDwellingHouses11mTo18m_WhenCategoryIsC()
        {
            // Arrange
            string category = "C";
            string expectedDescription = "Residential flats and dwelling houses, 11 metres or more but less than 18 metres in height";

            // Act
            string actualDescription = _categoryMapper.MapCategoryDescription(category);

            // Assert
            Assert.Equal(expectedDescription, actualDescription);
        }

        [Fact]
        public void MapCategoryDescription_ShouldReturnAllBuildingTypesAndUsesLessThan7_5m_WhenCategoryIsD()
        {
            // Arrange
            string category = "D";
            string expectedDescription = "All building types and uses less than 7.5 metres in height";

            // Act
            string actualDescription = _categoryMapper.MapCategoryDescription(category);

            // Assert
            Assert.Equal(expectedDescription, actualDescription);
        }

        [Fact]
        public void MapCategoryDescription_ShouldReturnAllBuildingTypes7_5mTo11m_WhenCategoryIsE()
        {
            // Arrange
            string category = "E";
            string expectedDescription = "All building types 7.5 metres or more but less than 11 metres in height";

            // Act
            string actualDescription = _categoryMapper.MapCategoryDescription(category);

            // Assert
            Assert.Equal(expectedDescription, actualDescription);
        }

        [Fact]
        public void MapCategoryDescription_ShouldReturnAllBuildingTypes11mTo18m_WhenCategoryIsF()
        {
            // Arrange
            string category = "F";
            string expectedDescription = "All building types 11 metres or more but less than 18 metres in height";

            // Act
            string actualDescription = _categoryMapper.MapCategoryDescription(category);

            // Assert
            Assert.Equal(expectedDescription, actualDescription);
        }

        [Fact]
        public void MapCategoryDescription_ShouldReturnAllBuildingTypesIncludingStandardAndNonStandard_WhenCategoryIsG()
        {
            // Arrange
            string category = "G";
            string expectedDescription = "All building types, including standard and non-standard but excluding high-risk, with no height limit";

            // Act
            string actualDescription = _categoryMapper.MapCategoryDescription(category);

            // Assert
            Assert.Equal(expectedDescription, actualDescription);
        }

        [Fact]
        public void MapCategoryDescription_ShouldReturnAllBuildingTypesIncludingHighRisk_WhenCategoryIsH()
        {
            // Arrange
            string category = "H";
            string expectedDescription = "All building types, including high-risk";

            // Act
            string actualDescription = _categoryMapper.MapCategoryDescription(category);

            // Assert
            Assert.Equal(expectedDescription, actualDescription);
        }

        [Fact]
        public void MapCategoryDescription_ShouldReturnCategoryNotFound_WhenCategoryIsInvalid()
        {
            // Arrange
            string category = "X";
            string expectedDescription = "Category not found";

            // Act
            string actualDescription = _categoryMapper.MapCategoryDescription(category);

            // Assert
            Assert.Equal(expectedDescription, actualDescription);
        }
    }
}
