using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HSE.RP.API.Functions;
using HSE.RP.API.Models.DynamicsDataExport;
using HSE.RP.API.Models.Register;
using HSE.RP.API.Mappers;
using HSE.RP.API.Services;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using HSE.RP.API.Models.Enums;
using Grpc.Core;

namespace HSE.RP.API.UnitTests.Functions
{
    public class ExportFunctionsTests
    {
        private readonly Mock<IDynamicsService> _dynamicsServiceMock;
        private readonly Mock<IApplicationMapper> _applicationMapperMock;
        private readonly Mock<ICosmosDbService> _cosmosDbServiceMock;
        private readonly Mock<IOptions<FeatureOptions>> _featureOptionsOptionsMock;
        private readonly ExportFunctions exportFunctions;

        public ExportFunctionsTests()
        {
            _dynamicsServiceMock = new Mock<IDynamicsService>();
            _applicationMapperMock = new Mock<IApplicationMapper>();
            _cosmosDbServiceMock = new Mock<ICosmosDbService>();

            exportFunctions = new ExportFunctions(
                _dynamicsServiceMock.Object,
                _applicationMapperMock.Object,
                _cosmosDbServiceMock.Object
            );
        }

        /*[Fact]
        public async Task ExportRBIApplicationsToCosmos_ShouldExportApplicationsToCosmosDb()
        {
            // Arrange
            var dynamicsRBIApplications = new List<DynamicsBuildingProfessionRegisterApplication>
            {
                new DynamicsBuildingProfessionRegisterApplication {
                    BuildingProfessionApplicationDynamicsId = "1",
                    ApplicationId = "1",
                    BuildingProfessionType = BuildingProfessionType.BuildingInspector,
                    DecisionDate = "2022-01-01",
                    DecisionCondition = "Condition 1",
                    BuildingProfessionRegulatoryDecisionStatus = BuildingProfessionRegulatoryDecisionStatus.Approved,
                    ReviewDecision = "Decision 1",
                    Applicant = new DynamicsApplicant {
                        ApplicantFirstName = "John",
                        ApplicantLastName = "Doe",
                        ApplicantContactId = "1"
                    },
                    ApplicantEmploymentDetails = new List<ApplicantEmploymentDetail>
                    {
                        new ApplicantEmploymentDetail {
                            EmploymentDetailId = "1",
                            Employer = new DynamicsEmployer
                            {
                                EmployerAccountId = "1",
                                EmployerName = "Employer 1",
                                EmployerAddress = "Address 1"
                            }
                        },
                    },
                    ApplicantClassDetails = new List<ApplicantClassDetails>
                    {
                        new ApplicantClassDetails
                        {
                            ClassDetailsId = "1",
                            Class = new DynamicsClass
                            {
                                ClassId = "1",
                                ClassName = "Class 1"
                            },
                            StatusCode = BuildingInspectorRegistrationClassStatus.Registered
                        },
                    },
                    ApplicantCountryDetails = new List<ApplicantCountryDetails>
                    {
                        new ApplicantCountryDetails
                        {
                            CountryDetailsId = "1",
                            Country = new DynamicsCountry
                            {
                                CountryId = "1",
                                CountryName = "England"
                            }
                        },
                    },
                },
                new DynamicsBuildingProfessionRegisterApplication {
                    BuildingProfessionApplicationDynamicsId = "2",
                    ApplicationId = "2",
                    BuildingProfessionType = BuildingProfessionType.BuildingInspector,
                    DecisionDate = "2023-01-01",
                    DecisionCondition = "Condition 2",
                    BuildingProfessionRegulatoryDecisionStatus = BuildingProfessionRegulatoryDecisionStatus.Approved,
                    ReviewDecision = "Decision 2",
                    Applicant = new DynamicsApplicant {
                        ApplicantFirstName = "Jane",
                        ApplicantLastName = "Doe",
                        ApplicantContactId = "1"
                    },
                    ApplicantEmploymentDetails = new List<ApplicantEmploymentDetail>
                    {
                        new ApplicantEmploymentDetail {
                            EmploymentDetailId = "2",
                            Employer = new DynamicsEmployer
                            {
                                EmployerAccountId = "2",
                                EmployerName = "Employer 2",
                                EmployerAddress = "Address 2"
                            }
                        },
                    },
                    ApplicantClassDetails = new List<ApplicantClassDetails>
                    {
                        new ApplicantClassDetails
                        {
                            ClassDetailsId = "1",
                            Class = new DynamicsClass
                            {
                                ClassId = "1",
                                ClassName = "Class 2"
                            },
                            StatusCode = BuildingInspectorRegistrationClassStatus.Registered
                        },
                    },
                    ApplicantCountryDetails = new List<ApplicantCountryDetails>
                    {
                        new ApplicantCountryDetails
                        {
                            CountryDetailsId = "1",
                            Country = new DynamicsCountry
                            {
                                CountryId = "1",
                                CountryName = "England"
                            }
                        },
                        new ApplicantCountryDetails
                        {
                            CountryDetailsId = "2",
                            Country = new DynamicsCountry
                            {
                                CountryId = "2",
                                CountryName = "Wales"
                            }
                        },
                    },
                    ApplicantActivityDetails = new List<ApplicantActivityDetails>
                    {
                        new ApplicantActivityDetails
                        {
                            ApplicantActivityDetailsId = "1",
                            Activity = new DynamicsActivity
                            {
                                ActivityId = "1",
                                ActivityName = "Assessing Plans"
                            },
                            Category = new DynamicsCategory
                            {
                                CategoryId = "1",
                                CategoryName = "Category A"
                            },
                            ActivityStatus = BuildingInspectorRegistrationActivityStatus.Registered
                        },
                        new ApplicantActivityDetails
                        {
                            ApplicantActivityDetailsId = "2",
                            Activity = new DynamicsActivity
                            {
                                ActivityId = "2",
                                ActivityName = "Assessing Plans"
                            },
                            Category = new DynamicsCategory
                            {
                                CategoryId = "1",
                                CategoryName = "Category B"
                            },
                            ActivityStatus = BuildingInspectorRegistrationActivityStatus.Registered
                        },
                    },
            }};

            var rbiApplications = new List<BuildingProfessionApplication>
            {
                new BuildingProfessionApplication {
                    Id = "1",
                    BuildingProfessionType = BuildingProfessionType.BuildingInspector.ToString(),
                    Applicant = new Applicant
                    {
                        ApplicantName = "John Doe"
                    },
                    Employer = new Employer
                    {
                        EmployerName = "Employer 1",
                        EmployerAddress = "Address 1"
                    },
                    Countries = new List<string> { "England" },
                    Classes = new List<string> { "Class 1" },
                    CreationDate = DateTime.UtcNow.AddDays(-1)
                },
                new BuildingProfessionApplication {
                    Id = "2",
                    BuildingProfessionType = BuildingProfessionType.BuildingInspector.ToString(),
                    Applicant = new Applicant
                    {
                        ApplicantName = "Jane Doe"
                    },
                    Employer = new Employer
                    {
                        EmployerName = "Employer 2",
                        EmployerAddress = "Address 2"
                    },
                    Countries = new List<string> { "England", "Wales" },
                    Classes = new List<string> { "Class 2" },
                    Activities = new List<Activity>
                    {
                        new Activity
                        {
                            ActivityName = "Assessing Plans",
                            Categories = new List<Category> {
                                new Category
                                {
                                    CategoryName = "A",
                                    CategoryDescription = "Residential dwelling houses (single household) less than 7.5 metres in height"
                                },
                                new Category
                                {
                                    CategoryName = "B",
                                    CategoryDescription = "Residential flats and dwelling houses, less than 11 metres in height"
                                }
                            }
                        }
                    },
                    CreationDate = DateTime.UtcNow.AddDays(-1)
                }
            };

            _dynamicsServiceMock.Setup(service => service.GetDynamicsRBIApplications())
                .ReturnsAsync(dynamicsRBIApplications);

            _applicationMapperMock.Setup(mapper => mapper.ToRBIApplication(dynamicsRBIApplications[0]))
                .Returns((DynamicsBuildingProfessionRegisterApplication dynamicsRBIApplication) =>
                {
                    // Map DynamicsRBIApplication to RBIApplication
                    return rbiApplications[0];
                });
            _applicationMapperMock.Setup(mapper => mapper.ToRBIApplication(dynamicsRBIApplications[1]))
                .Returns((DynamicsBuildingProfessionRegisterApplication dynamicsRBIApplication) =>
                {
                    // Map DynamicsRBIApplication to RBIApplication
                    return rbiApplications[1];
                });

            _cosmosDbServiceMock.Setup(service => service.AddItemAsync(It.IsAny<BuildingProfessionApplication>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            _cosmosDbServiceMock.Setup(service => service.RemoveItemsByBuildingProfessionTypeAndCreationDateAsync<BuildingProfessionApplication>(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(Task.CompletedTask);

            // Act
            await exportFunctions.ExportRBIApplicationsToCosmos(null);

            // Assert
            _dynamicsServiceMock.Verify(service => service.GetDynamicsRBIApplications(), Times.Once);
            _applicationMapperMock.Verify(mapper => mapper.ToRBIApplication(It.IsAny<DynamicsBuildingProfessionRegisterApplication>()), Times.Exactly(dynamicsRBIApplications.Count));
            _cosmosDbServiceMock.Verify(service => service.AddItemAsync(It.IsAny<BuildingProfessionApplication>(), It.IsAny<string>()), Times.Exactly(rbiApplications.Count));
            _cosmosDbServiceMock.Verify(service => service.RemoveItemsByBuildingProfessionTypeAndCreationDateAsync<BuildingProfessionApplication>(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);
        }*/


    }
}
