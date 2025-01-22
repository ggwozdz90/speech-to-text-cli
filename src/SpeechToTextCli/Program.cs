using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using SpeechToTextCli.Configuration;

var host = HostBuilderFactory.CreateHostBuilder(args).Build();
var rootCommand = host.Services.GetRequiredService<RootCommand>();
await rootCommand.InvokeAsync(args);