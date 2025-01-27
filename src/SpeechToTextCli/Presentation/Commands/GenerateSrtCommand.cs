using System.CommandLine;
using SpeechToTextCli.Application.UseCases;
using SpeechToTextCli.Presentation.CommandOptions;

namespace SpeechToTextCli.Presentation.Commands;

internal sealed class GenerateSrtCommand : Command, IApplicationCommand
{
    public GenerateSrtCommand(
        CommandFileOption fileOption,
        CommandSourceLanguageOption sourceLanguageOption,
        IGenerateSrtUseCase handler
    )
        : base("generate-srt", "Generate SRT subtitles from audio file")
    {
        AddAlias("gs");

        AddOption(fileOption);
        AddOption(sourceLanguageOption);

        this.SetHandler(handler.InvokeAsync, fileOption, sourceLanguageOption);
    }
}
