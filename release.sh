#!/bin/bash

set -euo pipefail

version_increment="${1:-}"

nbgv_output=$(if [ -z "$version_increment" ]; then
  nbgv prepare-release --format json
else
  nbgv prepare-release --format json --versionIncrement "$version_increment"
fi)

new_branch=$(echo "$nbgv_output" | jq -r '.NewBranch.Name')
current_branch=$(echo "$nbgv_output" | jq -r '.CurrentBranch.Name')

git checkout "$new_branch"
git commit --amend --no-edit -S
git rebase "$new_branch" "$current_branch" -Xtheirs
git tag "$new_branch" "$new_branch"
git branch -d "$new_branch"
