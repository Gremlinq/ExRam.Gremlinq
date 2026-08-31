#!/usr/bin/env bash
#
# Prerequisite checks for the prepare-release skill.

set -euo pipefail

fail() { echo "ERROR: $*" >&2; shift 0; exit 1; }

command -v nbgv >/dev/null 2>&1 || fail "Nerdbank.GitVersioning (nbgv) is not installed. Install with: dotnet tool install -g nbgv"
command -v git  >/dev/null 2>&1 || fail "Git is not installed."
command -v jq   >/dev/null 2>&1 || fail "jq is not installed."
# Needed by the write-release-announcements skill, which runs before the version bump.
command -v gh   >/dev/null 2>&1 || fail "GitHub CLI (gh) is not installed."

gh auth status >/dev/null 2>&1 || fail "GitHub CLI is not authenticated. Run 'gh auth login'."

git rev-parse --git-dir >/dev/null 2>&1 || fail "Not inside a git repository."
[ -f "ExRam.Gremlinq.slnx" ] || fail "Not in the root of the ExRam.Gremlinq repository."

if ! git diff --quiet || ! git diff --cached --quiet; then
    echo "ERROR: The working tree has uncommitted changes." >&2
    git status --short >&2
    echo "Commit or stash everything before preparing a release." >&2
    exit 1
fi

remote="$(git remote -v | grep -m1 'github\.com' | awk '{print $1}')"
[ -n "$remote" ] || fail "No git remote pointing at github.com."

branch="$(git rev-parse --abbrev-ref HEAD)"
version="$(nbgv get-version --format json | jq -r '.SimpleVersion')"

# A tag that already exists means this release was prepared before. Creating it again
# fails halfway through prepare.sh, after the version commits have been made.
if git rev-parse --verify --quiet "refs/tags/$version" >/dev/null; then
    fail "Tag '$version' already exists locally. This release has already been prepared."
fi

echo "All prerequisite checks passed."
echo "remote=$remote"
echo "branch=$branch"
echo "version=$version"
