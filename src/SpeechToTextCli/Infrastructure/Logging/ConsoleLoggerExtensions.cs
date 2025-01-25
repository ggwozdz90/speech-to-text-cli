using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace SpeechToTextCli.Infrastructure.Logging;

internal static class ConsoleLoggerExtensions
{
    public static void AddConsoleLogger(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
    {
        loggingBuilder.AddSimpleConsole(options =>
        {
            options.SingleLine = GetConfigurationValue(
                configuration,
                key: "Logging:Console:SingleLine",
                defaultValue: true
            );
            options.IncludeScopes = GetConfigurationValue(
                configuration,
                key: "Logging:Console:IncludeScopes",
                defaultValue: false
            );
            options.UseUtcTimestamp = GetConfigurationValue(
                configuration,
                key: "Logging:Console:UseUtcTimestamp",
                defaultValue: false
            );
            options.TimestampFormat = GetConfigurationValue(
                configuration,
                key: "Logging:Console:TimestampFormat",
                defaultValue: "yyyy-MM-dd HH:mm:ss fff"
            );
            options.ColorBehavior = GetConfigurationValue(
                configuration,
                key: "Logging:Console:ColorBehavior",
                defaultValue: LoggerColorBehavior.Enabled
            );
        });
    }

    private static T GetConfigurationValue<T>(IConfiguration configuration, string key, T defaultValue)
    {
        var value = configuration[key];

        if (value == null)
        {
            return defaultValue;
        }

        if (typeof(T).IsEnum)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
    }
}
