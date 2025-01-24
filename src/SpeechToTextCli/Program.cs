using System.CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpeechToTextCli.DependencyInjection;
using SpeechToTextCli.Infrastructure.Logging;
using SpeechToTextProcessor.DependencyInjection;

using var host = CreateHostBuilder(args).Build();
var rootCommand = host.Services.GetRequiredService<RootCommand>();
await rootCommand.InvokeAsync(args).ConfigureAwait(false);
return 0;

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((_, config) => config.AddJsonFile(
            "appsettings.json",
            optional: false,
            reloadOnChange: true))
        .ConfigureLogging((context, logging) =>
        {
            logging.AddConfiguration(context.Configuration.GetSection("Logging"));
            logging.AddConsoleLogger(context.Configuration);
        })
        .ConfigureServices((_, services) =>
        {
            services.AddLogging();
            services.AddSpeechToTextAdapter();
            services.AddInternalDependencies();
        });
}
