using System.CommandLine;
using SpeechToTextCli.Application.UseCases;

namespace SpeechToTextCli.Presentation.Commands;

internal sealed class GenerateSrtCommand : Command, IApplicationCommand
{
    public GenerateSrtCommand(IGenerateSrtUseCase handler)
        : base("generate-srt", "Generate SRT subtitles from audio file")
    {
        AddAlias("gs");

        var fileOption = new Option<FileInfo?>("--file", "The audio file to transcribe.");
        fileOption.AddAlias("-f");
        fileOption.IsRequired = true;
        AddOption(fileOption);

        var sourceLanguageOption = new Option<string>(
            "--source-language",
            "The source language of the audio in the pattern xx_XX."
        );
        sourceLanguageOption.AddAlias("-sl");
        sourceLanguageOption.SetDefaultValue("en_US");
        AddOption(sourceLanguageOption);

        this.SetHandler(handler.InvokeAsync, fileOption, sourceLanguageOption);
    }
}
