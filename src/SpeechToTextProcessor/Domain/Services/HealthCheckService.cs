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
    public Task<string> HealthCheckAsync()
    {
        logger.LogDebug("Health check invoked from service...");
        return speechToTextRepository.HealthCheckAsync();
    }
}
