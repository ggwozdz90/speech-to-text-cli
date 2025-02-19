# GitHub Copilot Code Instructions

## Code Style and Best Practices

- Ensure the code follows C# conventions and adheres to StyleCop rules
- The code should be self-explanatory and easy to understand
- Avoid magic numbers and strings; use constants or enums instead
- Use simple and clear naming conventions
- Prefer primary constructors where applicable rather than traditional constructors
- Do not use "_" as a prefix for private fields

## Architecture and Design

- Structure the project following Clean Architecture principles
- Separate the code into distinct layers:
  - Domain: Contains business logic and entities
  - Application: Includes service implementations, use cases, and application-specific logic
  - Data: Handles persistence, repositories, and data access
  - Topmost Layer: Depending on the project, API, Adapters, UI
- Use design patterns where appropriate
- Follow SOLID principles strictly for maintainable and scalable code
- Classes and methods should have clear boundaries and responsibilities
- Interfaces should be focused and cohesive

## Error Handling and Logging

- The code must handle unexpected responses and exceptions
- Throw custom exceptions with meaningful messages
- Ensure proper error propagation through all layers
- Properly handle AggregateException in asynchronous operations
- Include contextual information in log messages with appropriate log levels
- Methods should include extensive logging for debugging purposes
- Ensure logs do not include sensitive information

## Performance and Efficiency

- Aim for low computational and memory complexity
- Minimize the number of database queries
- When optimizing LINQ vs. loops, prefer the approach that minimizes the number of database queries to improve performance
- Consider caching strategies where appropriate

## Method Structure, Testability and Dependencies

- Methods should be simple, small and focused on a single task
- Complex operations should be split into smaller methods
- Ensure that generated code follows dependency injection best practices
- Keep cyclomatic complexity low
- Use meaningful parameter and return types
- Prefer composition over inheritance

## Security and Documentation

- The code must follow OWASP security guidelines
- No hardcoded secrets in the codebase
- Only public classes and methods require documentation
- Validate all security-critical inputs
