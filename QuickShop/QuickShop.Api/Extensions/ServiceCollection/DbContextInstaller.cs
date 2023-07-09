using QuickShop.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace QuickShop.Api.Extensions.ServiceCollection;

public static class DbContextInstaller
{
    private const string DatabaseConnectionStringKey = "QuickShopDatabase";
    
    public static IServiceCollection InstallDbContext(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<QuickShopDbContext>(options =>
        {
            var path = Path.Combine(Environment.CurrentDirectory, "database.db;");
            options.UseSqlite($"Data Source={path};",
                b =>
                {
                    b.MigrationsAssembly(typeof(ApiAssemblyMarker).Assembly.FullName);
                });
        });


        return services;
    }
}