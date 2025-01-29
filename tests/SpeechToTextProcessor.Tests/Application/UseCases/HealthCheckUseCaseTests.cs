using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using SpeechToTextProcessor.Application.UseCases;
using SpeechToTextProcessor.Domain.Services;

namespace SpeechToTextProcessor.Tests.Application.UseCases;

[TestFixture]
internal sealed class HealthCheckUseCaseTests
{
    private ILogger<HealthCheckUseCase> logger = null!;
    private IHealthCheckService healthCheckService = null!;
    private HealthCheckUseCase healthCheckUseCase = null!;

    [SetUp]
    public void Setup()
    {
        logger = Substitute.For<ILogger<HealthCheckUseCase>>();
        healthCheckService = Substitute.For<IHealthCheckService>();
        healthCheckUseCase = new HealthCheckUseCase(logger, healthCheckService);
    }

    [Test]
    public async Task InvokeAsync_ShouldReturnOk_WhenHealthCheckIsSuccessfulAsync()
    {
        // Given
        const string expectedResult = "OK";
        healthCheckService.HealthCheckAsync().Returns(expectedResult);

        // When
        var result = await healthCheckUseCase.InvokeAsync().ConfigureAwait(false);

        // Then
        result.Should().Be(expectedResult);
        await healthCheckService.Received(1).HealthCheckAsync().ConfigureAwait(false);
        logger
            .Received(1)
            .Log(
                LogLevel.Trace,
                Arg.Any<EventId>(),
                Arg.Is<object>(v => v.ToString() == "Health check invoked from use case..."),
                exception: null,
                Arg.Any<Func<object, Exception?, string>>()
            );
    }

    [Test]
    public async Task InvokeAsync_ShouldThrowException_WhenHealthCheckFailsAsync()
    {
        // Given
        healthCheckService.HealthCheckAsync().ThrowsAsync(new InvalidOperationException("Health check failed"));

        // When
        Func<Task> act = async () => await healthCheckUseCase.InvokeAsync().ConfigureAwait(false);

        // Then
        await act.Should().ThrowAsync<Exception>().WithMessage("Health check failed").ConfigureAwait(false);
        await healthCheckService.Received(1).HealthCheckAsync().ConfigureAwait(false);
        logger
            .Received(1)
            .Log(
                LogLevel.Trace,
                Arg.Any<EventId>(),
                Arg.Is<object>(v => v.ToString() == "Health check invoked from use case..."),
                exception: null,
                Arg.Any<Func<object, Exception?, string>>()
            );
    }
}
