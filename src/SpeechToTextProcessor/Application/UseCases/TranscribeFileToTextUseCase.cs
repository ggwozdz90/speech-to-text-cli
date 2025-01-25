using Microsoft.Extensions.Logging;
using SpeechToTextProcessor.Domain.Services;

namespace SpeechToTextProcessor.Application.UseCases;

internal interface ITranscribeFileToTextUseCase
{
    Task<string> InvokeAsync(string filePath);
}

internal sealed class TranscribeFileToTextUseCase(
    ILogger<TranscribeFileToTextUseCase> logger,
    ITranscribeService transcribeService
) : ITranscribeFileToTextUseCase
{
    public async Task<string> InvokeAsync(string filePath)
    {
        logger.LogDebug("Transcribing file to text invoked from use case...");

        var result = await transcribeService.TranscribeAsync(filePath).ConfigureAwait(false);

        return result;
    }
}
