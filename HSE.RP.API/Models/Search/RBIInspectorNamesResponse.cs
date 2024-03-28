using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE.RP.API.Models.Search
{
    public class RBIInspectorNamesResponse
    {
        public RBIInspectorNamesResponse()
        {
            Inspectors = new List<string>();
        }

        public List<string> Inspectors { get; set; }
        public int Results { get; set; }

    }

}
