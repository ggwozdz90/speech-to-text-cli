using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SpeechToTextProcessor.Adapters;

namespace SpeechToTextCli.Application.UseCases;

internal interface IGenerateTranslatedSrtUseCase
{
    Task<int> InvokeAsync(FileInfo? file);
}

internal sealed class GenerateTranslatedSrtUseCase(
    ILogger<GenerateTranslatedSrtUseCase> logger,
    ISpeechToTextAdapter speechToTextAdapter,
    IConfiguration configuration
) : IGenerateTranslatedSrtUseCase
{
    public async Task<int> InvokeAsync(FileInfo? file)
    {
        if (file == null)
        {
            logger.LogError("No file provided.");
            return 1;
        }

        var targetLanguage = configuration["Translation:TargetLanguage"];

        if (string.IsNullOrWhiteSpace(targetLanguage))
        {
            logger.LogError("No target language provided.");
            return 1;
        }

        logger.LogInformation("Generating translated SRT for file: {FullName}", file.FullName);
        var srtFilePath = await speechToTextAdapter
            .TranscribeAndTranslateAsync(file.FullName, targetLanguage)
            .ConfigureAwait(false);
        logger.LogInformation("Translated SRT file generated: {SrtFilePath}", srtFilePath);

        return 0;
    }
}
