# Best Practices and C# Testing Principles

1. **Test Naming**:
   - Use descriptive names for test methods that clearly indicate what is being tested.
   - Follow a clear pattern, such as `Test_Functionality_Scenario`.

2. **Given-When-Then Structure**:
   - Structure your tests using the Given-When-Then format.
     - **Given**: Set up the initial context or state.
     - **When**: Execute the action or method being tested.
     - **Then**: Assert the expected outcome.

3. **Use of Fixtures**:
   - Utilize NUnit fixtures to set up any necessary configurations, dependencies, or mock objects.

4. **Mocking**:
   - Use NSubstitute or similar libraries to mock external dependencies and functions.

5. **Assertions**:
   - Use FluentAssertions for more readable and expressive assertions.

## Coverage of Class Code

- Ensure that the tests cover the main functionality of the class or method being tested.
- Include tests for:
  - **Success Scenarios**: Verify that the method behaves as expected under normal conditions.
  - **Failure Scenarios**: Verify that the method handles errors and exceptions gracefully.
  - **Edge Cases**: Test boundary conditions and unusual inputs.
- Avoid redundant tests that do not add value or test the same functionality multiple times.

## Workarounds and Code Quality

- Avoid workarounds or anti-patterns in the test code.
- Ensure that the test code is straightforward and readable.

## Example Test Code

```csharp
using System.IO.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using SpeechToTextCli.Application.UseCases;
using SpeechToTextCli.Domain.ErrorCodes;
using SpeechToTextCli.Domain.Services;
using SpeechToTextProcessor.Domain.Exceptions;

namespace SpeechToTextCli.Tests.Application.UseCases;

[TestFixture]
internal sealed class GenerateSrtUseCaseTest
{
    private ILogger<GenerateSrtUseCase> logger;
    private ISrtGenerationService srtGenerationService;
    private IFileInfo file;
    private GenerateSrtUseCase useCase;

    [SetUp]
    public void Setup()
    {
        logger = Substitute.For<ILogger<GenerateSrtUseCase>>();
        srtGenerationService = Substitute.For<ISrtGenerationService>();
        file = Substitute.For<IFileInfo>();
        file.FullName.Returns("testfile.txt");
        useCase = new GenerateSrtUseCase(logger, srtGenerationService);
    }

    [Test]
    public async Task InvokeAsync_ShouldReturnSuccess_WhenNoExceptionOccursAsync()
    {
        // Given
        const string SourceLanguage = "en";

        // When
        var result = await useCase.InvokeAsync(file, SourceLanguage).ConfigureAwait(false);

        // Then
        result.Should().Be(ErrorCode.Success);
        await srtGenerationService.Received(1).GenerateSrtAsync(file.FullName, SourceLanguage).ConfigureAwait(false);
    }

   [Test]
    public async Task InvokeAsync_ShouldReturnNetworkError_WhenNetworkExceptionOccursAsync()
    {
        // Given
        const string SourceLanguage = "en";
        srtGenerationService.GenerateSrtAsync(file.FullName, SourceLanguage).ThrowsAsync(new NetworkException());

        // When
        var result = await useCase.InvokeAsync(file, SourceLanguage).ConfigureAwait(false);

        // Then
        result.Should().Be(ErrorCode.NetworkError);
        logger
            .Received(1)
            .Log(
                Arg.Is<LogLevel>(level => level == LogLevel.Error),
                Arg.Any<EventId>(),
                Arg.Is<object>(entry => entry.ToString() == "Network error occurred."),
                Arg.Any<NetworkException>(),
                Arg.Any<Func<object?, Exception?, string>>()
            );
    }
}
```
