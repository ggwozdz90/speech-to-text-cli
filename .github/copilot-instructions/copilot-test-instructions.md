# C# Testing Guidelines for GitHub Copilot

## Naming Convention

```csharp
MethodName_Scenario_ExpectedResult

Examples:
- GetUser_WithValidId_ReturnsUser
- SaveData_WhenDatabaseError_ThrowsException
```

## Tools Usage

- **NUnit**: Test framework
- **NSubstitute**: For mocking (`Substitute.For<T>()`)
- **FluentAssertions**: For assertions (`result.Should().Be()`)
- **System.IO.Abstractions.TestingHelpers**: For file system mocking

## Common Scenarios Reference

Check these aspects when writing tests:

- Validation logic
- Error handling
- External dependencies
- Async operations
- Resource cleanup
- Test coverage

## Required Test Cases

1. Success path
2. Error handling
3. Edge cases (null, empty, invalid data)
4. Async operations
5. Dependencies behavior

## Test Structure

```csharp
using System.IO.Abstractions.TestingHelpers;
using Example.Data.Repositories;
using FluentAssertions;
using NUnit.Framework;

namespace Example.Tests.Data.Repositories;

[TestFixture]
public class FileRepositoryTests
{
    private MockFileSystem mockFileSystem = null!;
    private FileRepository fileRepository = null!;

    [SetUp]
    public void SetUp()
    {
        mockFileSystem = new MockFileSystem();
        fileRepository = new FileRepository(mockFileSystem);
    }

    [Test]
    public void GetFileSize_WhenFileExists_ShouldReturnFileSize()
    {
        // Given
        var filePath = @"C:\test\file.txt";
        var fileContent = "Hello, world!";
        var expectedFileSize = fileContent.Length;

        mockFileSystem.AddFile(filePath, new MockFileData(fileContent));

        // When
        var result = fileRepository.GetFileSize(filePath);

        // Then
        result.Should().Be(expectedFileSize);
    }
}
```
