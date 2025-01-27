using System.CommandLine;

namespace SpeechToTextCli.Presentation.CommandOptions;

internal sealed class CommandFileOption : Option<FileInfo>
{
    public CommandFileOption()
        : base("--file", "The audio file to transcribe.")
    {
        AddAlias("-f");
        IsRequired = true;
        AddValidator(result =>
        {
            if (result.GetValueOrDefault<FileInfo>() == null)
            {
                result.ErrorMessage = "File option is required.";
            }
        });
    }
}
