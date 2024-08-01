using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskLorena.Data;
using TaskLorena.Interfaces;
using TaskLorena.Services;
using TaskLorena.ViewModels;

namespace TaskLorena;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = ConfigureServices();

        InitializeDatabase(services);

        MainWindow = new MainWindow
        {
            DataContext = services.GetRequiredService<MainViewModel>()
        };
        MainWindow.Show();
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddTransient<MainViewModel>();
        services.AddTransient<ICalculationService, CalculationService>();
        services.AddSingleton<IConfiguration>(_ => Helpers.BuildConfiguration());
        services.AddDbContextFactory<DataContext>();

        return services.BuildServiceProvider();
    }

    /// <summary>
    /// Инициализация базы данных
    /// </summary>
    /// <param name="services"></param>
    private static void InitializeDatabase(IServiceProvider services)
    {
        using var dataContext = services.GetRequiredService<IDbContextFactory<DataContext>>().CreateDbContext();

        // Применение миграций
        dataContext.Database.Migrate();

        // Добавление базового набора данных
        if (!dataContext.Salons.Any() && !dataContext.Results.Any())
        {
            var miassEntity = AddSalonEntity(dataContext, "Миасс", 4);
            var ameliyaEntity = AddSalonEntity(dataContext, "Амелия", 5, miassEntity.Id);
            var test1Entity = AddSalonEntity(dataContext, "Тест1", 2, ameliyaEntity.Id);
            var test2Entity = AddSalonEntity(dataContext, "Тест2", 0, miassEntity.Id);
            var kurganEntity = AddSalonEntity(dataContext, "Курган", 11);

            AddResultEntity(dataContext, miassEntity.Id, 57470);
            AddResultEntity(dataContext, ameliyaEntity.Id, 5360);
            AddResultEntity(dataContext, test1Entity.Id, 136540);
            AddResultEntity(dataContext, test2Entity.Id, 54054);
            AddResultEntity(dataContext, kurganEntity.Id, 57850);
        }
    }

    private static SalonEntity AddSalonEntity(DataContext dataContext, string name, double discount = 0,
        int? parentId = null)
    {
        var entity = new SalonEntity
        {
            Name = name,
            Discount = discount,
            ParentId = parentId,
            Dependency = parentId > 0
        };
        dataContext.Salons.Add(entity);
        dataContext.SaveChanges();
        return entity;
    }

    private static ResultEntity AddResultEntity(DataContext dataContext, int salonId, double price)
    {
        var entity = new ResultEntity
        {
            SalonId = salonId,
            Price = price
        };
        dataContext.Results.Add(entity);
        dataContext.SaveChanges();
        return entity;
    }
}
