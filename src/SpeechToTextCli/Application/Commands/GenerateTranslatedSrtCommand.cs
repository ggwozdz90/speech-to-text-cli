using System.CommandLine;
using System.CommandLine.Invocation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Threading;
using SpeechToTextProcessor.Adapters;

namespace SpeechToTextCli.Application.Commands;

internal sealed class GenerateTranslatedSrtCommand : ICommandHandler
{
    private readonly IConfiguration _configuration;
    private readonly Option<FileInfo?> _fileOption;
    private readonly ILogger<GenerateTranslatedSrtCommand> _logger;
    private readonly ISpeechToTextAdapter _speechToTextAdapter;

    public GenerateTranslatedSrtCommand(ILogger<GenerateTranslatedSrtCommand> logger,
        ISpeechToTextAdapter speechToTextAdapter, IConfiguration configuration)
    {
        _logger = logger;
        _speechToTextAdapter = speechToTextAdapter;
        _configuration = configuration;
        _fileOption = new Option<FileInfo?>(
            "--file",
            "The audio file to transcribe and translate.");
        _fileOption.AddAlias("-f");
        Command = new Command("generate-translated-srt", "Generate translated SRT subtitles from audio file")
        {
            _fileOption,
        };
        Command.AddAlias("gts");
        Command.Handler = this;
    }

    public Command Command { get; }

    public int Invoke(InvocationContext context)
    {
        using var taskContext = new JoinableTaskContext();
        var taskFactory = new JoinableTaskFactory(taskContext);
        return taskFactory.Run(async () => await InvokeAsync(context).ConfigureAwait(false));
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        var file = context.ParseResult.GetValueForOption(_fileOption);

        if (file == null)
        {
            _logger.LogError("No file provided.");
            return 1;
        }

        var targetLanguage = _configuration["Translation:TargetLanguage"];

        if (string.IsNullOrWhiteSpace(targetLanguage))
        {
            _logger.LogError("No target language provided.");
            return 1;
        }

        _logger.LogInformation("Generating translated SRT for file: {FullName}", file.FullName);
        var srtFilePath = await _speechToTextAdapter.TranscribeAndTranslateAsync(file.FullName, targetLanguage).ConfigureAwait(false);
        _logger.LogInformation("Translated SRT file generated: {SrtFilePath}", srtFilePath);

        return 0;
    }
}
