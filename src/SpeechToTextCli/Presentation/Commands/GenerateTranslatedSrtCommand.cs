using System.CommandLine;
using SpeechToTextCli.Application.UseCases;
using SpeechToTextCli.Presentation.CommandOptions;

namespace SpeechToTextCli.Presentation.Commands;

internal sealed class GenerateTranslatedSrtCommand : Command, IApplicationCommand
{
    public GenerateTranslatedSrtCommand(
        CommandFileOption fileOption,
        CommandSourceLanguageOption sourceLanguageOption,
        CommandTargetLanguageOption targetLanguageOption,
        IGenerateTranslatedSrtUseCase handler
    )
        : base("generate-translated-srt", "Generate translated SRT subtitles from audio file")
    {
        AddAlias("gts");

        AddOption(fileOption);
        AddOption(sourceLanguageOption);
        AddOption(targetLanguageOption);

        this.SetHandler(handler.InvokeAsync, fileOption, sourceLanguageOption, targetLanguageOption);
    }
}
