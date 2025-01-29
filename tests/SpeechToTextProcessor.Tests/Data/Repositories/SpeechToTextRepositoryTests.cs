#pragma warning disable IDISP004, RCS1261, MA0042 // Don't ignore created IDisposable -> Justification: Analyzer is reporting issue even when stream is disposed in the test method.

using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Refit;
using SpeechToTextProcessor.Data.DataSources;
using SpeechToTextProcessor.Data.DTOs;
using SpeechToTextProcessor.Data.Repositories;
using SpeechToTextProcessor.Domain.Exceptions;

namespace SpeechToTextProcessor.Tests.Data.Repositories
{
    [TestFixture]
    internal sealed class SpeechToTextRepositoryTests
    {
        private ILogger<SpeechToTextRepository> logger = null!;
        private IFileAccessLocalDataSource fileAccessLocalDataSource = null!;
        private ITranscribeRemoteDataSource transcribeRemoteDataSource = null!;
        private IHealthCheckRemoteDataSource healthCheckRemoteDataSource = null!;
        private SpeechToTextRepository repository = null!;

        [SetUp]
        public void SetUp()
        {
            logger = Substitute.For<ILogger<SpeechToTextRepository>>();
            fileAccessLocalDataSource = Substitute.For<IFileAccessLocalDataSource>();
            transcribeRemoteDataSource = Substitute.For<ITranscribeRemoteDataSource>();
            healthCheckRemoteDataSource = Substitute.For<IHealthCheckRemoteDataSource>();
            repository = new SpeechToTextRepository(
                logger,
                fileAccessLocalDataSource,
                transcribeRemoteDataSource,
                healthCheckRemoteDataSource
            );
        }

        [Test]
        public async Task TranscribeAsync_ShouldReturnTranscription_WhenNoExceptionOccursAsync()
        {
            // Given
            const string filePath = "testfile.txt";
            const string sourceLanguage = "en";
            const string expectedTranscription = "Transcription result";
            fileAccessLocalDataSource.GetFileName(filePath).Returns("testfile.txt");
            using var fileStream = Substitute.For<Stream>();
            fileAccessLocalDataSource.GetFileSystemStream(filePath).Returns(fileStream);
            transcribeRemoteDataSource
                .TranscribeAsync(Arg.Any<StreamPart>(), sourceLanguage)
                .Returns(expectedTranscription);

            // When
            var result = await repository.TranscribeAsync(filePath, sourceLanguage).ConfigureAwait(false);

            // Then
            result.Should().Be(expectedTranscription);
            await fileStream.Received(1).DisposeAsync().ConfigureAwait(false);
        }

        [Test]
        public async Task TranscribeAsync_ShouldThrowNetworkException_WhenHttpRequestExceptionOccursAsync()
        {
            // Given
            const string filePath = "testfile.txt";
            const string sourceLanguage = "en";
            fileAccessLocalDataSource.GetFileName(filePath).Returns("testfile.txt");
            using var fileStream = Substitute.For<Stream>();
            fileAccessLocalDataSource.GetFileSystemStream(filePath).Returns(fileStream);
            transcribeRemoteDataSource
                .TranscribeAsync(Arg.Any<StreamPart>(), sourceLanguage)
                .ThrowsAsync(new HttpRequestException());

            // When
            Func<Task> act = async () =>
                await repository.TranscribeAsync(filePath, sourceLanguage).ConfigureAwait(false);

            // Then
            await act.Should().ThrowAsync<NetworkException>().ConfigureAwait(false);
            await transcribeRemoteDataSource
                .Received(1)
                .TranscribeAsync(Arg.Any<StreamPart>(), sourceLanguage)
                .ConfigureAwait(false);
        }

        [Test]
        public async Task TranscribeAsync_ShouldThrowTranscribeException_WhenApiExceptionOccursAsync()
        {
            // Given
            const string filePath = "testfile.txt";
            const string sourceLanguage = "en";
            using var httpRequestMessage = new HttpRequestMessage();
            using var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var exception = await ApiException
                .Create(httpRequestMessage, HttpMethod.Post, httpResponseMessage, new RefitSettings())
                .ConfigureAwait(false);
            fileAccessLocalDataSource.GetFileName(filePath).Returns("testfile.txt");
            using var fileStream = Substitute.For<Stream>();
            fileAccessLocalDataSource.GetFileSystemStream(filePath).Returns(fileStream);
            transcribeRemoteDataSource.TranscribeAsync(Arg.Any<StreamPart>(), sourceLanguage).ThrowsAsync(exception);

            // When
            Func<Task> act = async () =>
                await repository.TranscribeAsync(filePath, sourceLanguage).ConfigureAwait(false);

            // Then
            await act.Should().ThrowAsync<TranscribeException>().ConfigureAwait(false);
            await transcribeRemoteDataSource
                .Received(1)
                .TranscribeAsync(Arg.Any<StreamPart>(), sourceLanguage)
                .ConfigureAwait(false);
        }

