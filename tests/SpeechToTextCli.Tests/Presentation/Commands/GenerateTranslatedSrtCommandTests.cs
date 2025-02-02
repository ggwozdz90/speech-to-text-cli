using System.IO.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;
using SpeechToTextCli.Application.UseCases;
using SpeechToTextCli.Presentation.CommandOptions;
using SpeechToTextCli.Presentation.Commands;
using SpeechToTextCli.Presentation.Validators;

namespace SpeechToTextCli.Tests.Presentation.Commands;

[TestFixture]
internal sealed class GenerateTranslatedSrtCommandTests
{
    private CommandFileOption fileOption = null!;
    private CommandSourceLanguageOption sourceLanguageOption = null!;
    private CommandTargetLanguageOption targetLanguageOption = null!;
    private IFileSystem fileSystem = null!;
    private IGenerateTranslatedSrtUseCase handler = null!;
    private GenerateTranslatedSrtCommand generateTranslatedSrtCommand = null!;

    [SetUp]
    public void Setup()
    {
        fileOption = new CommandFileOption();
        sourceLanguageOption = new CommandSourceLanguageOption(
            Substitute.For<IConfiguration>(),
            Substitute.For<ILanguageCodeValidator>()
        );
        targetLanguageOption = new CommandTargetLanguageOption(
            Substitute.For<IConfiguration>(),
            Substitute.For<ILanguageCodeValidator>()
        );
        fileSystem = Substitute.For<IFileSystem>();
        handler = Substitute.For<IGenerateTranslatedSrtUseCase>();
        generateTranslatedSrtCommand = new GenerateTranslatedSrtCommand(
            fileOption,
            sourceLanguageOption,
            targetLanguageOption,
            fileSystem,
            handler
        );
    }

    [Test]
    public void GenerateTranslatedSrtCommand_ShouldHaveCorrectName()
    {
        // Given, When
        var name = generateTranslatedSrtCommand.Name;

        // Then
        name.Should().Be("generate-translated-srt");
    }

    [Test]
    public void GenerateTranslatedSrtCommand_ShouldHaveCorrectDescription()
    {
        // Given, When
        var description = generateTranslatedSrtCommand.Description;

        // Then
        description.Should().Be("Generate translated SRT subtitles from audio file");
    }

    [Test]
    public void GenerateTranslatedSrtCommand_ShouldHaveCorrectAlias()
    {
        // Given, When
        var aliases = generateTranslatedSrtCommand.Aliases;

        // Then
        aliases.Should().Contain("gts");
    }
}
