using System;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Web.Http;

namespace HangfireTest.Worker.Console
{
    [RoutePrefix("api/calc")]
    public class CalculatorController : ApiController
    {
        [Route("add/{arg1:double}/{arg2:double}")]
        [HttpGet]
        public double Add(double arg1, double arg2)
        {
            if (Sleep)
            {
                Thread.Sleep(GetSleepTime());
            }

            return arg1 + arg2;
        }

        private int GetSleepTime()
        {
            int sleepSeconds;

            int.TryParse(ConfigurationManager.AppSettings["sleep"], out sleepSeconds);

            if (sleepSeconds < 0)
            {
                sleepSeconds = new Random().Next(0, 30);
            }

            return sleepSeconds;
        }

        public bool Sleep => 
             GetSleepTime() != 0;
    }
}