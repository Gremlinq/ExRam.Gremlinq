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
# The version being released is always the current preview version with its suffix
# dropped: 14.1.2-preview.{height} releases as 14.1.2. Afterwards the branch moves on by
# one build number. That is the whole rule, and there is no variant of it -- a minor or
# major bump is decided in the pull request that changes the public API, by editing
# version.json there. By the time a release is prepared, the version has long been fixed.
#
# The increment is not passed on the command line either: version.json already declares
# it as "release": { "versionIncrement": "build" }, and nbgv reads it from there.
#
# MUST NOT push anything to any remote.
#
# Usage: prepare.sh

set -euo pipefail

# Loud rather than silent: 'prepare.sh minor' used to be accepted, and an agent working
# from a stale copy of the skill would otherwise get a build increment while believing it
# had asked for something else.
[ $# -eq 0 ] || {
    echo "ERROR: This script takes no arguments (got: $*)." >&2
    echo "The released version follows from version.json. To release a minor or major," >&2
    echo "raise version.json in the pull request that changes the public API instead." >&2
    exit 2
}

nbgv_output="$(nbgv prepare-release --format json)"

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
