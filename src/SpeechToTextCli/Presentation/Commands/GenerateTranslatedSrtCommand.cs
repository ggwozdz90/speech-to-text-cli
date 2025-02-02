using System.CommandLine;
using System.IO.Abstractions;
using SpeechToTextCli.Application.UseCases;
using SpeechToTextCli.Presentation.CommandOptions;

namespace SpeechToTextCli.Presentation.Commands;

internal sealed class GenerateTranslatedSrtCommand : Command, IApplicationCommand
{
    public GenerateTranslatedSrtCommand(
        CommandFileOption fileOption,
        CommandSourceLanguageOption sourceLanguageOption,
        CommandTargetLanguageOption targetLanguageOption,
        IFileSystem fileSystem,
        IGenerateTranslatedSrtUseCase handler
    )
        : base("generate-translated-srt", "Generate translated SRT subtitles from audio file")
    {
        AddAlias("gts");

        AddOption(fileOption);
        AddOption(sourceLanguageOption);
        AddOption(targetLanguageOption);

        this.SetHandler(
            async (string filePath, string sourceLanguage, string targetLanguage) =>
            {
                var fileInfo = fileSystem.FileInfo.New(filePath);
                await handler.InvokeAsync(fileInfo, sourceLanguage, targetLanguage).ConfigureAwait(false);
            },
            fileOption,
            sourceLanguageOption,
            targetLanguageOption
        );
    }
}
