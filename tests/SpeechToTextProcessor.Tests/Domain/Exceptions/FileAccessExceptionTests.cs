using FluentAssertions;
using NUnit.Framework;
using SpeechToTextProcessor.Domain.Exceptions;

namespace SpeechToTextProcessor.Tests.Domain.Exceptions;

[TestFixture]
internal sealed class FileAccessExceptionTests
{
    [Test]
    public void FileAccessException_ShouldInitializeWithDefaultMessage()
    {
        // When
        var exception = new FileAccessException();

        // Then
        exception.Message.Should().Be("Error occurred during file access operation.");
    }

    [Test]
    public void FileAccessException_ShouldInitializeWithSpecifiedMessage()
    {
        // Given
        const string expectedMessage = "Custom error message.";

        // When
        var exception = new FileAccessException(expectedMessage);

        // Then
        exception.Message.Should().Be(expectedMessage);
    }

    [Test]
    public void FileAccessException_ShouldInitializeWithInnerException()
    {
        // Given
        var innerException = new InvalidOperationException("Inner exception message.");

        // When
        var exception = new FileAccessException(innerException);

        // Then
        exception.Message.Should().Be("Error occurred during file access operation.");
        exception.InnerException.Should().Be(innerException);
    }

    [Test]
    public void FileAccessException_ShouldInitializeWithSpecifiedMessageAndInnerException()
    {
        // Given
        const string expectedMessage = "Custom error message.";
        var innerException = new InvalidOperationException("Inner exception message.");

        // When
        var exception = new FileAccessException(expectedMessage, innerException);

        // Then
        exception.Message.Should().Be(expectedMessage);
        exception.InnerException.Should().Be(innerException);
    }
}
