#!/bin/bash

# Main script for generating pull request descriptions

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
    echo "  Created pull request description: releases/notes/$pr_number.txt"
    
    # Clean up
    rm -f "/tmp/pr_$pr_number.json"
done < /tmp/unique_pr_numbers.txt

echo "Pull request descriptions generation completed."