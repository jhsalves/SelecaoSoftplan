using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net;

namespace SelecaoSoftplan.API2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CalculaJurosController : ControllerBase
    {
        private readonly string ApiTaxaJuros;
        public CalculaJurosController(IConfiguration configuration)
        {
            ApiTaxaJuros = configuration["API2"].ToString();
        }
        // GET api/values
        [HttpGet]
        public async Task<string> Get(float valorInicial, float meses)
        {
            var valorTaxa = await ValorTaxa();
            var resultado = valorInicial * Math.Pow(1 + valorTaxa, meses);
            return resultado.ToString();
        }

        private async Task<float> ValorTaxa()
        {
            using (var client = new HttpClient())
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                using (var response = await client.GetAsync(ApiTaxaJuros))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var taxaResult = await response.Content.ReadAsStringAsync();
                        return float.Parse(Convert.ToString(taxaResult).Replace(",", "."));
                    }
                    throw new InvalidProgramException("Não foi possível recuperar o valor da taxa.");
                }
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
