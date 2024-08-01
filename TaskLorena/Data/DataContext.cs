using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TaskLorena.Data;

public sealed class DataContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<ResultEntity> Results { get; set; }

    public DbSet<SalonEntity> Salons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSqlite(_configuration.GetConnectionString("DataContext"));
    }
}
