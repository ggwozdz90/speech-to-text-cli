using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using Refit;
using SpeechToTextProcessor.Data.DataSources;
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
        logger.LogDebug("Transcribing file to text invoked from repository...");

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
            logger.LogError(httpEx, "HTTP error occurred while transcribing the file.");
            throw;
        }
        catch (ApiException apiEx)
        {
            logger.LogError(apiEx, "API error occurred while transcribing the file.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while transcribing the file.");
            throw;
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
        logger.LogDebug("Transcribing and translating file to text invoked from repository...");

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
            logger.LogError(httpEx, "HTTP error occurred while transcribing and translating the file.");
            throw;
        }
        catch (ApiException apiEx)
        {
            logger.LogError(apiEx, "API error occurred while transcribing and translating the file.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while transcribing and translating the file.");
            throw;
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
        logger.LogDebug("Health check invoked from repository...");

        try
        {
            var result = await healthCheckRemoteDataSource.HealthCheckAsync().ConfigureAwait(false);

            return result;
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(httpEx, "HTTP error occurred during health check.");
            throw;
        }
        catch (ApiException apiEx)
        {
            logger.LogError(apiEx, "API error occurred during health check.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during health check.");
            throw;
        }
    }
}
