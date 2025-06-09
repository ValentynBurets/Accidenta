using Accidenta.Infrastructure.DataContext;
using Accidenta.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text.Json;

namespace Accidenta.Infrastructure.Factories
{
    /// <summary>
    /// The design-time factory for AccidentaDbContext.
    /// </summary>
    /// <seealso cref="IDesignTimeDbContextFactory{AccidentaDbContext}"/>
    public class AccidentaDbContextFactory : IDesignTimeDbContextFactory<AccidentaDbContext>
    {
        /// <summary>
        /// Creates the AccidentaDbContext for design-time services like migrations.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        /// <returns>The configured AccidentaDbContext.</returns>
        /// <exception cref="FileNotFoundException">Thrown when appsettings.json cannot be found.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the connection string is missing or empty.</exception>
        public AccidentaDbContext CreateDbContext(string[] args)
        {
            // Locate the appsettings.json file in the main project directory
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../Accidenta.Api");
            var appSettingsPath = Path.Combine(basePath, "appsettings.json");

            if (!File.Exists(appSettingsPath))
            {
                throw new FileNotFoundException($"Configuration file not found at path: {appSettingsPath}");
            }

            // Read the connection string from appsettings.json
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

            // Configure DbContext options
            var optionsBuilder = new DbContextOptionsBuilder<AccidentaDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            // Create a logger (optional)
            var loggerFactory = LoggerFactory.Create(builder => { });
            var logger = loggerFactory.CreateLogger<AccidentaDbContext>();

            var databaseSettings = new DatabaseSettings { ConnectionString = connectionString };
            
            return new AccidentaDbContext(
                Microsoft.Extensions.Options.Options.Create(databaseSettings),
                logger
            );
        }
    }
}
