using Microsoft.Extensions.Logging;
using SpeechToTextCli.Domain.Repositories;

namespace SpeechToTextCli.Domain.Services;

internal interface ISrtGenerationService
{
    Task GenerateSrtAsync(string filePath, string sourceLanguage);
    Task GenerateTranslatedSrtAsync(string filePath, string sourceLanguage, string targetLanguage);
}

internal sealed class SrtGenerationService(
    ILogger<SrtGenerationService> logger,
    ISrtGenerationRepository srtGenerationRepository,
    IFileRepository fileRepository
) : ISrtGenerationService
{
    public async Task GenerateSrtAsync(string filePath, string sourceLanguage)
    {
        logger.LogTrace("Generating SRT for file: {FilePath}", filePath);

        var isHealthy = await srtGenerationRepository.HealthCheckAsync().ConfigureAwait(false);

        if (!string.Equals(isHealthy, "OK", StringComparison.OrdinalIgnoreCase))
        {
            logger.LogError("The speech-to-text API is not healthy.");
            return;
        }

        var srtContent = await srtGenerationRepository.TranscribeAsync(filePath, sourceLanguage).ConfigureAwait(false);
        var srtFilePath = fileRepository.GetFilePathWithSrtExtension(filePath);
        await fileRepository.WriteAllTextAsync(srtFilePath, srtContent).ConfigureAwait(false);

        logger.LogInformation("SRT file generated: {SrtFilePath}", srtFilePath);
    }

    public async Task GenerateTranslatedSrtAsync(string filePath, string sourceLanguage, string targetLanguage)
    {
        logger.LogTrace("Generating translated SRT for file: {FilePath}", filePath);

        var isHealthy = await srtGenerationRepository.HealthCheckAsync().ConfigureAwait(false);

        if (!string.Equals(isHealthy, "OK", StringComparison.OrdinalIgnoreCase))
        {
            logger.LogError("The speech-to-text API is not healthy.");
            return;
        }

        var srtContent = await srtGenerationRepository
            .TranscribeAndTranslateAsync(filePath, sourceLanguage, targetLanguage)
            .ConfigureAwait(false);
        var srtFilePath = fileRepository.GetFilePathWithSrtExtension(filePath);
        await fileRepository.WriteAllTextAsync(srtFilePath, srtContent).ConfigureAwait(false);

        logger.LogInformation("Translated SRT file generated: {SrtFilePath}", srtFilePath);
    }
}
