# ExRam.Gremlinq - Agent Instructions

This file provides guidance for AI agents working with the ExRam.Gremlinq repository. It contains project-specific instructions that complement the general development guidelines.

## Repository Overview

ExRam.Gremlinq is a .NET object-graph-mapper (OGM) for Apache TinkerPop™ Gremlin-enabled graph databases. It translates strongly-typed C# LINQ-style queries into Gremlin bytecode/scripts and handles serialization/deserialization.

**Key Details:**
- **SDK:** .NET 10.0.400 (pinned in `global.json` with `rollForward: disable`)
- **Language:** C# 14.0 with nullable enabled, implicit usings, and `TreatWarningsAsErrors`
- **Solution file:** `ExRam.Gremlinq.slnx` (XML-based slnx format)
- **Versioning:** Nerdbank.GitVersioning (`version.json`)
- **Package management:** Central Package Management via `Directory.Packages.props` files in src/, test/, and analyzers/ directories
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

### Microsoft.Testing.Platform (MTP) Reporting

All test projects reference these MTP reporting extensions (wired up in `test/Directory.Build.props`; no code changes needed, they auto-register via `Microsoft.Testing.Platform.MSBuild`):

- `Microsoft.Testing.Extensions.TrxReport` — enable with `--report-trx`. TRX results stream to disk as the run progresses, so a hard crash still leaves a usable partial report.
- `Microsoft.Testing.Extensions.GitHubActionsReport` — enable with `--report-gh`. **Preview package (alpha-only version).** Only activates when `GITHUB_ACTIONS=true`, so it's a no-op for local `dotnet test` runs. Emits inline failure/skip annotations, per-assembly log groups, and a job summary on GitHub Actions.

`Microsoft.Testing.Extensions.TrxReport` and `GitHubActionsReport` themselves require `Microsoft.Testing.Platform` 2.3.3+, which NuGet resolves transitively without needing an explicit override in `test/Directory.Packages.props` (the version xunit.v3.mtp-v2 brings in is only a floor of 2.0.2). No stable xunit.v3.mtp-v2 release currently bumps that floor to 2.3.x on its own (only 4.0.0 prereleases do).

Report file names default to the deterministic `{asm}_{tfm}_{arch}` form (MTP 2.3.0+), so multi-targeted test projects (net8.0/net9.0/net10.0) don't overwrite each other's reports.

`.github/workflows/checkPullRequest.yml` passes `--report-trx --report-gh` on the test step.

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
4. Run tests in Release mode with coverage and MTP reporting (TRX, GitHub Actions annotations)
5. Upload coverage reports to Codecov

**Required Commands:**
```bash
dotnet test -c Release --solution ./ExRam.Gremlinq.slnx --coverlet --coverlet-output-format opencover --ignore-exit-code 8 --report-trx --report-gh
```

`.github/workflows/checkPullRequestDescription.yml` runs a second, cheap check named
`check-description`. It rejects pull requests whose body is missing, or barely longer than
the title, after HTML comments, code blocks, markdown syntax, bare links and issue
references have been stripped. Release preparation pull requests, bot authors and anything
labelled `skip-changelog` are exempt.

## Pull Request Workflow

The default branch is a release branch (`14.x`), not `main`, and it is protected: every
change arrives through a pull request, merges are rebase-only, commits must be signed, and
`enforce_admins` is on.

**Pull request descriptions are the source of the release notes.** The text above the
first `##` heading in a pull request body becomes that change's entry in
`releases/<version>/release-notes.md`, which becomes the GitHub release body, which
`.github/workflows/announce.yml` copies verbatim into the blog on docs.gremlinq.net. A
missing description is a missing changelog entry, and no later step can recover it.

Use the `open-pull-request` skill to open or repair a pull request. It covers the case
where the agent made the changes itself (write the description from the known intent) and
the case where it is looking at a cold branch (reconstruct it from the code, and ask
rather than guess). `.agents/skills/open-pull-request/references/description-style.md`
holds the structure and worked examples from this repository.

Two reserved conventions:

- **`Prepare release`** as a pull request title skips the test matrix *and* the description
  check. Never use it for anything but an actual release preparation.
