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
        logger.LogTrace(
            "Transcribing file {FilePath} from {SourceLanguage} invoked from use case...",
            filePath,
            sourceLanguage
        );

        var result = await transcribeService.TranscribeAsync(filePath, sourceLanguage).ConfigureAwait(false);

        return result;
    }
}
