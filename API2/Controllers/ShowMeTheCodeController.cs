using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace SelecaoSoftplan.API2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ShowMeTheCodeController : ControllerBase
    {
        private readonly string _codeRepo;
        public ShowMeTheCodeController(IConfiguration configuration)
        {
            _codeRepo = configuration["CodeRepo"].ToString();
        }

        [HttpGet]
        public async Task<string> Get()
        {
            return await Task.FromResult(_codeRepo);
        }
    }
}
