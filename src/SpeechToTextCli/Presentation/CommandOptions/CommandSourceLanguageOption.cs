using System.CommandLine;
using Microsoft.Extensions.Configuration;
using SpeechToTextCli.Presentation.Validators;

namespace SpeechToTextCli.Presentation.CommandOptions;

internal sealed class CommandSourceLanguageOption : Option<string>
{
    public CommandSourceLanguageOption(IConfiguration configuration, ILanguageCodeValidator languageCodeValidator)
        : base(
            "--source-language",
            () => configuration.GetValue("SpeechToText:SourceLanguage", string.Empty),
            "The source language of the audio in the pattern xx_XX."
        )
    {
        AddAlias("-sl");
        AddValidator(result =>
        {
            var value = result.GetValueOrDefault<string>();
            if (string.IsNullOrEmpty(value) || !languageCodeValidator.IsValid(value))
            {
                result.ErrorMessage = "Source language must be in the pattern xx_XX.";
            }
        });
    }
}
