using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace SpeechToTextCli.Extensions;

public static class ConsoleLoggerExtensions
{
    public static void AddConsoleLogger(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
    {
        loggingBuilder.AddSimpleConsole(options =>
        {
            options.SingleLine = GetConfigurationValue(configuration, "Logging:Console:SingleLine", true);
            options.IncludeScopes = GetConfigurationValue(configuration, "Logging:Console:IncludeScopes", false);
            options.UseUtcTimestamp = GetConfigurationValue(configuration, "Logging:Console:UseUtcTimestamp", false);
            options.TimestampFormat = GetConfigurationValue(configuration, "Logging:Console:TimestampFormat",
                "yyyy-MM-dd HH:mm:ss fff");
            options.ColorBehavior = GetConfigurationValue(configuration, "Logging:Console:ColorBehavior",
                LoggerColorBehavior.Enabled);
        });
    }

    private static T GetConfigurationValue<T>(IConfiguration configuration, string key, T defaultValue)
    {
        var value = configuration[key];

        if (value == null)
            return defaultValue;

        if (typeof(T).IsEnum)
            return (T)Enum.Parse(typeof(T), value);

        return (T)Convert.ChangeType(value, typeof(T));
    }
}