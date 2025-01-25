using Microsoft.Extensions.DependencyInjection;
using SpeechToTextProcessor.Adapter.Adapters;
using SpeechToTextProcessor.Application.UseCases;
using SpeechToTextProcessor.Data.DataSources;
using SpeechToTextProcessor.Data.Repositories;
using SpeechToTextProcessor.Domain.Repositories;
using SpeechToTextProcessor.Domain.Services;

namespace SpeechToTextProcessor.DependencyInjection;

/// <summary>
///     This class is responsible for registering internal services in the external container in the application that will
///     use this library.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds the SpeechToTextAdapter service to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the service to.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddSpeechToTextProcessor(this IServiceCollection services)
    {
        services.AddTransient<ITranscribeRemoteDataSource, TranscribeRemoteDataSource>();
        services.AddTransient<IHealthCheckRemoteDataSource, HealthCheckRemoteDataSource>();
        services.AddTransient<ISpeechToTextRepository, SpeechToTextRepository>();
        services.AddTransient<ITranscribeService, TranscribeService>();
        services.AddTransient<IHealthCheckService, HealthCheckService>();
        services.AddTransient<ITranscribeFileToTextUseCase, TranscribeFileToTextUseCase>();
        services.AddTransient<ITranscribeAndTranslateFileToTextUseCase, TranscribeAndTranslateFileToTextUseCase>();
        services.AddTransient<IHealthCheckUseCase, HealthCheckUseCase>();
        services.AddTransient<ISpeechToTextAdapter, SpeechToTextAdapter>();

        return services;
    }
}
