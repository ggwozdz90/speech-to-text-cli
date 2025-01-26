using Refit;

namespace SpeechToTextProcessor.Data.DataSources;

internal interface ITranscribeRemoteDataSource
{
    [Multipart]
    [Post("/transcribe/srt")]
    Task<string> TranscribeAsync(
        [AliasAs("file")] StreamPart file,
        [AliasAs("source_language")] string sourceLanguage,
        [AliasAs("transcription_parameters")] string? transcriptionParameters = null
    );

    [Multipart]
    [Post("/transcribe/srt")]
    Task<string> TranscribeAndTranslateAsync(
        [AliasAs("file")] StreamPart file,
        [AliasAs("source_language")] string sourceLanguage,
        [AliasAs("target_language")] string? targetLanguage = null,
        [AliasAs("transcription_parameters")] string? transcriptionParameters = null,
        [AliasAs("translation_parameters")] string? translationParameters = null
    );
}
