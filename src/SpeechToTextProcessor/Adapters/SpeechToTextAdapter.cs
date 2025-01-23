using System.Diagnostics.CodeAnalysis;

namespace SpeechToTextProcessor.Adapters;

[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses",
    Justification = "Instantiated by dependency injection container.")]
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
