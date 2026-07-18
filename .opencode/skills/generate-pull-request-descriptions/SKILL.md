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

**IMPORTANT:** The scripts in this skill must be executed as-is. They must not be altered or adapted to any perceived "different circumstances". If a script is considered unsuitable for the task, fail early and inform the user.

- The skill assumes the repository is `Gremlinq/ExRam.Gremlinq` (hardcoded in GraphQL queries)
- The skill uses the GitHub CLI for authentication to the GraphQL API
- All operations are performed in the current working directory
- The skill creates physical files in the repository for the pull request descriptions
  - Pull request descriptions are committed to the current branch in `./releases/notes/`
  - The skill does NOT modify version or create tags - use `prepare-release` skill for that
- The current commit must already be on the remote repository for the GraphQL comparison to work
- The skill generates LLM-based summaries and titles from commit messages for PRs without a body

## Workflow

### Step 1: Run Prerequisite Checks

Run the prerequisite checks script to validate the environment:

```bash
scripts/prerequisites.sh
```

All prerequisite checks must pass before proceeding with the workflow. If any check fails, the skill must exit immediately with a non-zero exit code and display the specific error message with installation/remediation instructions.

### Step 2: Generate Pull Request Descriptions

Run the main generation script to retrieve PRs and generate descriptions:

```bash
scripts/generate-descriptions.sh
```

This script:
1. Captures the current commit SHA
2. Gets the previous tag (most recent tag before the current commit)
3. Uses GitHub GraphQL API to compare the previous tag with the current commit SHA
4. Retrieves all commits between the tags
5. For each commit, gets the associated pull requests
6. Deduplicates the list of pull requests (since a PR may have multiple commits)
7. Filters to only PRs that do not have a body
8. For each PR without a body:
   - Retrieves the PR details and all its commits using GitHub API
   - Generates an LLM-based summary and title by analyzing the commit messages
   - Posts the generated summary as a body and the generated title on the PR
   - Creates a text document named `{PR_NUMBER}.txt` in `./releases/notes/`

**IMPORTANT**: If the script determines there are 0 unique pull requests, don't regard it as an error, but instead exit this skill successfully.

**Summary and Title Generation Guidelines:**
- Generate an LLM-based summary and title by analyzing the commit messages
- Categorize commits by their purpose (features, bug fixes, refactoring, tests, maintenance, documentation)
- Use clear, descriptive language
- Mention key changes and their impact
- Keep it concise but informative
- Use bullet points for readability
- Include specific technical details from the commits
- Generate a title from the first commit message if PR title is empty or generic

### Step 3: Commit Pull Request Descriptions

Run the commit script to add and commit the generated files:

```bash
scripts/commit-descriptions.sh
```

This script:
1. Adds all files from the `releases/` directory to the staging area
2. Commits the pull request descriptions to the current branch