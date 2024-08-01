using Microsoft.Extensions.Configuration;

namespace TaskLorena;

public static class Helpers
{
    public static IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    }
}
