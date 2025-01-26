namespace SpeechToTextProcessor.Domain.Repositories;

internal interface ISpeechToTextRepository
{
    Task<string> TranscribeAsync(string filePath, string sourceLanguage);
    Task<string> TranscribeAndTranslateAsync(string filePath, string sourceLanguage, string targetLanguage);
    Task<string> HealthCheckAsync();
}
