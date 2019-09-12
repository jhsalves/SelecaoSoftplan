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

namespace SelecaoSoftplan.API2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CalculaJurosController : ControllerBase
    {
        private readonly string ApiTaxaJuros;
        public CalculaJurosController(IConfiguration configuration)
        {
            ApiTaxaJuros = configuration["API1"].ToString();
        }
        // GET api/values
        [HttpGet]
        public async Task<string> Get(float valorInicial, float meses)
        {
            float valorTaxa;
            try
            {
                valorTaxa = await ValorTaxa();
            }
            catch
            {
               valorTaxa = 0.01F;
            }
            var resultado = valorInicial * Math.Pow(1 + valorTaxa, meses);
            return String.Format("{0:.##}", resultado);
        }

        private async Task<float> ValorTaxa()
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync(ApiTaxaJuros))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var taxaResult = await response.Content.ReadAsStringAsync();
                            return (float)Convert.ToDouble(taxaResult);
                        }
                        throw new InvalidOperationException("Não foi possível recuperar o valor da taxa junto ao BC.");
                    }
                }
            }
        }
    }
}
