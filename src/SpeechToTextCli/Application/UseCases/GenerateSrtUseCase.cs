using Microsoft.Extensions.Logging;
using SpeechToTextProcessor.Adapter.Adapters;

namespace SpeechToTextCli.Application.UseCases;

internal interface IGenerateSrtUseCase
{
    Task<int> InvokeAsync(FileInfo? file, string sourceLanguage);
}

internal sealed class GenerateSrtUseCase(ILogger<GenerateSrtUseCase> logger, ISpeechToTextAdapter speechToTextAdapter)
    : IGenerateSrtUseCase
{
    public async Task<int> InvokeAsync(FileInfo? file, string sourceLanguage)
    {
        if (file == null)
        {
            logger.LogError("No file provided.");
            return 1;
        }

        var isHealthy = await speechToTextAdapter.HealthCheckAsync().ConfigureAwait(false);

        if (!string.Equals(isHealthy, "OK", StringComparison.OrdinalIgnoreCase))
        {
            logger.LogError("The speech-to-text API is not healthy.");
            return 1;
        }

        logger.LogInformation("Generating SRT for file: {FullName}", file.FullName);
        var srtFilePath = await speechToTextAdapter
            .TranscribeAsync(file.FullName, sourceLanguage)
            .ConfigureAwait(false);
        logger.LogInformation("SRT file generated: {SrtFilePath}", srtFilePath);

        return 0;
    }
}
