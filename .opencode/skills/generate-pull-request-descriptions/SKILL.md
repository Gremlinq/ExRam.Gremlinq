---
name: generate-pull-request-descriptions
description: Generates pull request descriptions by looking up pull requests since the last tag from the current commit and commenting nice summaries on PRs that lack a body. This skill does NOT modify version or create tags.
---

# Generate Pull Request Descriptions Skill

This skill automates the generation of pull request descriptions for ExRam.Gremlinq by:
1. Retrieving all pull requests between the current commit and the previous tag
2. Filtering to only PRs that do not have a body
3. For each PR without a body, generating an LLM-based summary and title by analyzing commit messages
4. Posting the summary as a body and the generated title on the PR
5. Creating pull request description files in a structured format
6. Committing the pull request descriptions to the current branch

## Usage

```
generate pull request descriptions
```

## General Notes

**IMPORTANT:** The skill MUST dynamically determine the Git remote that points to github.com. It MUST NOT hardcode any remote name like "origin" or "github". Always use `git remote -v | grep github.com | head -1 | awk '{print $1}'` to get the remote name.

**IMPORTANT:** The scripts in this skill must be executed as-is. They must not be altered
or adapted to any perceived "different circumstances". If a script is considered unsuitable for the task, fail early and inform the user.

- The skill assumes the repository is `Gremlinq/ExRam.Gremlinq` (hardcoded in GraphQL queries)
- The skill uses the GitHub CLI for authentication to the GraphQL API
- All operations are performed in the current working directory
- The skill creates physical files in the repository for the pull request descriptions
  - Pull request descriptions are committed to the current branch in `./releases/notes/`
  - The skill does NOT modify version or create tags - use `prepare-release` skill for that
- The current commit must already be on the remote repository for the GraphQL comparison to work
- The skill generates LLM-based summaries from commit messages for PRs without a body, not just regex-based categorization

## Workflow

## Prerequisite Checks

Before executing the workflow, the skill MUST perform the following prerequisite checks and fail early with clear error messages and installation instructions:

### 1. GitHub CLI (`gh`) Check

```bash
if ! command -v gh &> /dev/null; then
    echo "ERROR: GitHub CLI (gh) is not installed."
    echo "Installation instructions: https://cli.github.com/manual/installation"
    exit 1
fi

if ! gh auth status &> /dev/null; then
    echo "ERROR: GitHub CLI is not authenticated."
    echo "Run: gh auth login"
    exit 1
fi
```

### 2. jq Check
```bash
if ! command -v jq &> /dev/null; then
    echo "ERROR: jq is not installed."
    echo "Installation instructions:"
    echo "  Ubuntu/Debian: sudo apt-get install jq"
    echo "  macOS: brew install jq"
    echo "  Windows: choco install jq or winget install jqlang.jq"
    exit 1
fi
```

### 3. Git Check
```bash
if ! command -v git &> /dev/null; then
    echo "ERROR: Git is not installed."
    echo "Installation instructions: https://git-scm.com/downloads"
    exit 1
fi
```

### 4. Working Directory Check
```bash
if [ ! -f "ExRam.Gremlinq.slnx" ] || [ ! -d ".git" ]; then
    echo "ERROR: Not in the root of the ExRam.Gremlinq repository."
    echo "Please navigate to the repository root directory."
    exit 1
fi
```

### 5. Clean Working Tree Check
```bash
# Only check for staged or modified files, ignore untracked files
if ! git diff --quiet || ! git diff --cached --quiet; then
    echo "ERROR: Working directory has uncommitted changes."
    echo "Status:"
    git status --short
    echo ""
    echo "Please commit or stash all changes before generating pull request descriptions."
    exit 1
fi
```

### 6. GitHub Authentication Token Check
```bash
if [ -z "$(gh auth status --show-token 2>/dev/null)" ]; then
    echo "ERROR: GitHub CLI token not available or expired."
    echo "Run: gh auth login --with-token"
    echo "Or: gh auth refresh -h github.com"
    exit 1
fi
```

### 7. GitHub Remote Check
```bash
# Dynamically determine the Git remote that points to github.com
github_remote=$(git remote -v | grep github.com | head -1 | awk '{print $1}')

if [ -z "$github_remote" ]; then
    echo "ERROR: No Git remote pointing to github.com found."
    echo "Available remotes:"
    git remote -v
    exit 1
fi
```

