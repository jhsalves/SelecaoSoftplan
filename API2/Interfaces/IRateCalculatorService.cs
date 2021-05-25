using API2.Model;

namespace API2.Interfaces
{
    public interface IRateCalculatorService
    {
        float Calculate(InterestRateRequest interestRateRequest, float currentInterestRate);
    }
}