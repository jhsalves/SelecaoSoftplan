using SelecaoSoftplan.API1.Controllers;
using System;
using System.Globalization;
using Xunit;

namespace UnitTest
{
    public class TestApi1
    {
        [Fact]
        public void RetrieveCurrentRateShouldSucceed() {
            var controller = new TaxaJurosController();
            var interest = float.Parse(controller.Get(), CultureInfo.CurrentCulture);

            Assert.True((float)0.01 == interest, "O valor da taxa é o esperado.");
        }

        [Fact]
        public void RetrieveCurrentRateShouldFail()
        {
            var controller = new TaxaJurosController();
            var interest = float.Parse(controller.Get(), CultureInfo.CurrentCulture);

            Assert.False((float)0.01 != interest, "O valor da taxa é inesperado.");
        }
    }
}
