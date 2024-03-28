using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE.RP.API.Models.Register
{
    public class Category
    {
        public required string CategoryName { get; set; }
        public required string CategoryDescription { get; set; }
    }
}
