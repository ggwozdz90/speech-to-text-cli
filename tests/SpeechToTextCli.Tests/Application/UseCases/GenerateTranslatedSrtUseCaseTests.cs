using System.IO.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using SpeechToTextCli.Application.UseCases;
using SpeechToTextCli.Domain.ErrorCodes;
using SpeechToTextCli.Domain.Services;
using SpeechToTextProcessor.Domain.Exceptions;

namespace SpeechToTextCli.Tests.Application.UseCases;

[TestFixture]
internal sealed class GenerateTranslatedSrtUseCaseTest
{
    private ILogger<GenerateTranslatedSrtUseCase> logger = null!;
    private ISrtGenerationService translatedSrtGenerationService = null!;
    private IFileInfo file = null!;
    private GenerateTranslatedSrtUseCase useCase = null!;

    [SetUp]
    public void Setup()
    {
        logger = Substitute.For<ILogger<GenerateTranslatedSrtUseCase>>();
        translatedSrtGenerationService = Substitute.For<ISrtGenerationService>();
        file = Substitute.For<IFileInfo>();
        file.FullName.Returns("testfile.txt");
        useCase = new GenerateTranslatedSrtUseCase(logger, translatedSrtGenerationService);
    }

    [Test]
    public async Task InvokeAsync_ShouldReturnSuccess_WhenNoExceptionOccursAsync()
    {
        // Given
        const string SourceLanguage = "en";
        const string TargetLanguage = "fr";

        // When
        var result = await useCase.InvokeAsync(file, SourceLanguage, TargetLanguage).ConfigureAwait(false);

        // Then
        result.Should().Be(ErrorCode.Success);
        await translatedSrtGenerationService
            .Received(1)
            .GenerateTranslatedSrtAsync(file.FullName, SourceLanguage, TargetLanguage)
            .ConfigureAwait(false);
    }

    [Test]
    public async Task InvokeAsync_ShouldReturnNetworkError_WhenNetworkExceptionOccursAsync()
    {
        // Given
        const string SourceLanguage = "en";
        const string TargetLanguage = "fr";
        translatedSrtGenerationService
            .GenerateTranslatedSrtAsync(file.FullName, SourceLanguage, TargetLanguage)
            .ThrowsAsync(new NetworkException());

        // When
        var result = await useCase.InvokeAsync(file, SourceLanguage, TargetLanguage).ConfigureAwait(false);

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
        const string TargetLanguage = "fr";
        translatedSrtGenerationService
            .GenerateTranslatedSrtAsync(file.FullName, SourceLanguage, TargetLanguage)
            .ThrowsAsync(new FileAccessException());

        // When
        var result = await useCase.InvokeAsync(file, SourceLanguage, TargetLanguage).ConfigureAwait(false);

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
        const string TargetLanguage = "fr";
        translatedSrtGenerationService
            .GenerateTranslatedSrtAsync(file.FullName, SourceLanguage, TargetLanguage)
            .ThrowsAsync(new HealthCheckException());

        // When
        var result = await useCase.InvokeAsync(file, SourceLanguage, TargetLanguage).ConfigureAwait(false);

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
        const string TargetLanguage = "fr";
        translatedSrtGenerationService
            .GenerateTranslatedSrtAsync(file.FullName, SourceLanguage, TargetLanguage)
            .ThrowsAsync(new TranscribeException());

        // When
        var result = await useCase.InvokeAsync(file, SourceLanguage, TargetLanguage).ConfigureAwait(false);

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
        const string TargetLanguage = "fr";
        var unexpectedException = new InvalidOperationException("Unexpected error");
        translatedSrtGenerationService
            .GenerateTranslatedSrtAsync(file.FullName, SourceLanguage, TargetLanguage)
            .ThrowsAsync(unexpectedException);

        // When
        Func<Task> act = async () =>
            await useCase.InvokeAsync(file, SourceLanguage, TargetLanguage).ConfigureAwait(false);

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
