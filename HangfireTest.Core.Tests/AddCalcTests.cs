using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using RestSharp;
using RestSharp.Extensions;

namespace HangfireTest.Core.Tests
{
    public class RestCalculatorTests
    {
        private readonly RestCalculator _sut;
        private readonly Mock<IRestClient> _mockRestClient;

        public RestCalculatorTests()
        {
            _mockRestClient = new Mock<IRestClient>();
            _mockRestClient.Setup(c => c.ExecuteGetTaskAsync<double>(It.IsAny<IRestRequest>()))
                .Returns<IRestRequest>((req) =>
                {
                    var arg1 = double.Parse(req.Parameters[0].Value.ToString());
                    var arg2 = double.Parse(req.Parameters[1].Value.ToString());
                    var response = new RestResponse<double> { Data = arg1 + arg2 } as IRestResponse<double>;
                    return Task.FromResult(response);
                });
            _sut = new RestCalculator(_mockRestClient.Object);
        }

        [TestCase(1.0, 2.0, ExpectedResult = 3.0)]
        [TestCase(10.0, 12.0, ExpectedResult = 22.0)]
        [TestCase(11.0, 62.0, ExpectedResult = 73.0)]
        [TestCase(14.0, 22.0, ExpectedResult = 36.0)]
        [TestCase(16.0, 21.0, ExpectedResult = 37.0)]
        [TestCase(-12.0, 4.0, ExpectedResult = -8.0)]
        public double AddingTwoDoublesShouldReturnCorrectResult(double arg1, double arg2)
        {
            var actual = _sut.Add(arg1, arg2);
            return actual;
        }
    }
}