using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using SpeechToTextCli.Domain.ErrorCodes;
using SpeechToTextCli.Domain.Services;
using SpeechToTextProcessor.Domain.Exceptions;

namespace SpeechToTextCli.Application.UseCases;

internal interface IGenerateSrtUseCase
{
    Task<int> InvokeAsync(IFileInfo file, string sourceLanguage);
}

internal sealed class GenerateSrtUseCase(ILogger<GenerateSrtUseCase> logger, ISrtGenerationService srtGenerationService)
    : IGenerateSrtUseCase
{
    public async Task<int> InvokeAsync(IFileInfo file, string sourceLanguage)
    {
        try
        {
            await srtGenerationService.GenerateSrtAsync(file.FullName, sourceLanguage).ConfigureAwait(false);

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
