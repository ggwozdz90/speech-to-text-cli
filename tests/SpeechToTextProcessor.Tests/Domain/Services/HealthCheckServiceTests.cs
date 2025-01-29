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
internal sealed class HealthCheckServiceTests
{
    private ILogger<HealthCheckService> logger = null!;
    private ISpeechToTextRepository repository = null!;
    private HealthCheckService service = null!;

    [SetUp]
    public void SetUp()
    {
        logger = Substitute.For<ILogger<HealthCheckService>>();
        repository = Substitute.For<ISpeechToTextRepository>();
        service = new HealthCheckService(logger, repository);
    }

    [Test]
    public async Task HealthCheckAsync_ShouldReturnHealthStatus_WhenNoExceptionOccursAsync()
    {
        // Given
        const string expectedStatus = "OK";
        repository.HealthCheckAsync().Returns(expectedStatus);

        // When
        var result = await service.HealthCheckAsync().ConfigureAwait(false);

        // Then
        result.Should().Be(expectedStatus);
    }

    [Test]
    public async Task HealthCheckAsync_ShouldThrowNetworkException_WhenNetworkExceptionOccursAsync()
    {
        // Given
        repository.HealthCheckAsync().ThrowsAsync(new NetworkException());

        // When
        Func<Task> act = async () => await service.HealthCheckAsync().ConfigureAwait(false);

        // Then
        await act.Should().ThrowAsync<NetworkException>().ConfigureAwait(false);
        await repository.Received(1).HealthCheckAsync().ConfigureAwait(false);
    }

    [Test]
    public async Task HealthCheckAsync_ShouldThrowHealthCheckException_WhenHealthCheckExceptionOccursAsync()
    {
        // Given
        repository.HealthCheckAsync().ThrowsAsync(new HealthCheckException());

        // When
        Func<Task> act = async () => await service.HealthCheckAsync().ConfigureAwait(false);

        // Then
        await act.Should().ThrowAsync<HealthCheckException>().ConfigureAwait(false);
        await repository.Received(1).HealthCheckAsync().ConfigureAwait(false);
    }

    [Test]
    public async Task HealthCheckAsync_ShouldLogErrorAndThrow_WhenUnexpectedExceptionOccursAsync()
    {
        // Given
        var unexpectedException = new InvalidOperationException("Unexpected error");
        repository.HealthCheckAsync().ThrowsAsync(unexpectedException);

        // When
        Func<Task> act = async () => await service.HealthCheckAsync().ConfigureAwait(false);

        // Then
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Unexpected error")
            .ConfigureAwait(false);
        await repository.Received(1).HealthCheckAsync().ConfigureAwait(false);
    }
}
