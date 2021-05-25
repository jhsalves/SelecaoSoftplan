using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API2.Interfaces
{
    public interface IRateService
    {
        Task<float> GetCurrentInterestRate();
    }
}
