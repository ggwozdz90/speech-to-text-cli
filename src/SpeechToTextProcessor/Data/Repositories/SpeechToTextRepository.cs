using Microsoft.Extensions.Logging;
using SpeechToTextProcessor.Data.DataSources;
using SpeechToTextProcessor.Domain.Repositories;

namespace SpeechToTextProcessor.Data.Repositories;

internal sealed class SpeechToTextRepository(
    ILogger<SpeechToTextRepository> logger,
    ITranscribeRemoteDataSource transcribeRemoteDataSource,
    IHealthCheckRemoteDataSource healthCheckRemoteDataSource
) : ISpeechToTextRepository
{
    public Task<string> TranscribeAsync(string filePath)
    {
        logger.LogDebug("Transcribing file to text invoked from repository...");
        return transcribeRemoteDataSource.TranscribeAsync(filePath);
    }

    public Task<string> TranscribeAndTranslateAsync(string filePath, string targetLanguage)
    {
        logger.LogDebug("Transcribing and translating file to text invoked from repository...");
        return transcribeRemoteDataSource.TranscribeAndTranslateAsync(filePath, targetLanguage);
    }

    public Task<string> HealthCheckAsync()
    {
        logger.LogDebug("Health check invoked from repository...");
        return healthCheckRemoteDataSource.HealthCheckAsync();
    }
}
