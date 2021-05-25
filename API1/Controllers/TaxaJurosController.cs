using Microsoft.AspNetCore.Mvc;

namespace SelecaoSoftplan.API1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TaxaJurosController : ControllerBase
    {
        [HttpGet]
        public string Get() => $"{0.01}";
    }
}
