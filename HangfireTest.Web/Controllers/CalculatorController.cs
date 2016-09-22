using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using Hangfire.Storage.Monitoring;

namespace HangfireTest.Web.Controllers
{
    [RoutePrefix("api/calc")]
    public class CalculatorController : ApiController
    {
        [Route("add/{arg1:double}/{arg2:double}")]
        [HttpGet]
        public double Add(double arg1,double arg2)
        {
            Thread.Sleep(TimeSpan.FromSeconds(30));
            return arg1 + arg2;
        }
    }
}
