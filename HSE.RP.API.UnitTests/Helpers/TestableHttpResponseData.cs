﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;

namespace HSE.RP.API.UnitTests.Helpers
{
    public class TestableHttpResponseData : HttpResponseData
    {
        public TestableHttpResponseData(FunctionContext functionContext) : base(functionContext)
        {
        }
        public override HttpStatusCode StatusCode { get; set; }
        public override HttpHeadersCollection Headers { get; set; } = new();
        public override Stream Body { get; set; } = new MemoryStream();
        public override HttpCookies Cookies { get; }
    }
}
