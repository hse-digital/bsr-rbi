using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace HSE.RP.API.Models
{
    public class CustomHttpResponseData
    {
        [CosmosDBOutput("hseportal", "regulating-professions", Connection = "CosmosConnection")]
        public object Application { get; set; }
        public HttpResponseData HttpResponse { get; set; }
    }
}