- **`skip-changelog`** as a label exempts a pull request from the description check and
  keeps it out of the release notes. It is the right answer for CI tweaks, dependency
  bumps and refactorings with no user visible effect -- better than padding a chore up to
  the character threshold.

## Release / Publishing Workflow

Use the `prepare-release` skill. It writes the texts first, then bumps the version and
creates the tag, and it pushes nothing.

**The tag is what drives a release.** `prepare-release` produces two version commits and a
tag; only the tag is pushed. The version commits reach the release branch later, carried
by whatever pull request is opened next. Everything the pipeline reads out of the
repository is read from the tagged commit, which is why `releases/<version>/` has to be
committed *before* `nbgv prepare-release` runs -- that command creates the branch the tag
is placed on, and the current branch is rebased onto it, so the tag points at the earlier
state.

`releases/<version>/` holds the four texts a release needs, written by the
`write-release-announcements` skill from the bodies of the pull requests merged since the
previous tag:

| File | Becomes |
|---|---|
| `release-notes.md` | the GitHub release body, and from there the blog post |
| `linkedin.md` | a manual LinkedIn post |
| `discord-tinkerpop.md` | a manual post in the TinkerPop Discord |
| `discord-dotnet.md` | a post in the .NET Discord, optionally sent by webhook |

Pushing the tag triggers `.github/workflows/pack.yml`, which builds, packs, attests and
creates a **draft** release using `releases/<version>/release-notes.md` as its body
(falling back to release-drafter, and a flat list of pull request titles, if that file is
missing). Nothing is public until that draft is published by hand.

Publishing it fires three workflows:

- **`.github/workflows/pushStable.yml`** — pushes stable packages to NuGet.org.
- **`.github/workflows/announce.yml`** — checks out `Gremlinq/docs.gremlinq.net` with a
  PAT (`DOCS_TOKEN`) and writes the release body **verbatim** into `docs/blog/posts/`.
  This is why the release notes have to read as published prose, not as internal notes.
- **`.github/workflows/announcementKit.yml`** — checks out the tag, reads the announcement
  texts and opens an issue with one checkbox per channel. It posts to the .NET Discord if
  a `DISCORD_WEBHOOK_DOTNET` secret is configured; TinkerPop and LinkedIn are always
  manual, because those servers are not ours to automate and LinkedIn member tokens expire
  every 60 days.

Separately, **`.github/workflows/pushPreview.yml`** pushes preview packages to GitHub
Packages using a PAT secret (`PUSH_TO_PACKAGES_PAT`) whenever `Pack` succeeds.

`pushStable.yml` publishes to NuGet.org using **NuGet Trusted Publishing (OIDC)** instead of a long-lived API key:

1. The job requests a GitHub OIDC token (`permissions: id-token: write`).
2. The `NuGet/login` action exchanges that token for a short-lived (1-hour) NuGet.org API key.
3. `dotnet nuget push` uses that temporary key to publish the packages.

This requires a one-time setup on nuget.org (a Trusted Publishing policy scoped to this repository and the `pushStable.yml` workflow file) and a `NUGET_USER` repository secret containing the nuget.org account name used for that policy. There is no long-lived NuGet API key secret to rotate or leak.

When modifying `pushStable.yml`, keep in mind:
- The workflow file name itself (`pushStable.yml`) is part of the nuget.org Trusted Publishing policy — renaming the file requires updating the policy on nuget.org.
- `id-token: write` permission must remain on the job/workflow for the OIDC exchange to work.

## Project Structure

```
├── analyzers/
│   ├── Analyzers/                   # Roslyn source analyzers (netstandard2.0)
│   ├── Core.Generators/             # Roslyn source generators (netstandard2.0)
│   └── Testing.AirRoutes.Generators/
├── src/
│   ├── Core/                        # Core OGM library (query building, serialization, models)
│   ├── Core.AspNet/                 # ASP.NET Core DI integration for Core
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
├── src/
│   └── Directory.Packages.props     # Central package versions for src projects
├── test/
│   └── Directory.Packages.props     # Central package versions for test projects
├── analyzers/
│   └── Directory.Packages.props     # Central package versions for analyzers
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