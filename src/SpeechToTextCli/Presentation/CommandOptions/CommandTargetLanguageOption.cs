using System.CommandLine;
using Microsoft.Extensions.Configuration;
using SpeechToTextCli.Presentation.Validators;

namespace SpeechToTextCli.Presentation.CommandOptions;

internal sealed class CommandTargetLanguageOption : Option<string>
{
    public CommandTargetLanguageOption(IConfiguration configuration, ILanguageCodeValidator languageCodeValidator)
        : base(
            "--target-language",
            () => configuration.GetValue("SpeechToText:TargetLanguage", string.Empty),
            "The target language of the audio in the pattern xx_XX."
        )
    {
        AddAlias("-tl");
        AddValidator(result =>
        {
            var value = result.GetValueOrDefault<string>();
            if (string.IsNullOrEmpty(value) || !languageCodeValidator.IsValid(value))
            {
                result.ErrorMessage = "Target language must be in the pattern xx_XX.";
            }
        });
    }
}
