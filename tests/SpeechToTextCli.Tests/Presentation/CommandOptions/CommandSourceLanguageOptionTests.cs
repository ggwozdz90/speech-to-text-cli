using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;
using SpeechToTextCli.Presentation.CommandOptions;
using SpeechToTextCli.Presentation.Validators;

namespace SpeechToTextCli.Tests.Presentation.CommandOptions;

[TestFixture]
internal sealed class CommandSourceLanguageOptionTests
{
    private IConfiguration configuration = null!;
    private ILanguageCodeValidator languageCodeValidator = null!;
    private CommandSourceLanguageOption commandSourceLanguageOption = null!;

    [SetUp]
    public void Setup()
    {
        configuration = Substitute.For<IConfiguration>();
        languageCodeValidator = Substitute.For<ILanguageCodeValidator>();
        commandSourceLanguageOption = new CommandSourceLanguageOption(configuration, languageCodeValidator);
    }

    [Test]
    public void CommandSourceLanguageOption_ShouldHaveCorrectName()
    {
        // Given, When
        var name = commandSourceLanguageOption.Name;

        // Then
        name.Should().Be("source-language");
    }

    [Test]
    public void CommandSourceLanguageOption_ShouldHaveCorrectDescription()
    {
        // Given, When
        var description = commandSourceLanguageOption.Description;

        // Then
        description.Should().Be("The source language of the audio in the pattern xx_XX.");
    }

    [Test]
    public void CommandSourceLanguageOption_ShouldHaveCorrectAlias()
    {
        // Given, When
        var aliases = commandSourceLanguageOption.Aliases;

        // Then
        aliases.Should().Contain("-sl");
    }
}
