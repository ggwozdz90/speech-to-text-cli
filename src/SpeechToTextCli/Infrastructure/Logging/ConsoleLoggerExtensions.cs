using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace SpeechToTextCli.Infrastructure.Logging;

internal static class ConsoleLoggerExtensions
{
    internal static void AddConsoleLogger(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
    {
        loggingBuilder.AddSimpleConsole(options =>
        {
            options.SingleLine = configuration.GetValue("Logging:Console:SingleLine", defaultValue: true);
            options.IncludeScopes = configuration.GetValue("Logging:Console:IncludeScopes", defaultValue: false);
            options.UseUtcTimestamp = configuration.GetValue("Logging:Console:UseUtcTimestamp", defaultValue: false);
            options.TimestampFormat = configuration.GetValue(
                "Logging:Console:TimestampFormat",
                defaultValue: "yyyy-MM-dd HH:mm:ss fff"
            );
            options.ColorBehavior = configuration.GetValue(
                key: "Logging:Console:ColorBehavior",
                defaultValue: LoggerColorBehavior.Enabled
            );
        });
    }
}
