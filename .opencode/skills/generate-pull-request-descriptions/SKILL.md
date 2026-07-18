---
name: generate-pull-request-descriptions
description: Generates pull request descriptions by looking up pull requests since the last tag from the current commit and commenting nice summaries on PRs that lack a body. This skill does NOT modify version or create tags.
---

# Generate Pull Request Descriptions

Automates PR description generation for ExRam.Gremlinq by analyzing commit messages and code changes.

## Usage

```
generate pull request descriptions
```

## Workflow

1. Run `scripts/prerequisites.sh` to validate environment
2. Run `scripts/generate-descriptions.sh` to collect PR data into `/tmp/pr_*.json`
3. For each JSON file:
   - Analyze `commit_messages` AND `code_changes`
   - Generate narrative summary and title (see [requirements](references/requirements.md))
   - Update PR via GitHub API
   - Create `releases/notes/<number>.txt`
4. Run `scripts/commit-descriptions.sh`

## Key Requirements

- MUST dynamically determine Git remote: `git remote -v | grep github.com | head -1 | awk '{print $1}'`
- Scripts must be executed as-is
- Repository hardcoded as `Gremlinq/ExRam.Gremlinq`
- Uses GitHub CLI for authentication
- Does NOT modify version or create tags

## References

- [Workflow Details](references/workflow.md) - Script execution order and file formats
- [Summary & Title Requirements](references/requirements.md) - Quality standards and examples
