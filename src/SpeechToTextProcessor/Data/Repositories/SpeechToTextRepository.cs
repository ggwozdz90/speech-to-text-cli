using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using Refit;
using SpeechToTextProcessor.Data.DataSources;
using SpeechToTextProcessor.Domain.Exceptions;
using SpeechToTextProcessor.Domain.Repositories;

namespace SpeechToTextProcessor.Data.Repositories;

internal sealed class SpeechToTextRepository(
    ILogger<SpeechToTextRepository> logger,
    IFileAccessLocalDataSource fileAccessLocalDataSource,
    ITranscribeRemoteDataSource transcribeRemoteDataSource,
    IHealthCheckRemoteDataSource healthCheckRemoteDataSource
) : ISpeechToTextRepository
{
    public async Task<string> TranscribeAsync(string filePath, string sourceLanguage)
    {
        logger.LogTrace(
            "Transcribing file {FilePath} from {SourceLanguage} invoked from repository...",
            filePath,
            sourceLanguage
        );

        FileSystemStream? fileStream = null;

        try
        {
            var fileName = fileAccessLocalDataSource.GetFileName(filePath);
            fileStream = fileAccessLocalDataSource.GetFileSystemStream(filePath);
            var streamPart = new StreamPart(fileStream, fileName);

            var result = await transcribeRemoteDataSource
                .TranscribeAsync(streamPart, sourceLanguage)
                .ConfigureAwait(false);

            return result;
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(
                httpEx,
                "HTTP error occurred while transcribing the file {FilePath} from {SourceLanguage} from repository...",
                filePath,
                sourceLanguage
            );
            throw new NetworkException(httpEx);
        }
        catch (ApiException apiEx)
        {
            logger.LogError(
                apiEx,
                "API error occurred while transcribing the file {FilePath} from {SourceLanguage} from repository...",
                filePath,
                sourceLanguage
            );
            throw new TranscribeException(apiEx);
        }
        finally
        {
            if (fileStream != null)
            {
                await fileStream.DisposeAsync().ConfigureAwait(false);
            }
        }
    }

    public async Task<string> TranscribeAndTranslateAsync(string filePath, string sourceLanguage, string targetLanguage)
    {
        logger.LogTrace(
            "Transcribing and translating file {FilePath} from {SourceLanguage} to {TargetLanguage} invoked from repository...",
            filePath,
            sourceLanguage,
            targetLanguage
        );

        FileSystemStream? fileStream = null;

        try
        {
            var fileName = fileAccessLocalDataSource.GetFileName(filePath);
            fileStream = fileAccessLocalDataSource.GetFileSystemStream(filePath);
            var streamPart = new StreamPart(fileStream, fileName);

            var result = await transcribeRemoteDataSource
                .TranscribeAndTranslateAsync(streamPart, sourceLanguage, targetLanguage)
                .ConfigureAwait(false);

            return result;
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(
                httpEx,
                "HTTP error occurred while transcribing and translating the file {FilePath} from {SourceLanguage} to {TargetLanguage} from repository...",
                filePath,
                sourceLanguage,
                targetLanguage
            );
            throw new NetworkException(httpEx);
        }
        catch (ApiException apiEx)
        {
            logger.LogError(
                apiEx,
                "API error occurred while transcribing and translating the file {FilePath} from {SourceLanguage} to {TargetLanguage} from repository...",
                filePath,
                sourceLanguage,
                targetLanguage
            );
            throw new TranscribeException(apiEx);
        }
        finally
        {
            if (fileStream != null)
            {
                await fileStream.DisposeAsync().ConfigureAwait(false);
            }
        }
    }

    public async Task<string> HealthCheckAsync()
    {
        logger.LogTrace("Health check invoked from repository...");

        try
        {
            var dto = await healthCheckRemoteDataSource.HealthCheckAsync().ConfigureAwait(false);

            return dto.Status;
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(httpEx, "HTTP error occurred during health check from repository...");
            throw new NetworkException(httpEx);
        }
        catch (ApiException apiEx)
        {
            logger.LogError(apiEx, "API error occurred during health check from repository...");
            throw new HealthCheckException(apiEx);
        }
    }
}
