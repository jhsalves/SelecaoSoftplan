using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using API2.Model;
using API2.Interfaces;

namespace SelecaoSoftplan.API2.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CalculaJurosController : ControllerBase
    {
        private readonly IRateService _rateService;
        private readonly IRateCalculatorService _rateCalculator;

        public CalculaJurosController(IRateService rateService, 
                                      IRateCalculatorService interestRateCalculator)
        {
            _rateService = rateService;
            _rateCalculator = interestRateCalculator;
        }

        [HttpGet]
        public async Task<string> Get([FromQuery]InterestRateRequest interestRateRequest)
        {
            var currentInterestRate = await _rateService.GetCurrentInterestRate();
            var resultado = _rateCalculator.Calculate(interestRateRequest, currentInterestRate);
            return $"{resultado:0.##}";
        }
    }
}
