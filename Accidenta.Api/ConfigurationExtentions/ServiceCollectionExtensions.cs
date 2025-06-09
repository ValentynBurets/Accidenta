using Accidenta.Application.Accounts.Commands;
using Accidenta.Application.Accounts.Queries;
using Accidenta.Application.Accounts.Validators;
using Accidenta.Application.Contacts.Queries;
using Accidenta.Application.DTO;
using Accidenta.Application.Incidents.Commands;
using Accidenta.Application.Incidents.Queries;
using Accidenta.Application.Incidents.Validation;
using Accidenta.Domain.Interfaces;
using Accidenta.Infrastructure.DataContext;
using Accidenta.Infrastructure.Options;
using Accidenta.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Accidenta.Api.ConfigurationExtentions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        #region Options
        services.Configure<SerilogSettings>(configuration.GetSection("SerilogSettings"));
        services.Configure<DatabaseSettings>(configuration.GetSection("DatabaseSettings"));
        #endregion

        services.AddDbContext<AccidentaDbContext>((serviceProvider, options) =>
        {
            var databaseSettings = serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            options.UseSqlServer(databaseSettings.ConnectionString);
        });

        #region Repositories
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IIncidentRepository, IncidentRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        #endregion

        #region Validators
        services.AddTransient<IValidator<CreateAccountRequest>, CreateAccountRequestValidator>();
        services.AddTransient<IValidator<CreateIncidentRequest>, CreateIncidentRequestValidator>();
        #endregion

        #region MediatR Handlers Registration
        var assembliesToRegister = new[]
        {
            typeof(CreateAccountHandler).Assembly,
            typeof(CreateIncidentHandler).Assembly,
            typeof(GetAccountByIdQueryHandler).Assembly,
            typeof(GetAccountByNameHandler).Assembly,
            typeof(GetAllAccountsQueryHandler).Assembly,
            typeof(GetAllContactsQueryHandler).Assembly,
            typeof(GetContactByEmailHandler).Assembly,
            typeof(GetContactByIdQueryHandler).Assembly,
            typeof(GetAllIncidentsQueryHandler).Assembly,
            typeof(GetIncidentByIdQueryHandler).Assembly,
        };

        foreach (var assembly in assembliesToRegister.Distinct())
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        }
        #endregion

        return services;
    }
}
