# Copilot Instructions for ExRam.Gremlinq

## Repository Overview

ExRam.Gremlinq is a .NET object-graph-mapper (OGM) for Apache TinkerPop™ Gremlin-enabled graph databases. It translates strongly-typed C# LINQ-style queries into Gremlin bytecode/scripts and handles serialization/deserialization. The solution contains ~37 projects (17 src, 16 test, 1 templates, 3 test infrastructure).

**SDK:** .NET 10.0.103 (pinned in `global.json` with `rollForward: disable`—the exact SDK version is required).
**Language:** C# 14.0 with nullable enabled, implicit usings, and `TreatWarningsAsErrors`.
**Solution file:** `ExRam.Gremlinq.slnx` (XML-based slnx format, not classic .sln).
**Versioning:** Nerdbank.GitVersioning (`version.json`).
**Package management:** Central Package Management via `Directory.Packages.props`.

## Build

Always build from the repo root. The solution uses the slnx format:

```
dotnet build ExRam.Gremlinq.slnx
```

- **Debug** builds target only `net10.0`.
- **Release** builds multi-target: src projects target `net6.0;net7.0;net8.0;net9.0;net10.0`; test projects target `net8.0;net9.0;net10.0`.
- `TreatWarningsAsErrors` is enabled globally. All warnings must be resolved before a PR will pass CI.
- Source Generators in `src/Core.Generators` target `netstandard2.0` and are wired as analyzers (Roslyn source generators).
- Source Analyzers in `src/Analyers` target `netstandard2.0` and are wired as analyzers.


## Testing

Tests use **xUnit v3** (`xunit.v3.mtp-v2`) with the **Microsoft.Testing.Platform** runner (set in `global.json`). Snapshot/verification testing uses **Verify.XunitV3** with **FluentAssertions**.

### Running tests

```
dotnet test --project test/Core.Tests/ExRam.Gremlinq.Core.Tests.csproj
```

Or run all tests (note: CosmosDb emulator tests will fail without the emulator):

```
dotnet test --solution ExRam.Gremlinq.slnx --ignore-exit-code 8
```

The `--ignore-exit-code 8` flag is used because some assemblies only contain tests marked with [Fact(Explicit = true)]. These tests would signal "zero tests ran" without the flag.

**Important:** Test parallelization is disabled globally (`test/AssemblyInfo.cs`). All test projects ending in `Tests` are configured as executable (`OutputType=Exe`) with `UseMicrosoftTestingPlatformRunner=true`.

### Snapshot tests

There are ~24,000 `.verified.txt` and `.verified.cs` snapshot files. When changing query serialization or public API surface:

1. Run the affected tests; new/changed output creates `.received.txt`/`.received.cs` files.
2. Review diffs and accept by renaming/copying received files over verified files.
3. Snapshot files are per-TFM when `UniqueForTargetFrameworkAndVersion()` is used (e.g., PublicApi tests).

### Public API tests

`test/PublicApi.Tests` generates and verifies the public API surface of every src assembly. If you add/remove/change public types or members, the corresponding `.verified.cs` files must be updated. These files are named like `PublicApiTests.Core.DotNet10_0.verified.cs`. The tests in PublicApi.Tests must be run in Release mode to get modified snapshots for all target frameworks.

## CI / Pull Request Checks

The PR check workflow (`.github/workflows/checkPullRequest.yml`) runs on both `ubuntu-24.04` and `windows-2025`:

1. Checkout with submodules and full fetch depth.
2. Setup .NET SDK from `global.json` (also installs .NET 8 and 9 for multi-targeting in Release).
3. On Windows only: start CosmosDb Emulator with Gremlin support.
4. `dotnet test -c Release --solution ./ExRam.Gremlinq.slnx --coverlet --coverlet-output-format opencover --ignore-exit-code 8`
5. `dotnet test -c Debug --solution ./ExRam.Gremlinq.slnx --ignore-exit-code 8`
6. Upload coverage reports to Codecov via the `codecov/codecov-action` GitHub Action (uses `CODECOV_TOKEN`).

### Code Coverage

