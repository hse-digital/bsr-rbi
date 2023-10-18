using AutoMapper;
using HSE.RP.API.Models;

namespace HSE.RP.API.Services.CompanySearch;

public class SocialHousingSearch : ISearchCompany
{
    private readonly IDynamicsService dynamicsService;
    private readonly IMapper mapper;

    public SocialHousingSearch(IDynamicsService dynamicsService, IMapper mapper)
    {
        this.dynamicsService = dynamicsService;
        this.mapper = mapper;
    }
    public async Task<CompanySearchResponse> SearchCompany(string company)
    {
        var localAuthorityResponse = await dynamicsService.SearchSocialHousingOrganisations(company);
        return mapper.Map<CompanySearchResponse>(localAuthorityResponse);
    }
}