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
internal sealed class GenerateSrtCommandTests
{
    private CommandFileOption fileOption = null!;
    private CommandSourceLanguageOption sourceLanguageOption = null!;
    private IFileSystem fileSystem = null!;
    private IGenerateSrtUseCase handler = null!;
    private GenerateSrtCommand generateSrtCommand = null!;

    [SetUp]
    public void Setup()
    {
        fileOption = new CommandFileOption();
        sourceLanguageOption = new CommandSourceLanguageOption(
            Substitute.For<IConfiguration>(),
            Substitute.For<ILanguageCodeValidator>()
        );
        fileSystem = Substitute.For<IFileSystem>();
        handler = Substitute.For<IGenerateSrtUseCase>();
        generateSrtCommand = new GenerateSrtCommand(fileOption, sourceLanguageOption, fileSystem, handler);
    }

    [Test]
    public void GenerateSrtCommand_ShouldHaveCorrectName()
    {
        // Given, When
        var name = generateSrtCommand.Name;

        // Then
        name.Should().Be("generate-srt");
    }

    [Test]
    public void GenerateSrtCommand_ShouldHaveCorrectDescription()
    {
        // Given, When
        var description = generateSrtCommand.Description;

        // Then
        description.Should().Be("Generate SRT subtitles from audio file");
    }

    [Test]
    public void GenerateSrtCommand_ShouldHaveCorrectAlias()
    {
        // Given, When
        var aliases = generateSrtCommand.Aliases;

        // Then
        aliases.Should().Contain("gs");
    }
}
