namespace SpeechToTextCli.Domain.Repositories;

internal interface ISrtGenerationRepository
{
    Task<string> TranscribeAsync(string filePath, string sourceLanguage);
    Task<string> TranscribeAndTranslateAsync(string filePath, string sourceLanguage, string targetLanguage);
    Task<string> HealthCheckAsync();
}
