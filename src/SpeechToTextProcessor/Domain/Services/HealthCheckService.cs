using Microsoft.Extensions.Logging;
using SpeechToTextProcessor.Domain.Repositories;

namespace SpeechToTextProcessor.Domain.Services;

internal interface IHealthCheckService
{
    Task<string> HealthCheckAsync();
}

internal sealed class HealthCheckService(
    ILogger<HealthCheckService> logger,
    ISpeechToTextRepository speechToTextRepository
) : IHealthCheckService
{
    public async Task<string> HealthCheckAsync()
    {
        logger.LogTrace("Health check invoked from service...");

        try
        {
            return await speechToTextRepository.HealthCheckAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during health check from service...");
            throw;
        }
    }
}
