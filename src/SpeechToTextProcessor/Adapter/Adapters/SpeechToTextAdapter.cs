using Microsoft.Extensions.Logging;
using SpeechToTextProcessor.Application.UseCases;
using SpeechToTextProcessor.Domain.Exceptions;

namespace SpeechToTextProcessor.Adapter.Adapters;

/// <summary>
///     Interface for speech-to-text adapter.
/// </summary>
public interface ISpeechToTextAdapter
{
    /// <summary>
    ///     Sends a request to the API to transcribe the audio from the specified file and generates an SRT file with the text.
    /// </summary>
    /// <param name="filePath">The path to the audio file to be transcribed.</param>
    /// <param name="sourceLanguage">The source language of the audio in the pattern xx_XX.</param>
    /// <returns>The task result contains the transcribed text.</returns>
    /// <exception cref="NetworkException">Thrown when an HTTP error occurs during the transcription process.</exception>
    /// <exception cref="TranscribeException">Thrown when an error occurs during the transcription process.</exception>
    /// <exception cref="FileAccessException">Thrown when an error occurs during file access operations.</exception>
    Task<string> TranscribeAsync(string filePath, string sourceLanguage);

    /// <summary>
    ///     Sends a request to the API to transcribe the audio from the specified file and translates the detected text to the specified language.
    ///     The language should be provided in the pattern xx_XX.
    /// </summary>
    /// <param name="filePath">The path to the audio file to be transcribed.</param>
    /// <param name="sourceLanguage">The source language of the audio in the pattern xx_XX.</param>
    /// <param name="targetLanguage">The target language for translation in the pattern xx_XX.</param>
    /// <returns>
    ///     The task result contains the transcribed and translated text.
    /// </returns>
    /// <exception cref="NetworkException">Thrown when an HTTP error occurs during the transcription and translation process.</exception>
    /// <exception cref="TranscribeException">Thrown when an error occurs during the transcription and translation process.</exception>
    /// <exception cref="FileAccessException">Thrown when an error occurs during file access operations.</exception>
    Task<string> TranscribeAndTranslateAsync(string filePath, string sourceLanguage, string targetLanguage);

    /// <summary>
    ///     Checks the health of the API.
    /// </summary>
    /// <returns>The task result contains the health status, which will be "OK" if the API is healthy.</returns>
    /// <exception cref="NetworkException">Thrown when an HTTP error occurs during the health check process.</exception>
    /// <exception cref="HealthCheckException">Thrown when an error occurs during the health check process.</exception>
    Task<string> HealthCheckAsync();
}

internal sealed class SpeechToTextAdapter(
    ILogger<SpeechToTextAdapter> logger,
    ITranscribeFileToTextUseCase transcribeFileToTextUseCase,
    ITranscribeAndTranslateFileToTextUseCase transcribeAndTranslateFileToTextUseCase,
    IHealthCheckUseCase healthCheckUseCase
) : ISpeechToTextAdapter
{
    public async Task<string> TranscribeAsync(string filePath, string sourceLanguage)
    {
        logger.LogInformation(
            "Transcribe invoked with file path: {FilePath} and source language: {SourceLanguage}",
            filePath,
            sourceLanguage
        );

        var result = await transcribeFileToTextUseCase.InvokeAsync(filePath, sourceLanguage).ConfigureAwait(false);

        logger.LogInformation(
            "Transcribe completed with file path: {FilePath} and source language: {SourceLanguage} with result: {Result}",
            filePath,
            sourceLanguage,
            result
        );

        return result;
    }

    public async Task<string> TranscribeAndTranslateAsync(string filePath, string sourceLanguage, string targetLanguage)
    {
        logger.LogInformation(
            "Transcribe and translate invoked with file path: {FilePath}, source language: {SourceLanguage}, and target language: {TargetLanguage}",
            filePath,
            sourceLanguage,
            targetLanguage
        );

        var result = await transcribeAndTranslateFileToTextUseCase
            .InvokeAsync(filePath, sourceLanguage, targetLanguage)
            .ConfigureAwait(false);

        logger.LogInformation(
            "Transcribe and translate completed with file path: {FilePath}, source language: {SourceLanguage}, target language: {TargetLanguage} with result: {Result}",
            filePath,
            sourceLanguage,
            targetLanguage,
            result
        );

        return result;
    }

    public async Task<string> HealthCheckAsync()
    {
        logger.LogInformation("Health check invoked...");

        var result = await healthCheckUseCase.InvokeAsync().ConfigureAwait(false);

        logger.LogInformation("Health check completed with result: {Result}", result);

        return result;
    }
}
