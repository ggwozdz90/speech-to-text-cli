namespace SpeechToTextProcessor.Adapters;

internal sealed class SpeechToTextAdapter : ISpeechToTextAdapter
{
    public async Task<string> TranscribeAsync(string filePath)
    {
        await Task.Delay(1000).ConfigureAwait(false);
        return "SRT path";
    }

    public async Task<string> TranscribeAndTranslateAsync(string filePath, string targetLanguage)
    {
        await Task.Delay(1000).ConfigureAwait(false);
        return "Translated SRT path";
    }
}
