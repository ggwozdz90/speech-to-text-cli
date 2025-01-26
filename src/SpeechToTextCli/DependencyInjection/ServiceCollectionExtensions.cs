using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using SpeechToTextCli.Application.UseCases;
using SpeechToTextCli.Data.Repositories;
using SpeechToTextCli.Domain.Repositories;
using SpeechToTextCli.Domain.Services;
using SpeechToTextCli.Presentation.Commands;

namespace SpeechToTextCli.DependencyInjection;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInternalDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IApplicationCommand, GenerateSrtCommand>();
        services.AddSingleton<IApplicationCommand, GenerateTranslatedSrtCommand>();

        services.AddSingleton<IGenerateSrtUseCase, GenerateSrtUseCase>();
        services.AddSingleton<IGenerateTranslatedSrtUseCase, GenerateTranslatedSrtUseCase>();

        services.AddSingleton<ISrtGenerationService, SrtGenerationService>();

        services.AddSingleton<IFileRepository, FileRepository>();
        services.AddSingleton<ISrtGenerationRepository, SrtGenerationRepository>();

        services.AddSingleton(provider =>
        {
            var rootCommand = new RootCommand("Speech to text API CLI");

            foreach (var command in provider.GetServices<IApplicationCommand>().OfType<Command>())
            {
                rootCommand.AddCommand(command);
            }

            return rootCommand;
        });

        return services;
    }
}
