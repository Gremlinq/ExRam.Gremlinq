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

# For each unique PR number, get PR details and commits AND local code changes
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
    
    echo "  Found $total_commits commit(s), collecting code changes..."
    
    # Extract commit SHAs for local git analysis
    commit_shas=$(echo "$pr_data" | jq -r '.data.repository.pullRequest.commits.nodes[] | .commit.oid')
    
    # Get code changes for each commit using local git
    code_changes=""
    first_commit_sha=""
    all_commit_messages=""
    
    while IFS= read -r commit_sha; do
        [ -z "$commit_sha" ] && continue
        
        if [ -z "$first_commit_sha" ]; then
            first_commit_sha="$commit_sha"
        fi
        
        # Get commit message
        commit_message=$(echo "$pr_data" | jq -r --arg sha "$commit_sha" '.data.repository.pullRequest.commits.nodes[] | select(.commit.oid == $sha) | .commit.messageHeadline + (if .commit.messageBody and .commit.messageBody != "" then ": " + .commit.messageBody else "" end)')
        if [ -n "$all_commit_messages" ]; then
            all_commit_messages+="\n"
        fi
        all_commit_messages+="$commit_message"
        
        # Get the parent commit SHA for diff
        parent_sha=$(git rev-parse "$commit_sha^" 2>/dev/null || echo "")
        
        if [ -n "$parent_sha" ]; then
            # Get the diff for this commit
            commit_diff=$(git diff "$parent_sha" "$commit_sha" --stat 2>/dev/null || echo "")
            if [ -n "$commit_diff" ]; then
                if [ -n "$code_changes" ]; then
                    code_changes+="\n\n---\n"
                fi
                code_changes+="Commit: $commit_sha\n"
                code_changes+="$commit_diff"
            fi
        else
            # For root commits, get the full commit content
            commit_content=$(git show "$commit_sha" --stat 2>/dev/null || echo "")
            if [ -n "$commit_content" ]; then
                if [ -n "$code_changes" ]; then
                    code_changes+="\n\n---\n"
                fi
                code_changes+="Commit: $commit_sha\n"
                code_changes+="$commit_content"
            fi
        fi
    done <<< "$commit_shas"
    
    # Store PR data for LLM summary generation including code changes
    # This JSON will be used by the LLM to generate proper narrative summaries
    jq -n --arg prn "$pr_number" \
           --arg title "$pr_title" \
           --arg body "$pr_body" \
           --arg node_id "$pr_node_id" \
           --arg commits "$all_commit_messages" \
           --arg code_changes "$code_changes" \
           '{
             pr_number: $prn,
             title: $title,
             body: $body,
             node_id: $node_id,
             commit_messages: $commits,
             code_changes: $code_changes
           }' > /tmp/pr_$pr_number.json
    
    echo "  PR data stored for LLM summary generation: /tmp/pr_$pr_number.json"
done < /tmp/unique_pr_numbers.txt

echo "Data collection completed. The following files contain PR data for LLM processing:"
ls -la /tmp/pr_*.json 2>/dev/null || echo "No PR data files found."

echo "Pull request data collection completed."
