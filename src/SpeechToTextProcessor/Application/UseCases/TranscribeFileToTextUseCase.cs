using Microsoft.Extensions.Logging;
using SpeechToTextProcessor.Domain.Services;

namespace SpeechToTextProcessor.Application.UseCases;

internal interface ITranscribeFileToTextUseCase
{
    Task<string> InvokeAsync(string filePath, string sourceLanguage);
}

internal sealed class TranscribeFileToTextUseCase(
    ILogger<TranscribeFileToTextUseCase> logger,
    ITranscribeService transcribeService
) : ITranscribeFileToTextUseCase
{
    public async Task<string> InvokeAsync(string filePath, string sourceLanguage)
    {
        logger.LogDebug("Transcribing file to text invoked from use case...");

        var result = await transcribeService.TranscribeAsync(filePath, sourceLanguage).ConfigureAwait(false);

        return result;
    }
}
