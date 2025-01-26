using Refit;

namespace SpeechToTextProcessor.Data.DataSources;

internal interface IHealthCheckRemoteDataSource
{
    [Get("/healthcheck")]
    Task<string> HealthCheckAsync();
}