All prerequisite checks must pass before proceeding with the workflow. If any check fails, the skill must exit immediately with a non-zero exit code and display the specific error message with installation/remediation instructions.

## Main logic

### Step 1: Retrieve Pull Requests Since Last Tag

After prerequisite validation, the skill:

1. Captures the current commit SHA
2. Gets the previous tag (most recent tag before the current commit)
3. Uses GitHub GraphQL API to compare the previous tag with the current commit SHA
4. Retrieves all commits between the tags
5. For each commit, gets the associated pull requests
6. Deduplicates the list of pull requests (since a PR may have multiple commits)
7. Filters to only PRs that do not have a body (using the body field from the first GraphQL query)

**Bash Implementation for Step 1:**
```bash
# Get the current commit SHA
current_sha=$(git rev-parse HEAD)

# Get the previous tag (most recent tag that is an ancestor of current commit)
previous_tag=$(git describe --tags --abbrev=0 HEAD^ 2>/dev/null || echo "")

if [ -z "$previous_tag" ]; then
    echo "ERROR: Could not find a previous tag to compare against."
    exit 1
fi

echo "Generating pull request descriptions for commits between $previous_tag and $current_sha"

# Query GitHub GraphQL API to get PRs between previous tag and current commit
gh api graphql -f query="{
  repository(owner: \"Gremlinq\", name: \"ExRam.Gremlinq\") {
    ref(qualifiedName: \"$previous_tag\") {
      compare(headRef: \"$current_sha\") {
        commits(first: 100) {
          nodes {
            oid
            messageHeadline
            associatedPullRequests(first: 100) {
              nodes {
                number
                mergedAt
                title
                url
                body
              }
            }
          }
        }
      }
    }
  }
}" | jq -c '.data.repository.ref.compare.commits.nodes[].associatedPullRequests.nodes[] | select(.number != null and (.body == null or .body == ""))' > /tmp/prs.json

# Extract unique PR numbers
cat /tmp/prs.json | jq -r '.number' | sort -u > /tmp/unique_pr_numbers.txt

# Count the number of unique PRs
pr_count=$(wc -l < /tmp/unique_pr_numbers.txt)
echo "Found $pr_count unique pull request(s) without a body"
```

### Step 2: Generate LLM-Based Summaries and Pull Request Descriptions

For each unique pull request (which already have no body):

1. Retrieve the PR details and all its commits using GitHub API
2. Generate an LLM-based summary and title by analyzing the commit messages
3. Post the generated summary as a body and the generated title on the PR
4. Create a text document named `{PR_NUMBER}.txt` in `./releases/notes/`

**Summary Generation Guidelines:**
- Generate an LLM-based summary by analyzing the commit messages
- Categorize commits by their purpose (features, bug fixes, refactoring, tests, maintenance, documentation)
- Use clear, descriptive language
- Mention key changes and their impact
- Keep it concise but informative
- Use bullet points for readability
- Include specific technical details from the commits

