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

## Agent Skills

The skills live in **`.agents/skills/`** so that they are not tied to one agent tool.
Claude Code only ever scans `.claude/skills/`, so `.claude/skills` is committed as a
**symlink** pointing at `../.agents/skills`.

**A clone therefore needs `core.symlinks=true`.** Git only materialises a symlink when
that option is on; with `core.symlinks=false` — the default on Windows unless developer
mode or an elevated shell is used — it writes a plain text file containing the target path
instead. Claude Code then finds no skills at all, and says nothing about it. The same
happens to a working symlink whenever a `git checkout` touches the path.

If `ls .claude/skills/` fails with *Not a directory*, that is what happened:

```bash
git config core.symlinks true
rm .claude/skills && git checkout -- .claude/skills
```

`CLAUDE.md` deliberately is **not** a symlink for the same reason. It is a regular file
containing the single line `@AGENTS.md`, which is Claude Code's documented way to import
another instruction file and works regardless of `core.symlinks` and of the operating
system.

## Build Instructions

Always build from the repo root using the slnx format:

```bash
# Debug build (net8.0;net9.0;net10.0)
dotnet build ExRam.Gremlinq.slnx

# Release build (the same frameworks)
dotnet build ExRam.Gremlinq.slnx -c Release
```

**Important Build Notes:**
- `TreatWarningsAsErrors` is enabled globally - all warnings must be resolved

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

When changing:

1. **Query serialization** or **public API surface**:
   - Run affected tests → creates `.received.txt`/`.received.cs` files
   - Review diffs and accept by renaming received files to verified files
   - Snapshot files are per-TFM when `UniqueForTargetFrameworkAndVersion()` is used

2. **Public API changes**:
   - Update corresponding `.verified.cs` files in `test/PublicApi.Tests`
   - Files follow naming pattern: `PublicApiTests.Core.DotNet10_0.verified.cs`
   - Run in Release mode to get modified snapshots for all target frameworks

3. **Verifying against another class's snapshots**:
   - `query.Verify()` returns Verify's `SettingsTask`, after the verifier's own settings, so a
     test can chain further settings onto it.
   - The facts of `QueryExecutionTest` and `GraphsonSupportTestBase` return that `SettingsTask`.
     An override chains its settings onto `base.Fact()`.
   - xUnit awaits only `Task` and `ValueTask`. It would pass a `SettingsTask` fact without
     verifying. `GremlinqTestRunner` (`test/Tests.Infrastructure/`) awaits the `SettingsTask`
     instead. `GremlinqTestFramework` installs it.
   - `xUnit1028` rejects `SettingsTask` as a return type. It is off for the test projects
     (`test/Directory.Build.props`).
   - A test class that answers like another class verifies against that class's snapshots with
     `UseSnapshotDirectoryAndNameOf<T>()`. Adding `UseCanonicalComparer()`
     (`test/Tests.Infrastructure/Snapshots/`) compares the canonical forms of both texts: the order
     of array items and object members is ignored, and an exception document is compared as
     `EXCEPTION`. This is for another implementation of the same semantics, which may answer in
     another order where Gremlin defines none, and fail with an exception of its own.
   - Under `UseCanonicalComparer()`, the class overrides every fact whose query has an `order()`
     step and chains `.UseDefaultComparer()` onto the base fact, since sorting would hide the order:

     ```csharp
     public override SettingsTask Order_scalars() => base.Order_scalars().UseDefaultComparer();
     ```

   - A received file in another class's snapshot directory is never accepted: the snapshot belongs
     to that class.

### Code Coverage

Uses **Coverlet** (`coverlet.MTP`) with Microsoft.Testing.Platform runner:

```bash
# Generate local coverage report
dotnet test -c Release --solution ExRam.Gremlinq.slnx --coverlet --coverlet-output-format opencover
```

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
references have been stripped. Bot authors and anything labelled `skip-changelog` are
exempt; nothing else is.

## Pull Request Workflow

