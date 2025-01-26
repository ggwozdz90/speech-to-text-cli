namespace SpeechToTextProcessor.Domain.Exceptions;

/// <summary>
///     Exception thrown when an error occurs during health check process.
/// </summary>
public class HealthCheckException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="HealthCheckException"/> class with a default error message.
    /// </summary>
    public HealthCheckException()
        : base("API error occurred during health check.") { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="HealthCheckException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public HealthCheckException(string message)
        : base(message) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="HealthCheckException"/> class with a default error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="innerException">The inner exception.</param>
    public HealthCheckException(Exception innerException)
        : base("API error occurred during health check.", innerException) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="HealthCheckException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public HealthCheckException(string message, Exception innerException)
        : base(message, innerException) { }
}
