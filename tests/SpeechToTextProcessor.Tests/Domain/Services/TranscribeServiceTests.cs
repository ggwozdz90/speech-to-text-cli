using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using SpeechToTextProcessor.Domain.Exceptions;
using SpeechToTextProcessor.Domain.Repositories;
using SpeechToTextProcessor.Domain.Services;

namespace SpeechToTextProcessor.Tests.Domain.Services;

[TestFixture]
internal sealed class TranscribeServiceTests
{
    private ILogger<TranscribeService> logger = null!;
    private ISpeechToTextRepository repository = null!;
    private TranscribeService service = null!;

    [SetUp]
    public void SetUp()
    {
        logger = Substitute.For<ILogger<TranscribeService>>();
        repository = Substitute.For<ISpeechToTextRepository>();
        service = new TranscribeService(logger, repository);
    }

    [Test]
    public async Task TranscribeAsync_ShouldReturnTranscription_WhenNoExceptionOccursAsync()
    {
        // Given
        const string filePath = "test.wav";
        const string sourceLanguage = "en";
        const string expectedTranscription = "Hello, world!";
        repository.TranscribeAsync(filePath, sourceLanguage).Returns(expectedTranscription);

        // When
        var result = await service.TranscribeAsync(filePath, sourceLanguage).ConfigureAwait(false);

        // Then
        result.Should().Be(expectedTranscription);
    }

    [Test]
    public async Task TranscribeAsync_ShouldThrowTranscribeException_WhenRepositoryThrowsTranscribeExceptionAsync()
    {
        // Given
        const string filePath = "test.wav";
        const string sourceLanguage = "en";
        repository.TranscribeAsync(filePath, sourceLanguage).ThrowsAsync(new TranscribeException());

        // When
        Func<Task> act = async () => await service.TranscribeAsync(filePath, sourceLanguage).ConfigureAwait(false);

        // Then
        await act.Should().ThrowAsync<TranscribeException>().ConfigureAwait(false);
        await repository.Received(1).TranscribeAsync(filePath, sourceLanguage).ConfigureAwait(false);
    }

    [Test]
    public async Task TranscribeAsync_ShouldLogErrorAndThrow_WhenRepositoryThrowsUnexpectedExceptionAsync()
    {
        // Given
        const string filePath = "test.wav";
        const string sourceLanguage = "en";
        repository.TranscribeAsync(filePath, sourceLanguage).ThrowsAsync(new InvalidOperationException());

        // When
        Func<Task> act = async () => await service.TranscribeAsync(filePath, sourceLanguage).ConfigureAwait(false);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>().ConfigureAwait(false);
        await repository.Received(1).TranscribeAsync(filePath, sourceLanguage).ConfigureAwait(false);
    }

    [Test]
    public async Task TranscribeAndTranslateAsync_ShouldReturnTranscription_WhenNoExceptionOccursAsync()
    {
        // Given
        const string filePath = "test.wav";
        const string sourceLanguage = "en";
        const string targetLanguage = "fr";
        const string expectedTranscription = "Bonjour, le monde!";
        repository.TranscribeAndTranslateAsync(filePath, sourceLanguage, targetLanguage).Returns(expectedTranscription);

        // When
        var result = await service
            .TranscribeAndTranslateAsync(filePath, sourceLanguage, targetLanguage)
            .ConfigureAwait(false);

        // Then
        result.Should().Be(expectedTranscription);
    }

    [Test]
    public async Task TranscribeAndTranslateAsync_ShouldThrowTranscribeException_WhenRepositoryThrowsTranscribeExceptionAsync()
    {
        // Given
        const string filePath = "test.wav";
        const string sourceLanguage = "en";
        const string targetLanguage = "fr";
        repository
            .TranscribeAndTranslateAsync(filePath, sourceLanguage, targetLanguage)
            .ThrowsAsync(new TranscribeException());

        // When
        Func<Task> act = async () =>
            await service.TranscribeAndTranslateAsync(filePath, sourceLanguage, targetLanguage).ConfigureAwait(false);

        // Then
        await act.Should().ThrowAsync<TranscribeException>().ConfigureAwait(false);
        await repository
            .Received(1)
            .TranscribeAndTranslateAsync(filePath, sourceLanguage, targetLanguage)
            .ConfigureAwait(false);
    }

    [Test]
    public async Task TranscribeAndTranslateAsync_ShouldLogErrorAndThrow_WhenRepositoryThrowsUnexpectedExceptionAsync()
    {
        // Given
        const string filePath = "test.wav";
        const string sourceLanguage = "en";
        const string targetLanguage = "fr";
        repository
            .TranscribeAndTranslateAsync(filePath, sourceLanguage, targetLanguage)
            .ThrowsAsync(new InvalidOperationException());

        // When
        Func<Task> act = async () =>
            await service.TranscribeAndTranslateAsync(filePath, sourceLanguage, targetLanguage).ConfigureAwait(false);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>().ConfigureAwait(false);
        await repository
            .Received(1)
            .TranscribeAndTranslateAsync(filePath, sourceLanguage, targetLanguage)
            .ConfigureAwait(false);
    }
}
