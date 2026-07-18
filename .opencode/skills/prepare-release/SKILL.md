---
name: prepare-release
description: Prepares a new release by stripping the preview suffix from version.json and tagging the stable version. The development branch is automatically updated to the next preview version by the rebase step. This skill ONLY handles version bumping and tagging - it does NOT generate release notes or PR summaries.
---

# Prepare Release Skill

This skill automates the preparation of a release for ExRam.Gremlinq by:
1. Preparing a release using Nerdbank.GitVersioning

The skill MUST NOT push any tags to any remote.

## Usage

```
prepare release
```

## Quick Start - Verify Prerequisites

Run these commands to verify all dependencies are installed:

```bash
# Check all required tools
command -v nbgv && command -v git && echo "All tools installed!" || echo "Missing tools!"

# Check working directory
ls ExRam.Gremlinq.slnx .git/ > /dev/null && echo "In correct directory" || echo "Wrong directory"

# Check clean working tree
git diff --quiet && git diff --cached --quiet && echo "Clean working tree" || echo "Uncommitted changes"
```

## Requirements

- `nbgv` (Nerdbank.GitVersioning) CLI tool must be installed
- Git must be installed
- Working directory must be the root of the ExRam.Gremlinq repository

## Prerequisite Checks

Before executing the release workflow, the skill MUST perform the following prerequisite checks and fail early with clear error messages and installation instructions:

### 1. Nerdbank.GitVersioning (`nbgv`) Check
```bash
if ! command -v nbgv &> /dev/null; then
    echo "ERROR: Nerdbank.GitVersioning CLI (nbgv) is not installed."
    echo "Install with: dotnet tool install -g nbgv"
    exit 1
fi
```

### 2. Git Check
```bash
if ! command -v git &> /dev/null; then
    echo "ERROR: Git is not installed."
    echo "Install from: https://git-scm.com/downloads"
    exit 1
fi
```

### 3. Working Directory Check
```bash
if [ ! -f "ExRam.Gremlinq.slnx" ] || [ ! -d ".git" ]; then
    echo "ERROR: Not in the root of the ExRam.Gremlinq repository."
    echo "Please navigate to the repository root directory."
    exit 1
fi
```

### 4. Clean Working Tree Check
```bash
if ! git diff --quiet || ! git diff --cached --quiet; then
    echo "ERROR: Working directory has uncommitted changes."
    echo "Status:"
    git status --short
    echo ""
    echo "Please commit or stash all changes before creating a release."
    exit 1
fi
```

All prerequisite checks must pass before proceeding with the release workflow. If any check fails, the skill must exit immediately with a non-zero exit code and display the specific error message with installation/remediation instructions.

## Workflow

### Step 0: Prerequisite Validation (Mandatory)

Before any release operations begin, execute all prerequisite checks in order:

1. **Nerdbank.GitVersioning check** - Verify `nbgv` is installed and in PATH
2. **Git check** - Verify `git` is installed and in PATH
3. **Working directory check** - Verify current directory contains `ExRam.Gremlinq.slnx` and `.git`
4. **Clean working tree check** - Verify no uncommitted changes exist

If ANY check fails, display the specific error message with installation/remediation instructions and exit immediately with exit code 1.

**Bash Implementation:**
```bash
# 1. Nerdbank.GitVersioning check
if ! command -v nbgv &> /dev/null; then
    echo "ERROR: Nerdbank.GitVersioning CLI (nbgv) is not installed."
    echo "Install with: dotnet tool install -g nbgv"
    exit 1
fi

# 2. Git check
if ! command -v git &> /dev/null; then
    echo "ERROR: Git is not installed."
    echo "Install from: https://git-scm.com/downloads"
    exit 1
fi

# 3. Working directory check
if [ ! -f "ExRam.Gremlinq.slnx" ] || [ ! -d ".git" ]; then
    echo "ERROR: Not in the root of the ExRam.Gremlinq repository."
    echo "Current directory: $(pwd)"
    echo "Expected: Repository root containing ExRam.Gremlinq.slnx and .git/"
    exit 1
fi

# 4. Clean working tree check
if ! git diff --quiet || ! git diff --cached --quiet; then
    echo "ERROR: Working directory has uncommitted changes."
    echo "Status:"
    git status --short
    echo ""
    echo "Please commit or stash all changes before creating a release."
    exit 1
fi

echo "All prerequisite checks passed!"
```

### Step 1: Prepare Release with Nerdbank.GitVersioning

The skill executes the release preparation steps:

1. Runs `nbgv prepare-release`
2. Extracts the new branch name and current branch name from the JSON output
3. Checks out the new branch
4. Amends the commit with `--no-edit -S` (signs the commit)
5. Rebases the current branch onto the new branch with `-Xtheirs` strategy
6. Tags the new branch with its name
7. Deletes the new branch

This creates a new tag in the repository.

**Bash Commands:**
```bash
# Get the current commit SHA
current_sha=$(git rev-parse HEAD)

nbgv_output=$(nbgv prepare-release --format json --versionIncrement build)

new_branch=$(echo "$nbgv_output" | jq -r '.NewBranch.Name')
current_branch=$(echo "$nbgv_output" | jq -r '.CurrentBranch.Name')

echo "Preparing release"
echo "New branch: $new_branch"
echo "Current branch: $current_branch"

# Store the current branch for later return
git checkout "$new_branch"
git commit --amend --no-edit -S
git rebase "$new_branch" "$current_branch" -Xtheirs
git tag "$new_branch" "$new_branch"
git branch -d "$new_branch"

# Return to the original branch
git checkout "$current_branch"

echo "Tag created: $new_branch"
```

## Implementation Details

### Error Handling

The skill MUST:
- **Fail early with clear messages** - All prerequisite checks must run before any release operations
- Validate that the working directory is a git repository
- Check that required tools (nbgv, git) are available and properly configured

**Prerequisite Check Priority:**
1. Check external dependencies first (nbgv, git) - these are hardest to fix mid-workflow
2. Check working directory - user can easily navigate to correct location
3. Check repository state (clean working tree) - user can commit/stash changes

Each failed check must:
- Display a clear, specific error message
- Provide exact installation/remediation commands or URLs
- Exit immediately with code 1
- Not attempt any release operations

## Example Session

```
User: prepare release

OpenCode:
1. Preparing release
   Running: nbgv prepare-release --format json --versionIncrement build
   New branch: 10.0.0
   Current branch: main
   Checking out 10.0.0...
   Amending commit...
   Rebasing main onto 10.0.0...
   Tagging 10.0.0...
   New tag created: 10.0.0

Release preparation completed successfully!
```

## Notes

- The skill ONLY handles version bumping and tagging
- It does NOT generate pull request descriptions or PR summaries
- Use the `generate-pull-request-descriptions` skill for generating pull request descriptions and PR summaries
- The skill MUST NOT push any tags to any remote
- All operations are performed in the current working directory
