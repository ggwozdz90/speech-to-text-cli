namespace SpeechToTextCli.Domain.ErrorCodes;

internal static class ErrorCode
{
    public const int Success = 0;
    public const int NetworkError = 2;
    public const int FileAccessError = 3;
    public const int HealthCheckError = 4;
    public const int TranscribeError = 5;
}
