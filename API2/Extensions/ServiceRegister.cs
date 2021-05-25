using API2.Interfaces;
using API2.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API2.Extensions
{
    public static class ServiceRegister
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IRateService, RateService>();
            services.AddSingleton<IRateCalculatorService, RateCalculatorService>();
        }
    }
}
