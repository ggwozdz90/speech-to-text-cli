using System.CommandLine;
using System.CommandLine.Invocation;
using Microsoft.Extensions.Logging;
using SpeechToTextProcessor.Adapters;

namespace SpeechToTextCli.Commands;

public class GenerateSrtCommand : ICommandHandler
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

        Command = new Command("generate-srt", "Generate SRT subtitles from audio file")
        {
            _fileOption
        };
        Command.AddAlias("gs");

        Command.Handler = this;
    }

    public Command Command { get; }

    public int Invoke(InvocationContext context)
    {
        return InvokeAsync(context).GetAwaiter().GetResult();
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        var file = context.ParseResult.GetValueForOption(_fileOption);

        if (file == null)
        {
            _logger.LogError("No file provided.");
            return 1;
        }

        _logger.LogInformation($"Generating SRT for file: {file.FullName}");
        var srtFilePath = await _speechToTextAdapter.TranscribeAsync(file.FullName);
        _logger.LogInformation($"SRT file generated: {srtFilePath}");

        return 0;
    }
}