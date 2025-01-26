namespace SpeechToTextCli.Presentation.Validators;

internal static class LanguageCodeValidator
{
    public static bool IsValid(string languageCode)
    {
        return languageCode.Length == 5
            && char.IsLower(languageCode[0])
            && char.IsLower(languageCode[1])
            && languageCode[2] == '_'
            && char.IsUpper(languageCode[3])
            && char.IsUpper(languageCode[4]);
    }
}
