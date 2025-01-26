using System.CommandLine;
using Microsoft.Extensions.Configuration;
using SpeechToTextCli.Application.UseCases;
using SpeechToTextCli.Presentation.Validators;

namespace SpeechToTextCli.Presentation.Commands;

internal sealed class GenerateSrtCommand : Command, IApplicationCommand
{
    public GenerateSrtCommand(IConfiguration configuration, IGenerateSrtUseCase handler)
        : base("generate-srt", "Generate SRT subtitles from audio file")
    {
        AddAlias("gs");

        var fileOption = AddFileOption();
        var sourceLanguageOption = AddSourceLanguageOption(configuration);

        this.SetHandler(handler.InvokeAsync, fileOption, sourceLanguageOption);
    }

    private Option<FileInfo> AddFileOption()
    {
        var fileOption = new Option<FileInfo>("--file", "The audio file to transcribe.");

        fileOption.AddAlias("-f");
        fileOption.IsRequired = true;
        fileOption.AddValidator(result =>
        {
            if (result.GetValueOrDefault<FileInfo>() == null)
            {
                result.ErrorMessage = "File option is required.";
            }
        });
        AddOption(fileOption);

        return fileOption;
    }

    private Option<string> AddSourceLanguageOption(IConfiguration configuration)
    {
        var sourceLanguageOption = new Option<string>(
            "--source-language",
            () => configuration.GetValue("SpeechToText:SourceLanguage", string.Empty),
            "The source language of the audio in the pattern xx_XX."
        );

        sourceLanguageOption.AddAlias("-sl");
        sourceLanguageOption.AddValidator(result =>
        {
            var value = result.GetValueOrDefault<string>();
            if (string.IsNullOrEmpty(value) || !LanguageCodeValidator.IsValid(value))
            {
                result.ErrorMessage = "Source language must be in the pattern xx_XX.";
            }
        });
        AddOption(sourceLanguageOption);

        return sourceLanguageOption;
    }
}
