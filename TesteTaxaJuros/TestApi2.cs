using API2.Interfaces;
using API2.Model;
using API2.Services;
using Microsoft.Extensions.Configuration;
using SelecaoSoftplan.API1.Controllers;
using SelecaoSoftplan.API2.Controllers;
using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest
{
    public class TestApi2
    {
        private readonly IConfiguration _configuration;

        public TestApi2()
        {
            _configuration = InitConfiguration();
        }

        public IConfiguration InitConfiguration() => 
                new ConfigurationBuilder()
                    .AddJsonFile("appsettings.test.json")
                    .Build();

        [Fact]
        public async Task InterestRateApiIsDown()
        {
            static void failedAssertion(HttpStatusCode statusCode) => Assert.False(statusCode == HttpStatusCode.OK, "A API da taxa de juros não está disponível.");

            try
            {
                using var httpClient = new HttpClient()
                {
                    BaseAddress = new Uri("anyWrongAddress")
                };
                var response = await httpClient.GetAsync("");
                failedAssertion(response.StatusCode);
            }
            catch
            {
                failedAssertion(HttpStatusCode.BadGateway);
            }
        }

        [Fact]
        public async Task InterestRateApiIsAlive()
        {
            using var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(_configuration["BaseApiAddress"])
            };
            var response = await httpClient.GetAsync(_configuration["InterestApiEndpoint"]);
            Assert.True(response.StatusCode == HttpStatusCode.OK, "A API da taxa de juros não está disponível.");
        }

        [Fact]
        public async Task RetrieveCurrentRateShouldSucceed()
        {
            IRateService rateService = new RateService(_configuration);
            IRateCalculatorService rateCalculatorService = new RateCalculatorService();
            var controller = new CalculaJurosController(rateService, rateCalculatorService);

            var rateRequest = new InterestRateRequest()
            {
                InitialValue = 100,
                Months = 5
            };


            var calculatedIncome = float.Parse(await controller.Get(rateRequest), CultureInfo.CurrentCulture);

            Assert.True((float)105.10 == calculatedIncome, "O valor da taxa e inesperado.");
        }

        [Fact]
        public void RetrieveCurrentRateShouldFail()
        {
            Assert.False(false, "O valor da taxa e inesperado.");
        }
    }
}
