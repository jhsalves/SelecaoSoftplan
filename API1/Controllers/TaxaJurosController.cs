using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Globalization;

namespace SelecaoSoftplan.API1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TaxaJurosController : ControllerBase
    {
        private readonly string ApiBC;
        public TaxaJurosController(IConfiguration configuration)
        {
            ApiBC = configuration["ApiBancoCentral"].ToString();
        }

        private async Task<IEnumerable<dynamic>> GetTaxasSelicAsync()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xmlapplication/xml");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");
                using (var response = await client.GetAsync(ApiBC))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var ProdutoJsonString = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<IEnumerable<dynamic>>(ProdutoJsonString);
                    }
                    return new List<dynamic>();
                }
            }
        }

        [HttpGet]
        public async Task<string> Get()
        {
            var taxas = await GetTaxasSelicAsync();
            return taxas.OrderByDescending(x => DateTime.ParseExact(
                Convert.ToString(x.data), "dd/MM/yyyy", CultureInfo.InvariantCulture)
                ).Select<dynamic, string>(x => Convert.ToString(x.valor)).FirstOrDefault().Replace(".",",");
        }
    }
}
