using System.CommandLine;
using System.IO.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using SpeechToTextApiClient.Adapter.Adapters;
using SpeechToTextCli.Application.UseCases;
using SpeechToTextCli.DependencyInjection;
using SpeechToTextCli.Domain.Repositories;
using SpeechToTextCli.Domain.Services;
using SpeechToTextCli.Presentation.CommandOptions;
using SpeechToTextCli.Presentation.Commands;
using SpeechToTextCli.Presentation.Validators;

namespace SpeechToTextCli.Tests.DependencyInjection;

[TestFixture]
internal sealed class ServiceCollectionExtensionsTests
{
    private IServiceCollection services = null!;
    private ISpeechToTextAdapter speechToTextAdapterMock = null!;

    [SetUp]
    public void Setup()
    {
        speechToTextAdapterMock = Substitute.For<ISpeechToTextAdapter>();
        services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
        services.AddScoped<IFileSystem, FileSystem>();
        services.AddLogging();
        services.AddSingleton(speechToTextAdapterMock);
    }

    [Test]
    public void AddInternalDependencies_ShouldRegisterCommandOptions()
    {
        // Given, When
        services.AddInternalDependencies();
        using var serviceProvider = services.BuildServiceProvider();

        // Then
        serviceProvider.GetService<CommandFileOption>().Should().NotBeNull();
        serviceProvider.GetService<CommandSourceLanguageOption>().Should().NotBeNull();
        serviceProvider.GetService<CommandTargetLanguageOption>().Should().NotBeNull();
    }

    [Test]
    public void AddInternalDependencies_ShouldRegisterApplicationCommands()
    {
        // Given, When
        services.AddInternalDependencies();
        using var serviceProvider = services.BuildServiceProvider();

        // Then
        serviceProvider.GetService<IApplicationCommand>().Should().NotBeNull();
        serviceProvider
            .GetServices<IApplicationCommand>()
            .Should()
            .ContainSingle(command => command is GenerateSrtCommand);
        serviceProvider
            .GetServices<IApplicationCommand>()
            .Should()
            .ContainSingle(command => command is GenerateTranslatedSrtCommand);
    }

    [Test]
    public void AddInternalDependencies_ShouldRegisterValidators()
    {
        // Given, When
        services.AddInternalDependencies();
        using var serviceProvider = services.BuildServiceProvider();

        // Then
        serviceProvider.GetService<ILanguageCodeValidator>().Should().NotBeNull();
    }

    [Test]
    public void AddInternalDependencies_ShouldRegisterUseCases()
    {
        // Given, When
        services.AddInternalDependencies();
        using var serviceProvider = services.BuildServiceProvider();

        // Then
        serviceProvider.GetService<IGenerateSrtUseCase>().Should().NotBeNull();
        serviceProvider.GetService<IGenerateTranslatedSrtUseCase>().Should().NotBeNull();
    }

    [Test]
    public void AddInternalDependencies_ShouldRegisterServices()
    {
        // Given, When
        services.AddInternalDependencies();
        using var serviceProvider = services.BuildServiceProvider();

        // Then
        serviceProvider.GetService<ISrtGenerationService>().Should().NotBeNull();
    }

    [Test]
    public void AddInternalDependencies_ShouldRegisterRepositories()
    {
        // Given, When
        services.AddInternalDependencies();
        using var serviceProvider = services.BuildServiceProvider();

        // Then
        serviceProvider.GetService<IFileRepository>().Should().NotBeNull();
        serviceProvider.GetService<ISrtGenerationRepository>().Should().NotBeNull();
    }

    [Test]
    public void AddInternalDependencies_ShouldRegisterRootCommand()
    {
        // Given, When
        services.AddInternalDependencies();
        using var serviceProvider = services.BuildServiceProvider();

        // Then
        var rootCommand = serviceProvider.GetService<RootCommand>();
        rootCommand.Should().NotBeNull();
        rootCommand.Description.Should().Be("Speech to text API CLI");
        rootCommand.Subcommands.Should().ContainSingle(command => command is GenerateSrtCommand);
        rootCommand.Subcommands.Should().ContainSingle(command => command is GenerateTranslatedSrtCommand);
    }
}
