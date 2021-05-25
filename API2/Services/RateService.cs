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
        private readonly string _interestApiAddress;
        private float _currentRate;

        public RateService(IConfiguration configuration)
        {
            _interestApiAddress = configuration["interestApiAddress"].ToString();
            RetrieveCurrentInterestRate().GetAwaiter().GetResult();
        }

        public async Task<float> GetCurrentInterestRate()
        {
            if(_currentRate != default)
            {
                return _currentRate;
            }

            await RetrieveCurrentInterestRate();

            return _currentRate;
        }

        private async Task RetrieveCurrentInterestRate()
        {
            using var client = new HttpClient();
            using var response = await client.GetAsync(_interestApiAddress);
            if (response.IsSuccessStatusCode)
            {
                var taxaResult = await response.Content.ReadAsStringAsync();
                _currentRate = float.Parse(taxaResult, CultureInfo.CurrentCulture);
            }
        }
    }
}
