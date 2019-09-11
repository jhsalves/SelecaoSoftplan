using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using SelecaoSoftplan.API1.Libs;
using Xunit;

namespace SelecaoSoftplan.TesteUnitario
{
    public class UnitTest1
    {
        public Dictionary<DateTime, string> _juros;
        public UnitTest1()
        {
            var taxaSelic = new TaxaSelic("https://api.bcb.gov.br/dados/serie/bcdata.sgs.11/dados?formato=json");
            var result = taxaSelic.GetTaxasSelicAsync().Result;
            _juros = result.Select(x => new KeyValuePair<DateTime, string>(DateTime.ParseExact(
            Convert.ToString(x.data), "dd/MM/yyyy", CultureInfo.InvariantCulture), Convert.ToString(x.valor).Replace(".", ","))).
            OrderBy(x => x.Key).ToDictionary(x => x.Key, v => v.Value);
        }

        [Fact]
        public void DicionarioSelicFoiPopulado()
        {
            Assert.NotNull(_juros);
        }

        private string GetCalculoTaxaAPI2(float valorInicial, int meses)
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient())
                {
                    using (var response = client.GetAsync($"http://localhost:5002/CalculaJuros?valorInicial={valorInicial}&meses={meses}").Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return response.Content.ReadAsStringAsync().Result;
                        }
                        throw new InvalidOperationException("Não foi possível recuperar o valor da taxa junto ao BC.");
                    }
                }
            }
        }

        [Fact]
        public void TestaApi2()
        {
            Assert.IsType(typeof(string), GetCalculoTaxaAPI2(100, 12));
        }

        [Fact]
        public void CalculoJurosCorreto()
        {
            var juro = (float)Convert.ToDouble(_juros.LastOrDefault().Value);
            var meses = 12;
            var valorInicial = 100;
            var calculo = valorInicial * Math.Pow(1 + juro, meses);
            var resultadoApi = GetCalculoTaxaAPI2(valorInicial, meses);
            var resultado = String.Format("{0:.##}", calculo);
            Assert.Equal(resultado, resultadoApi);
        }

        [Fact]
        public void CalculoJurosNaoCorreto()
        {
            var juro = (float)Convert.ToDouble(_juros.LastOrDefault().Value);
            var meses = 12;
            var valorInicial = 100;
            var calculo = valorInicial * Math.Pow(1 + juro, meses);
            var resultadoApi = GetCalculoTaxaAPI2(101, meses);
            var resultado = String.Format("{0:.##}", calculo);
            Assert.NotEqual(resultado, resultadoApi);
        }
    }
}