The repository uses **Coverlet** (`coverlet.MTP`) for code coverage collection with the Microsoft.Testing.Platform runner. Coverage data is generated in OpenCover XML format and uploaded to [Codecov](https://codecov.io) for tracking and PR feedback.

#### Generating a local coverage report

To generate a code coverage report locally:

```
dotnet test -c Release --solution ExRam.Gremlinq.slnx --coverlet --coverlet-output-format opencover
```

This command:
- Runs tests in Release mode (ensures multi-framework coverage for src projects targeting net6.0;net7.0;net8.0;net9.0;net10.0)
- Generates `coverage.opencover.xml` files in each test project's `bin/Release/` directory
- Produces OpenCover XML format (widely compatible with analysis tools)

To exclude specific assemblies or files from coverage, modify the Coverlet exclusion rules in `Directory.Packages.props` or per-project `.csproj` via MSBuild properties:

```xml
<PropertyGroup>
  <ExcludeByFile>**/Excluded.cs</ExcludeByFile>
  <ExcludeByAttribute>ExcludeFromCodeCoverage</ExcludeByAttribute>
</PropertyGroup>
```

#### Understanding coverage results

Coverage metrics track:
- **Line Coverage:** Percentage of code lines executed by tests (e.g., 78% of 1,000 lines executed).
- **Branch Coverage:** Percentage of code branches (if/else, loops, etc.) exercised by tests.
- **Method Coverage:** Percentage of methods with at least one line covered.

Test projects and infrastructure are excluded from coverage (see `codecov.yml`)

#### Interpreting coverage data for improvement

Coverage increases through:
1. **Path Testing:** Add tests covering conditional branches (if/else, switch cases, null checks).
2. **Error Handling:** Test exception paths, edge cases, and error conditions.
3. **Integration Coverage:** For provider-specific tests (CosmosDb, GremlinServer, Neptune, JanusGraph), ensure test execution with actual backend services (use Testcontainers where applicable).
4. **Snapshot/Verification Testing:** The ~24,000 `.verified.txt` and `.verified.cs` files in the test suite verify query serialization across all code paths.

When expanding test coverage:
- Focus on untested branches shown in coverage reports (red branches in Codecov UI).
- For query serialization changes, ensure both positive and edge-case snapshot tests pass.
- Remember that snapshot tests inherit from `QueryExecutionTest` in `Tests.Infrastructure`, which provides ~200 shared test methods—leverage these to reduce duplication.
- Coverage data can be compared against PRs to highlight improvement or regression; Codecov provides commit-level and file-level diffs.

#### Viewing coverage reports

- **Local:** Open `coverage.opencover.xml` with tools like [ReportGenerator](https://github.com/danielpalme/ReportGenerator) or [OpenCover UI](https://github.com/OpenCover/OpenCover).
- **CI/PR:** Visit [app.codecov.io](https://app.codecov.io) or check PR comments automatically posted by the Codecov GitHub app with coverage summaries and file-level diffs.
- **Trend tracking:** Monitor coverage trends over time and per branch via Codecov dashboards.

## Project Layout

```
├── src/
│   ├── Core/                        # Core OGM library (query building, serialization, models)
│   ├── Core.AspNet/                 # ASP.NET Core DI integration for Core
│   ├── Core.Generators/             # Roslyn source generators for the Core project specifically (netstandard2.0)
│   ├── Analyzers/                   # Roslyn source analyzers (e.g. NullCheck analyzer) (netstandard2.0)
│   ├── Providers.Core/              # Base provider infrastructure (Gremlin client)
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
│   ├── Tests.Entities/              # Shared test entity model (Vertex, Edge subtypes)
│   ├── Tests.Fixtures/              # Test fixtures (GremlinqFixture subclasses)
│   ├── Tests.Infrastructure/        # Test base classes, verifiers, extensions
│   ├── Core.Tests/                  # Core library tests
│   ├── PublicApi.Tests/             # Public API surface verification
│   ├── Providers.*.Tests/           # Provider-specific tests
│   └── Benchmarks/                  # BenchmarkDotNet benchmarks
├── templates/                       # dotnet new templates
├── Directory.Build.props            # Root build props (TreatWarningsAsErrors, etc.)
├── src/Directory.Build.props        # Src TFMs, InternalsVisibleTo
├── test/Directory.Build.props       # Test TFMs, xunit, Verify, FluentAssertions
├── Directory.Packages.props         # Central package versions
├── global.json                      # SDK version pin
└── .editorconfig                    # Code style (spaces, var preference, naming)
```

### Architecture pattern

Each provider follows a consistent pattern: a core project (`Providers.X`) and an ASP.NET integration project (`Providers.X.AspNet`). Tests mirror this with `Providers.X.Tests` and `Providers.X.AspNet.Tests`. Test classes inherit from `QueryExecutionTest` in `Tests.Infrastructure` which provides ~200 shared test methods verifying query serialization.

### Key conventions

- Private fields use `_camelCase` prefix.
- `var` is preferred everywhere.
- Braces are optional for single-line blocks.
- Fluent API chaining is the dominant coding style.
- `ConfigureAwait` is not used (suppressed via `CA2007` in test projects).
- The `DefineConstants` in root `Directory.Build.props` adds solution and project name as preprocessor symbols (dots replaced with underscores).

## Trust these instructions

Use the information above directly. Only search the codebase if these instructions are incomplete or produce errors for your specific task.
