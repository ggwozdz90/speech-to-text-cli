using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using SpeechToTextProcessor.Application.UseCases;
using SpeechToTextProcessor.Domain.Services;

namespace SpeechToTextProcessor.Tests.Application.UseCases;

[TestFixture]
internal sealed class TranscribeFileToTextUseCaseTests
{
    private ILogger<TranscribeFileToTextUseCase> logger = null!;
    private ITranscribeService transcribeService = null!;
    private TranscribeFileToTextUseCase usecase = null!;

    [SetUp]
    public void Setup()
    {
        logger = Substitute.For<ILogger<TranscribeFileToTextUseCase>>();
        transcribeService = Substitute.For<ITranscribeService>();
        usecase = new TranscribeFileToTextUseCase(logger, transcribeService);
    }

    [Test]
    public async Task InvokeAsync_ShouldReturnTranscriptionResult_WhenTranscriptionIsSuccessfulAsync()
    {
        // Given
        const string filePath = "testfile.wav";
        const string sourceLanguage = "en";
        const string expectedResult = "Transcription result";
        transcribeService.TranscribeAsync(filePath, sourceLanguage).Returns(expectedResult);

        // When
        var result = await usecase.InvokeAsync(filePath, sourceLanguage).ConfigureAwait(false);

        // Then
        result.Should().Be(expectedResult);
        await transcribeService.Received(1).TranscribeAsync(filePath, sourceLanguage).ConfigureAwait(false);
    }

    [Test]
    public async Task InvokeAsync_ShouldThrowException_WhenTranscriptionFailsAsync()
    {
        // Given
        const string filePath = "testfile.wav";
        const string sourceLanguage = "en";
        var exception = new InvalidOperationException("Transcription failed");
        transcribeService.TranscribeAsync(filePath, sourceLanguage).ThrowsAsync(exception);

        // When
        Func<Task> act = async () => await usecase.InvokeAsync(filePath, sourceLanguage).ConfigureAwait(false);

        // Then
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Transcription failed")
            .ConfigureAwait(false);
        await transcribeService.Received(1).TranscribeAsync(filePath, sourceLanguage).ConfigureAwait(false);
        logger
            .Received(1)
            .Log(
                LogLevel.Trace,
                Arg.Any<EventId>(),
                Arg.Is<object>(v => v.ToString() == "Transcribing file testfile.wav from en invoked from use case..."),
                exception: null,
                Arg.Any<Func<object, Exception?, string>>()
            );
    }
}
