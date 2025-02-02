using System.IO.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using SpeechToTextApiClient.Domain.Exceptions;
using SpeechToTextCli.Application.UseCases;
using SpeechToTextCli.Domain.ErrorCodes;
using SpeechToTextCli.Domain.Services;

namespace SpeechToTextCli.Tests.Application.UseCases;

[TestFixture]
internal sealed class GenerateSrtUseCaseTest
{
    private ILogger<GenerateSrtUseCase> logger = null!;
    private ISrtGenerationService srtGenerationService = null!;
    private IFileInfo file = null!;
    private GenerateSrtUseCase useCase = null!;

    [SetUp]
    public void Setup()
    {
        logger = Substitute.For<ILogger<GenerateSrtUseCase>>();
        srtGenerationService = Substitute.For<ISrtGenerationService>();
        file = Substitute.For<IFileInfo>();
        file.FullName.Returns("testfile.txt");
        useCase = new GenerateSrtUseCase(logger, srtGenerationService);
    }

    [Test]
    public async Task InvokeAsync_ShouldReturnSuccess_WhenNoExceptionOccursAsync()
    {
        // Given
        const string SourceLanguage = "en";

        // When
        var result = await useCase.InvokeAsync(file, SourceLanguage).ConfigureAwait(false);

        // Then
        result.Should().Be(ErrorCode.Success);
        await srtGenerationService.Received(1).GenerateSrtAsync(file.FullName, SourceLanguage).ConfigureAwait(false);
    }

    [Test]
    public async Task InvokeAsync_ShouldReturnNetworkError_WhenNetworkExceptionOccursAsync()
    {
        // Given
        const string SourceLanguage = "en";
        srtGenerationService.GenerateSrtAsync(file.FullName, SourceLanguage).ThrowsAsync(new NetworkException());

        // When
        var result = await useCase.InvokeAsync(file, SourceLanguage).ConfigureAwait(false);

        // Then
        result.Should().Be(ErrorCode.NetworkError);
        logger
            .Received(1)
            .Log(
                Arg.Is<LogLevel>(level => level == LogLevel.Error),
                Arg.Any<EventId>(),
                Arg.Is<object>(entry => entry.ToString() == "Network error occurred."),
                Arg.Any<NetworkException>(),
                Arg.Any<Func<object?, Exception?, string>>()
            );
    }

    [Test]
    public async Task InvokeAsync_ShouldReturnFileAccessError_WhenFileAccessExceptionOccursAsync()
    {
        // Given
        const string SourceLanguage = "en";
        srtGenerationService.GenerateSrtAsync(file.FullName, SourceLanguage).ThrowsAsync(new FileAccessException());

        // When
        var result = await useCase.InvokeAsync(file, SourceLanguage).ConfigureAwait(false);

        // Then
        result.Should().Be(ErrorCode.FileAccessError);
        logger
            .Received(1)
            .Log(
                Arg.Is<LogLevel>(level => level == LogLevel.Error),
                Arg.Any<EventId>(),
                Arg.Is<object>(entry => entry.ToString() == "File access error occurred."),
                Arg.Any<FileAccessException>(),
                Arg.Any<Func<object?, Exception?, string>>()
            );
    }

    [Test]
    public async Task InvokeAsync_ShouldReturnHealthCheckError_WhenHealthCheckExceptionOccursAsync()
    {
        // Given
        const string SourceLanguage = "en";
        srtGenerationService.GenerateSrtAsync(file.FullName, SourceLanguage).ThrowsAsync(new HealthCheckException());

        // When
        var result = await useCase.InvokeAsync(file, SourceLanguage).ConfigureAwait(false);

        // Then
        result.Should().Be(ErrorCode.HealthCheckError);
        logger
            .Received(1)
            .Log(
                Arg.Is<LogLevel>(level => level == LogLevel.Error),
                Arg.Any<EventId>(),
                Arg.Is<object>(entry => entry.ToString() == "Health check error occurred."),
                Arg.Any<HealthCheckException>(),
                Arg.Any<Func<object?, Exception?, string>>()
            );
    }

    [Test]
    public async Task InvokeAsync_ShouldReturnTranscribeError_WhenTranscribeExceptionOccursAsync()
    {
        // Given
        const string SourceLanguage = "en";
        srtGenerationService.GenerateSrtAsync(file.FullName, SourceLanguage).ThrowsAsync(new TranscribeException());

        // When
        var result = await useCase.InvokeAsync(file, SourceLanguage).ConfigureAwait(false);

        // Then
        result.Should().Be(ErrorCode.TranscribeError);
        logger
            .Received(1)
            .Log(
                Arg.Is<LogLevel>(level => level == LogLevel.Error),
                Arg.Any<EventId>(),
                Arg.Is<object>(entry => entry.ToString() == "Transcription error occurred."),
                Arg.Any<TranscribeException>(),
                Arg.Any<Func<object?, Exception?, string>>()
            );
    }

    [Test]
    public async Task InvokeAsync_ShouldThrowException_WhenUnexpectedExceptionOccursAsync()
    {
        // Given
        const string SourceLanguage = "en";
        var unexpectedException = new InvalidOperationException("Unexpected error");
        srtGenerationService.GenerateSrtAsync(file.FullName, SourceLanguage).ThrowsAsync(unexpectedException);

        // When
        Func<Task> act = async () => await useCase.InvokeAsync(file, SourceLanguage).ConfigureAwait(false);

        // Then
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Unexpected error")
            .ConfigureAwait(false);
        logger
            .Received(1)
            .Log(
                Arg.Is<LogLevel>(level => level == LogLevel.Error),
                Arg.Any<EventId>(),
                Arg.Is<object>(entry => entry.ToString() == "An unexpected error occurred."),
                Arg.Any<Exception>(),
                Arg.Any<Func<object?, Exception?, string>>()
            );
    }
}
