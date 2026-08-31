#!/usr/bin/env bash
#
# Verifies the tooling this skill needs, resolves the remote and the base branch, and
# reports whether the current branch already has an open pull request.
#
# Prints a block of key=value lines on success. Read them; do not re-derive any of it.

set -euo pipefail

fail() { echo "ERROR: $*" >&2; exit 1; }

for tool in git gh jq; do
    command -v "$tool" >/dev/null 2>&1 || fail "$tool is not installed."
done

gh auth status >/dev/null 2>&1 || fail "GitHub CLI is not authenticated. Run 'gh auth login'."

git rev-parse --git-dir >/dev/null 2>&1 || fail "Not inside a git repository."

repo_root="$(git rev-parse --show-toplevel)"
[ -f "$repo_root/ExRam.Gremlinq.slnx" ] || fail "Not in the ExRam.Gremlinq repository root."

# The remote is resolved rather than hardcoded: this repository also carries a
# non-GitHub backup remote, and the GitHub one is not always named 'origin'.
remote="$(git remote -v | grep -m1 'github\.com' | awk '{print $1}')" \
    || fail "No git remote pointing at github.com."
[ -n "$remote" ] || fail "No git remote pointing at github.com."

branch="$(git rev-parse --abbrev-ref HEAD)"
[ "$branch" != 'HEAD' ] || fail "Detached HEAD. Check out a branch first."

# The base is the repository's default branch, which is a release branch such as '14.x'
# rather than 'main'. Never assume the name.
base="$(gh repo view --json defaultBranchRef --jq '.defaultBranchRef.name')"
[ -n "$base" ] || fail "Could not determine the default branch."
[ "$branch" != "$base" ] || fail "Currently on the base branch '$base'. Check out a feature branch first."

# An empty result means no pull request exists yet, which is not an error.
pr="$(gh pr list --head "$branch" --state open --json number,title,url,labels --jq '.[0] // empty')"

echo "repo=$(gh repo view --json nameWithOwner --jq '.nameWithOwner')"
echo "remote=$remote"
echo "base=$base"
echo "branch=$branch"
echo "commits_ahead=$(git rev-list --count "$remote/$base..HEAD" 2>/dev/null || echo 'unknown')"
echo "pushed=$(git rev-parse --verify --quiet "$remote/$branch" >/dev/null && echo 'true' || echo 'false')"

if [ -n "$pr" ]; then
    echo "existing_pr=$(jq -r '.number' <<<"$pr")"
    echo "existing_pr_url=$(jq -r '.url' <<<"$pr")"
    echo "existing_pr_title=$(jq -r '.title' <<<"$pr")"
    echo "existing_pr_labels=$(jq -r '[.labels[].name] | join(",")' <<<"$pr")"
    echo "mode_hint=C"
else
    echo "existing_pr="
    echo "mode_hint=A_or_B"
fi
