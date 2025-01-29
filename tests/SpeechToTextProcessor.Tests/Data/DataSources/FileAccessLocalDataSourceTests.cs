using System.IO.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using SpeechToTextProcessor.Data.DataSources;
using SpeechToTextProcessor.Domain.Exceptions;

namespace SpeechToTextProcessor.Tests.Data.DataSources;

[TestFixture]
internal sealed class FileAccessLocalDataSourceTests
{
    private ILogger<FileAccessLocalDataSource> logger = null!;
    private IFileSystem fileSystem = null!;
    private FileAccessLocalDataSource dataSource = null!;

    [SetUp]
    public void Setup()
    {
        logger = Substitute.For<ILogger<FileAccessLocalDataSource>>();
        fileSystem = Substitute.For<IFileSystem>();
        dataSource = new FileAccessLocalDataSource(logger, fileSystem);
    }

    [Test]
    public void GetFileSystemStream_WhenFileExists_ShouldReturnFileStream()
    {
        // Given
        const string filePath = "file.txt";
        var fileStream = Substitute.For<FileSystemStream>(null, filePath, null);
        using (var stream = fileSystem.FileStream.New(filePath, FileMode.Open))
        {
            stream.Returns(fileStream);
        }

        // When
        using var result = dataSource.GetFileSystemStream(filePath);

        // Then
        result.Should().BeSameAs(fileStream);
    }

    [Test]
    public void GetFileSystemStream_WhenFileDoesNotExist_ShouldThrowFileAccessException()
    {
        // Given
        const string filePath = "file.txt";
        using (var stream = fileSystem.FileStream.New(filePath, FileMode.Open))
        {
            stream.Returns(_ => throw new InvalidOperationException("File not found."));
        }

        // When
        Action act = () => dataSource.GetFileSystemStream(filePath);

        // Then
        act.Should().Throw<FileAccessException>();
    }

    [Test]
    public void GetFileName_WhenFileExists_ShouldReturnFileName()
    {
        // Given
        const string filePath = "file.txt";
        const string fileName = "file.txt";
        fileSystem.Path.GetFileName(filePath).Returns(fileName);

        // When
        var result = dataSource.GetFileName(filePath);

        // Then
        result.Should().Be(fileName);
    }

    [Test]
    public void GetFileName_WhenFileDoesNotExist_ShouldThrowFileAccessException()
    {
        // Given
        const string filePath = "file.txt";
        fileSystem.Path.GetFileName(filePath).Returns(_ => throw new InvalidOperationException("File not found."));

        // When
        Action act = () => dataSource.GetFileName(filePath);

        // Then
        act.Should().Throw<FileAccessException>();
    }
}
