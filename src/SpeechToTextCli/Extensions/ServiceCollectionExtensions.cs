using System.CommandLine;
using System.CommandLine.Invocation;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SpeechToTextCli.Commands;

namespace SpeechToTextCli.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddSingleton<ICommandHandler, GenerateSrtCommand>();
        services.AddSingleton<ICommandHandler, GenerateTranslatedSrtCommand>();

        services.AddSingleton<RootCommand>(provider =>
        {
            var rootCommand = new RootCommand("Speech to text API CLI");

            foreach (var commandHandler in provider.GetServices<ICommandHandler>())
            {
                var commandProperty = commandHandler.GetType()
                    .GetProperty("Command", BindingFlags.Public | BindingFlags.Instance);

                if (commandProperty != null)
                {
                    var command = commandProperty.GetValue(commandHandler);
                    if (command is Command c) rootCommand.AddCommand(c);
                }
            }

            return rootCommand;
        });

        return services;
    }
}