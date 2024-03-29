﻿using API2.Interfaces;
using API2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API2.Services
{
    public class RateCalculatorService : IRateCalculatorService
    {
        public float Calculate(InterestRateRequest interestRateRequest, float currentInterestRate) =>
            interestRateRequest.InitialValue * (float)Math.Pow(1 + currentInterestRate, interestRateRequest.Months);
    }
}
