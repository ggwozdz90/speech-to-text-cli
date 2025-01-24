using System.CommandLine;
using SpeechToTextCli.Application.UseCases;

namespace SpeechToTextCli.Presentation.Commands;

internal sealed class GenerateTranslatedSrtCommand : Command, IApplicationCommand
{
    public GenerateTranslatedSrtCommand(IGenerateTranslatedSrtUseCase handler)
    : base(
        "generate-translated-srt",
        "Generate translated SRT subtitles from audio file")
    {
        AddAlias("gts");

        var fileOption = new Option<FileInfo?>(
            "--file",
            "The audio file to transcribe and translate.");
        fileOption.AddAlias("-f");

        AddOption(fileOption);

        this.SetHandler(handler.InvokeAsync, fileOption);
    }
}
