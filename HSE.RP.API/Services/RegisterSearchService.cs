using HSE.RP.API.Models.Register;
using HSE.RP.API.Models.Search;
using HSE.RP.API.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HSE.RP.API.Services
{

    public interface IRegisterSearchService
    {
        Task<RBIInspectorCompaniesResponse> GetRBICompaniesByNameAndCountry(string company, string country);
        Task<RBIInspectorNamesResponse> GetRBIInspectorsByNameAndCountry(string name, string country);
        Task<string> GetRegisterLastUpdated(string service, string country);
        Task<RBISearchResponse> SearchRBIRegister(string name, string company, string country);

        Task<BuildingProfessionApplication> GetRBIDetails(string applicationNumber);
    }

    public class RegisterSearchService : IRegisterSearchService
    {
        private readonly ICosmosDbService cosmosDbService;

        public RegisterSearchService(ICosmosDbService cosmosDbService)
        {
            this.cosmosDbService = cosmosDbService;
        }

        public async Task<RBIInspectorNamesResponse> GetRBIInspectorsByNameAndCountry(string name, string country)
        {
            var names = new RBIInspectorNamesResponse();
            var nameResponse = await cosmosDbService.GetInspectorsByNameAndCountry(name, country);

            // Check if nameResponse contains applications
            if (nameResponse != null && nameResponse.Any())
            {
                // Get the unique inspector names from the applications
                var inspectorNames = nameResponse.Select(a => a.Applicant.ApplicantName).Distinct().ToList();
                names.Inspectors = inspectorNames;
                names.Results = inspectorNames.Count;
            }
            else
            {
                names.Results = 0;
            }

            return names;

        }

        public async Task<RBIInspectorCompaniesResponse> GetRBICompaniesByNameAndCountry(string company, string country)
        {
            var companies = new RBIInspectorCompaniesResponse();

            var companyResponse = await cosmosDbService.GetInspectorsByCompanyAndCountry(company, country);

            if (companyResponse != null && companyResponse.Any())
            {
                var inspectorNames = companyResponse.Select(a => a.Employer.EmployerName.Trim()).Distinct().ToList();
                companies.Companies = inspectorNames;
                companies.Results = inspectorNames.Count;
            }
            else
            {
                companies.Results = 0;
            }

            return companies;
        }

        public async Task<string> GetRegisterLastUpdated(string service, string country)
        {
            var lastUpdated = await cosmosDbService.GetLastUpdatedByBuildingProfessionTypeAndCountry(service, country);

            if(lastUpdated != null)
            {
                return lastUpdated.CreationDate.ToString("d MMMM yyyy");
            }
            else
            {
                return "Not available";
            }
        }

        public async Task<RBISearchResponse> SearchRBIRegister(string name, string company, string country)
        {
            var searchResult = new RBISearchResponse();

            var searchResponse = await cosmosDbService.SearchRBIRegister(name, company, country);

            if (searchResponse != null && searchResponse.Any())
            {
                searchResult.RBIApplications = searchResponse.OrderBy(a => a.Applicant.ApplicantName).ToList();
                searchResult.Results = searchResponse.Count;
            }
            else
            {
                searchResult.Results = 0;
            }

            return searchResult;
        }



        public async Task<BuildingProfessionApplication> GetRBIDetails(string applicationNumber)
        {
            var rbiDetails = await cosmosDbService.GetApplicationByApplicationNumber<BuildingProfessionApplication>(applicationNumber, "BuildingInspector");

            return rbiDetails;
        }

        public async Task<BuildingProfessionApplication> GetRBCADetails(string applicationNumber)
        {
            var rbcaDetails = await cosmosDbService.GetApplicationByApplicationNumber<BuildingProfessionApplication>(applicationNumber, "BuildingControlApprover");

            return rbcaDetails;
        }
    }

}
