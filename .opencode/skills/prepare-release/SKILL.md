---
name: prepare-release
description: Prepares a new release by stripping the preview suffix from version.json and tagging the stable version. The development branch is automatically updated to the next preview version by the rebase step. This skill ONLY handles version bumping and tagging - it does NOT generate pull request descriptions or PR summaries.
---

# Prepare Release Skill

This skill automates the preparation of a release for ExRam.Gremlinq by:
1. Preparing a release using Nerdbank.GitVersioning

The skill MUST NOT push any tags to any remote.

## Usage

```
prepare release
```

## General Notes

**IMPORTANT:** The skill MUST dynamically determine the Git remote that points to github.com. It MUST NOT hardcode any remote name like "origin" or "github". Always use `git remote -v | grep github.com | head -1 | awk '{print $1}'` to get the remote name.

**IMPORTANT:** The scripts in this skill must be executed as-is. They must not be altered
or adapted to any perceived "different circumstances". If a script is considered unsuitable for the task, fail early and inform the user.

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

## Main logic

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
