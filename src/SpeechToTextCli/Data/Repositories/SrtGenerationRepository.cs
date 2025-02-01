using Microsoft.Extensions.Logging;
using SpeechToTextApiClient.Adapter.Adapters;
using SpeechToTextCli.Domain.Repositories;

namespace SpeechToTextCli.Data.Repositories;

internal sealed class SrtGenerationRepository(
    ILogger<SrtGenerationRepository> logger,
    ISpeechToTextAdapter speechToTextAdapter
) : ISrtGenerationRepository
{
    public Task<string> TranscribeAsync(string filePath, string sourceLanguage)
    {
        logger.LogTrace("Transcribing file: {FilePath}, source language: {SourceLanguage}", filePath, sourceLanguage);

        return speechToTextAdapter.TranscribeAsync(filePath, sourceLanguage);
    }

    public Task<string> TranscribeAndTranslateAsync(string filePath, string sourceLanguage, string targetLanguage)
    {
        logger.LogTrace(
            "Transcribing and translating file: {FilePath}, source language: {SourceLanguage}, target language: {TargetLanguage}",
            filePath,
            sourceLanguage,
            targetLanguage
        );

        return speechToTextAdapter.TranscribeAndTranslateAsync(filePath, sourceLanguage, targetLanguage);
    }

    public Task<string> HealthCheckAsync()
    {
        logger.LogTrace("Performing health check...");

        return speechToTextAdapter.HealthCheckAsync();
    }
}
