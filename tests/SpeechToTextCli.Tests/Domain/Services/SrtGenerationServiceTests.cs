using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using SpeechToTextCli.Domain.Repositories;
using SpeechToTextCli.Domain.Services;

namespace SpeechToTextCli.Tests.Domain.Services;

[TestFixture]
internal sealed class SrtGenerationServiceTests
{
    private ILogger<SrtGenerationService> logger = null!;
    private ISrtGenerationRepository srtGenerationRepository = null!;
    private IFileRepository fileRepository = null!;
    private SrtGenerationService srtGenerationService = null!;

    [SetUp]
    public void Setup()
    {
        logger = Substitute.For<ILogger<SrtGenerationService>>();
        srtGenerationRepository = Substitute.For<ISrtGenerationRepository>();
        fileRepository = Substitute.For<IFileRepository>();
        srtGenerationService = new SrtGenerationService(logger, srtGenerationRepository, fileRepository);
    }

    [Test]
    public async Task GenerateSrtAsync_ShouldGenerateSrt_WhenApiIsHealthyAsync()
    {
        // Given
        const string filePath = "testfile.wav";
        const string sourceLanguage = "en";
        const string srtContent = "Transcribed text";
        const string srtFilePath = "testfile.srt";

        srtGenerationRepository.HealthCheckAsync().Returns(Task.FromResult("OK"));
        srtGenerationRepository.TranscribeAsync(filePath, sourceLanguage).Returns(Task.FromResult(srtContent));
        fileRepository.GetFilePathWithSrtExtension(filePath).Returns(srtFilePath);

        // When
        await srtGenerationService.GenerateSrtAsync(filePath, sourceLanguage).ConfigureAwait(false);

        // Then
        await fileRepository.Received(1).WriteAllTextAsync(srtFilePath, srtContent).ConfigureAwait(false);
        logger
            .Received(1)
            .Log(
                Arg.Is<LogLevel>(level => level == LogLevel.Information),
                Arg.Any<EventId>(),
                Arg.Is<object>(entry => entry.ToString() == $"SRT file generated: {srtFilePath}"),
                Arg.Any<Exception>(),
                Arg.Any<Func<object?, Exception?, string>>()
            );
    }

    [Test]
    public async Task GenerateSrtAsync_ShouldLogError_WhenApiIsNotHealthyAsync()
    {
        // Given
        const string filePath = "testfile.wav";
        const string sourceLanguage = "en";

        srtGenerationRepository.HealthCheckAsync().Returns(Task.FromResult("NOT_OK"));

        // When
        await srtGenerationService.GenerateSrtAsync(filePath, sourceLanguage).ConfigureAwait(false);

        // Then
        logger
            .Received(1)
            .Log(
                Arg.Is<LogLevel>(level => level == LogLevel.Error),
                Arg.Any<EventId>(),
                Arg.Is<object>(entry => entry.ToString() == "The speech-to-text API is not healthy."),
                Arg.Any<Exception>(),
                Arg.Any<Func<object?, Exception?, string>>()
            );
    }

    [Test]
    public async Task GenerateTranslatedSrtAsync_ShouldGenerateTranslatedSrt_WhenApiIsHealthyAsync()
    {
        // Given
        const string filePath = "testfile.wav";
        const string sourceLanguage = "en";
        const string targetLanguage = "fr";
        const string srtContent = "Transcribed and translated text";
        const string srtFilePath = "testfile.srt";

        srtGenerationRepository.HealthCheckAsync().Returns(Task.FromResult("OK"));
        srtGenerationRepository
            .TranscribeAndTranslateAsync(filePath, sourceLanguage, targetLanguage)
            .Returns(Task.FromResult(srtContent));
        fileRepository.GetFilePathWithSrtExtension(filePath).Returns(srtFilePath);

        // When
        await srtGenerationService
            .GenerateTranslatedSrtAsync(filePath, sourceLanguage, targetLanguage)
            .ConfigureAwait(false);

        // Then
        await fileRepository.Received(1).WriteAllTextAsync(srtFilePath, srtContent).ConfigureAwait(false);
        logger
            .Received(1)
            .Log(
                Arg.Is<LogLevel>(level => level == LogLevel.Information),
                Arg.Any<EventId>(),
                Arg.Is<object>(entry => entry.ToString() == $"Translated SRT file generated: {srtFilePath}"),
                Arg.Any<Exception>(),
                Arg.Any<Func<object?, Exception?, string>>()
            );
    }

    [Test]
    public async Task GenerateTranslatedSrtAsync_ShouldLogError_WhenApiIsNotHealthyAsync()
    {
        // Given
        const string filePath = "testfile.wav";
        const string sourceLanguage = "en";
        const string targetLanguage = "fr";

        srtGenerationRepository.HealthCheckAsync().Returns(Task.FromResult("NOT_OK"));

        // When
        await srtGenerationService
            .GenerateTranslatedSrtAsync(filePath, sourceLanguage, targetLanguage)
            .ConfigureAwait(false);

        // Then
        logger
            .Received(1)
            .Log(
                Arg.Is<LogLevel>(level => level == LogLevel.Error),
                Arg.Any<EventId>(),
                Arg.Is<object>(entry => entry.ToString() == "The speech-to-text API is not healthy."),
                Arg.Any<Exception>(),
                Arg.Any<Func<object?, Exception?, string>>()
            );
    }
}