        [Test]
        public async Task TranscribeAndTranslateAsync_ShouldReturnTranscription_WhenNoExceptionOccursAsync()
        {
            // Given
            const string filePath = "testfile.txt";
            const string sourceLanguage = "en";
            const string targetLanguage = "fr";
            const string expectedTranscription = "Transcription result";
            fileAccessLocalDataSource.GetFileName(filePath).Returns("testfile.txt");
            using var fileStream = Substitute.For<Stream>();
            fileAccessLocalDataSource.GetFileSystemStream(filePath).Returns(fileStream);
            transcribeRemoteDataSource
                .TranscribeAndTranslateAsync(Arg.Any<StreamPart>(), sourceLanguage, targetLanguage)
                .Returns(expectedTranscription);

            // When
            var result = await repository
                .TranscribeAndTranslateAsync(filePath, sourceLanguage, targetLanguage)
                .ConfigureAwait(false);

            // Then
            result.Should().Be(expectedTranscription);
        }

        [Test]
        public async Task TranscribeAndTranslateAsync_ShouldThrowNetworkException_WhenHttpRequestExceptionOccursAsync()
        {
            // Given
            const string filePath = "testfile.txt";
            const string sourceLanguage = "en";
            const string targetLanguage = "fr";
            fileAccessLocalDataSource.GetFileName(filePath).Returns("testfile.txt");
            using var fileStream = Substitute.For<Stream>();
            fileAccessLocalDataSource.GetFileSystemStream(filePath).Returns(fileStream);
            transcribeRemoteDataSource
                .TranscribeAndTranslateAsync(Arg.Any<StreamPart>(), sourceLanguage, targetLanguage)
                .ThrowsAsync(new HttpRequestException());

            // When
            Func<Task> act = async () =>
                await repository
                    .TranscribeAndTranslateAsync(filePath, sourceLanguage, targetLanguage)
                    .ConfigureAwait(false);

            // Then
            await act.Should().ThrowAsync<NetworkException>().ConfigureAwait(false);
            await transcribeRemoteDataSource
                .Received(1)
                .TranscribeAndTranslateAsync(Arg.Any<StreamPart>(), sourceLanguage, targetLanguage)
                .ConfigureAwait(false);
        }

        [Test]
        public async Task TranscribeAndTranslateAsync_ShouldThrowTranscribeException_WhenApiExceptionOccursAsync()
        {
            // Given
            const string filePath = "testfile.txt";
            const string sourceLanguage = "en";
            const string targetLanguage = "fr";
            using var httpRequestMessage = new HttpRequestMessage();
            using var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var exception = await ApiException
                .Create(httpRequestMessage, HttpMethod.Post, httpResponseMessage, new RefitSettings())
                .ConfigureAwait(false);
            fileAccessLocalDataSource.GetFileName(filePath).Returns("testfile.txt");
            using var fileStream = Substitute.For<Stream>();
            fileAccessLocalDataSource.GetFileSystemStream(filePath).Returns(fileStream);
            transcribeRemoteDataSource
                .TranscribeAndTranslateAsync(Arg.Any<StreamPart>(), sourceLanguage, targetLanguage)
                .ThrowsAsync(exception);

            // When
            Func<Task> act = async () =>
                await repository
                    .TranscribeAndTranslateAsync(filePath, sourceLanguage, targetLanguage)
                    .ConfigureAwait(false);

            // Then
            await act.Should().ThrowAsync<TranscribeException>().ConfigureAwait(false);
            await transcribeRemoteDataSource
                .Received(1)
                .TranscribeAndTranslateAsync(Arg.Any<StreamPart>(), sourceLanguage, targetLanguage)
                .ConfigureAwait(false);
        }

        [Test]
        public async Task HealthCheckAsync_ShouldReturnHealthStatus_WhenNoExceptionOccursAsync()
        {
            // Given
            const string expectedStatus = "Healthy";
            var healthCheckDto = new HealthCheckDto(expectedStatus);
            healthCheckRemoteDataSource.HealthCheckAsync().Returns(healthCheckDto);

            // When
            var result = await repository.HealthCheckAsync().ConfigureAwait(false);

            // Then
            result.Should().Be(expectedStatus);
        }

        [Test]
        public async Task HealthCheckAsync_ShouldThrowNetworkException_WhenHttpRequestExceptionOccursAsync()
        {
            // Given
            healthCheckRemoteDataSource.HealthCheckAsync().ThrowsAsync(new HttpRequestException());

            // When
            Func<Task> act = async () => await repository.HealthCheckAsync().ConfigureAwait(false);

            // Then
            await act.Should().ThrowAsync<NetworkException>().ConfigureAwait(false);
            await healthCheckRemoteDataSource.Received(1).HealthCheckAsync().ConfigureAwait(false);
        }

        [Test]
        public async Task HealthCheckAsync_ShouldThrowHealthCheckException_WhenApiExceptionOccursAsync()
        {
            // Given
            using var httpRequestMessage = new HttpRequestMessage();
            using var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var exception = await ApiException
                .Create(httpRequestMessage, HttpMethod.Post, httpResponseMessage, new RefitSettings())
                .ConfigureAwait(false);
            healthCheckRemoteDataSource.HealthCheckAsync().ThrowsAsync(exception);

            // When
            Func<Task> act = async () => await repository.HealthCheckAsync().ConfigureAwait(false);

            // Then
            await act.Should().ThrowAsync<HealthCheckException>().ConfigureAwait(false);
            await healthCheckRemoteDataSource.Received(1).HealthCheckAsync().ConfigureAwait(false);
        }
    }
}

#pragma warning restore IDISP004, RCS1261, MA0042 // Don't ignore created IDisposable
