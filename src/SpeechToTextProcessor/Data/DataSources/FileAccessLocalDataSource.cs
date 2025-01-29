using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using SpeechToTextProcessor.Domain.Exceptions;

namespace SpeechToTextProcessor.Data.DataSources;

internal interface IFileAccessLocalDataSource
{
    Stream GetFileSystemStream(string filePath);
    string GetFileName(string filePath);
}

internal sealed class FileAccessLocalDataSource(ILogger<FileAccessLocalDataSource> logger, IFileSystem fileSystem)
    : IFileAccessLocalDataSource
{
    public Stream GetFileSystemStream(string filePath)
    {
        logger.LogTrace("Getting file system stream for {FilePath} invoked from local data source...", filePath);

        try
        {
            return fileSystem.FileStream.New(filePath, FileMode.Open);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get file system stream for {FilePath} from local data source...", filePath);
            throw new FileAccessException("Failed to get file system stream.", ex);
        }
    }

    public string GetFileName(string filePath)
    {
        logger.LogTrace("Getting file name for {FilePath} invoked from local data source...", filePath);

        try
        {
            return fileSystem.Path.GetFileName(filePath);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get file name for {FilePath} from local data source...", filePath);
            throw new FileAccessException("Failed to get file name.", ex);
        }
    }
}
