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
4. `dotnet test -c Release --solution ./ExRam.Gremlinq.slnx --ignore-exit-code 8`
5. `dotnet test -c Debug --solution ./ExRam.Gremlinq.slnx --ignore-exit-code 8`

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
