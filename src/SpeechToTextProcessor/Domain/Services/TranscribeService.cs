using Microsoft.Extensions.Logging;
using SpeechToTextProcessor.Domain.Repositories;

namespace SpeechToTextProcessor.Domain.Services;

internal interface ITranscribeService
{
    Task<string> TranscribeAsync(string filePath, string sourceLanguage);
    Task<string> TranscribeAndTranslateAsync(string filePath, string sourceLanguage, string targetLanguage);
}

internal sealed class TranscribeService(
    ILogger<TranscribeService> logger,
    ISpeechToTextRepository speechToTextRepository
) : ITranscribeService
{
    public Task<string> TranscribeAsync(string filePath, string sourceLanguage)
    {
        logger.LogDebug("Transcribing file to text invoked from service...");
        return speechToTextRepository.TranscribeAsync(filePath, sourceLanguage);
    }

    public Task<string> TranscribeAndTranslateAsync(string filePath, string sourceLanguage, string targetLanguage)
    {
        logger.LogDebug("Transcribing and translating file to text invoked from service...");
        return speechToTextRepository.TranscribeAndTranslateAsync(filePath, sourceLanguage, targetLanguage);
    }
}
