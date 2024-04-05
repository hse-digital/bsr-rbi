
using HSE.RP.API.Models.Dictionarys;
using HSE.RP.API.Models.DynamicsDataExport;
using HSE.RP.API.Models.Register;
using Microsoft.Azure.Cosmos.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE.RP.API.Mappers
{
    public interface IApplicationMapper
    {


        /// <summary>
        /// Converts a DynamicsRBIApplication object to an RBIApplication object.
        /// </summary>
        /// <param name="dynamicsRBIApplication">The DynamicsRBIApplication object to convert.</param>
        /// <returns>The converted RBIApplication object.</returns>
        BuildingProfessionApplication ToRBIApplication(DynamicsBuildingProfessionRegisterApplication dynamicsRBIApplication);
    }

    public class ApplicationMapper : IApplicationMapper
    {

        /// <inheritdoc/>
        public BuildingProfessionApplication ToRBIApplication(DynamicsBuildingProfessionRegisterApplication dynamicsRBIApplication)
        {
            DateTime validFromDate = ParseValidFromDate(dynamicsRBIApplication.DecisionDate);

            var countries = dynamicsRBIApplication.ApplicantCountryDetails?.Select(c => c.Country.CountryName).ToList() ?? new List<string>();

            var classes = dynamicsRBIApplication.ApplicantClassDetails?.Select(c => c.Class.ClassName).ToList() ?? new List<string>();

            var employmentType = EmploymentTypeDict.GetEmploymentType(dynamicsRBIApplication.ApplicantEmploymentDetails?.Select(e => e.EmploymentType).FirstOrDefault() ?? "");

            var employment = new Employer();

            if (employmentType == "Unemployed" || employmentType == string.Empty)
            {
                employment.EmployerName = "None";
                employment.EmployerAddress = "None";
                employment.EmploymentType = "Unemployed";
            }
            else if(employmentType == "Public" || employmentType == "Private")
            {
               employment = dynamicsRBIApplication.ApplicantEmploymentDetails?.Select(e => new Employer
                {
                    EmployerName = e.Employer?.EmployerName ?? "",
                    EmployerAddress = e.Employer?.EmployerAddress ?? "",
                    EmploymentType = employmentType
            }).FirstOrDefault() ?? new Employer { EmployerName = "", EmployerAddress = "", EmploymentType = employmentType };
            }
            else if(employmentType == "Other")
            {
                employment = dynamicsRBIApplication.ApplicantEmploymentDetails?.Select(e => new Employer
                {
                    EmployerName = e.Employer?.EmployerName ?? "",
                    EmployerAddress = e.Employer?.EmployerAddress ?? "",
                    EmploymentType = employmentType
                }).FirstOrDefault() ?? new Employer { EmployerName = "", EmployerAddress = "", EmploymentType = employmentType };

                if (employment.EmployerName == "")
                {
                    employment.EmployerAddress = dynamicsRBIApplication.Applicant.BusinessAddress;
                }

            }


            var applicant = dynamicsRBIApplication.Applicant;

            var firstName = applicant?.ApplicantFirstName ?? "";
            var lastName = applicant?.ApplicantLastName ?? "";

            var activities = new List<Activity>();

            if (dynamicsRBIApplication.ApplicantActivityDetails.Any())
            {
                activities = dynamicsRBIApplication.ApplicantActivityDetails?.Select(a => new Activity
                {
                    ActivityName = a.Activity.ActivityName,
                    Categories = new List<Category>()
                }).GroupBy(a => a.ActivityName).Select(g => g.First()).ToList(); ;

                foreach (var category in dynamicsRBIApplication.ApplicantActivityDetails!)
                {
                    var categoryLetter = category.Category.CategoryName.Split(' ')[1];
                    var categoryDescription = new CategoryMapper().MapCategoryDescription(categoryLetter);
                    var newCategory = new Category
                    {
                        CategoryName = categoryLetter,
                        CategoryDescription = categoryDescription
                    };
                    var activityName = category.Activity.ActivityName;
                    var matchingActivity = activities.FirstOrDefault(a => a.ActivityName == activityName);
                    if (matchingActivity != null)
                    {
                        matchingActivity.Categories.Add(newCategory);
                        matchingActivity.Categories = matchingActivity.Categories.OrderBy(c => c.CategoryName).ToList();
                    }
                }

            }

            return new BuildingProfessionApplication
            {
                Id = dynamicsRBIApplication.ApplicationId,
                BuildingProfessionType = dynamicsRBIApplication.BuildingProfessionType.ToString(),
                Applicant = new Applicant
                {
                    ApplicantName = firstName.Trim() + " " + lastName.Trim(),
                },
                Employer = employment,
                Countries = countries,
                Classes = classes,
                Activities = activities,
                DecisionCondition = dynamicsRBIApplication.BuildingProfessionRegulatoryDecisionStatus == Models.Enums.BuildingProfessionRegulatoryDecisionStatus.Approved ? "None" : dynamicsRBIApplication.DecisionCondition, 
                ValidFrom = validFromDate,
                ValidTo = validFromDate.AddYears(4),
                CreationDate = DateTime.UtcNow.AddDays(-1)
            };
        }

        private DateTime ParseValidFromDate(string decisionDate)
        {
            DateTime validFromDate;
            if (DateTime.TryParse(decisionDate, out validFromDate))
            {
                validFromDate = validFromDate.Date;
            }
            else
            {
                // Handle invalid DecisionDate here
                // For example, you can assign a default value or throw an exception
                validFromDate = DateTime.MinValue;
            }
            return validFromDate;
        }
    }

    
}
