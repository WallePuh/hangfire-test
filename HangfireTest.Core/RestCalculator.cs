using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace HangfireTest.Core
{
    public class RestCalculator:ICalculator
    {
        private readonly IRestClient _restClient;

        public RestCalculator(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public double Add(double d1, double d2)
        {
            var req = new RestRequest("/add", Method.GET);
            req.AddQueryParameter("arg1", d1.ToString(CultureInfo.InvariantCulture));
            req.AddQueryParameter("arg2", d2.ToString(CultureInfo.InvariantCulture));

            var response = _restClient.ExecuteGetTaskAsync<double>(req).Result;
            return response.Data;
        }
    }
}
