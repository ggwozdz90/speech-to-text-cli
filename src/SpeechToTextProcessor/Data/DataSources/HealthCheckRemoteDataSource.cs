using Refit;
using SpeechToTextProcessor.Data.DTOs;

namespace SpeechToTextProcessor.Data.DataSources;

internal interface IHealthCheckRemoteDataSource
{
    [Get("/healthcheck")]
    Task<HealthCheckDto> HealthCheckAsync();
}
