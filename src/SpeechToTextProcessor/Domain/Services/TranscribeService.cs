using Microsoft.Extensions.Logging;
using SpeechToTextProcessor.Domain.Repositories;

namespace SpeechToTextProcessor.Domain.Services;

internal interface ITranscribeService
{
    Task<string> TranscribeAsync(string filePath);
    Task<string> TranscribeAndTranslateAsync(string filePath, string targetLanguage);
}

internal sealed class TranscribeService(
    ILogger<TranscribeService> logger,
    ISpeechToTextRepository speechToTextRepository
) : ITranscribeService
{
    public Task<string> TranscribeAsync(string filePath)
    {
        logger.LogDebug("Transcribing file to text invoked from service...");
        return speechToTextRepository.TranscribeAsync(filePath);
    }

    public Task<string> TranscribeAndTranslateAsync(string filePath, string targetLanguage)
    {
        logger.LogDebug("Transcribing and translating file to text invoked from service...");
        return speechToTextRepository.TranscribeAndTranslateAsync(filePath, targetLanguage);
    }
}
