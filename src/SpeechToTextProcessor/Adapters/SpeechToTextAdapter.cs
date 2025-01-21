namespace SpeechToTextProcessor.Adapters;

public interface ISpeechToTextAdapter
{
    Task<string> TranscribeAsync(string filePath);
    Task<string> TranscribeAndTranslateAsync(string filePath, string targetLanguage);
}

public class SpeechToTextAdapter : ISpeechToTextAdapter
{
    public async Task<string> TranscribeAsync(string filePath)
    {
        await Task.Delay(1000);
        return "SRT path";
    }

    public async Task<string> TranscribeAndTranslateAsync(string filePath, string targetLanguage)
    {
        await Task.Delay(1000);
        return "Translated SRT path";
    }
}