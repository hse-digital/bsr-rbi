﻿using HSE.RP.API.Models;

namespace HSE.RP.API.Services.CompanySearch;

public class CompanySearchService
{
    private readonly CompanySearchFactory companySearchFactory;

    public CompanySearchService(CompanySearchFactory companySearchFactory)
    {
        this.companySearchFactory = companySearchFactory;
    }

    public async Task<CompanySearchResponse> SearchCompany(string companyType, string company)
    {
        var companySearch = companySearchFactory.GetSearchCompanyInstance(companyType);
        return await companySearch.SearchCompany(company);
    }
}