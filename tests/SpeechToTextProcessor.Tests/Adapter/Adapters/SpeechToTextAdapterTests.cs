using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using SpeechToTextProcessor.Adapter.Adapters;
using SpeechToTextProcessor.Application.UseCases;

namespace SpeechToTextProcessor.Tests.Adapter.Adapters;

[TestFixture]
internal sealed class SpeechToTextAdapterTests
{
    private ILogger<SpeechToTextAdapter> logger = null!;
    private ITranscribeFileToTextUseCase transcribeFileToTextUseCase = null!;
    private ITranscribeAndTranslateFileToTextUseCase transcribeAndTranslateFileToTextUseCase = null!;
    private IHealthCheckUseCase healthCheckUseCase = null!;
    private SpeechToTextAdapter adapter = null!;

    [SetUp]
    public void Setup()
    {
        logger = Substitute.For<ILogger<SpeechToTextAdapter>>();
        transcribeFileToTextUseCase = Substitute.For<ITranscribeFileToTextUseCase>();
        transcribeAndTranslateFileToTextUseCase = Substitute.For<ITranscribeAndTranslateFileToTextUseCase>();
        healthCheckUseCase = Substitute.For<IHealthCheckUseCase>();
        adapter = new SpeechToTextAdapter(
            logger,
            transcribeFileToTextUseCase,
            transcribeAndTranslateFileToTextUseCase,
            healthCheckUseCase
        );
    }

    [Test]
    public async Task TranscribeAsync_ShouldReturnResult_WhenNoExceptionOccursAsync()
    {
        // Arrange
        const string filePath = "testfile.wav";
        const string sourceLanguage = "en_US";
        const string expectedResult = "Transcribed text";
        transcribeFileToTextUseCase.InvokeAsync(filePath, sourceLanguage).Returns(expectedResult);

        // Act
        var result = await adapter.TranscribeAsync(filePath, sourceLanguage).ConfigureAwait(false);

        // Assert
        result.Should().Be(expectedResult);
        await transcribeFileToTextUseCase.Received(1).InvokeAsync(filePath, sourceLanguage).ConfigureAwait(false);
    }

    [Test]
    public async Task TranscribeAndTranslateAsync_ShouldReturnResult_WhenNoExceptionOccursAsync()
    {
        // Arrange
        const string filePath = "testfile.wav";
        const string sourceLanguage = "en_US";
        const string targetLanguage = "es_ES";
        const string expectedResult = "Translated text";
        transcribeAndTranslateFileToTextUseCase
            .InvokeAsync(filePath, sourceLanguage, targetLanguage)
            .Returns(expectedResult);

        // Act
        var result = await adapter
            .TranscribeAndTranslateAsync(filePath, sourceLanguage, targetLanguage)
            .ConfigureAwait(false);

        // Assert
        result.Should().Be(expectedResult);
        await transcribeAndTranslateFileToTextUseCase
            .Received(1)
            .InvokeAsync(filePath, sourceLanguage, targetLanguage)
            .ConfigureAwait(false);
    }

    [Test]
    public async Task HealthCheckAsync_ShouldReturnOk_WhenNoExceptionOccursAsync()
    {
        // Arrange
        const string expectedResult = "OK";
        healthCheckUseCase.InvokeAsync().Returns(expectedResult);

        // Act
        var result = await adapter.HealthCheckAsync().ConfigureAwait(false);

        // Assert
        result.Should().Be(expectedResult);
        await healthCheckUseCase.Received(1).InvokeAsync().ConfigureAwait(false);
    }
}
