namespace SpeechToTextCli.Domain.Repositories;

internal interface IFileRepository
{
    string GetFilePathWithSrtExtension(string filePath);
    Task WriteAllTextAsync(string filePath, string content);
}
