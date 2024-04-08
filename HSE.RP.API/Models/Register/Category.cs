using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HSE.RP.API.Models.Register
{
    public class Category
    {
        [JsonPropertyName("categoryName")]

        public required string CategoryName { get; set; }

        [JsonPropertyName("categoryDescription")]
        public required string CategoryDescription { get; set; }
    }
}
