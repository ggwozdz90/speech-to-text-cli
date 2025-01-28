using FluentAssertions;
using NUnit.Framework;
using SpeechToTextCli.Presentation.Validators;

namespace SpeechToTextCli.Tests.Presentation.Validators;

[TestFixture]
internal sealed class LanguageCodeValidatorTests
{
    private LanguageCodeValidator validator = null!;

    [SetUp]
    public void Setup()
    {
        validator = new LanguageCodeValidator();
    }

    [Test]
    public void IsValid_ShouldReturnTrue_WhenLanguageCodeIsValid()
    {
        // Given
        const string validLanguageCode = "en_US";

        // When
        var result = validator.IsValid(validLanguageCode);

        // Then
        result.Should().BeTrue();
    }

    [Test]
    public void IsValid_ShouldReturnFalse_WhenLanguageCodeIsTooShort()
    {
        // Given
        const string invalidLanguageCode = "en_U";

        // When
        var result = validator.IsValid(invalidLanguageCode);

        // Then
        result.Should().BeFalse();
    }

    [Test]
    public void IsValid_ShouldReturnFalse_WhenLanguageCodeIsTooLong()
    {
        // Given
        const string invalidLanguageCode = "en_USA";

        // When
        var result = validator.IsValid(invalidLanguageCode);

        // Then
        result.Should().BeFalse();
    }

    [Test]
    public void IsValid_ShouldReturnFalse_WhenFirstTwoCharactersAreNotLowercase()
    {
        // Given
        const string invalidLanguageCode = "EN_US";

        // When
        var result = validator.IsValid(invalidLanguageCode);

        // Then
        result.Should().BeFalse();
    }

    [Test]
    public void IsValid_ShouldReturnFalse_WhenThirdCharacterIsNotUnderscore()
    {
        // Given
        const string invalidLanguageCode = "enUS";

        // When
        var result = validator.IsValid(invalidLanguageCode);

        // Then
        result.Should().BeFalse();
    }

    [Test]
    public void IsValid_ShouldReturnFalse_WhenLastTwoCharactersAreNotUppercase()
    {
        // Given
        const string invalidLanguageCode = "en_us";

        // When
        var result = validator.IsValid(invalidLanguageCode);

        // Then
        result.Should().BeFalse();
    }
}
