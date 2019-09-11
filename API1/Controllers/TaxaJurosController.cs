using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Globalization;
using SelecaoSoftplan.API1.Libs;

namespace SelecaoSoftplan.API1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TaxaJurosController : ControllerBase
    {
        private readonly string _apiBC;
        public TaxaJurosController(IConfiguration configuration)
        {
            _apiBC = configuration["ApiBancoCentral"].ToString();
        }

        [HttpGet]
        public async Task<string> Get()
        {
            var taxaSelic = new TaxaSelic(_apiBC);
            var taxas = await taxaSelic.GetTaxasSelicAsync();
            return taxas.OrderByDescending(x => DateTime.ParseExact(
                Convert.ToString(x.data), "dd/MM/yyyy", CultureInfo.InvariantCulture)
                ).Select<dynamic, string>(x => Convert.ToString(x.valor)).FirstOrDefault().Replace(".",",");
        }
    }
}
