using System.IO.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using SpeechToTextCli.Data.Repositories;

namespace SpeechToTextCli.Tests.Data.Repositories;

[TestFixture]
internal sealed class FileRepositoryTests
{
    private ILogger<FileRepository> logger = null!;
    private IFileSystem fileSystem = null!;
    private FileRepository fileRepository = null!;

    [SetUp]
    public void Setup()
    {
        logger = Substitute.For<ILogger<FileRepository>>();
        fileSystem = Substitute.For<IFileSystem>();
        fileRepository = new FileRepository(logger, fileSystem);
    }

    [Test]
    public void GetFilePathWithSrtExtension_ShouldReturnCorrectPath_WhenFilePathIsValid()
    {
        // Given
        const string filePath = "C:\\temp\\example.txt";
        const string expectedPath = "C:\\temp\\example.srt";
        fileSystem.Path.GetDirectoryName(filePath).Returns("C:\\temp");
        fileSystem.Path.GetFileNameWithoutExtension(filePath).Returns("example");
        fileSystem.Path.Combine("C:\\temp", "example.srt").Returns(expectedPath);

        // When
        var result = fileRepository.GetFilePathWithSrtExtension(filePath);

        // Then
        result.Should().Be(expectedPath);
        logger
            .Received(1)
            .Log(
                Arg.Is<LogLevel>(level => level == LogLevel.Trace),
                Arg.Any<EventId>(),
                Arg.Is<object>(entry => entry.ToString() == $"Getting SRT file path for file: {filePath}"),
                Arg.Any<Exception>(),
                Arg.Any<Func<object?, Exception?, string>>()
            );
    }

    [Test]
    public void GetFilePathWithSrtExtension_ShouldReturnFileNameWithSrtExtension_WhenDirectoryIsNull()
    {
        // Given
        const string filePath = "example.txt";
        const string expectedPath = "example.srt";
        fileSystem.Path.GetDirectoryName(filePath).Returns((string?)null);
        fileSystem.Path.GetFileNameWithoutExtension(filePath).Returns("example");
        fileSystem.Path.Combine(string.Empty, "example.srt").Returns(expectedPath);

        // When
        var result = fileRepository.GetFilePathWithSrtExtension(filePath);

        // Then
        result.Should().Be(expectedPath);
        logger
            .Received(1)
            .Log(
                Arg.Is<LogLevel>(level => level == LogLevel.Trace),
                Arg.Any<EventId>(),
                Arg.Is<object>(entry => entry.ToString() == $"Getting SRT file path for file: {filePath}"),
                Arg.Any<Exception>(),
                Arg.Any<Func<object?, Exception?, string>>()
            );
    }

    [Test]
    public async Task WriteAllTextAsync_ShouldWriteContentToFile_WhenCalledAsync()
    {
        // Given
        const string filePath = "C:\\temp\\example.srt";
        const string content = "Sample content";

        // When
        await fileRepository.WriteAllTextAsync(filePath, content).ConfigureAwait(false);

        // Then
        await fileSystem.File.Received(1).WriteAllTextAsync(filePath, content).ConfigureAwait(false);
        logger
            .Received(1)
            .Log(
                Arg.Is<LogLevel>(level => level == LogLevel.Trace),
                Arg.Any<EventId>(),
                Arg.Is<object>(entry => entry.ToString() == $"Writing content to file: {filePath}"),
                Arg.Any<Exception>(),
                Arg.Any<Func<object?, Exception?, string>>()
            );
    }
}
