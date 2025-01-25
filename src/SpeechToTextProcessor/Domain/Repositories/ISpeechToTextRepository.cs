namespace SpeechToTextProcessor.Domain.Repositories;

internal interface ISpeechToTextRepository
{
    Task<string> TranscribeAsync(string filePath);
    Task<string> TranscribeAndTranslateAsync(string filePath, string targetLanguage);
    Task<string> HealthCheckAsync();
}
