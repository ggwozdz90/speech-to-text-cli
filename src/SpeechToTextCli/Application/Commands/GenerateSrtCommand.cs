using System.CommandLine;
using System.CommandLine.Invocation;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Threading;
using SpeechToTextProcessor.Adapters;

namespace SpeechToTextCli.Application.Commands;

internal sealed class GenerateSrtCommand : ICommandHandler
{
    private readonly Option<FileInfo?> _fileOption;
    private readonly ILogger<GenerateSrtCommand> _logger;
    private readonly ISpeechToTextAdapter _speechToTextAdapter;

    public GenerateSrtCommand(ILogger<GenerateSrtCommand> logger, ISpeechToTextAdapter speechToTextAdapter)
    {
        _logger = logger;
        _speechToTextAdapter = speechToTextAdapter;

        _fileOption = new Option<FileInfo?>(
            "--file",
            "The audio file to transcribe.");
        _fileOption.AddAlias("-f");

        Command = new Command("generate-srt", "Generate SRT subtitles from audio file") { _fileOption };
        Command.AddAlias("gs");

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

        _logger.LogInformation("Generating SRT for file: {FullName}", file.FullName);
        var srtFilePath = await _speechToTextAdapter.TranscribeAsync(file.FullName).ConfigureAwait(false);
        _logger.LogInformation("SRT file generated: {SrtFilePath}", srtFilePath);

        return 0;
    }
}
