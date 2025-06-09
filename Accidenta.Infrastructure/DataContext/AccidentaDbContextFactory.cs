using Accidenta.Infrastructure.DataContext;
using Accidenta.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Accidenta.Infrastructure.Factories;

public class AccidentaDbContextFactory : IDesignTimeDbContextFactory<AccidentaDbContext>
{
    public AccidentaDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../Accidenta.Api");
        var appSettingsPath = Path.Combine(basePath, "appsettings.json");

        if (!File.Exists(appSettingsPath))
        {
            throw new FileNotFoundException($"Configuration file not found at path: {appSettingsPath}");
        }

        var json = File.ReadAllText(appSettingsPath);
        var configuration = JsonDocument.Parse(json);
        var connectionString = configuration
            .RootElement
            .GetProperty("DatabaseSettings")
            .GetProperty("ConnectionString")
            .GetString();

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string is missing or empty in appsettings.json.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<AccidentaDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        var loggerFactory = LoggerFactory.Create(builder => { });
        var logger = loggerFactory.CreateLogger<AccidentaDbContext>();

        var databaseSettings = new DatabaseSettings { ConnectionString = connectionString };

        return new AccidentaDbContext(
            Microsoft.Extensions.Options.Options.Create(databaseSettings),
            logger
        );
    }
}
