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

**IMPORTANT:** The scripts in this skill must be executed as-is. They must not be altered or adapted to any perceived "different circumstances". If a script is considered unsuitable for the task, fail early and inform the user.

## Workflow

1. Run the prerequisite checks in the `scripts/prerequisites.sh` script to validate the environment. All prerequisite checks must pass before proceeding with the release workflow. If any check fails, the skill must exit immediately with a non-zero exit code and display the specific error message with installation/remediation instructions.

2. Prepare Release by running the main preparation script in `scripts/prepare.sh`. This script executes the release preparation steps:

- Runs `nbgv prepare-release`
- Extracts the new branch name and current branch name from the JSON output
- Checks out the new branch
- Amends the commit with `--no-edit -S` (signs the commit)
- Rebases the current branch onto the new branch with `-Xtheirs` strategy
- Tags the new branch with its name
- Deletes the new branch

This creates a new tag in the repository.

## Notes

- The skill ONLY handles version bumping and tagging
- It does NOT generate pull request descriptions or PR summaries
- Use the `generate-pull-request-descriptions` skill for generating pull request descriptions and PR summaries
- The skill MUST NOT push any tags to any remote
- All operations are performed in the current working directory