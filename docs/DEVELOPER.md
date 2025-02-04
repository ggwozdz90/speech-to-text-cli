# Developer Guide

Based on the [C# Project Template](https://github.com/ggwozdz90/csharp-project-template).

## Development Environment Setup

### Visual Studio Code

- Pre configured [VS Code settings](../.vscode/settings.json) for optimal development experience.

### Code Generation and Assistance

- **Test Generation**: [Github Copilot Test Instructions](../.github/copilot-instructions/copilot-test-instructions.md)
  - Use NUnit for unit testing.
  - Use FluentAssertions for expressive assertions.
  - Use NSubstitute for mocking dependencies.
  - Use System.IO.Abstractions for file system mocking.
  - Follow the Given-When-Then pattern for structuring tests.

- **Commit Message Guidance**: [Github Copilot Commit Instructions](../.github/copilot-instructions/copilot-commit-instructions.md)
  - Follows [Conventional Commits](https://www.conventionalcommits.org) specification.

### Package Management

- **Paket**: [link](https://fsprojects.github.io/Paket/index.html)
  - Supports diverse package sources (NuGet, GitHub, Gists, HTTP).
  - Separates transitive dependencies and provides control over conflicting package versions.
  - Configuration in [paket.dependencies](../paket.dependencies).

### Common Configuration

- [`Directory.Build.props`](../Directory.Build.props): Centralized common property definitions. Enables internal member exposure for testing via `InternalsVisibleTo`.
- [`Directory.Packages.props`](../Directory.Packages.props): Standardized package references across solution. Used for analyzers and tools.

### Static Analysis

- Includes analyzers such as `Microsoft.CodeAnalysis.NetAnalyzers`, `Meziantou.Analyzer`, `Roslynator` and `StyleCop.Analyzers`.
- Enforces consistent code quality through [`.editorconfig`](../.editorconfig).

### Namespace and Dependency Management

- **NsDepCop**: [link](https://github.com/realvizu/NsDepCop)
  - Enforces namespace dependency rules following Clean Architecture principles.
  - Configuration in [config.nsdepcop](../config.nsdepcop) and [config.nsdepcop](../src/Example/config.nsdepcop).

### Git Integration

- **GitFlow**: [link](https://nvie.com/posts/a-successful-git-branching-model/).
- Carefully crafted [`.gitignore`](../.gitignore) and [`.gitattributes`](../.gitattributes).
- **[Husky.NET](https://alirezanet.github.io/Husky.Net/)**: Implements pre-commit quality checks.
  - Git hooks available in `.husky` directory.
  - Task runner configuration in [`task-runner.json`](../.husky/task-runner.json).
- **Commitlint**: Validates commit message formatting.
  - Configured with [`.commitlintrc.json`](../.config/.commitlintrc.json).

### Code Style Tools

- **Prettier**: Formats JSON and YAML files.
- **Markdownlint**: Ensures Markdown file consistency.
- **Cspell**: Checks spelling across various file types.
  - Configured with [`.cspell.json`](../.config/cspell.json).

### Version Management

- **Versionize**: Automated [semantic versioning](https://semver.org) based on commit messages.

### GitHub Actions Workflows

- **Code Quality Checks**:
  - Comprehensive validation in [`check.yaml`](../.github/workflows/check.yaml)
  - Validates code style, dependencies, spelling
  - Builds and tests project automatically

- **Release Automation**:
  - Streamlined process in [`release.yaml`](../.github/workflows/release.yaml)
  - Follows GitFlow
  - Automatic changelog generation
  - GitHub release deployment in [`deploy-github-release.yaml`](../.github/workflows/deploy-github-release.yaml)

## Getting Started

1. Clone the repository:

    ```bash
    git clone https://github.com/ggwozdz90/csharp-project-template.git
    ```

2. Install the dotnet CLI tools:

    ```bash
    dotnet tool restore
    ```
  
3. Install the Paket dependencies:

    ```bash
    dotnet paket restore
    ```

4. Install Node.js dependencies:

    ```bash
    npm install
    ```

5. Create a new solution with a project and a test project using the example as a template and start coding.

## Paket Quick Reference

- **Install Paket CLI**:

    ```bash
    dotnet new tool-manifest
    dotnet tool install paket
    ```

- **Restore Paket dependencies**:

    ```bash
    dotnet paket restore
    ```

- **Add a NuGet package dependency**:

    ```bash
    dotnet paket add <package ID> --project <project-name>
    ```

- **Remove a NuGet package dependency**:

    ```bash
    dotnet paket remove <package ID> --project <project-name>
    ```

- **Update packages to the latest version**:

    ```bash
    dotnet paket update
    ```

- **Convert NuGet references to Paket references**:

    ```bash
    dotnet paket convert-from-nuget
    ```

## Husky.NET Quick Reference

- **Install Husky.NET CLI**:

    ```bash
    dotnet new tool-manifest
    dotnet tool install Husky
    ```

- **Set up Husky.NET in the project**:

    ```bash
    dotnet husky install
    ```

- **Run Husky.NET tasks**:

    ```bash
    dotnet husky run
    ```

- **Add new git hooks**:

    ```bash
    dotnet husky add <hook-name>
    ```

## Dotnet format Quick Reference

- **Run dotnet format and apply formatting**:

    ```bash
    dotnet format --verbosity detailed --severity info
    ```

- **Run dotnet format and check formatting**:

    ```bash
    dotnet format --verify-no-changes --verbosity detailed --severity info
    ```

## CSharpier Quick Reference

- **Install CSharpier CLI**:

    ```bash
    dotnet new tool-manifest
    dotnet tool install csharpier
    ```

- **Run CSharpier and apply formatting**:

    ```bash
    dotnet csharpier . --loglevel debug
    ```

- **Run CSharpier and check formatting**:

    ```bash
    dotnet csharpier . --check --loglevel debug
    ```

## Prettier Quick Reference

- **Install Prettier CLI**:

    ```bash
    npm install --save-dev prettier
    ```

- **Run Prettier and apply formatting**:

    ```bash
    npx prettier "**/*.{json,yaml}" --write --log-level log
    ```

- **Run Prettier and check formatting**:

    ```bash
    npx prettier "**/*.{json,yaml}" --check --log-level log
    ```

## Markdownlint Quick Reference

- **Install Markdownlint CLI**:

    ```bash
    npm install --save-dev markdownlint-cli
    ```

- **Run Markdownlint**:

    ```bash
    npx markdownlint . --dot --ignore node_modules --ignore packages --ignore CHANGELOG.md --disable MD013
    ```

## Cspell Quick Reference

- **Install Cspell CLI**:

    ```bash
    npm install --save-dev cspell
    ```

- **Run Cspell**:

    ```bash
    npx cspell "**/*.{md,txt,yaml,json,cs}" --config .config/cspell.json
    ```

## Table of Contents

- [Developer Guide](#developer-guide)
  - [Development Environment Setup](#development-environment-setup)
    - [Visual Studio Code](#visual-studio-code)
    - [Code Generation and Assistance](#code-generation-and-assistance)
    - [Package Management](#package-management)
    - [Common Configuration](#common-configuration)
    - [Static Analysis](#static-analysis)
    - [Namespace and Dependency Management](#namespace-and-dependency-management)
    - [Git Integration](#git-integration)
    - [Code Style Tools](#code-style-tools)
    - [Version Management](#version-management)
    - [GitHub Actions Workflows](#github-actions-workflows)
  - [Getting Started](#getting-started)
  - [Paket Quick Reference](#paket-quick-reference)
  - [Husky.NET Quick Reference](#huskynet-quick-reference)
  - [Dotnet format Quick Reference](#dotnet-format-quick-reference)
  - [CSharpier Quick Reference](#csharpier-quick-reference)
  - [Prettier Quick Reference](#prettier-quick-reference)
  - [Markdownlint Quick Reference](#markdownlint-quick-reference)
  - [Cspell Quick Reference](#cspell-quick-reference)
  - [Table of Contents](#table-of-contents)
