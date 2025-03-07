using System.CommandLine;
using System.IO.Abstractions;
using SpeechToTextCli.Application.UseCases;
using SpeechToTextCli.Presentation.CommandOptions;

namespace SpeechToTextCli.Presentation.Commands;

internal sealed class GenerateSrtCommand : Command, IApplicationCommand
{
    public GenerateSrtCommand(
        CommandFileOption fileOption,
        CommandSourceLanguageOption sourceLanguageOption,
        IFileSystem fileSystem,
        IGenerateSrtUseCase handler
    )
        : base("generate-srt", "Generate SRT subtitles from audio file")
    {
        AddAlias("gs");

        AddOption(fileOption);
        AddOption(sourceLanguageOption);

        this.SetHandler(
            async (string filePath, string sourceLanguage) =>
            {
                var fileInfo = fileSystem.FileInfo.New(filePath);
                await handler.InvokeAsync(fileInfo, sourceLanguage).ConfigureAwait(false);
            },
            fileOption,
            sourceLanguageOption
        );
    }
}
