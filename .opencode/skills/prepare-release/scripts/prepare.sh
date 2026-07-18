#!/bin/bash

# Prepare Release with Nerdbank.GitVersioning

# The skill MUST NOT push any tags to any remote.

# Get the current commit SHA
current_sha=$(git rev-parse HEAD)

nbgv_output=$(nbgv prepare-release --format json --versionIncrement build)

new_branch=$(echo "$nbgv_output" | jq -r '.NewBranch.Name')
current_branch=$(echo "$nbgv_output" | jq -r '.CurrentBranch.Name')

echo "Preparing release"
echo "New branch: $new_branch"
echo "Current branch: $current_branch"

# Store the current branch for later return
git checkout "$new_branch"
git commit --amend --no-edit -S
git rebase "$new_branch" "$current_branch" -Xtheirs
git tag "$new_branch" "$new_branch"
git branch -d "$new_branch"

# Return to the original branch
git checkout "$current_branch"

echo "Tag created: $new_branch"

echo "Release preparation completed successfully!"
