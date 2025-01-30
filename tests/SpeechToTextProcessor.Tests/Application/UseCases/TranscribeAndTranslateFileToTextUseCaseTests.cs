using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using SpeechToTextProcessor.Application.UseCases;
using SpeechToTextProcessor.Domain.Services;

namespace SpeechToTextProcessor.Tests.Application.UseCases;

[TestFixture]
internal sealed class TranscribeAndTranslateFileToTextUseCaseTests
{
    private ILogger<TranscribeAndTranslateFileToTextUseCase> logger = null!;
    private ITranscribeService transcribeService = null!;
    private TranscribeAndTranslateFileToTextUseCase useCase = null!;

    [SetUp]
    public void SetUp()
    {
        logger = Substitute.For<ILogger<TranscribeAndTranslateFileToTextUseCase>>();
        transcribeService = Substitute.For<ITranscribeService>();
        useCase = new TranscribeAndTranslateFileToTextUseCase(logger, transcribeService);
    }

    [Test]
    public async Task InvokeAsync_ShouldReturnTranscribedText_WhenNoExceptionOccursAsync()
    {
        // Given
        const string filePath = "testfile.txt";
        const string sourceLanguage = "en";
        const string targetLanguage = "fr";
        const string expectedText = "transcribed and translated text";
        transcribeService
            .TranscribeAndTranslateAsync(filePath, sourceLanguage, targetLanguage)
            .Returns(Task.FromResult(expectedText));

        // When
        var result = await useCase.InvokeAsync(filePath, sourceLanguage, targetLanguage).ConfigureAwait(false);

        // Then
        result.Should().Be(expectedText);
        await transcribeService
            .Received(1)
            .TranscribeAndTranslateAsync(filePath, sourceLanguage, targetLanguage)
            .ConfigureAwait(false);
    }

    [Test]
    public async Task InvokeAsync_ShouldThrowException_WhenTranscribeAndTranslateFailsAsync()
    {
        // Given
        const string filePath = "testfile.txt";
        const string sourceLanguage = "en";
        const string targetLanguage = "fr";
        transcribeService
            .TranscribeAndTranslateAsync(filePath, sourceLanguage, targetLanguage)
            .ThrowsAsync(new InvalidOperationException("Transcribe and translate failed"));

        // When
        Func<Task> act = async () =>
            await useCase.InvokeAsync(filePath, sourceLanguage, targetLanguage).ConfigureAwait(false);

        // Then
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Transcribe and translate failed")
            .ConfigureAwait(false);
        await transcribeService
            .Received(1)
            .TranscribeAndTranslateAsync(filePath, sourceLanguage, targetLanguage)
            .ConfigureAwait(false);
        logger
            .Received(1)
            .Log(
                LogLevel.Trace,
                Arg.Any<EventId>(),
                Arg.Is<object>(v =>
                    v.ToString()
                    == "Transcribing and translating file testfile.txt from en to fr invoked from use case..."
                ),
                exception: null,
                Arg.Any<Func<object, Exception?, string>>()
            );
    }
}
