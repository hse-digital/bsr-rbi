using HSE.RP.API.Models;

namespace HSE.RP.API.Services.CompanySearch;

public interface ISearchCompany
{
    Task<CompanySearchResponse> SearchCompany(string company);
}