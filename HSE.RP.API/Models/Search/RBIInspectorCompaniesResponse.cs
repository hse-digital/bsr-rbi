using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE.RP.API.Models.Search
{
    public class RBIInspectorCompaniesResponse
    {
        public RBIInspectorCompaniesResponse()
        {
            Companies = new List<string>();
        }

        public List<string> Companies { get; set; }
        public int Results { get; set; }
    }
}