**Bash Implementation for Step 2:**
```bash
# Create the releases directory structure
mkdir -p "releases/notes"

# For each unique PR number, get PR details and commits
while read -r pr_number; do
    echo "Processing PR #$pr_number..."
    
    # Get PR details including commits
    pr_data=$(gh api graphql -f query="{
      repository(owner: \"Gremlinq\", name: \"ExRam.Gremlinq\") {
        pullRequest(number: $pr_number) {
          number
          title
          body
          url
          id
          commits(first: 100) {
            nodes {
              commit {
                messageHeadline
                messageBody
                oid
              }
            }
          }
        }
      }
    }")
    
    # Extract relevant fields
    pr_title=$(echo "$pr_data" | jq -r '.data.repository.pullRequest.title')
    pr_body=$(echo "$pr_data" | jq -r '.data.repository.pullRequest.body')
    pr_node_id=$(echo "$pr_data" | jq -r '.data.repository.pullRequest.id')
    total_commits=$(echo "$pr_data" | jq -r '.data.repository.pullRequest.commits.nodes | length')
    
    echo "  Found $total_commits commit(s), generating LLM-based summary..."
    
    # Store PR data for LLM summary generation
    echo "$pr_data" | jq -r --arg prn "$pr_number" --arg title "$pr_title" '{
      pr_number: $prn,
      title: $title,
      body: .data.repository.pullRequest.body,
      node_id: .data.repository.pullRequest.id,
      commits: [.data.repository.pullRequest.commits.nodes[] | .commit | {headline: .messageHeadline, body: (.messageBody // "")}],
    }' > /tmp/pr_$pr_number.json
    
    echo "  PR data stored for LLM summary generation: /tmp/pr_$pr_number.json"
done < /tmp/unique_pr_numbers.txt

# Now process each PR with LLM-generated summaries
while read -r pr_number; do
    pr_file="/tmp/pr_$pr_number.json"
    if [ ! -f "$pr_file" ]; then
        continue
    fi
    
    pr_data=$(cat "$pr_file")
    pr_title=$(echo "$pr_data" | jq -r '.title')
    pr_body=$(echo "$pr_data" | jq -r '.body')
    pr_node_id=$(echo "$pr_data" | jq -r '.node_id')
    
    echo "Processing PR #$pr_number with LLM..."

    # Extract all commit messages for summary and title generation
    all_commits=$(echo "$pr_data" | jq -r '.commits[] | .headline + (if .body and .body != "" then ": " + .body else "" end)')
    first_commit=$(echo "$pr_data" | jq -r '.commits[0] | .headline + (if .body and .body != "" then ": " + .body else "" end)')

    # For actual LLM processing, the skill execution will handle this
    # The bash script stores the data, and the LLM generates the summary and title
    # Generate a placeholder summary and title for now (will be replaced by LLM)
    final_summary="## Changes\n\n"
    
    if [ -n "$all_commits" ]; then
        while IFS= read -r commit_line; do
            [ -z "$commit_line" ] && continue
            final_summary+="  - $commit_line\n"
        done <<< "$all_commits"
    else
        final_summary+="- Various improvements and fixes"
    fi

    # Generate a title from the first commit message if PR title is empty or generic
    if [ -z "$pr_title" ] || [ "$pr_title" = "null" ] || [ "$pr_title" = "Update" ] || [ "$pr_title" = "Fix" ] || [ "$pr_title" = "Changes" ] || [ "$pr_title" = "WIP" ] || [ "$pr_title" = "Work in progress" ]; then
        generated_title="$first_commit"
    else
        generated_title="$pr_title"
    fi

    # Clean up the summary and title
    final_summary=$(echo -e "$final_summary" | sed '/^$/d' | sed 's/^ *//')
    generated_title=$(echo "$generated_title" | sed 's/^ *//' | sed 's/ *$//')

    echo "  Generated summary and title for PR #$pr_number"
    
    # If the PR does not have a body, update it with the generated summary and title
    if [ -z "$pr_body" ] || [ "$pr_body" = "null" ]; then
        echo "  PR #$pr_number has no body, updating with generated summary and title..."
        gh api graphql -f query="
        mutation {
          updatePullRequest(
            input: {
              pullRequestId: \"$pr_node_id\",
              title: \"$generated_title\",
              body: \"\"\"$final_summary\"\"\"
            }
          ) {
            pullRequest {
              id
              title
              body
            }
          }
        }"
        echo "  Updated PR #$pr_number with generated title and body"
    fi

     # Use the generated summary as the content
     content="PR #$pr_number: $pr_title\n\n$final_summary"

     echo -e "$content" > "releases/notes/$pr_number.txt"
     echo "  Created release note: releases/notes/$pr_number.txt"

     # Clean up
     rm -f "/tmp/pr_$pr_number.json"
done < /tmp/unique_pr_numbers.txt
```

### Step 3: Commit Pull Request Descriptions to Current Branch

After all pull request descriptions are generated and PR descriptions are updated (if needed):

1. Add all files from the `releases/` directory to the staging area
2. Commit the pull request descriptions to the current branch

**Bash Commands:**
```bash
# Add all pull request descriptions files
git add releases/

# Commit the pull request descriptions to the current branch
git commit -m "Generate pull request descriptions"

echo "Pull request descriptions committed to current branch"
```
