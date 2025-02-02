using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpeechToTextApiClient.DependencyInjection;
using SpeechToTextCli.DependencyInjection;
using SpeechToTextCli.Infrastructure.Logging;

namespace SpeechToTextCli;

/// <summary>
/// The main entry point for the application.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class Program
{
    public static async Task<int> Main(string[] args)
    {
        using var host = CreateHostBuilder(args).Build();
        var rootCommand = host.Services.GetRequiredService<RootCommand>();
        var result = await rootCommand.InvokeAsync(args).ConfigureAwait(false);
        return result;
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(
                (_, config) => config.AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
            )
            .ConfigureLogging(
                (context, logging) =>
                {
                    logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                    logging.AddConsoleLogger(context.Configuration);
                }
            )
            .ConfigureServices(
                (context, services) =>
                {
                    services.AddScoped<IFileSystem, FileSystem>();
                    services.AddLogging();
                    services.AddInternalDependencies();
                    services.AddSpeechToTextProcessor(context.Configuration);
                }
            );
    }
}
