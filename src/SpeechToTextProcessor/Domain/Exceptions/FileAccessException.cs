namespace SpeechToTextProcessor.Domain.Exceptions;

/// <summary>
///     Exception thrown when an error occurs during file access operations.
/// </summary>
public class FileAccessException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="FileAccessException"/> class with a default error message.
    /// </summary>
    public FileAccessException()
        : base("Error occurred during file access operation.") { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="FileAccessException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public FileAccessException(string message)
        : base(message) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="FileAccessException"/> class with a default error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="innerException">The inner exception.</param>
    public FileAccessException(Exception innerException)
        : base("Error occurred during file access operation.", innerException) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="FileAccessException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public FileAccessException(string message, Exception innerException)
        : base(message, innerException) { }
}
