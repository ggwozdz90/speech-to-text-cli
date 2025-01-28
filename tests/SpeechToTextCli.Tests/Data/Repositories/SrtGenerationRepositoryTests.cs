using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using SpeechToTextCli.Data.Repositories;
using SpeechToTextProcessor.Adapter.Adapters;

namespace SpeechToTextCli.Tests.Data.Repositories;

[TestFixture]
internal sealed class SrtGenerationRepositoryTests
{
    private ILogger<SrtGenerationRepository> logger = null!;
    private ISpeechToTextAdapter speechToTextAdapter = null!;
    private SrtGenerationRepository srtGenerationRepository = null!;

    [SetUp]
    public void Setup()
    {
        logger = Substitute.For<ILogger<SrtGenerationRepository>>();
        speechToTextAdapter = Substitute.For<ISpeechToTextAdapter>();
        srtGenerationRepository = new SrtGenerationRepository(logger, speechToTextAdapter);
    }

    [Test]
    public async Task TranscribeAsync_ShouldReturnTranscription_WhenCalledAsync()
    {
        // Given
        const string filePath = "testfile.wav";
        const string sourceLanguage = "en";
        const string expectedTranscription = "Transcribed text";
        speechToTextAdapter.TranscribeAsync(filePath, sourceLanguage).Returns(Task.FromResult(expectedTranscription));

        // When
        var result = await srtGenerationRepository.TranscribeAsync(filePath, sourceLanguage).ConfigureAwait(false);

        // Then
        result.Should().Be(expectedTranscription);
        logger
            .Received(1)
            .Log(
                Arg.Is<LogLevel>(level => level == LogLevel.Trace),
                Arg.Any<EventId>(),
                Arg.Is<object>(entry =>
                    entry.ToString() == $"Transcribing file: {filePath}, source language: {sourceLanguage}"
                ),
                Arg.Any<Exception>(),
                Arg.Any<Func<object?, Exception?, string>>()
            );
    }

    [Test]
    public async Task TranscribeAndTranslateAsync_ShouldReturnTranslatedTranscription_WhenCalledAsync()
    {
        // Given
        const string filePath = "testfile.wav";
        const string sourceLanguage = "en";
        const string targetLanguage = "fr";
        const string expectedTranslation = "Texte transcrit et traduit";
        speechToTextAdapter
            .TranscribeAndTranslateAsync(filePath, sourceLanguage, targetLanguage)
            .Returns(Task.FromResult(expectedTranslation));

        // When
        var result = await srtGenerationRepository
            .TranscribeAndTranslateAsync(filePath, sourceLanguage, targetLanguage)
            .ConfigureAwait(false);

        // Then
        result.Should().Be(expectedTranslation);
        logger
            .Received(1)
            .Log(
                Arg.Is<LogLevel>(level => level == LogLevel.Trace),
                Arg.Any<EventId>(),
                Arg.Is<object>(entry =>
                    entry.ToString()
                    == $"Transcribing and translating file: {filePath}, source language: {sourceLanguage}, target language: {targetLanguage}"
                ),
                Arg.Any<Exception>(),
                Arg.Any<Func<object?, Exception?, string>>()
            );
    }

    [Test]
    public async Task HealthCheckAsync_ShouldReturnHealthStatus_WhenCalledAsync()
    {
        // Given
        const string expectedHealthStatus = "Healthy";
        speechToTextAdapter.HealthCheckAsync().Returns(Task.FromResult(expectedHealthStatus));

        // When
        var result = await srtGenerationRepository.HealthCheckAsync().ConfigureAwait(false);

        // Then
        result.Should().Be(expectedHealthStatus);
        logger
            .Received(1)
            .Log(
                Arg.Is<LogLevel>(level => level == LogLevel.Trace),
                Arg.Any<EventId>(),
                Arg.Is<object>(entry => entry.ToString() == "Performing health check..."),
                Arg.Any<Exception>(),
                Arg.Any<Func<object?, Exception?, string>>()
            );
    }
}
