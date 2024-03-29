using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Security.Claims;
using HSE.RP.API.Mappers;
using HSE.RP.API.Models.DynamicsDataExport;
using HSE.RP.API.Models.Enums;
using HSE.RP.API.Models.Register;
using Xunit;

namespace HSE.RP.API.UnitTests.Mappers
{
    public class ApplicationMapperTests
    {
        private readonly IApplicationMapper _applicationMapper;

        public ApplicationMapperTests()
        {
            _applicationMapper = new ApplicationMapper();
        }



        [Fact]
        public void ToRBIApplication_ShouldConvertDynamicsRBIApplicationToRBIApplication()
        {
            // Arrange
            var dynamicsRBIApplication = new DynamicsBuildingProfessionRegisterApplication
            {
                BuildingProfessionApplicationDynamicsId = "123",
                DecisionDate = "2022-01-01",
                ApplicantCountryDetails = new List<ApplicantCountryDetails>
                {

                    new ApplicantCountryDetails
                    {
                        CountryDetailsId = "1",
                        Country = new DynamicsCountry
                        {
                            CountryId = "1",
                            CountryName = "Country 1"
                        }
                    },
                    new ApplicantCountryDetails
                    {
                        CountryDetailsId = "2",
                        Country = new DynamicsCountry
                        {
                            CountryId = "2",
                            CountryName = "Country 2"
                        }
                    },
                },
                ApplicantClassDetails = new List<ApplicantClassDetails>
                {
                    new ApplicantClassDetails
                    {
                        ClassDetailsId = "2",
                        Class = new DynamicsClass
                        {
                            ClassId = "2",
                            ClassName = "Class 2"
                        }
                    },
                    new ApplicantClassDetails
                    {
                        ClassDetailsId = "4",
                        Class = new DynamicsClass
                        {
                            ClassId = "4",
                            ClassName = "Class 4"
                        }
                    },
                },
                ApplicantEmploymentDetails = new List<ApplicantEmploymentDetail>
                {
                    new ApplicantEmploymentDetail
                    {
                        EmploymentDetailId = "1",
                        Employer = new DynamicsEmployer
                        {
                            EmployerAccountId = "1",
                            EmployerName = "Employer 1",
                            EmployerAddress = "Address 1"
                        },
                        EmploymentType = "e5a761f1-0932-ee11-bdf3-0022481b56d1"
                    }
                },
                Applicant = new DynamicsApplicant
                {
                    ApplicantContactId = "1",
                    ApplicantFirstName = "John",
                    ApplicantLastName = "Doe"
                },
                ApplicantActivityDetails = new List<ApplicantActivityDetails>
                {
                    new ApplicantActivityDetails
                    {
                        ApplicantActivityDetailsId = "1",
                        ActivityStatus = BuildingInspectorRegistrationActivityStatus.Registered,
                        Activity = new DynamicsActivity
                        {
                            ActivityId = "1",
                            ActivityName = "Activity 1"
                        },
                        Category = new DynamicsCategory
                        {
                            CategoryId = "1",
                            CategoryName = "Category A"
                        }
                    },
                    new ApplicantActivityDetails
                    {
                        ApplicantActivityDetailsId = "2",
                        ActivityStatus = BuildingInspectorRegistrationActivityStatus.Registered,
                        Activity = new DynamicsActivity
                        {
                            ActivityId = "1",
                            ActivityName = "Activity 1"
                        },
                        Category = new DynamicsCategory
                        {
                            CategoryId = "2",
                            CategoryName = "Category B"
                        }
                    },
                                        new ApplicantActivityDetails
                    {
                        ApplicantActivityDetailsId = "3",
                        ActivityStatus = BuildingInspectorRegistrationActivityStatus.Registered,
                        Activity = new DynamicsActivity
                        {
                            ActivityId = "2",
                            ActivityName = "Activity 2"
                        },
                        Category = new DynamicsCategory
                        {
                            CategoryId = "1",
                            CategoryName = "Category A"
                        }
                    },
                },
                ApplicationId = "123",
                BuildingProfessionType = BuildingProfessionType.BuildingInspector
            };

            // Act
            var result = _applicationMapper.ToRBIApplication(dynamicsRBIApplication);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Id);
            Assert.Equal("123", result.ApplicationNumber);
            Assert.Equal(BuildingProfessionType.BuildingInspector.ToString(), result.BuildingProfessionType);
            Assert.Equal("John Doe", result.Applicant.ApplicantName);
            Assert.NotNull(result.Employer);
            Assert.Equal("Employer 1", result.Employer.EmployerName);
            Assert.Equal("Address 1", result.Employer.EmployerAddress);
            Assert.Equal("Public", result.Employer.EmploymentType);
            Assert.Equal(2, result.Countries.Count);
            Assert.Contains("Country 1", result.Countries);
            Assert.Contains("Country 2", result.Countries);
            Assert.Equal(2, result.Classes.Count);
            Assert.Contains("Class 2", result.Classes);
            Assert.Contains("Class 4", result.Classes);
            Assert.Equal(2, result.Activities.Count);
            Assert.Equal("Activity 1", result.Activities[0].ActivityName);
            Assert.Equal("A", result.Activities[0].Categories[0].CategoryName);
            Assert.Equal("Activity 1", result.Activities[0].ActivityName);
            Assert.Equal("B", result.Activities[0].Categories[1].CategoryName);
            Assert.Equal("Activity 2", result.Activities[1].ActivityName);
            Assert.Equal("A", result.Activities[1].Categories[0].CategoryName);
            Assert.Equal(new DateTime(2022, 1, 1), result.ValidFrom);
            Assert.Equal(new DateTime(2026, 1, 1), result.ValidTo);
            Assert.Equal(DateTime.UtcNow.AddDays(-1).Date, result.CreationDate.Date);
        }
    }
}
