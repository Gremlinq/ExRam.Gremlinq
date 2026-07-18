---
name: prepare-release
description: Use this skill when preparing a new release. Handles version bumping by removing preview suffixes and creating release tags. Invokes when asked to "prepare release" or similar. ONLY manages version/tag operations - does NOT generate PR descriptions or push to remotes.
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
