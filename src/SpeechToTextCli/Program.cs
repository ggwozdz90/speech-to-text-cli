using System.CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpeechToTextCli.Extensions;
using SpeechToTextProcessor.Core;

var host = CreateHostBuilder(args).Build();

var rootCommand = host.Services.GetRequiredService<RootCommand>();
await rootCommand.InvokeAsync(args);
return;

static IHostBuilder CreateHostBuilder(string[] args)
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