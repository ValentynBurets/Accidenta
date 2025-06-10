using Accidenta.Application.Accounts.Commands;
using Accidenta.Application.Accounts.DTO;
using Accidenta.Application.Accounts.Queries;
using Accidenta.Application.Accounts.Validators;
using Accidenta.Application.Common.DTO;
using Accidenta.Application.Common.Mediator;
using Accidenta.Application.Common.Mediator.Dispatchers;
using Accidenta.Application.Contacts.Commands;
using Accidenta.Application.Contacts.DTO;
using Accidenta.Application.Contacts.Queries;
using Accidenta.Application.DTO;
using Accidenta.Application.Incidents.Commands;
using Accidenta.Application.Incidents.DTO;
using Accidenta.Application.Incidents.Queries;
using Accidenta.Application.Incidents.Validation;
using Accidenta.Domain.Interfaces;
using Accidenta.Infrastructure.DataContext;
using Accidenta.Infrastructure.Options;
using Accidenta.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Accidenta.Api.ConfigurationExtentions
{
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

            services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();

            // Account Handlers
            services.AddTransient<ICommandHandler<CreateAccountCommand, Guid>, CreateAccountHandler>();
            services.AddTransient<IQueryHandler<GetAccountByIdQuery, AccountDto>, GetAccountByIdQueryHandler>();
            services.AddTransient<IQueryHandler<GetAccountByNameQuery, AccountDto>, GetAccountByNameQueryHandler>();
            services.AddTransient<IQueryHandler<GetAllAccountsQuery, PagedResult<AccountDto>>, GetAllAccountsQueryHandler>();

            // Contact Handlers
            services.AddTransient<ICommandHandler<CreateContactCommand, Guid>, CreateContactHandler>();
            services.AddTransient<IQueryHandler<GetAllContactsQuery, PagedResult<ContactDto>>, GetAllContactsQueryHandler>();
            services.AddTransient<IQueryHandler<GetContactByEmailQuery, ContactDto>, GetContactByEmailQueryHandler>();
            services.AddTransient<IQueryHandler<GetContactByIdQuery, ContactDto>, GetContactByIdQueryHandler>();

            // Incident Handlers
            services.AddTransient<ICommandHandler<CreateIncidentCommand, Guid>, CreateIncidentCommandHandler>();
            services.AddTransient<IQueryHandler<GetAllIncidentsQuery, PagedResult<IncidentDto>>, GetAllIncidentsQueryHandler>();
            services.AddTransient<IQueryHandler<GetIncidentByIdQuery, IncidentDto>, GetIncidentByIdQueryHandler>();

            #endregion

            return services;
        }
    }
}
