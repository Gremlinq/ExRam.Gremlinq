# Workflow Details

## Script Execution Order

1. **Prerequisites Check**: Run `scripts/prerequisites.sh`
2. **Data Collection**: Run `scripts/generate-descriptions.sh`
3. **Agent Processing**: For each `/tmp/pr_<number>.json` file:
   - Read and analyze `commit_messages` AND `code_changes`
   - Generate narrative summary and title
   - Update PR via GitHub API
   - Create local description file
4. **Commit**: Run `scripts/commit-descriptions.sh`

## File Format: /tmp/pr_<number>.json

```json
{
  "pr_number": "123",
  "title": "Existing PR title",
  "body": "Existing PR body",
  "node_id": "PR_kwDO...",
  "commit_messages": "Commit message 1\nCommit message 2",
  "code_changes": "Commit: abc123\n file.cs | 10 +\n---\nCommit: def456\n other.cs | 5 +"
}
```

## GitHub API Update Command

```bash
gh api graphql -f query="mutation { updatePullRequest(input: { pullRequestId: \"$node_id\", title: \"$generated_title\", body: \"\"\"$generated_summary\"\"\" }) { pullRequest { id title body } } }"
```

## Local File Creation

```bash
echo -e "PR #$pr_number: $generated_title\n\n$generated_summary" > releases/notes/$pr_number.txt
```
