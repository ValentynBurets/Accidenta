using Accidenta.Api.ConfigurationExtentions;
using Accidenta.Api.Logging;
using Accidenta.Api.Middlewares;
using Accidenta.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

builder.Host.UseSerilog();

builder.Services.AddSingleton(Log.Logger);
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

var app = builder.Build();

var serilogSettings = app.Services.GetService<IOptionsMonitor<SerilogSettings>>();
if (serilogSettings is not null)
{
    Log.Logger = new SerilogConfigurator(serilogSettings).Configure().CreateLogger();
    Log.Information("Serilog configured with custom settings.");
}
else
{
    Log.Warning("SerilogSettings not available. Using fallback logger.");
}

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
