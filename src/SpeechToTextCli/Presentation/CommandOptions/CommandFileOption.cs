using System.CommandLine;

namespace SpeechToTextCli.Presentation.CommandOptions;

internal sealed class CommandFileOption : Option<string>
{
    public CommandFileOption()
        : base("--file", "The audio file to transcribe.")
    {
        AddAlias("-f");
        IsRequired = true;

        AddValidator(result =>
        {
            if (result.GetValueOrDefault<string>() == null)
            {
                result.ErrorMessage = "File option is required.";
            }
        });
    }
}
