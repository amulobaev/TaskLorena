using TaskLorena.Interfaces;

namespace TaskLorena.Services;

public class CalculationService : ICalculationService
{
    public double Calculate(double price, double discount, double discountParent)
    {
        return price - (price * ((discount + discountParent) / 100));
    }
}
