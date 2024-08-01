using CommunityToolkit.Mvvm.ComponentModel;

namespace TaskLorena.ViewModels;

public class ItemViewModel : ObservableObject
{
    private string _name = string.Empty;
    private double _price;
    private double _calculatedPrice;

    public int Id { get; set; }

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public double Price
    {
        get => _price;
        set => SetProperty(ref _price, value);
    }

    public double CalculatedPrice
    {
        get => _calculatedPrice;
        set => SetProperty(ref _calculatedPrice, value);
    }

    public double Discount { get; set; }
    
    public double DiscountParent { get; set; }
}
