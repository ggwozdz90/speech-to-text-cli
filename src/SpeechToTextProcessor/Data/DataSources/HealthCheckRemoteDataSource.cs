using Microsoft.Extensions.Logging;

namespace SpeechToTextProcessor.Data.DataSources;

internal interface IHealthCheckRemoteDataSource
{
    Task<string> HealthCheckAsync();
}

internal sealed class HealthCheckRemoteDataSource(ILogger<HealthCheckRemoteDataSource> logger)
    : IHealthCheckRemoteDataSource
{
    public async Task<string> HealthCheckAsync()
    {
        logger.LogDebug("Health check invoked from repository...");

        await Task.Delay(1000).ConfigureAwait(false);

        return "OK";
    }
}
