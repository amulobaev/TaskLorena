namespace TaskLorena.Interfaces;

public interface ICalculationService
{
    double Calculate(double price, double discount, double discountParent);
}
