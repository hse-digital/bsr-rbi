using AutoMapper;
using HSE.RP.API.Models;

namespace HSE.RP.API.Services.CompanySearch;

public class SocialHousingSearch : ISearchCompany
{
    private readonly DynamicsService dynamicsService;
    private readonly IMapper mapper;

    public SocialHousingSearch(DynamicsService dynamicsService, IMapper mapper)
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