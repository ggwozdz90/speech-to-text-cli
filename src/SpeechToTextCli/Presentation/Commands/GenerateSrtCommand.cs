using System.CommandLine;
using SpeechToTextCli.Application.UseCases;

namespace SpeechToTextCli.Presentation.Commands;

internal sealed class GenerateSrtCommand : Command, IApplicationCommand
{
    public GenerateSrtCommand(IGenerateSrtUseCase handler)
    : base(
        "generate-srt",
        "Generate SRT subtitles from audio file")
    {
        AddAlias("gs");

        var fileOption = new Option<FileInfo?>(
            "--file",
            "The audio file to transcribe.");
        fileOption.AddAlias("-f");

        AddOption(fileOption);

        this.SetHandler(handler.InvokeAsync, fileOption);
    }
}
