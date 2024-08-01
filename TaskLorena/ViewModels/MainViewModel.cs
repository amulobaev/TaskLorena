using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using TaskLorena.Data;
using TaskLorena.Interfaces;

namespace TaskLorena.ViewModels;

public class MainViewModel : ObservableObject
{
    private readonly IDbContextFactory<DataContext> _dataContextFactory;
    private readonly ICalculationService _calculationService;
    private ICommand? _calculateCommand;

    public MainViewModel(IDbContextFactory<DataContext> dataContextFactory, ICalculationService calculationService)
    {
        _dataContextFactory = dataContextFactory;
        _calculationService = calculationService;

        Refresh();
    }

    public ICommand CalculateCommand => _calculateCommand ??= new AsyncRelayCommand(Calculate);

    public ObservableCollection<ItemViewModel> Items { get; } = [];

    void Refresh()
    {
        Items.Clear();

        using var dataContext = _dataContextFactory.CreateDbContext();

        var salonEntities = dataContext.Salons.OrderBy(x => x.Id).ToList();
        var resultEntities = dataContext.Results.ToList();

        foreach (var salonEntity in salonEntities)
        {
            var item = new ItemViewModel
            {
                Id = salonEntity.Id,
                Name = salonEntity.Name,
                Price = resultEntities.Where(x => x.SalonId == salonEntity.Id).Select(x => x.Price).FirstOrDefault(),
                Discount = salonEntity.Discount,
                //DiscountParent = salonEntity.Parent?.Discount ?? 0,
            };

            Items.Add(item);
        }
    }

    private async Task Calculate()
    {
        await using var dataContext = await _dataContextFactory.CreateDbContextAsync();

        foreach (var item in Items)
        {
            item.CalculatedPrice = _calculationService.Calculate(item.Price, item.Discount, item.DiscountParent);
        }
    }
}
