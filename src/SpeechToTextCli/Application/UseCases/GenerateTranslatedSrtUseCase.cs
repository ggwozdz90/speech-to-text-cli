using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using SpeechToTextCli.Domain.ErrorCodes;
using SpeechToTextCli.Domain.Services;
using SpeechToTextProcessor.Domain.Exceptions;

namespace SpeechToTextCli.Application.UseCases;

internal interface IGenerateTranslatedSrtUseCase
{
    Task<int> InvokeAsync(IFileInfo file, string sourceLanguage, string targetLanguage);
}

internal sealed class GenerateTranslatedSrtUseCase(
    ILogger<GenerateTranslatedSrtUseCase> logger,
    ISrtGenerationService translatedSrtGenerationService
) : IGenerateTranslatedSrtUseCase
{
    public async Task<int> InvokeAsync(IFileInfo file, string sourceLanguage, string targetLanguage)
    {
        try
        {
            await translatedSrtGenerationService
                .GenerateTranslatedSrtAsync(file.FullName, sourceLanguage, targetLanguage)
                .ConfigureAwait(false);

            return ErrorCode.Success;
        }
        catch (NetworkException ex)
        {
            logger.LogError(ex, "Network error occurred.");
            return ErrorCode.NetworkError;
        }
        catch (FileAccessException ex)
        {
            logger.LogError(ex, "File access error occurred.");
            return ErrorCode.FileAccessError;
        }
        catch (HealthCheckException ex)
        {
            logger.LogError(ex, "Health check error occurred.");
            return ErrorCode.HealthCheckError;
        }
        catch (TranscribeException ex)
        {
            logger.LogError(ex, "Transcription error occurred.");
            return ErrorCode.TranscribeError;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred.");
            throw;
        }
    }
}
