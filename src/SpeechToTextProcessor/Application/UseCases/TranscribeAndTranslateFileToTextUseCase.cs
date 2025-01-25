using Microsoft.Extensions.Logging;
using SpeechToTextProcessor.Domain.Services;

namespace SpeechToTextProcessor.Application.UseCases;

internal interface ITranscribeAndTranslateFileToTextUseCase
{
    Task<string> InvokeAsync(string filePath, string targetLanguage);
}

internal sealed class TranscribeAndTranslateFileToTextUseCase(
    ILogger<TranscribeAndTranslateFileToTextUseCase> logger,
    ITranscribeService transcribeService
) : ITranscribeAndTranslateFileToTextUseCase
{
    public async Task<string> InvokeAsync(string filePath, string targetLanguage)
    {
        logger.LogDebug("Transcribing and translating file to text invoked from use case...");

        var result = await transcribeService
            .TranscribeAndTranslateAsync(filePath, targetLanguage)
            .ConfigureAwait(false);

        return result;
    }
}
