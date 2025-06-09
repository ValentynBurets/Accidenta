using Accidenta.Api.ConfigurationExtentions;
using Accidenta.Api.Logging;
using Accidenta.Api.Middlewares;
using Accidenta.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Accidenta API",
        Version = "v1"
    });
});

builder.Host.UseSerilog((hostContext, services, loggerConfig) =>
{
    var serilogSettings = services.GetService<IOptionsMonitor<SerilogSettings>>();
    if (serilogSettings is null)
    {
        loggerConfig.WriteTo.Console();
        return;
    }

    var configurator = new SerilogConfigurator(serilogSettings);
    var config = configurator.Configure();

    loggerConfig
        .MinimumLevel.Is(LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Sink(config.CreateLogger());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

try
{
    Log.Information("Starting up the application");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup failed");
}
finally
{
    Log.CloseAndFlush();
}
