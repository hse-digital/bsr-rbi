using AutoMapper;
using Microsoft.Extensions.Options;

namespace HSE.RP.API.Services.CompanySearch;

public class CompanySearchFactory
{
    private readonly IMapper mapper;
    private readonly IntegrationsOptions integrationsOptions;
    private readonly IDynamicsService dynamicsService;

    public CompanySearchFactory(IOptions<IntegrationsOptions> integrationsOptions, IDynamicsService dynamicsService, IMapper mapper)
    {
        this.mapper = mapper;
        this.dynamicsService = dynamicsService;
        this.integrationsOptions = integrationsOptions.Value;
    }

    public ISearchCompany GetSearchCompanyInstance(string companyType)
    {
        switch (companyType)
        {
            case "commonhold-association":
            case "management-company":
            case "rmc-or-organisation":
            case "rtm-or-organisation":
            case "other":
                return new CompaniesHouseSearch(integrationsOptions, mapper);
            case "housing-association":
                return new SocialHousingSearch(dynamicsService, mapper);
            case "local-authority":
                return new LocalAuthoritySearch(dynamicsService, mapper);
        }

        return null;
    }
}