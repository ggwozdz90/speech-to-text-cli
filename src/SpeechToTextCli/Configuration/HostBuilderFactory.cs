using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpeechToTextCli.Extensions;
using SpeechToTextProcessor.Core;

namespace SpeechToTextCli.Configuration;

public static class HostBuilderFactory
{
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", false, true);
            })
            .ConfigureLogging((context, logging) =>
            {
                logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                logging.AddConsoleLogger(context.Configuration);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddLogging();
                services.AddSpeechToTextAdapter();
                services.AddCommands();
            });
    }
}