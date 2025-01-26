using System.CommandLine;
using Microsoft.Extensions.Configuration;
using SpeechToTextCli.Application.UseCases;
using SpeechToTextCli.Presentation.Validators;

namespace SpeechToTextCli.Presentation.Commands;

internal sealed class GenerateTranslatedSrtCommand : Command, IApplicationCommand
{
    public GenerateTranslatedSrtCommand(IConfiguration configuration, IGenerateTranslatedSrtUseCase handler)
        : base("generate-translated-srt", "Generate translated SRT subtitles from audio file")
    {
        AddAlias("gts");

        var fileOption = AddFileOption();
        var sourceLanguageOption = AddSourceLanguageOption(configuration);
        var targetLanguageOption = AddTargetLanguageOption(configuration);

        this.SetHandler(handler.InvokeAsync, fileOption, sourceLanguageOption, targetLanguageOption);
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

    private Option<string> AddTargetLanguageOption(IConfiguration configuration)
    {
        var targetLanguageOption = new Option<string>(
            "--target-language",
            () => configuration.GetValue("SpeechToText:TargetLanguage", string.Empty),
            "The target language of the audio in the pattern xx_XX."
        );

        targetLanguageOption.AddAlias("-tl");
        targetLanguageOption.AddValidator(result =>
        {
            var value = result.GetValueOrDefault<string>();
            if (string.IsNullOrEmpty(value) || !LanguageCodeValidator.IsValid(value))
            {
                result.ErrorMessage = "Target language must be in the pattern xx_XX.";
            }
        });
        AddOption(targetLanguageOption);

        return targetLanguageOption;
    }
}
