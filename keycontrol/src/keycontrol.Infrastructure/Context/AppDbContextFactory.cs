using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace keycontrol.Infrastructure.Context;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public static bool IsMigrationContext { get; private set; }
    public AppDbContext CreateDbContext(string[] args)
    {
        IsMigrationContext = true;
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "keycontrol.Api");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.Development.json")
            .Build();
        
        var connectionString = configuration.GetConnectionString("Default");
        var options = new DbContextOptionsBuilder<AppDbContext>();
        options.UseNpgsql(connectionString, p => p.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
        return new AppDbContext(options.Options, configuration);
    }
}