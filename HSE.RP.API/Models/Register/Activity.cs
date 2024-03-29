using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE.RP.API.Models.Register
{
    public class Activity
    {
        public required string ActivityName { get; set; }

        public required List<Category> Categories { get; set; }
    }
}
