
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
    public interface ICategoryMapper
    {
        /// <summary>
        /// Maps the category description based on the provided activity details.
        /// </summary>
        /// <param name="activityDetails">The activity details.</param>
        /// <returns>The mapped category description.</returns>
        string MapCategoryDescription(string category);
    }

    public class CategoryMapper : ICategoryMapper
    {
        /// <inheritdoc/>
        public string MapCategoryDescription(string category)
        {
            switch (category)
            {
                case "A":
                    return "Residential dwelling houses (single household) less than 7.5 metres in height";
                case "B":
                    return "Residential flats and dwelling houses, less than 11 metres in height";
                case "C":
                    return "Residential flats and dwelling houses, 11 metres or more but less than 18 metres in height";
                case "D":
                    return "All building types and uses less than 7.5 metres in height";
                case "E":
                    return "All building types 7.5 metres or more but less than 11 metres in height";
                case "F":
                    return "All building types 11 metres or more but less than 18 metres in height";
                case "G":
                    return "All building types, including standard and non-standard but excluding high-risk, with no height limit";
                case "H":
                    return "All building types, including high-risk";
                default:
                    return "Category not found";
            }
        }
    }
}


