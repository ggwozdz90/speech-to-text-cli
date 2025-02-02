using FluentAssertions;
using NUnit.Framework;
using SpeechToTextCli.Presentation.CommandOptions;

namespace SpeechToTextCli.Tests.Presentation.CommandOptions
{
    [TestFixture]
    internal sealed class CommandFileOptionTests
    {
        private CommandFileOption commandFileOption = null!;

        [SetUp]
        public void Setup()
        {
            commandFileOption = new CommandFileOption();
        }

        [Test]
        public void CommandFileOption_ShouldHaveCorrectName()
        {
            // Given, When
            var name = commandFileOption.Name;

            // Then
            name.Should().Be("file");
        }

        [Test]
        public void CommandFileOption_ShouldHaveCorrectDescription()
        {
            // Given, When
            var description = commandFileOption.Description;

            // Then
            description.Should().Be("The audio file to transcribe.");
        }

        [Test]
        public void CommandFileOption_ShouldHaveCorrectAlias()
        {
            // Given, When
            var aliases = commandFileOption.Aliases;

            // Then
            aliases.Should().Contain("-f");
        }

        [Test]
        public void CommandFileOption_ShouldBeRequired()
        {
            // Given, When
            var isRequired = commandFileOption.IsRequired;

            // Then
            isRequired.Should().BeTrue();
        }
    }
}
