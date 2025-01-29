using FluentAssertions;
using NUnit.Framework;
using SpeechToTextProcessor.Domain.Exceptions;

namespace SpeechToTextProcessor.Tests.Domain.Exceptions;

[TestFixture]
internal sealed class HealthCheckExceptionTests
{
    [Test]
    public void HealthCheckException_ShouldInitializeWithDefaultMessage()
    {
        // When
        var exception = new HealthCheckException();

        // Then
        exception.Message.Should().Be("API error occurred during health check.");
    }

    [Test]
    public void HealthCheckException_ShouldInitializeWithSpecifiedMessage()
    {
        // Given
        const string expectedMessage = "Custom error message.";

        // When
        var exception = new HealthCheckException(expectedMessage);

        // Then
        exception.Message.Should().Be(expectedMessage);
    }

    [Test]
    public void HealthCheckException_ShouldInitializeWithInnerException()
    {
        // Given
        var innerException = new InvalidOperationException("Inner exception message.");

        // When
        var exception = new HealthCheckException(innerException);

        // Then
        exception.Message.Should().Be("API error occurred during health check.");
        exception.InnerException.Should().Be(innerException);
    }

    [Test]
    public void HealthCheckException_ShouldInitializeWithSpecifiedMessageAndInnerException()
    {
        // Given
        const string expectedMessage = "Custom error message.";
        var innerException = new InvalidOperationException("Inner exception message.");

        // When
        var exception = new HealthCheckException(expectedMessage, innerException);

        // Then
        exception.Message.Should().Be(expectedMessage);
        exception.InnerException.Should().Be(innerException);
    }
}
