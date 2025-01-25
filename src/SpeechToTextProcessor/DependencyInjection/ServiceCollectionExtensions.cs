using Microsoft.Extensions.DependencyInjection;
using SpeechToTextProcessor.Adapters;

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
    public static IServiceCollection AddSpeechToTextAdapter(this IServiceCollection services)
    {
        services.AddTransient<ISpeechToTextAdapter, SpeechToTextAdapter>();

        return services;
    }
}