The default branch is a release branch (`14.x`), not `main`, and it is protected: every
change arrives through a pull request, merges are rebase-only, commits must be signed, and
`enforce_admins` is on.

**Pull request descriptions are the source of the release notes.** The text above the
first `##` heading in a pull request body becomes that change's entry in
`releases/<version>/release-notes.md`, which becomes the GitHub release body, which
`.github/workflows/publishBlogPost.yml` copies verbatim into the blog on
docs.gremlinq.net. A missing description is a missing changelog entry, and no later step
can recover it.

Use the `open-pull-request` skill to open or repair a pull request. It covers the case
where the agent made the changes itself (write the description from the known intent) and
the case where it is looking at a cold branch (reconstruct it from the code, and ask
rather than guess). `.agents/skills/open-pull-request/references/description-style.md`
holds the structure and worked examples from this repository.

One reserved convention: **`skip-changelog`** as a label exempts a pull request from the
description check and keeps it out of the release notes. It is the right answer for CI
tweaks, dependency bumps and refactorings with no user visible effect -- better than
padding a chore up to the character threshold.

No pull request title has any effect on CI. Every pull request runs the full test matrix,
release preparation included.

## Release / Publishing Workflow

Use the `prepare-release` skill. It writes the texts first, then bumps the version and
creates the tag, and it pushes nothing. Its `references/workflow.md` covers what the tag and
the published release trigger, the secrets those workflows rely on, and what a failed run
looks like. Minor and major bumps are never made at release time; see
[For Public API Changes](#for-public-api-changes).

### Modifying `pushStable.yml`

`pushStable.yml` publishes to NuGet.org through **NuGet Trusted Publishing (OIDC)** instead
of a long-lived API key: the job exchanges a GitHub OIDC token for a one-hour NuGet.org key
via `NuGet/login`. Keep in mind:
- The workflow file name is part of the Trusted Publishing policy on nuget.org — renaming
  the file requires updating the policy.
- `id-token: write` permission must remain on the job/workflow for the OIDC exchange to work.
- The `NUGET_USER` repository secret holds the nuget.org account name the policy is scoped to.

## Architecture Patterns

Each provider follows a consistent pattern:
- Core project: `Providers.X`
- ASP.NET integration: `Providers.X.AspNet`
- Tests mirror this structure: `Providers.X.Tests` and `Providers.X.AspNet.Tests`

Test classes inherit from `QueryExecutionTest` in `Tests.Infrastructure` which provides ~200 shared test methods for query serialization verification.

The facts of `QueryExecutionTest` that name an element by its id take that id from
`GetId(long)`. Its default is the number itself. A test class for a provider with string ids
overrides `GetId` once, as `StringIdIntegrationTests` does.

## Coding Conventions

- Private fields use `_camelCase` prefix
- `var` is preferred everywhere
- Braces are optional for single-line blocks
- Fluent API chaining is the dominant style
- `ConfigureAwait(false)` is applied to awaited Tasks and ValueTasks in src; `CA2007` enforces it there and is off in test projects
- Solution and project names are added as preprocessor symbols (dots replaced with underscores)

## Task-Specific Agent Guidance

### For Public API Changes
1. Modify the API in the appropriate src project
2. Run `test/PublicApi.Tests` in Release mode
3. Update corresponding `.verified.cs` files in `test/PublicApi.Tests`
4. Verify all target frameworks are covered (net8.0 through net10.0)
5. **Raise `version.json` in this same pull request** — to the next minor for an additive
   change, to the next major for a breaking one, keeping the `-preview.{height}` suffix
   (e.g. `14.1.2-preview.{height}` → `14.2.0-preview.{height}`). This is the only place
   the decision is ever made. Preparing a release just drops the preview suffix and moves
   on by one build number, so it can neither make nor recover a minor or major bump.

## Trust These Instructions

Use the information above directly. Only search the codebase if these instructions are incomplete or produce errors for your specific task.