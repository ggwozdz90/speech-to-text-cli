using System.IO.Abstractions;

namespace SpeechToTextProcessor.Data.DataSources;

internal interface IFileAccessLocalDataSource
{
    FileSystemStream GetFileSystemStream(string filePath);
    string GetFileName(string filePath);
}

internal sealed class FileAccessLocalDataSource(IFileSystem fileSystem) : IFileAccessLocalDataSource
{
    public FileSystemStream GetFileSystemStream(string filePath)
    {
        return fileSystem.FileStream.New(filePath, FileMode.Open);
    }

    public string GetFileName(string filePath)
    {
        return fileSystem.Path.GetFileName(filePath);
    }
}
