using Accidenta.Domain.Entities;
using Accidenta.Infrastructure.DataContext.TableConfigurations;
using Accidenta.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Accidenta.Infrastructure.DataContext;

public class AccidentaDbContext : DbContext
{
    private readonly string _connectionString;
    private readonly ILogger<AccidentaDbContext> _logger;

    public AccidentaDbContext(IOptions<DatabaseSettings> databaseSettings, ILogger<AccidentaDbContext> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");

        if (databaseSettings == null)
        {
            _logger.LogError("Database settings are null.");
            throw new ArgumentNullException(nameof(databaseSettings), "Database settings cannot be null.");
        }

        if (string.IsNullOrEmpty(databaseSettings.Value.ConnectionString))
        {
            _logger.LogError("ConnectionString is null or empty.");
            throw new ArgumentException("ConnectionString cannot be null or empty.", nameof(databaseSettings));
        }

        _connectionString = databaseSettings.Value.ConnectionString;
        _logger.LogInformation("AccidentaDbContext initialized with connection string.");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (string.IsNullOrEmpty(_connectionString))
        {
            _logger.LogError("The connection string is null or empty.");
            throw new InvalidOperationException("The connection string cannot be null or empty.");
        }

        optionsBuilder.UseSqlServer(_connectionString);
        _logger.LogInformation("Configured to use SQL Server with the provided connection string.");
    }

    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Incident> Incidents => Set<Incident>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.ApplyConfiguration(new AccountTableConfiguration());
        mb.ApplyConfiguration(new ContactTableConfiguration());
        mb.ApplyConfiguration(new IncidentTableConfiguration());

        _logger.LogInformation("Model configuration applied.");
    }
}
