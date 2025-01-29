using FluentAssertions;
using NUnit.Framework;
using SpeechToTextProcessor.Domain.Exceptions;

namespace SpeechToTextProcessor.Tests.Domain.Exceptions;

[TestFixture]
internal sealed class TranscribeExceptionTests
{
    [Test]
    public void TranscribeException_ShouldInitializeWithDefaultMessage()
    {
        // When
        var exception = new TranscribeException();

        // Then
        exception.Message.Should().Be("An error occurred while transcribing the file.");
    }

    [Test]
    public void TranscribeException_ShouldInitializeWithSpecifiedMessage()
    {
        // Given
        const string expectedMessage = "Custom error message.";

        // When
        var exception = new TranscribeException(expectedMessage);

        // Then
        exception.Message.Should().Be(expectedMessage);
    }

    [Test]
    public void TranscribeException_ShouldInitializeWithInnerException()
    {
        // Given
        var innerException = new InvalidOperationException("Inner exception message.");

        // When
        var exception = new TranscribeException(innerException);

        // Then
        exception.Message.Should().Be("An error occurred while transcribing the file.");
        exception.InnerException.Should().Be(innerException);
    }

    [Test]
    public void TranscribeException_ShouldInitializeWithSpecifiedMessageAndInnerException()
    {
        // Given
        const string expectedMessage = "Custom error message.";
        var innerException = new InvalidOperationException("Inner exception message.");

        // When
        var exception = new TranscribeException(expectedMessage, innerException);

        // Then
        exception.Message.Should().Be(expectedMessage);
        exception.InnerException.Should().Be(innerException);
    }
}
