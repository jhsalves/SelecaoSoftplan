using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace SelecaoSoftplan.API1.Libs
{
    public class TaxaSelic
    {
        private readonly string _apiBC;
        public TaxaSelic(string apiBC)
        {
            _apiBC = apiBC;
        }
        public async Task<IEnumerable<dynamic>> GetTaxasSelicAsync()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xmlapplication/xml");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");
                using (var response = await client.GetAsync(_apiBC))
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
    }
}