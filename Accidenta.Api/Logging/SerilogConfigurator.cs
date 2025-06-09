using Accidenta.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;

namespace Accidenta.Api.Logging
{
    internal class SerilogConfigurator
    {
        private readonly IOptionsMonitor<SerilogSettings> _settings;

        public SerilogConfigurator(IOptionsMonitor<SerilogSettings> settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings), "Serilog settings monitor cannot be null.");
        }

        public LoggerConfiguration Configure()
        {
            var config = _settings.CurrentValue ?? new SerilogSettings();

            var level = LogEventLevel.Information;
            if (!string.IsNullOrWhiteSpace(config.MinimumLevel))
            {
                if (!Enum.TryParse(config.MinimumLevel, true, out level))
                {
                    level = LogEventLevel.Information;
                }
            }

            var outputTemplate = !string.IsNullOrWhiteSpace(config.OutputTemplate)
                ? config.OutputTemplate
                : "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

            var filePath = !string.IsNullOrWhiteSpace(config.FilePath)
                ? config.FilePath
                : "Logs/log-.txt";

            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Is(level)
                .Enrich.FromLogContext();

            if (config.WriteToConsole)
            {
                loggerConfig = loggerConfig.WriteTo.Console(outputTemplate: outputTemplate);
            }

            if (config.WriteToFile)
            {
                try
                {
                    loggerConfig = loggerConfig.WriteTo.File(
                        path: filePath,
                        outputTemplate: outputTemplate,
                        rollingInterval: RollingInterval.Day);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[SerilogConfigurator] Failed to configure file sink: {ex.Message}");
                }
            }

            return loggerConfig;
        }
    }
}
