# ExRam.Gremlinq - Agent Instructions

This file provides guidance for AI agents working with the ExRam.Gremlinq repository. It contains project-specific instructions that complement the general development guidelines.

## Repository Overview

ExRam.Gremlinq is a .NET object-graph-mapper (OGM) for Apache TinkerPop™ Gremlin-enabled graph databases. It translates strongly-typed C# LINQ-style queries into Gremlin bytecode/scripts and handles serialization/deserialization.

**Key Details:**
- **SDK:** .NET 10.0.103 (pinned in `global.json` with `rollForward: disable`)
- **Language:** C# 14.0 with nullable enabled, implicit usings, and `TreatWarningsAsErrors`
- **Solution file:** `ExRam.Gremlinq.slnx` (XML-based slnx format)
- **Versioning:** Nerdbank.GitVersioning (`version.json`)
- **Package management:** Central Package Management via `Directory.Packages.props`
- **Projects:** ~37 projects (17 src, 16 test, 1 templates, 3 test infrastructure)

## Build Instructions

Always build from the repo root using the slnx format:

```bash
# Debug build (net10.0 only)
dotnet build ExRam.Gremlinq.slnx

# Release build (multi-target: net6.0;net7.0;net8.0;net9.0;net10.0)
dotnet build ExRam.Gremlinq.slnx -c Release
```

**Important Build Notes:**
- `TreatWarningsAsErrors` is enabled globally - all warnings must be resolved
- Source Generators in `src/Core.Generators` target `netstandard2.0`
- Source Analyzers in `src/Analyzers` target `netstandard2.0`

## Testing Guidelines

### Running Tests

```bash
# Run specific test project
dotnet test --project test/Core.Tests/ExRam.Gremlinq.Core.Tests.csproj

# Run all tests (ignore exit code 8 for explicit tests)
dotnet test --solution ExRam.Gremlinq.slnx --ignore-exit-code 8
```

**Test Configuration:**
- Uses **xUnit v3** (`xunit.v3.mtp-v2`) with **Microsoft.Testing.Platform** runner
- Snapshot/verification testing uses **Verify.XunitV3** with **FluentAssertions**
- Test parallelization is disabled globally (`test/AssemblyInfo.cs`)
- All test projects ending in `Tests` are configured as executable (`OutputType=Exe`)

### Snapshot Testing

There are ~24,000 `.verified.txt` and `.verified.cs` snapshot files. When changing:

1. **Query serialization** or **public API surface**:
   - Run affected tests → creates `.received.txt`/`.received.cs` files
   - Review diffs and accept by renaming received files to verified files
   - Snapshot files are per-TFM when `UniqueForTargetFrameworkAndVersion()` is used

2. **Public API changes**:
   - Update corresponding `.verified.cs` files in `test/PublicApi.Tests`
   - Files follow naming pattern: `PublicApiTests.Core.DotNet10_0.verified.cs`
   - Run in Release mode to get modified snapshots for all target frameworks

### Code Coverage

Uses **Coverlet** (`coverlet.MTP`) with Microsoft.Testing.Platform runner:

```bash
# Generate local coverage report
dotnet test -c Release --solution ExRam.Gremlinq.slnx --coverlet --coverlet-output-format opencover
```

**Coverage Metrics:**
- **Line Coverage:** Percentage of code lines executed
- **Branch Coverage:** Percentage of code branches exercised
- **Method Coverage:** Percentage of methods with coverage

**Coverage Exclusions:**
- Test projects and infrastructure are excluded from coverage (see `codecov.yml`)
- Use Coverlet exclusion rules in `Directory.Packages.props` or `.csproj` files

## CI / Pull Request Checks

The PR workflow (`.github/workflows/checkPullRequest.yml`) runs on `ubuntu-24.04` and `windows-2025`:

1. Checkout with submodules and full fetch depth
2. Setup .NET SDK from `global.json`
3. On Windows only: start CosmosDb Emulator with Gremlin support
4. Run tests in Release mode with coverage
5. Run tests in Debug mode
6. Upload coverage reports to Codecov

**Required Commands:**
```bash
dotnet test -c Release --solution ./ExRam.Gremlinq.slnx --coverlet --coverlet-output-format opencover --ignore-exit-code 8
dotnet test -c Debug --solution ./ExRam.Gremlinq.slnx --ignore-exit-code 8
```

## Project Structure

