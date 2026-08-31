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
[ "$branch" != 'HEAD' ] \
    || fail "Detached HEAD. A previous run probably died mid-rebase; sort that out first."

# prepare.sh makes two commits, a branch and a tag in one go, and can die between them --
# an unavailable signing key, or a rebase conflict that -Xtheirs cannot resolve, such as
# add/add or modify/delete. The three checks below are what a half-finished run leaves
# behind. Without them a re-run reports success here and then dies inside nbgv, having
# added yet more commits.
for state in rebase-merge rebase-apply; do
    if [ -e "$(git rev-parse --git-path "$state")" ]; then
        fail "A rebase is in progress. Finish or abort it ('git rebase --abort') first."
    fi
done

version="$(nbgv get-version --format json | jq -r '.SimpleVersion')"

if git rev-parse --verify --quiet "refs/tags/$version" >/dev/null; then
    fail "Tag '$version' already exists locally. This release has already been prepared."
fi

# 'nbgv prepare-release' names its temporary branch after the version, and prepare.sh
# deletes it again. One left over means the previous run did not get that far.
if git rev-parse --verify --quiet "refs/heads/$version" >/dev/null; then
    fail "Branch '$version' already exists -- left behind by a release preparation that did not finish. Inspect it, then delete it with 'git branch -D $version' before retrying."
fi

echo "All prerequisite checks passed."
echo "remote=$remote"
echo "branch=$branch"
echo "version=$version"
