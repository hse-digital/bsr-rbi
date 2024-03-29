using HSE.RP.API.Models.Dictionarys;
using Xunit;

namespace HSE.RP.API.Tests.Mappers
{
    public class EmploymentTypeTests
    {
        [Fact]
        public void GetEmploymentType_PublicKey_ReturnsPublic()
        {
            // Arrange
            string key = "e5a761f1-0932-ee11-bdf3-0022481b56d1";

            // Act
            string employmentType = EmploymentTypeDict.GetEmploymentType(key);

            // Assert
            Assert.Equal("Public", employmentType);
        }

        [Fact]
        public void GetEmploymentType_PrivateKey_ReturnsPrivate()
        {
            // Arrange
            string key = "f6d565f7-0932-ee11-bdf3-0022481b56d1";

            // Act
            string employmentType = EmploymentTypeDict.GetEmploymentType(key);

            // Assert
            Assert.Equal("Private", employmentType);
        }

        [Fact]
        public void GetEmploymentType_OtherKey_ReturnsOther()
        {
            // Arrange
            string key = "05d665f7-0932-ee11-bdf3-0022481b56d1";

            // Act
            string employmentType = EmploymentTypeDict.GetEmploymentType(key);

            // Assert
            Assert.Equal("Other", employmentType);
        }

        [Fact]
        public void GetEmploymentType_UnemployedKey_ReturnsUnemployed()
        {
            // Arrange
            string key = "6a3f65fd-0932-ee11-bdf3-0022481b56d1";

            // Act
            string employmentType = EmploymentTypeDict.GetEmploymentType(key);

            // Assert
            Assert.Equal("Unemployed", employmentType);
        }

        [Fact]
        public void GetEmploymentType_Unknown_ReturnsEmptyString()
        {
            // Arrange
            string key = "123";

            // Act
            string employmentType = EmploymentTypeDict.GetEmploymentType(key);

            // Assert
            Assert.Equal(string.Empty, employmentType);
        }
    }
}
