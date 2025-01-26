using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SpeechToTextProcessor.Adapter.Adapters;

namespace SpeechToTextCli.Application.UseCases;

internal interface IGenerateTranslatedSrtUseCase
{
    Task<int> InvokeAsync(FileInfo? file, string sourceLanguage);
}

internal sealed class GenerateTranslatedSrtUseCase(
    ILogger<GenerateTranslatedSrtUseCase> logger,
    ISpeechToTextAdapter speechToTextAdapter,
    IConfiguration configuration
) : IGenerateTranslatedSrtUseCase
{
    public async Task<int> InvokeAsync(FileInfo? file, string sourceLanguage)
    {
        if (file == null)
        {
            logger.LogError("No file provided.");
            return 1;
        }

        var targetLanguage = configuration.GetValue("Translation:TargetLanguage", string.Empty);

        if (string.IsNullOrWhiteSpace(targetLanguage))
        {
            logger.LogError("No target language provided.");
            return 1;
        }

        var isHealthy = await speechToTextAdapter.HealthCheckAsync().ConfigureAwait(false);

        if (!string.Equals(isHealthy, "OK", StringComparison.OrdinalIgnoreCase))
        {
            logger.LogError("The speech-to-text API is not healthy.");
            return 1;
        }

        logger.LogInformation("Generating translated SRT for file: {FullName}", file.FullName);
        var srtFilePath = await speechToTextAdapter
            .TranscribeAndTranslateAsync(file.FullName, sourceLanguage, targetLanguage)
            .ConfigureAwait(false);
        logger.LogInformation("Translated SRT file generated: {SrtFilePath}", srtFilePath);

        return 0;
    }
}
