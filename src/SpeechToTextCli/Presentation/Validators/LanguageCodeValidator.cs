namespace SpeechToTextCli.Presentation.Validators;

internal interface ILanguageCodeValidator
{
    bool IsValid(string languageCode);
}

internal sealed class LanguageCodeValidator : ILanguageCodeValidator
{
    public bool IsValid(string languageCode)
    {
        return languageCode.Length == 5
            && char.IsLower(languageCode[0])
            && char.IsLower(languageCode[1])
            && languageCode[2] == '_'
            && char.IsUpper(languageCode[3])
            && char.IsUpper(languageCode[4]);
    }
}
