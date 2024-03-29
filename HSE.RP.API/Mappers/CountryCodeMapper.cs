
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
    public interface ICountryCodeMapper
    {

        string MapCountry(string countryCode);
    }

    public class CountryCodeMapper : ICountryCodeMapper
    {
        /// <inheritdoc/>
        public string MapCountry(string countryCode)
        {
            switch (countryCode)
            {
                case "E":
                    return "England";
                case "W":
                    return "Wales";
                case "S":
                    return "Scotland";
                case "N":
                    return "Northern Ireland";
                case "L":
                    return "Channel Islands";
                case "M":
                    return "Isle of Man";
                case "J":
                    return "Outside of the land boundaries";
                default:
                    return "";
            }
        }
    }
}


