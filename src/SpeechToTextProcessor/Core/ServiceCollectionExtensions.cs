using Microsoft.Extensions.DependencyInjection;
using SpeechToTextProcessor.Adapters;

namespace SpeechToTextProcessor.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSpeechToTextAdapter(this IServiceCollection services)
    {
        services.AddTransient<ISpeechToTextAdapter, SpeechToTextAdapter>();

        return services;
    }
}