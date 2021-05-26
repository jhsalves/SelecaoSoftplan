using API2.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace API2.Services
{
    public class RateService : IRateService
    {
        private readonly string _baseAddress;
        private readonly string _endpoint;
        private float _currentRate;

        public RateService(IConfiguration configuration)
        {
            _baseAddress = configuration["BaseApiAddress"].ToString();
            _endpoint = configuration["InterestApiEndpoint"].ToString();
            RetrieveCurrentInterestRate().GetAwaiter();
        }

        public async Task<float> GetCurrentInterestRate()
        {
            if (_currentRate != default)
            {
                return _currentRate;
            }

            await RetrieveCurrentInterestRate();

            return _currentRate;
        }

        private async Task RetrieveCurrentInterestRate()
        {
            using var client = new HttpClient()
            {
                BaseAddress = new Uri(_baseAddress)
            };
            using var response = await client.GetAsync(_endpoint);
            if (!response.IsSuccessStatusCode)
            {
                return;
            }

            var taxaResult = await response.Content.ReadAsStringAsync();
            _currentRate = float.Parse(taxaResult, CultureInfo.CurrentCulture);
        }
    }
}
