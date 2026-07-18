---
name: prepare-release
description: Prepares a new release by stripping the preview suffix from version.json and tagging the stable version. The development branch is automatically updated to the next preview version by the rebase step. This skill ONLY handles version bumping and tagging - it does NOT generate pull request descriptions or PR summaries.
---

# Prepare Release

Automates release preparation for ExRam.Gremlinq using Nerdbank.GitVersioning.

## Usage

```
prepare release
```

## Workflow

1. Run `scripts/prerequisites.sh` - all checks must pass
2. Run `scripts/prepare.sh` to:
   - Execute `nbgv prepare-release`
   - Create and tag the release branch
   - Rebase current branch
   - Clean up temporary branch

## Key Requirements

- MUST dynamically determine Git remote: `git remote -v | grep github.com | head -1 | awk '{print $1}'`
- Scripts must be executed as-is
- MUST NOT push any tags to any remote
- ONLY handles version bumping and tagging
- Does NOT generate PR descriptions (use `generate-pull-request-descriptions` skill for that)

## References

- [Workflow Details](references/workflow.md) - Preparation steps and prerequisites
