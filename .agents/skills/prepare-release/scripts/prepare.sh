#!/usr/bin/env bash
#
# Bumps the version and creates the release tag, using Nerdbank.GitVersioning.
#
# 'nbgv prepare-release' creates a branch named after the release version carrying the
# "Set version to 'X'" commit, and leaves the current branch with the follow-up preview
# bump. This script turns that branch into a tag and rebases the current branch onto it,
# which is why the tag ends up pointing at the *earlier* of the two commits -- anything
# that has to be reachable from the tag must already be committed when this runs.
#
# MUST NOT push anything to any remote.
#
# Usage: prepare.sh [<version increment>]     # 'build' (default), 'minor' or 'major'

set -euo pipefail

version_increment="${1:-build}"

nbgv_output="$(nbgv prepare-release --format json --versionIncrement "$version_increment")"

new_branch="$(jq -r '.NewBranch.Name' <<<"$nbgv_output")"
current_branch="$(jq -r '.CurrentBranch.Name' <<<"$nbgv_output")"

[ -n "$new_branch" ] && [ "$new_branch" != 'null' ] || {
    echo "ERROR: nbgv did not report a new branch." >&2
    exit 1
}

# Checked as well: without it the rebase below would run as 'git rebase <branch> null',
# and by that point nbgv has already committed.
[ -n "$current_branch" ] && [ "$current_branch" != 'null' ] || {
    echo "ERROR: nbgv did not report the current branch." >&2
    exit 1
}

echo "Preparing release $new_branch from $current_branch"

git checkout "$new_branch"

# Re-sign the commit nbgv created. Signed commits are required on the release branches.
git commit --amend --no-edit -S

# 'git rebase <upstream> <branch>' checks out <branch> first, so this also returns us to
# the branch we started on.
git rebase "$new_branch" "$current_branch" -Xtheirs

git tag "$new_branch" "$new_branch"
git branch -d "$new_branch"

echo "Tag created: $new_branch"
echo "Nothing has been pushed."
