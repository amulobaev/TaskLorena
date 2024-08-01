using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TaskLorena.Data;

/// <summary>
/// Данный класс используется только для работы с миграциями
/// https://learn.microsoft.com/ru-ru/ef/core/get-started/overview/first-app?tabs=netcore-cli
/// Примеры команд:
/// dotnet ef migrations list
/// dotnet ef migrations add InitialCreate
/// </summary>
public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        var configuration = Helpers.BuildConfiguration();
        optionsBuilder.UseSqlite(configuration.GetConnectionString("DataContext"));

        return new DataContext(optionsBuilder.Options, configuration);
    }
}