```
├── src/
│   ├── Core/                        # Core OGM library (query building, serialization, models)
│   ├── Core.AspNet/                 # ASP.NET Core DI integration for Core
│   ├── Core.Generators/             # Roslyn source generators (netstandard2.0)
│   ├── Analyzers/                   # Roslyn source analyzers (netstandard2.0)
│   ├── Providers.Core/              # Base provider infrastructure
│   ├── Providers.CosmosDb/          # Azure CosmosDb provider
│   ├── Providers.CosmosDb.AspNet/   # CosmosDb + ASP.NET Core DI
│   ├── Providers.GremlinServer/     # Apache TinkerPop Gremlin Server provider
│   ├── Providers.GremlinServer.AspNet/
│   ├── Providers.JanusGraph/        # JanusGraph provider
│   ├── Providers.JanusGraph.AspNet/
│   ├── Providers.Neptune/           # AWS Neptune provider
│   ├── Providers.Neptune.AspNet/
│   ├── Support.NewtonsoftJson/      # Newtonsoft.Json serialization support
│   ├── Support.NewtonsoftJson.AspNet/
│   ├── Support.TestContainers/      # Testcontainers integration
│   ├── Testing.AirRoutes/           # Air routes test data
│   └── Testing.AirRoutes.Generator/ # Generator for air routes data
├── test/
│   ├── Tests.Entities/              # Shared test entity model
│   ├── Tests.Fixtures/              # Test fixtures (GremlinqFixture subclasses)
│   ├── Tests.Infrastructure/        # Test base classes, verifiers, extensions
│   ├── Core.Tests/                  # Core library tests
│   ├── PublicApi.Tests/             # Public API surface verification
│   ├── Providers.*.Tests/           # Provider-specific tests
│   └── Benchmarks/                  # BenchmarkDotNet benchmarks
├── templates/                       # dotnet new templates
├── Directory.Build.props            # Root build props
├── global.json                      # SDK version pin
└── .editorconfig                    # Code style
```

## Architecture Patterns

Each provider follows a consistent pattern:
- Core project: `Providers.X`
- ASP.NET integration: `Providers.X.AspNet`
- Tests mirror this structure: `Providers.X.Tests` and `Providers.X.AspNet.Tests`

Test classes inherit from `QueryExecutionTest` in `Tests.Infrastructure` which provides ~200 shared test methods for query serialization verification.

## Coding Conventions

- Private fields use `_camelCase` prefix
- `var` is preferred everywhere
- Braces are optional for single-line blocks
- Fluent API chaining is the dominant style
- `ConfigureAwait` is not used (suppressed via `CA2007` in test projects)
- Solution and project names are added as preprocessor symbols (dots replaced with underscores)

## Task-Specific Agent Guidance

### For Query Serialization Changes
1. Update query building logic in relevant provider/core projects
2. Run affected snapshot tests to generate `.received.txt` files
3. Review and accept changes by updating `.verified.txt` files
4. Ensure both positive and edge-case scenarios are covered

### For Public API Changes
1. Modify the API in the appropriate src project
2. Run `test/PublicApi.Tests` in Release mode
3. Update corresponding `.verified.cs` files in `test/PublicApi.Tests`
4. Verify all target frameworks are covered (net6.0 through net10.0)

### For Provider-Specific Changes
1. Modify the provider project (`Providers.X`)
2. Update provider-specific tests (`Providers.X.Tests`)
3. Test with actual backend services where applicable (use Testcontainers)
4. Ensure cross-framework compatibility (net6.0+)

### For Source Generator/Analyzer Changes
1. Modify generator/analyzer in respective project
2. Test with `Core.Tests` to ensure proper integration
3. Verify analyzer warnings/errors are properly surfaced

## Verification Steps

Before submitting changes, agents should verify:

1. **Build succeeds:** `dotnet build ExRam.Gremlinq.slnx`
2. **Tests pass:** `dotnet test --solution ExRam.Gremlinq.slnx --ignore-exit-code 8`
3. **No warnings:** All projects compile with `TreatWarningsAsErrors` enabled
4. **Code style:** Follows existing conventions (private fields, var usage, etc.)
5. **Snapshot tests:** Updated for any serialization or API changes
6. **Coverage:** New code paths have appropriate test coverage

## Additional Resources

- **Documentation:** https://docs.gremlinq.net
- **NuGet packages:** https://www.nuget.org/packages?q=ExRam.Gremlinq
- **Issue tracker:** GitHub Issues
- **Discussions:** GitHub Discussions

## Trust These Instructions

Use the information above directly. Only search the codebase if these instructions are incomplete or produce errors for your specific task.