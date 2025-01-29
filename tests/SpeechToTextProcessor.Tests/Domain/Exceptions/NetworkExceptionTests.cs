using FluentAssertions;
using NUnit.Framework;
using SpeechToTextProcessor.Domain.Exceptions;

namespace SpeechToTextProcessor.Tests.Domain.Exceptions;

[TestFixture]
internal sealed class NetworkExceptionTests
{
    [Test]
    public void NetworkException_ShouldInitializeWithDefaultMessage()
    {
        // When
        var exception = new NetworkException();

        // Then
        exception.Message.Should().Be("HTTP error occurred during health check.");
    }

    [Test]
    public void NetworkException_ShouldInitializeWithSpecifiedMessage()
    {
        // Given
        const string expectedMessage = "Custom error message.";

        // When
        var exception = new NetworkException(expectedMessage);

        // Then
        exception.Message.Should().Be(expectedMessage);
    }

    [Test]
    public void NetworkException_ShouldInitializeWithInnerException()
    {
        // Given
        var innerException = new InvalidOperationException("Inner exception message.");

        // When
        var exception = new NetworkException(innerException);

        // Then
        exception.Message.Should().Be("HTTP error occurred during health check.");
        exception.InnerException.Should().Be(innerException);
    }

    [Test]
    public void NetworkException_ShouldInitializeWithSpecifiedMessageAndInnerException()
    {
        // Given
        const string expectedMessage = "Custom error message.";
        var innerException = new InvalidOperationException("Inner exception message.");

        // When
        var exception = new NetworkException(expectedMessage, innerException);

        // Then
        exception.Message.Should().Be(expectedMessage);
        exception.InnerException.Should().Be(innerException);
    }
}
