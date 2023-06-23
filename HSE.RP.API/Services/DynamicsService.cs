using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using HSE.RP.API.Models;
using HSEPortal.Domain.DynamicsDefinitions;
using Microsoft.Extensions.Options;

namespace HSE.RP.API.Services
{
    public class DynamicsService
    {
        private readonly DynamicsModelDefinitionFactory dynamicsModelDefinitionFactory;
        private readonly SwaOptions swaOptions;
        private readonly DynamicsApi dynamicsApi;
        private readonly DynamicsOptions dynamicsOptions;
        public DynamicsService(DynamicsModelDefinitionFactory dynamicsModelDefinitionFactory, IOptions<DynamicsOptions> dynamicsOptions, IOptions<SwaOptions> swaOptions, DynamicsApi dynamicsApi)
        {
            this.dynamicsModelDefinitionFactory = dynamicsModelDefinitionFactory;
            this.dynamicsApi = dynamicsApi;
            this.swaOptions = swaOptions.Value;
            this.dynamicsOptions = dynamicsOptions.Value;
        }


    }
}
