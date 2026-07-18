---
name: generate-pull-request-descriptions
description: Generates pull request descriptions by looking up pull requests since the last tag from the current commit and commenting nice summaries on PRs that lack a body. This skill does NOT modify version or create tags.
---

# Generate Pull Request Descriptions Skill

This skill automates the generation of pull request descriptions for ExRam.Gremlinq.

## Usage

```
generate pull request descriptions
```

## Workflow

### Step 1: Run Prerequisite Checks

```bash
scripts/prerequisites.sh
```

### Step 2: Collect PR Data

```bash
scripts/generate-descriptions.sh
```

This collects PR data including commit messages and code changes into `/tmp/pr_<number>.json` files.

### Step 3: Process Each PR with LLM

For each JSON file in `/tmp/pr_*.json`:

1. Read the file: `pr_data=$(cat /tmp/pr_<number>.json)`
2. Extract fields:
   - `pr_number=$(echo "$pr_data" | jq -r '.pr_number')`
   - `node_id=$(echo "$pr_data" | jq -r '.node_id')`
   - `commit_messages=$(echo "$pr_data" | jq -r '.commit_messages')`
   - `code_changes=$(echo "$pr_data" | jq -r '.code_changes')`
   - `existing_title=$(echo "$pr_data" | jq -r '.title')`
3. **YOU (the Agent) MUST analyze BOTH `commit_messages` AND `code_changes` to generate:**
   - A **PROPER NARRATIVE SUMMARY** (NOT bullet points)
   - An appropriate title

**SUMMARY REQUIREMENTS:**
- MUST be a proper narrative paragraph, NOT bullet points
- Start with a clear statement of purpose
- Mention specific files changed and types of changes ONLY if relevant.
- Include technical details from the code
- Categorize changes (feature, bug fix, refactoring, tests, etc.)
- Be concise but informative

**TITLE REQUIREMENTS:**
- Take ALL commit messages into account
- Make it concise and descriptive
- Capitalize properly
- Remove trailing punctuation

4. Update the PR on GitHub:
```bash
gh api graphql -f query="mutation { updatePullRequest(input: { pullRequestId: \"$node_id\", title: \"$generated_title\", body: \"\"\"$generated_summary\"\"\" }) { pullRequest { id title body } } }"
```

5. Create local description file:
```bash
echo -e "PR #$pr_number: $generated_title\n\n$generated_summary" > releases/notes/$pr_number.txt
```

### Step 4: Commit Descriptions

```bash
scripts/commit-descriptions.sh
```

## Important Notes

- The skill MUST dynamically determine the Git remote that points to github.com: `git remote -v | grep github.com | head -1 | awk '{print $1}'`
- The scripts must be executed as-is
- The repository is hardcoded as `Gremlinq/ExRam.Gremlinq` in GraphQL queries
- Uses GitHub CLI for authentication
- Creates files in `./releases/notes/`
- Does NOT modify version or create tags
- The current commit must be on the remote for GraphQL comparison

## Example

If `/tmp/pr_123.json` contains:
```json
{
  "pr_number": "123",
  "title": "",
  "node_id": "PR_kwDO...",
  "commit_messages": "Add async support for GremlinServer\nFix null reference in VertexSerializer",
  "code_changes": "Commit: abc123\n src/Providers.GremlinServer/GremlinServerQueryExecutorAsync.cs | 150 ++++++\n test/Providers.GremlinServer.Tests/AsyncTests.cs | 200 +++++++\n 2 files changed, 350 insertions(+)"
}
```

**YOU (the Agent) should generate:**
- Title: "Add async query execution support for GremlinServer provider"
- Summary: "This pull request adds comprehensive async query execution support for the GremlinServer provider. A new `GremlinServerQueryExecutorAsync` class has been introduced with 150 lines of code, implementing async versions of all query methods. Corresponding tests have been added in `AsyncTests.cs` with 200 lines of test coverage. The implementation maintains parity with the existing sync executor while following proper async/await patterns throughout."

**NOT:**
- Title: "false"
- Summary: "- Add async support\n- Fix null reference"
