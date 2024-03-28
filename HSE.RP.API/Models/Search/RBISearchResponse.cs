using HSE.RP.API.Models.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HSE.RP.API.Models.Search
{
    public class RBISearchResponse
    {
        public RBISearchResponse()
        {
            RBIApplications = new List<BuildingProfessionApplication>();
        }

        public List<BuildingProfessionApplication> RBIApplications { get; set; }
        public int Results { get; set; }

    }

}
