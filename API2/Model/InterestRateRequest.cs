using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;

namespace API2.Model
{
    public class InterestRateRequest
    {
        [FromQuery(Name = "valorInicial")]
        public float InitialValue { get; set; }

        [FromQuery(Name = "meses")]
        public float Months { get; set; }
    }
}
