using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using SpeechToTextCli.Domain.Repositories;

namespace SpeechToTextCli.Data.Repositories;

internal sealed class FileRepository(ILogger<FileRepository> logger, IFileSystem fileSystem) : IFileRepository
{
    public string GetFilePathWithSrtExtension(string filePath)
    {
        logger.LogTrace("Getting SRT file path for file: {FilePath}", filePath);

        var directory = fileSystem.Path.GetDirectoryName(filePath) ?? string.Empty;
        var fileNameWithoutExtension = fileSystem.Path.GetFileNameWithoutExtension(filePath);
        var newFileName = $"{fileNameWithoutExtension}.srt";
        var result = fileSystem.Path.Combine(directory, newFileName);

        return result;
    }

    public Task WriteAllTextAsync(string filePath, string content)
    {
        logger.LogTrace("Writing content to file: {FilePath}", filePath);

        return fileSystem.File.WriteAllTextAsync(filePath, content);
    }
}
