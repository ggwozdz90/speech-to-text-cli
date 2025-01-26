namespace SpeechToTextProcessor.Domain.Exceptions;

/// <summary>
///     Exception thrown when an error occurs during the transcription process.
/// </summary>
public class TranscribeException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="TranscribeException"/> class with a default error message.
    /// </summary>
    public TranscribeException()
        : base("An error occurred while transcribing the file.") { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TranscribeException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public TranscribeException(string message)
        : base(message) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TranscribeException"/> class with a default error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="innerException">The inner exception.</param>
    public TranscribeException(Exception innerException)
        : base("An error occurred while transcribing the file.", innerException) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TranscribeException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public TranscribeException(string message, Exception innerException)
        : base(message, innerException) { }
}
