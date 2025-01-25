using Microsoft.Extensions.Logging;

namespace SpeechToTextProcessor.Data.DataSources;

internal interface ITranscribeRemoteDataSource
{
    Task<string> TranscribeAsync(string filePath);
    Task<string> TranscribeAndTranslateAsync(string filePath, string targetLanguage);
}

internal sealed class TranscribeRemoteDataSource(ILogger<TranscribeRemoteDataSource> logger)
    : ITranscribeRemoteDataSource
{
    public async Task<string> TranscribeAsync(string filePath)
    {
        logger.LogDebug("Transcribing file to text invoked from data source...");

        await Task.Delay(1000).ConfigureAwait(false);

        return "Transcribed text from file";
    }

    public async Task<string> TranscribeAndTranslateAsync(string filePath, string targetLanguage)
    {
        logger.LogDebug("Transcribing and translating file to text invoked from data source...");

        await Task.Delay(1000).ConfigureAwait(false);

        return "Transcribed and translated text from file";
    }
}
