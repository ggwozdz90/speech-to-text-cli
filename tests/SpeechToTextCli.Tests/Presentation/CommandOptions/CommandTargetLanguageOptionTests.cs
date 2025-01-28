using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;
using SpeechToTextCli.Presentation.CommandOptions;
using SpeechToTextCli.Presentation.Validators;

namespace SpeechToTextCli.Tests.Presentation.CommandOptions;

[TestFixture]
internal sealed class CommandTargetLanguageOptionTests
{
    private IConfiguration configuration = null!;
    private ILanguageCodeValidator languageCodeValidator = null!;
    private CommandTargetLanguageOption commandTargetLanguageOption = null!;

    [SetUp]
    public void Setup()
    {
        configuration = Substitute.For<IConfiguration>();
        languageCodeValidator = Substitute.For<ILanguageCodeValidator>();
        commandTargetLanguageOption = new CommandTargetLanguageOption(configuration, languageCodeValidator);
    }

    [Test]
    public void CommandTargetLanguageOption_ShouldHaveCorrectName()
    {
        // Given, When
        var name = commandTargetLanguageOption.Name;

        // Then
        name.Should().Be("target-language");
    }

    [Test]
    public void CommandTargetLanguageOption_ShouldHaveCorrectDescription()
    {
        // Given, When
        var description = commandTargetLanguageOption.Description;

        // Then
        description.Should().Be("The target language of the audio in the pattern xx_XX.");
    }

    [Test]
    public void CommandTargetLanguageOption_ShouldHaveCorrectAlias()
    {
        // Given, When
        var aliases = commandTargetLanguageOption.Aliases;

        // Then
        aliases.Should().Contain("-tl");
    }
}
