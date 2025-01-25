namespace SpeechToTextProcessor.Adapters;

/// <summary>
///     Interface for speech-to-text adapter.
/// </summary>
public interface ISpeechToTextAdapter
{
    /// <summary>
    ///     Transcribes the audio from the specified file and generates an SRT file with the text.
    /// </summary>
    /// <param name="filePath">The path to the audio file to be transcribed.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the transcribed text.</returns>
    Task<string> TranscribeAsync(string filePath);

    /// <summary>
    ///     Transcribes the audio from the specified file and translates the detected text to the specified language.
    ///     The language should be provided in the pattern xx_XX.
    /// </summary>
    /// <param name="filePath">The path to the audio file to be transcribed.</param>
    /// <param name="targetLanguage">The target language for translation in the pattern xx_XX.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the transcribed and translated
    ///     text.
    /// </returns>
    Task<string> TranscribeAndTranslateAsync(string filePath, string targetLanguage);
}
